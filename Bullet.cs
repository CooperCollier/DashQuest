using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//--------------------------------------------------------------------------------

	[SerializeField]
	float speed;

	bool arrived;

	public Vector2 destination;
	public Vector2 location;
	public Vector2 start;
    public Vector2 move;

	//--------------------------------------------------------------------------------

    void Start() {
    	destination = Camera.main.ScreenToWorldPoint(destination);
    	transform.position = start;
    	arrived = false;
    }

    //--------------------------------------------------------------------------------

    void Update() {
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
        if (other.tag != "Player") {
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------------

}
