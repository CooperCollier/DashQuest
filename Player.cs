using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//--------------------------------------------------------------------------------

	/* 7 Adjustable character attributes, set for the entire game. */

	[SerializeField]
    float startX;

    [SerializeField]
    float startY;

    [SerializeField]
    int startingHealth;

    [SerializeField]
    int maxDashTime;

	[SerializeField]
    float moveSpeed;

    [SerializeField]
    float jumpSpeed;

    [SerializeField]
    float dashSpeed;

    /* Used during raycasting & boxcasting towards platforms. A platform is anything
     * that the player can stand on and jump from. */
    [SerializeField]
    LayerMask detectPlatform;

    /* Objects for utility purposes. */
    public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public Bullet bullet;
    public HealthBar healthBar;

    /* Two fields used during the dash. DashTime is reset to 0 upon touching a platform.
     * Once DashTime > MaxDashTime, the dash ends. */
    Vector2 dashVector;
    int dashTime;

    /* Location is the player's current location. */
    public static Vector2 location;

    /* Status is an int that represents what the player is currently doing.
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
    public static int statusCode;

    /* How much health the player has left. */
    public static int health;

    /* How many coins the player has */
    public static int money;

    //--------------------------------------------------------------------------------

    void Start() {

    	/* Just initialize important variables. */
    	rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        transform.position = new Vector2(startX, startY);
        health = startingHealth;
        statusCode = 0;
        money = 0;

    }

    //--------------------------------------------------------------------------------

    void Update() {

    	/* Update the location variable & status code. */
        location = (Vector2) transform.position;
 		setStatusCode();

        /* Press W to jump if the player is on the ground. */
    	if (Input.GetKeyDown(KeyCode.W)) {
    		if (isGrounded()) {
    			rigidbody2D.velocity = Vector2.up * jumpSpeed;
    		}
    	}

    	/* Click the mouse to shoot if the player is on the ground. 
    	 * Click the mouse to dash if the player is in the air. */
    	if (Input.GetMouseButtonDown(0)) {
    		if (isGrounded()) {
    			Shoot(Input.mousePosition);
    		} else if (statusCode == 3 || statusCode == 4) {
    			startDash(Input.mousePosition);
    		}
    	}

    	/* Get keyboard input to move, unless the player is dashing. */
    	if (statusCode >= 5 && statusCode <= 8) {
    		continueDash();
    	} else {
    		Move();
    	}

    	/* The dash time resets to zero upon standing on a platform. */
    	if (isGrounded()) {
    		dashTime = 0;
    	}

    	/* Update the health bar. */
    	healthBar.setHealth(health);

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

        } else if (!isGrounded() && (xDirection() >= 0) && (dashTime == 0)) { // Jumping Right
        	statusCode = 4;
        } else if (!isGrounded() && (xDirection() == -1) && (dashTime == 0)) { // Jumping Left
        	statusCode = 3;

        } else if (!isGrounded() && (xDirection() >= 0) && (dashTime <= maxDashTime) && (yDirection() >= 0)) { // Dashing Right Up
        	statusCode = 7;
        } else if (!isGrounded() && (xDirection() == -1) && (dashTime <= maxDashTime) && (yDirection() >= 0)) { // Dashing Left Up
        	statusCode = 5;
        } else if (!isGrounded() && (xDirection() >= 0) && (dashTime <= maxDashTime) && (yDirection() == -1)) { // Dashing Right Down
        	statusCode = 8;
        } else if (!isGrounded() && (xDirection() == -1) && (dashTime <= maxDashTime) && (yDirection() == -1)) { // Dashing Left Down
        	statusCode = 6;

        } else if (!isGrounded() && (xDirection() >= 0) && (dashTime > maxDashTime)) { // Falling Right
        	statusCode = 10;
        } else if (!isGrounded() && (xDirection() == -1) && (dashTime > maxDashTime)) { // Falling Left
        	statusCode = 9;
        }

    }

    //--------------------------------------------------------------------------------

    /* This function is called every frame. It translates the keyboard input into
     * player movement. */

    private void Move() {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }  else if (leftCollision() && !isGrounded() && Input.GetKey(KeyCode.A)) {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        } else if (rightCollision() && !isGrounded() && Input.GetKey(KeyCode.D)) {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        } else if (Input.GetKey(KeyCode.D)) {
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        } else if (Input.GetKey(KeyCode.A)) {
            rigidbody2D.velocity = new Vector2(-moveSpeed, rigidbody2D.velocity.y);
        } else {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    //--------------------------------------------------------------------------------

    /* Handle collisions with specific game objects. */

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
        	money += 1;
        	Destroy(other.gameObject);
        } else if (other.tag == "Door") {
        	Debug.Log("You Win!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Spikes" && health > 0) {
            health -= 10;
        } else if (collision.gameObject.tag == "Enemy" && health > 0) {
            health -= 10;
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
        if (rigidbody2D.velocity.x == 0) {
        	return 0;
        } else if (rigidbody2D.velocity.x < 0) {
        	return -1;
        } else if (rigidbody2D.velocity.x > 0) {
        	return 1;
        }
        return 0;
    }

    private int yDirection() {
        if (rigidbody2D.velocity.y == 0) {
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
    	Instantiate(bullet);
    	bullet.start = transform.position;
    	bullet.destination = destination;
    }

    //--------------------------------------------------------------------------------

    /* StartDash runs at the beginning of every dash. ContinueDash runs every frame
     * as long as the dash is active. */

    private void startDash(Vector2 destination) {
    	dashTime = 1;
    	destination = Camera.main.ScreenToWorldPoint(destination);
    	dashVector = ((Vector2) destination) - ((Vector2) transform.position);
    	dashVector = dashVector.normalized;
    }

    private void continueDash() {
    	dashTime += 1;
    	rigidbody2D.velocity = dashVector * dashSpeed;
        if (dashTime >= maxDashTime) {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    //--------------------------------------------------------------------------------

    /* Detect if the player ran into a wall on their left or right. */

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

    /* These are 3 getter functions that other classes can easily access. */

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
