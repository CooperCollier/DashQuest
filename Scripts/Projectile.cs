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

    [SerializeField]
    string name;

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
