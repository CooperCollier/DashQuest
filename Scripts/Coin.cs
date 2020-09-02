using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour {

    //--------------------------------------------------------------------------------

    public Vector2 location;

    public Rigidbody2D rigidbody2D;

    //--------------------------------------------------------------------------------

    void Start() {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    	Vector2 random = new Vector2(UnityEngine.Random.Range(-2, 2),
    								 UnityEngine.Random.Range(0, 7));
        transform.position = location;
        rigidbody2D.velocity = random;
    }

    //--------------------------------------------------------------------------------

}
