using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimate : MonoBehaviour {

    //--------------------------------------------------------------------------------

    private Animator animator;
    public bool direction;
    public bool alive;
    public Enemy enemy;

    //--------------------------------------------------------------------------------

    void Start() {
    	animator = GetComponent<Animator>();
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	animator.SetBool("direction", direction);
    	animator.SetBool("alive", alive);
    }

    //--------------------------------------------------------------------------------

    void ReadDirection(bool newDirection) {
    	direction = newDirection;
    }

    void ReadAlive(bool newAlive) {
    	alive = newAlive;
    }

    //--------------------------------------------------------------------------------

}
