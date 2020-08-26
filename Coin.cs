using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    //--------------------------------------------------------------------------------

    public Vector2 location;

    //--------------------------------------------------------------------------------

    void Start() {
        transform.position = location;
    }

    //--------------------------------------------------------------------------------

    void Update() {
        
    }

    //--------------------------------------------------------------------------------

}
