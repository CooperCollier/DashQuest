using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //--------------------------------------------------------------------------------
    
    /* Adjustable traits for each projectile. */

    [SerializeField]
    int speed;

    [SerializeField]
    bool friendly;

    [SerializeField]
    bool gravity;

    [SerializeField]
    int attack;

    [SerializeField]
    int range;

    [SerializeField]
    string name;

    /* Start & Destination MUST be set by the entity spawning this projectile! 
     *
     * Start: where the projectile starts. 
     * Destination: where the projectile goes toward. */
    public Vector2 start;
    public Vector2 destination;

    /* the projectile's current location. */
    Vector2 location;

    /* the direction that the projectile is currently moving. */
    Vector2 move;

    /* If this is false, the projectile moves toward the destination. If this
     * is true, the projectile moves away from the destination. This is to achive
     * the effect of moving in a totally straight line that crosses the destination
     * point exactly once. */
    bool arrived;

    /* How long this projectile has existed. This is used to despawn it if it exists
     * for longer than the specified range. */
    int totalTicks = 0;

    //--------------------------------------------------------------------------------

    void Start() {
        destination = Camera.main.ScreenToWorldPoint(destination);
    	transform.position = start;
    	arrived = false;
    }

    //--------------------------------------------------------------------------------

    void Update() {
        totalTicks += 1;
        if (totalTicks > range) {
            StartCoroutine(Despawn());
        }
        location = transform.position;
    	if (location - destination == Vector2.zero) {
    		arrived = true;
    	}
    	if (!arrived) {
            move = destination - start;
    	} else {
            move = start - destination;
    	}
        transform.Translate(move.normalized * speed * Time.deltaTime);
    }

    //--------------------------------------------------------------------------------

    /* Handle collisions with certain gameObjects. Most collisions will just destroy
     * the projectile. */
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            return;
        }
    	if (friendly) {
        	if (other.tag != "Player") {
                StartCoroutine(Despawn());
        	}
        	if (other.tag == "Enemy") {
        		other.gameObject.SendMessage("TakeDamage", attack);
        	}
        } else {
        	if (other.tag != "Enemy") {
                StartCoroutine(Despawn());
        	}
        	if (other.tag == "Player") {
        		other.gameObject.SendMessage("TakeDamage", attack);
        	}
        }
    }

    //--------------------------------------------------------------------------------

    /* Despawns the bullet. This is an IEnumerator because the programs needs to pause briefly
     * so that projectile despawn animation has time ot play. */
    IEnumerator Despawn() {
        speed = 0;
        if (name == "bullet") {
            gameObject.SendMessage("DespawnBullet");
            yield return new WaitForSeconds(0.1f);
        } else if (name == "orb") {
            gameObject.SendMessage("DespawnOrb");
            yield return new WaitForSeconds(0.07f);
        }
        Destroy(gameObject);
    }

    //--------------------------------------------------------------------------------

}
