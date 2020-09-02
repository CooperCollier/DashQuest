using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHead : MonoBehaviour {

    //--------------------------------------------------------------------------------
    
	public Rigidbody2D rigidbody2D;

    //--------------------------------------------------------------------------------

    void Start(){
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    	Vector2 random = new Vector2(UnityEngine.Random.Range(-5, 5),
    								 UnityEngine.Random.Range(3, 8));
        rigidbody2D.velocity = random;
    }

    //--------------------------------------------------------------------------------

}
