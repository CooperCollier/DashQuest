using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    //--------------------------------------------------------------------------------

    public float smoothSpeed = 0.125f;

    //--------------------------------------------------------------------------------

    void LateUpdate() {
    	Vector2 target = Player.getLocation();
    	transform.position = new Vector3(target.x, target.y, -10);
    }

    //--------------------------------------------------------------------------------

}
