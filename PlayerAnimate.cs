using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour {

    //--------------------------------------------------------------------------------

    private Animator animator;
    public int statusCode;

    //--------------------------------------------------------------------------------

    void Start() {
    	animator = GetComponent<Animator>();
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	statusCode = Player.getStatusCode();
    	animator.SetInteger("statusCode", statusCode);
    }

    //--------------------------------------------------------------------------------

}
