using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	//--------------------------------------------------------------------------------

    /* Adjustable traits for each enemy. */

    [SerializeField]
    public int speed;

    [SerializeField]
    public int aggroRadius;

    [SerializeField]
    public int maxHealth;

    [SerializeField]
    public int reward;

    [SerializeField]
    public int attack;

    [SerializeField]
    public string name;

    /* Some utility objects. */
	public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public Coin coin;

    /* The player's location & the enemy's location. */
	public Vector2 playerLocation;
	public Vector2 currentLocation;

    /* The enemy's health. */
    public int health;

    /* Used for raycasting toward walls and floor. */
    [SerializeField]
    public LayerMask detectPlatform;

    /* When the enemy is damaged, it briefly flashes red. It stays red for as
     * long as redFrames > 0. */
    public int redFrames = 0;

    /* Counts frames so enemies can cycle through things on certain time intervals. */
    public int totalTicks = 0;

    //--------------------------------------------------------------------------------

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        health = maxHealth;
    }

    //--------------------------------------------------------------------------------

    /* SpecificUpdate covers enemy-specific behavior. The function is instantiated in
     * each individual enemy's script. This general update() only covers things that every
     * enemy does. */
    void Update() {
        totalTicks += 1;
    	if (health <= 0) {
            StartCoroutine(Despawn());
    	} else {
            if (redFrames == 0) {
                spriteRenderer.color = Color.white;
            } else {
                redFrames -= 1;
            }
            SpecificUpdate();
    	}
    }

    public abstract void SpecificUpdate();

    //--------------------------------------------------------------------------------

    /* Handle collisions with speicfic game objects. */

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
        	collision.gameObject.SendMessage("TakeDamage", attack);
            AvoidClip((Vector2) collision.gameObject.transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("TakeDamage", attack);
            AvoidClip((Vector2) collision.gameObject.transform.position);
        }
    }

    //--------------------------------------------------------------------------------

    /* This runs when the enemy takes damage. The enemy briefly
     * flashes red while taking damage. */
    public void TakeDamage(int damage) {
        if (health <= 0) {
            return;
        } else {
            health -= damage;
            redFrames = 15;
            spriteRenderer.color = Color.red;
        }
    }

    //--------------------------------------------------------------------------------

    /* Despawn the enemy. This is an IEnumerator because there needs to be a time stall
     * while the death animaiton plays. */
    IEnumerator Despawn() {
        speed = 0;
        if (name == "watcher") {
            gameObject.SendMessage("DespawnWatcher");
            yield return new WaitForSeconds(0.4f);
        } else if (name == "predictor") {
            gameObject.SendMessage("DespawnPredictor");
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < reward; i++) {
            Coin thisCoin = Instantiate(coin);
            thisCoin.location = currentLocation;
        }
        Destroy(gameObject);
    }

    //--------------------------------------------------------------------------------

    /* This is meant to avoid a bug where the player can push enemies through
     * walls by dashing into them. This funciton moves the enemy slightly away from the player
     * if they are dashing into the enemy. */
    public void AvoidClip(Vector2 player) {
        RaycastHit2D CheckWall = Physics2D.Raycast(currentLocation,
                                                  currentLocation - player,
                                                  3f,
                                                  detectPlatform);
        if (CheckWall.collider != null) {
            transform.Translate((player - currentLocation).normalized * 2);
        }
    }

    //--------------------------------------------------------------------------------

}
