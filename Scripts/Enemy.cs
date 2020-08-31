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

    [SerializeField]
    public LayerMask detectPlatform;

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

    public void TakeDamage(int damage) {
        if (health <= 0) {
            return;
        } else {
            health -= damage;
        }
    }

    //--------------------------------------------------------------------------------

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

    public abstract void SpecificUpdate();

    //--------------------------------------------------------------------------------

}
