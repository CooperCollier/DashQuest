using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimate : MonoBehaviour {

    //--------------------------------------------------------------------------------

    private Animator animator;
    public Bullet bullet;
    public Orb orb;

    //--------------------------------------------------------------------------------

    void Start() {
    	animator = GetComponent<Animator>();
    	animator.SetBool("bulletStatus", true);
    	animator.SetBool("orbStatus", true);
    }

    //--------------------------------------------------------------------------------

    void DespawnBullet() {
    	animator.SetBool("bulletStatus", false);
    }

    void DespawnOrb() {
    	animator.SetBool("orbStatus", false);
    }

    //--------------------------------------------------------------------------------

}