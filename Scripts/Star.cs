using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	//--------------------------------------------------------------------------------

	void Start() {
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}

    //--------------------------------------------------------------------------------

    void Update() {
        transform.Translate(new Vector2(-8, 0) * Time.deltaTime);
        if (transform.position.x < (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x)) {
        	Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------------
    
}
