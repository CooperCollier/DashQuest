using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingBox : MonoBehaviour {

    //--------------------------------------------------------------------------------

	/* Once the player steps on the box, stable = false, and the timeToVanish
	 * starts counting down. */
	bool stable = true;

	/* TimeToVanish is the number of frames until this block vanishes. */
    int timeToVanish = 200;

    /* timeGone is the number of frames that the block stays vanished for. */
    int timeGone = 800;

    /* TimeToRespawn is the number of frames until this block reappears. */
	int timeToRespawn = 50;

	/* Various utility objects. */
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer sprite;

    /* Player layermask. */
    [SerializeField]
    LayerMask detectPlayer;

    //--------------------------------------------------------------------------------

    void Start() {
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //--------------------------------------------------------------------------------

    void Update() {

    	/* If the player is standing on this block, start the vanishing process. */
        if (checkPlayer()) {
        	stable = false;
        }

        /* Once timeToVanish runs out, remove the collider. */
        if (timeToVanish == 0) {
        	boxCollider2D.enabled = false;
        }

        /* Decrement timetovanish, timegone, and timetorespawn, in that order. Also,
         * fade in and out when dissappearing and reappearing. */
        if (!stable) {
        	if (timeToVanish > 0) {
        		timeToVanish -= 1;
        		sprite.color = new Color(1f, 1f, 1f, (float) (timeToVanish * 0.005f));
        	} else if (timeGone > 0) {
        		timeGone -= 1;
        	} else if (timeToRespawn > 0) {
        		timeToRespawn -= 1;
        		sprite.color = new Color(1f, 1f, 1f, (float) ((50 - timeToRespawn) * 0.02f));
        	} else {
        		stable = true;
        		timeToVanish = 200;
        		timeGone = 1000;
        		timeToRespawn = 50;
        		boxCollider2D.enabled = true;
        	}
        }
    }

    //--------------------------------------------------------------------------------

    /* This function checks if the player is standing on the box. */
    bool checkPlayer() {
        RaycastHit2D checkPlayer = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                                      boxCollider2D.bounds.size,
                                                      0f,
                                                      Vector2.up,
                                                      0.3f,
                                                      detectPlayer);
        return (checkPlayer.collider != null);
    }

    //--------------------------------------------------------------------------------
}
