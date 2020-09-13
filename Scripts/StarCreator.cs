using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCreator : MonoBehaviour {

    //--------------------------------------------------------------------------------

    public Star star;

    //--------------------------------------------------------------------------------

    void Update() {
        int random = Random.Range(0, 15);
        if (random == 5 && Time.timeScale == 1f) {
        	int height = Random.Range(-Screen.height, 2 * Screen.height);
        	Star thisStar = Instantiate(star);
        	Vector3 start = new Vector3(Screen.width, height, 0);
        	thisStar.transform.position = Camera.main.ScreenToWorldPoint(start);
        }
    }

    //--------------------------------------------------------------------------------
    
}
