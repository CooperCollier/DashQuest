using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	//--------------------------------------------------------------------------------

	/* Adjustable character attributes, set for the entire game. */

	[SerializeField]
    float startX;

    [SerializeField]
    float startY;

    [SerializeField]
    int startingHealth;

    [SerializeField]
    int dashDistance;

	[SerializeField]
    float moveSpeed;

    [SerializeField]
    float jumpSpeed;

    [SerializeField]
    float dashSpeed;

    /* Two layerMasks sed during raycasting & boxcasting towards platforms.
     * A platform is anything that the player can stand on and jump from. */
    [SerializeField]
    LayerMask detectPlatform;
    [SerializeField]
    LayerMask detectEnemy;

    /* Objects for utility purposes. */
    public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public Bullet bullet;
    public HealthBar healthBar;

    /* DashDestination is where the player dashes to. Once they reach here, the
     * dash ends. It is exactly DashDistance away from the start of the dash. */
    Vector2 dashDestination;

    /* Dashstate = 0 means the player has not yet started dashing.
     * Dashstate = 1 means the player is currently dashing. 
     * Dashstate = 2 means the player finished dashing but has not hit ground yet. */
    int dashState = 0;

    /* Invincibility frames right after taking damage */
    int iFrames = 0;

    /* How long the jump button has been held down for. As this number increases, 
     * the player's jump gets higher. This number resets to zero upon hitting the ground. */
    int jumpTime = 0;

    /* Player doesn't start to fall until coyoteFrames = 0. coyoteFrames decrements
     * as soon as go go off the edge of a platform. coyoteFrames resets to 5 once you
     * stand on a platform. */
    int coyoteFrames = 30;

    /* How long the player has been running. As runtime increases, the player
     * runs faster. */
    int runTime;

    /* Multiply this by speed when calculating movement. */
    float runMultiplier;

    /* How many frames the player has been in the same position. This number is
     * updated anytime the player moves. This variable is used to end the dash early
     * in case the player gets stuck. */
    int staying = 0;

    /* If this number is greater than zero, the player should not be able to
     * start a dash. This is used in the AvoidClip function because sometimes it
     * can transport the player to a place in the air which would make them dash 
     * inadvertently. */
    int stopDash = 0;

    /* Status is an int that represents what the player is currently doing.
     *
     * 0: Idle.
     *
     * 1: Running Left.
     * 2: Running Right.
     *
     * 3: Jumping Left.
     * 4: Jumping Right.
     *
     * 5: Dashing Left Up.
     * 6: Dashing Left Down.
     * 7: Dashing Right Up. 
     * 8: Dashing Right Down. 
     *
     * 9: Falling Left.
     * 10: Falling Right.
     *
     * 11: Dead. */
    public static int statusCode = 0;

    /* Location is the player's current location. */
    public static Vector2 location;

    /* How much health the player has left. */
    public static int health;

    /* How many coins the player has */
    public static int money = 0;

    //--------------------------------------------------------------------------------

    void Start() {
    	  spriteRenderer = GetComponent<SpriteRenderer>();
    	  rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        transform.position = new Vector2(startX, startY);
        health = startingHealth;
        GetComponent<Renderer>().enabled = true;
    }

    //--------------------------------------------------------------------------------

    void Update() {

        /* Set the value of the 'staying' variable and update the 'location'
         * variable. */
        if ((location - (Vector2) transform.position).magnitude < 0.05) {
            staying += 1;
        } else {
            location = (Vector2) transform.position;
            staying = 0;
        }

    	  /* Update the status code. */
 		    setStatusCode();

        /* Press W to jump if the player is on the ground. */
    	  if (Input.GetKeyDown(KeyCode.W)) {
    		    if (isGrounded() || coyoteFrames > 0) {
                jumpTime = 0;
                rigidbody2D.velocity = Vector2.up * jumpSpeed;
    		    }
    	  }

        /* Jump time increases while the jump button is held down in the air.
         * The number 61 was chosen so that jumpTime doesn't become 60 again until 
         * after the jump is over, since jumpTime keeps going up. */
        if (Input.GetKey(KeyCode.W)) {
            jumpTime += 1;
            if (jumpTime == 60) {
                rigidbody2D.velocity = Vector2.up * jumpSpeed * 0.9f;
            }
        } else {
            jumpTime = 61;
        }

    	  /* Click the mouse to shoot if the player is on the ground. 
    	   * Click the mouse to dash if the player is in the air. */
    	  if (Input.GetMouseButtonDown(0)) {
    		    if (isGrounded()) {
    			  Shoot(Input.mousePosition);
    		    } else if ((statusCode == 3 || statusCode == 4) && stopDash == 0) {
    			    startDash(Input.mousePosition);
    		    }
    	  }

    	  /* Update the coyote frames based on wether or not the player is grounded. */
    	  if (!isGrounded() && coyoteFrames > 0) {
    		  coyoteFrames -= 1;
    	  } else if (isGrounded()) {
    		  coyoteFrames = 30;
    	  }

    	  /* Get keyboard input to move, unless the player is dashing. */
    	  if (statusCode >= 5 && statusCode <= 8) {
    		  continueDash();
    	  } else {
    		  Move();
    	  }

    	  /* The dash state resets to zero upon standing on a platform. */
    	  if (isGrounded()) {
    		  dashState = 0;
    	  }

    	  /* Update the health bar. */
    	  healthBar.setHealth(health);

        /* This decrements invincibility frames. The player's sprite gets changed to
         * red upon taking damage, and this is the code is to change it back to
         * white once enough frames have passed. Also, if the i-frames are just about to
         * run out we need to call the avoidClip function. Also, the player stops colliding 
         * with enemies right after taking damage, and this has the code to change the colliders 
         * abck to normal after all the i-frames are over. */
        if (iFrames > 0) {
            iFrames -= 1;
            if (iFrames == 1) {
                AvoidClip();
                Physics2D.IgnoreLayerCollision(8, 11, false);
            } else if (iFrames == 60) {
            	spriteRenderer.color = Color.white;
            }
        }

        /* Set the runTime & runMultiplier variables. */
        Run();

        /* Decrement stopDash if it is positive. */
        if (stopDash > 0) {
            stopDash -= 1;
        }

    }

    //--------------------------------------------------------------------------------

    /* Update the statusCode variable according to game state. */

    private void setStatusCode() {

    	if (health <= 0) { // Dead
        	statusCode = 11;

        } else if (isGrounded() && (xDirection() == 0)) { // Idle
        	statusCode = 0;

        } else if (isGrounded() && (xDirection() == 1)) { // Running Right
        	statusCode = 2;
        } else if (isGrounded() && (xDirection() == -1)) { // Running Left
        	statusCode = 1;

        } else if (!isGrounded() && (xDirection() >= 0) && (dashState == 0)) { // Jumping Right
        	statusCode = 4;
        } else if (!isGrounded() && (xDirection() == -1) && (dashState == 0)) { // Jumping Left
        	statusCode = 3;

        } else if (!isGrounded() && (xDirection() >= 0) && (dashState == 1) && (yDirection() >= 0)) { // Dashing Right Up
        	statusCode = 7;
        } else if (!isGrounded() && (xDirection() == -1) && (dashState == 1) && (yDirection() >= 0)) { // Dashing Left Up
        	statusCode = 5;
        } else if (!isGrounded() && (xDirection() >= 0) && (dashState == 1) && (yDirection() == -1)) { // Dashing Right Down
        	statusCode = 8;
        } else if (!isGrounded() && (xDirection() == -1) && (dashState == 1) && (yDirection() == -1)) { // Dashing Left Down
        	statusCode = 6;

        } else if (!isGrounded() && (xDirection() >= 0) && (dashState == 2)) { // Falling Right
        	statusCode = 10;
        } else if (!isGrounded() && (xDirection() == -1) && (dashState == 2)) { // Falling Left
        	statusCode = 9;
        }

    }

    //--------------------------------------------------------------------------------

    /* The move function is called every frame. It translates the keyboard input into
     * player movement. The HitWall function is called when the player hits a wall. If
     * the player's head is above the wall, then the player automatically climbs over. */
    private void Move() {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        } else if (leftCollision() && !isGrounded() && Input.GetKey(KeyCode.A)) {
        	HitWall(Vector2.left);
        } else if (rightCollision() && !isGrounded() && Input.GetKey(KeyCode.D)) {
        	HitWall(Vector2.right);
        } else if (Input.GetKey(KeyCode.D)) {
            rigidbody2D.velocity = new Vector2(moveSpeed * runMultiplier, rigidbody2D.velocity.y);
        } else if (Input.GetKey(KeyCode.A)) {
            rigidbody2D.velocity = new Vector2(-moveSpeed * runMultiplier, rigidbody2D.velocity.y);
        } else {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x / 2, rigidbody2D.velocity.y);
        }
    }

    //--------------------------------------------------------------------------------

    private void HitWall(Vector2 direction) {

        /* This function creates 3 raycasts--three_quarters, one_half, and one_quarter--
         * and checks if each one hits a wall. Each one corresponds to a portion of the player's
         * y-axis height. It then selects the lowest raycast that did hit a wall. It translates
         * player's lowest point up to the height of that raycast, and translates the player's
         * x-coordinate across the player width. */

        /* If any collider != null, that means it hit a wall, so we know that collider's
         * raycast is below the wall edge. Our goal is to find the LOWEST raycast that is
         * still ABOVE the wall edge. Therefore, we start at the top and check raycasts
         * as we go down. When one of them is below the wall edge, we select the one right
         * above it. */

         /* The offset vector's y-component will be the height of our desired raycast (the
         * lowest one above the wall) minus the bottom of the player. The offset vector's
         * x-component will be just the width of the player. This vector describes the
         * distance between where the player is now, and where the player will be as soon as
         * they climb the wall. */
        Vector2 offset;

        /* Raycast at full player height and at 3/4 of player height. */
        RaycastHit2D three_quarters = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max 
        	                - new Vector2(0, (boxCollider2D.bounds.size.y * (float) 0.25)),
                                                       direction,
                                                       10f,
                                                       detectPlatform);
        RaycastHit2D one = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max,
                                                       				   direction,
                                                       						10f,
                                                       			detectPlatform);
        if (three_quarters.collider != null || one.collider != null) {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            return;
        }

        /* Raycast at 1/2 of player height. */
        RaycastHit2D one_half = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max 
        	           - new Vector2(0, (boxCollider2D.bounds.size.y * (float) 0.5)),
                                                  direction,
                                                  .3f,
                                                  detectPlatform);
        if (one_half.collider == null) {
            offset = new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y * (float) 0.75);
            transform.Translate((location + offset) * Time.deltaTime);
            return;
        }

        /* Raycast at 1/4 of player height. */
        RaycastHit2D one_quarter = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max 
        	             - new Vector2(0, (boxCollider2D.bounds.size.y * (float) 0.75)),
                                                  direction,
                                                  .3f,
                                                  detectPlatform);
        if (one_quarter.collider == null) {
            offset = new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y * (float) 0.5);
            transform.Translate((location + offset) * Time.deltaTime);
            return;
        }

        /* If none of the raycasts are below a wall, we default to the 1/4 player height one. */
        offset = new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y * (float) 0.25);
    	  transform.Translate((location + offset) * Time.deltaTime);

    }

    //--------------------------------------------------------------------------------

    /* Handle collisions with specific game objects. */

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Door End" && Input.GetKey(KeyCode.Space)) {
        	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (other.tag == "Door Start" && Input.GetKey(KeyCode.Space)) {
        	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Spikes" && health > 0) {
            TakeDamage(10);
        } else if (collision.gameObject.tag == "Enemy") {
            Vector2 enemy = collision.gameObject.GetComponent<Transform>().position;
            PushFromEnemy(enemy);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Coin") {
            money += 1;
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "Spikes" && health > 0) {
            TakeDamage(10);
        } else if (collision.gameObject.tag == "Enemy") {
            Vector2 enemy = collision.gameObject.GetComponent<Transform>().position;
            PushFromEnemy(enemy);
        }
    }

    //--------------------------------------------------------------------------------

    /* Function to take damage. This starts up the invincibility frames, briefly turns the
     * player red, and makes them stop colliding with enemies. These changes are reverted to
     * normal in the update() function, after enough frames have passed. */
    public void TakeDamage(int damage) {
        if (health <= 0 || (statusCode >= 5 && statusCode <= 8)) {
            return;
        }
        if (iFrames == 0) {
            iFrames = 120;
            health -= damage;
            Physics2D.IgnoreLayerCollision(8, 11, true);
            spriteRenderer.color = Color.red;
        }
    }

    /* Push the player slightly in one direction. */
    private void PushFromEnemy(Vector2 enemy) {
    	  Vector2 difference = location - enemy;
        rigidbody2D.velocity = difference.normalized * 10;
    }

    /* Translate the player. Mainly used when the player is standing on a 
     * movingBox (this function is called via a sendMessage in the MovingBox
     * script). */
    public void Translate(Vector2 vector) {
        transform.Translate(vector * Time.deltaTime);
    }

    /* Called by the ButtonManager script through SendMessage once the
     * player dies. */
    public void Invisible() {
        GetComponent<Renderer>().enabled = false;
    }

    public void Visible() {
        GetComponent<Renderer>().enabled = true;
    }

    //--------------------------------------------------------------------------------

    /* Right after the player gets hit, the collision between player & enemy layes
     * is disabled. Right when it is re-enabled, if the player is standing inside
     * an enemy & right next to a wall, the player can get pushed through the wall.
     * This function counter-acts the bug by pushing the player upwards out of the
     * enemy. It is called in update() right before the invincibility frames run out. */

    private void AvoidClip() {

        float moveX = 0;
        float moveY = 0;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");

        RaycastHit2D leftWall = Physics2D.Raycast((Vector2) boxCollider2D.bounds.min,
                                                  Vector2.left,
                                                  .3f,
                                                  detectPlatform,
                                                  ~detectEnemy);
        RaycastHit2D rightWall = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max,
                                                  Vector2.right,
                                                  .3f,
                                                  detectPlatform,
                                                  ~detectEnemy);

        RaycastHit2D up = Physics2D.Raycast((Vector2) boxCollider2D.bounds.max,
                                                  Vector2.up,
                                                  .3f,
                                                  detectPlatform,
                                                  ~detectEnemy);

        if (leftWall.collider != null) {
            moveX = boxCollider2D.bounds.size.x * 1.5f;
        } else if (rightWall.collider != null) {
            moveX = -boxCollider2D.bounds.size.x * 1.5f;
        }

        if (up.collider != null) {
        	moveY = 0;
        } else {
        	moveY = boxCollider2D.bounds.size.y * 1.5f;
        }

        for (int i = 0; i < objects.Length; i++) {
            if (((Vector2) objects[i].transform.position - location).magnitude < 0.5) {
                transform.Translate(new Vector2(moveX, moveY));
                stopDash = 30;
            }
        }

    }

    //--------------------------------------------------------------------------------

    private void Run() {

        /* Runtime increases until the player stops*/
        if (statusCode == 0) {
            runTime  = 0;
        } else {
            runTime += 1;
        }

        /* Calculate run multiplier based on runTime. The longer the player
         * has been running, the faster they go. */
        if (runTime < 10) {
            runMultiplier = (float) 0.5;
        } else if (runTime < 30) {
            runMultiplier = (float) 0.9;
        } else if (runTime < 100) {
            runMultiplier = (float) 1.1;
        } else if (runTime < 250) {
            runMultiplier = (float) 1.2;
        } else if (runTime < 400) {
            runMultiplier = (float) 1.3;
        } else {
            runMultiplier = (float) 1.5;
        }

    }

    //--------------------------------------------------------------------------------

    /* IsGrounded returns true if the player is standing or running on a platform.
     * xDirection returns 0 if the player is idle, -1 if the player is moving
     * to the left, and 1 if the player is moving to the right. yDirection is the
     * same, but works on the y-axis. */

    private bool isGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                      boxCollider2D.bounds.size,
                                                      0f,
                                                      Vector2.down,
                                                      .3f,
                                                      detectPlatform);
        return (raycastHit2D.collider != null);
    }

    private int xDirection() {
        if (Input.GetKey(KeyCode.A)) {
    		    return -1;
    	  } else if (Input.GetKey(KeyCode.D)) {
    		    return 1;
    	  } else {
    		    return 0;
    	  }
    }

    private int yDirection() {
        if (rigidbody2D.velocity.y < 0.1 && rigidbody2D.velocity.y > -0.1) {
            return 0;
        } else if (rigidbody2D.velocity.y < 0) {
        	  return -1;
        } else if (rigidbody2D.velocity.y > 0) {
        	  return 1;
        }
        return 0;
    }

    //--------------------------------------------------------------------------------

    /* This function is run whenever the player wants to shoot a bullet. */

    private void Shoot(Vector2 destination) {
        Bullet thisBullet = Instantiate(bullet, transform.position, Quaternion.identity);
    	  thisBullet.start = transform.position;
    	  thisBullet.destination = destination;
    }

    //--------------------------------------------------------------------------------

    /* StartDash runs at the beginning of every dash. DashVector is just a 
     * placeholder variable, it doesn't necessarily have to have the same 
     * value in startDash and continueDash. */
    private void startDash(Vector2 destination) {
    	  dashState = 1;
    	  destination = Camera.main.ScreenToWorldPoint(destination);
    	  Vector2 dashVector = ((Vector2) destination) - location;
    	  dashVector = dashVector.normalized * dashDistance;
    	  dashDestination = location + dashVector;
    }

    /* ContinueDash runs every frame while the player is dashing. */
    private void continueDash() {

        /* DashVector points from the player to the destination. */
    	  Vector2 dashVector = (dashDestination - location);
    	  dashVector = dashVector.normalized * dashSpeed;
    	  rigidbody2D.velocity = dashVector;

        /* These four boxCasts check if the player is colliding with a wall, in
         * which case the dash should end early. There was a serious bug earlier where
         * the player would get stuck on a ledge if only HALF of the player's body
         * was above a wall (for example, dashing right at a corner). The bug should
         * be fixed in this implementation. */
        RaycastHit2D checkWallLeft = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                   boxCollider2D.bounds.size,
                                                   0f,
                                                   Vector2.left,
                                                   .3f,
                                                   detectPlatform);
        RaycastHit2D checkWallRight = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                   boxCollider2D.bounds.size,
                                                   0f,
                                                   Vector2.right,
                                                   .3f,
                                                   detectPlatform);
        RaycastHit2D checkWallUp = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                   boxCollider2D.bounds.size,
                                                   0f,
                                                   Vector2.up,
                                                   .3f,
                                                   detectPlatform);
        RaycastHit2D checkWallDown = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                   boxCollider2D.bounds.size,
                                                   0f,
                                                   Vector2.down,
                                                   .3f,
                                                   detectPlatform);

        /* If any of the colliders hit a wall, we should end the dash early.
         * Of course we also need to end it when the player's location is
         * close enough to the dash destination. The dash will also automatically
         * course-correct if it is very close to horizontal / verical but not
         * quite, which is what the < 0.5 checks are for. This is just for player
         * convenience. */
        if (checkWallLeft.collider != null && (dashVector.x < 0)) {
            if (dashVector.x < -0.5f) {
                dashDestination.x = location.x + 0.1f;
            } else {
                endDash();
            }
        } else if (checkWallRight.collider != null && (dashVector.x > 0)) {
            if (dashVector.x > 0.5f) {
                dashDestination.x = location.x - 0.1f;
            } else {
                endDash();
            }
        } else if (checkWallUp.collider != null && (dashVector.y > 0)) {
            if (dashVector.y > 0.5f) {
                dashDestination.y = location.y - 0.1f;
            } else {
                endDash();
            }
        } else if (checkWallDown.collider != null && (dashVector.y < 0)) {
            endDash();
        } else if ((dashDestination - location).magnitude < 0.5) {
            endDash();
        } else if (staying > 5) {
            endDash();
        }

    }

    /* Ends the dash. */
    private void endDash() {
    	  rigidbody2D.velocity = Vector2.zero;
        dashState = 2;
    }

    //--------------------------------------------------------------------------------

    /* Detect if the player hit a wall on their left or right. */

    private bool leftCollision() {
        RaycastHit2D collideLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, 
                                                      boxCollider2D.bounds.size,
                                                      0f,
                                                      Vector2.left,
                                                      .1f,
                                                      detectPlatform);
        return (collideLeft.collider != null);
    }

    private bool rightCollision() {
        RaycastHit2D collideRight = Physics2D.BoxCast(boxCollider2D.bounds.center, 
                                                      boxCollider2D.bounds.size,
                                                      0f,
                                                      Vector2.right,
                                                      .1f,
                                                      detectPlatform);
        return (collideRight.collider != null);
    }

    //--------------------------------------------------------------------------------

    /* These are 4 getter functions that other classes can easily access. */

    public static Vector2 getLocation() {
        return location;
    }

    public static int getStatusCode() {
        return statusCode;
    }

    public static int getHealth() {
    	return health;
    }

    public static int getMoney() {
        return money;
    }
    
    //--------------------------------------------------------------------------------

}
