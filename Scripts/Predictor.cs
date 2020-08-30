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

	[SerializeField]
	LayerMask detectPlatform;

	public Orb orb;

	int totalTicks = 0;

	//--------------------------------------------------------------------------------

    public override void SpecificUpdate() {

    	/* Set the anchor at the beginning. */
    	if (totalTicks == 0) {
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
				direction = true;
			} else {
				direction = false;
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

		/* Move. */
		rigidbody2D.velocity = (move - currentLocation).normalized * speed;
		totalTicks += 1;

		/* Give my direction to the animator. */
		gameObject.SendMessage("ReadDirection", direction);

    }

    public void Shoot() {
    	Orb thisOrb = Instantiate(orb);
		thisOrb.start = currentLocation;
		thisOrb.destination = Camera.main.WorldToScreenPoint(Player.getLocation());
    }

    //--------------------------------------------------------------------------------

    /* This function check if the random direction we chose will push the predictor
     * into a wall. If so, we reverse the direction. */
    public Vector2 checkWall(Vector2 vector) {
    	RaycastHit2D checkWall = Physics2D.Raycast(currentLocation,
    											   (vector + anchor) - currentLocation,
    											   0.5f,
    											   detectPlatform);
    	if (checkWall.collider != null) {
    		return -1 * vector;
    	} else {
    		return vector;
    	}
    }

    //--------------------------------------------------------------------------------

    public bool getDirection() {
    	return direction;
    }

    //--------------------------------------------------------------------------------

}
