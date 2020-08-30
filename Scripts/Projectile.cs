using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //--------------------------------------------------------------------------------
    
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

    /* Start & Destination MUST be set by the entity spawning this projectile! */
    public Vector2 start;
    public Vector2 destination;

    Vector2 location;
    Vector2 move;

    bool arrived;

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
            Destroy(gameObject);
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            return;
        }
    	if (friendly) {
        	if (other.tag != "Player") {
            	Destroy(gameObject);
        	}
        	if (other.tag == "Enemy") {
        		other.gameObject.SendMessage("TakeDamage", attack);
        	}
        } else {
        	if (other.tag != "Enemy") {
            	Destroy(gameObject);
        	}
        	if (other.tag == "Player") {
        		other.gameObject.SendMessage("TakeDamage", attack);
        	}
        }
    }

    //--------------------------------------------------------------------------------

}
