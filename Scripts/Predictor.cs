using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predictor : Enemy {

	//--------------------------------------------------------------------------------

	/* Direction = true means facing right. Direction = false means facing left. */
	public bool direction;

	/* Anchor is a location that the predictor tries to stay close to. This anchor
	 * can change over time. */
	private Vector2 anchor;

	/* Where the predictor is trying to move to. */
	private Vector2 move;

	public Orb orb;

	//--------------------------------------------------------------------------------

	/* Summary of predictor AI:
	 * Always stay close to an 'anchor' location. Every 300 frames, shoot a projectile
	 * at the player and move in a random direction. Avoid sticking to walls. */

    public override void SpecificUpdate() {

    	/* Set the anchor at the beginning. For some reason, the totalTicks variable
    	 * appears to start after zero. */
    	if (totalTicks < 5) {
    		anchor = transform.position;
    	}

    	/* Find the player location. PlayerDist is a vector pointing from the predictor
    	 * to the player. */
        playerLocation = (Vector2) Player.getLocation();
		currentLocation = (Vector2) transform.position;
		Vector2 playerDist = playerLocation - currentLocation;

		/* Check if the player is close enough to provoke the predictor. Also
		 * check if the player is on the left or right of the predictor. */
		if (playerDist.magnitude < aggroRadius) {
			if (playerDist.x > 0) {
				gameObject.SendMessage("PredictorRight");
			} else {
				gameObject.SendMessage("PredictorLeft");
			}

			/* Shoot a projectile at the player on a regular basis, and move around. */
			if (totalTicks % 300 == 0) {
				Shoot();
				Vector2 random = new Vector2(Random.Range(0, 10),
										Random.Range(0, 10));
				random = checkWall(random);
				move = random + anchor;
			}

		}

		/* Move around. */
		rigidbody2D.velocity = (move - currentLocation).normalized * speed;

    }

    /* Shoot an orb at the player. */
    public void Shoot() {
    	Orb thisOrb = Instantiate(orb);
		thisOrb.start = currentLocation;
		thisOrb.destination = Camera.main.WorldToScreenPoint(Player.getLocation());
    }

    //--------------------------------------------------------------------------------

    /* This function check if the random direction we chose will push the predictor
     * toward a wall. If so, we reverse the direction. This is just to minimize the 
     * time of the predictor sitting in a corner. */
    public Vector2 checkWall(Vector2 vector) {
    	RaycastHit2D checkWall = Physics2D.Raycast(currentLocation,
    											   (vector + anchor) - currentLocation,
    											   1f,
    											   detectPlatform);
    	if (checkWall.collider != null) {
    		return -1 * vector;
    	} else {
    		return vector;
    	}
    }

    //--------------------------------------------------------------------------------

}
