using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	//--------------------------------------------------------------------------------

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

	public int health;

	public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public Coin coin;

	public Vector2 playerLocation;
	public Vector2 currentLocation;

    //--------------------------------------------------------------------------------

    void Start() {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        health = maxHealth;
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	if (health <= 0) {
    		Destroy(this.gameObject);
            for (int i = 0; i < reward; i++) {
                Coin thisCoin = Instantiate(coin);
                thisCoin.location = currentLocation;
            }
    	} else {
            SpecificUpdate();
    	}
    }

    //--------------------------------------------------------------------------------

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
        	collision.gameObject.SendMessage("TakeDamage", attack);
        }
    }

    //--------------------------------------------------------------------------------

    public void TakeDamage(int damage) {
        if (health <= 0) {
            return;
        } else {
            health -= damage;
        }
    }

    //--------------------------------------------------------------------------------

    public abstract void SpecificUpdate();

    //--------------------------------------------------------------------------------

}
