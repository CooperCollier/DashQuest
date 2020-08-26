using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	//--------------------------------------------------------------------------------

	[SerializeField]
	int speed;

	[SerializeField]
	int aggroRadius;

	[SerializeField]
	int maxHealth;

    [SerializeField]
    int reward;

	int health;

	private Rigidbody2D rigidbody2D;
    public Coin coin;

	Vector2 playerLocation;
	Vector2 currentLocation;

    //--------------------------------------------------------------------------------

    void Start() {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	if (health == 0) {
    		Destroy(this.gameObject);
            for (int i = 0; i < reward; i++) {
                Instantiate(coin);
                coin.location = currentLocation;
            }
    	} else {
            //SpecificUpdate();
            /*
			playerLocation = (Vector2) Player.getLocation();
			currentLocation = (Vector2) transform.position;
			Vector2 move = playerLocation - currentLocation;
			if (move.magnitude < aggroRadius) {
				rigidbody2D.velocity = move.normalized * speed;
			} else {
				rigidbody2D.velocity = Vector2.zero;
			}*/
    	}
    }

    //--------------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bullet") {
        	health -= 1;
        	Destroy(other.gameObject);
        }
    }

    //--------------------------------------------------------------------------------

}
