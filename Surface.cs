using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {

    //--------------------------------------------------------------------------------

    void Start() {
        
    }

    //--------------------------------------------------------------------------------

    void Update() {
        
    }

    //--------------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bullet") {
        	//Destroy(other.gameObject);
        }
    }

    //--------------------------------------------------------------------------------

}
