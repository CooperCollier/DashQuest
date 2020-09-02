using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimate : MonoBehaviour {

    //--------------------------------------------------------------------------------

    private Animator animator;

    public Watcher watcher;
    public Predictor predictor;

    //--------------------------------------------------------------------------------

    void Start() {
    	animator = GetComponent<Animator>();
    	animator.SetBool("watcherAlive", true);
    	animator.SetBool("predictorAlive", true);
    }

    //--------------------------------------------------------------------------------

    void DespawnWatcher() {
    	animator.SetBool("watcherAlive", false);
    }

    //--------------------------------------------------------------------------------

    void PredictorLeft() {
    	animator.SetBool("predictorDirection", false);
    }

    void PredictorRight() {
    	animator.SetBool("predictorDirection", true);
    }

    void DespawnPredictor() {
    	animator.SetBool("predictorAlive", false);
    }

    //--------------------------------------------------------------------------------

}
