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

	private Vector2 move;

	/* TooCloseRadius is the distance such that, if the player gets closer than
	 * this, the predictor will retreat. */
	[SerializeField]
	private int tooCloseRadius;

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
		 * check if the player is on the elft or right of the predictor. */
		if (playerDist.magnitude < aggroRadius) {
			if (playerDist.x > 0) {
				direction = true;
			} else {
				direction = false;
			}

			/* shoot a projectile at the player on a regular basis, and move around. */
			if (totalTicks % 400 == 0) {
				Instantiate(orb);
				orb.destination = Player.getLocation();
				orb.start = currentLocation;
				Vector2 random = new Vector2(Random.Range(0, tooCloseRadius),
										Random.Range(0, tooCloseRadius));
				move = random + anchor;
				Debug.Log(move);
			}

		}

		/* Update the anchor if the player gets too close. */
		if (playerDist.magnitude < aggroRadius && playerDist.magnitude < tooCloseRadius) {
			anchor = new Vector2(Random.Range(anchor.x - 4, anchor.x + 4),
								Random.Range(anchor.y - 4, anchor.y + 4));
		}

		/* Move. */
		rigidbody2D.velocity = move.normalized * speed;
		totalTicks += 1;

		/* Give my direction to the animator. */
		gameObject.SendMessage("ReadDirection", direction);

    }

    //--------------------------------------------------------------------------------

    public bool getDirection() {
    	return direction;
    }

    //--------------------------------------------------------------------------------

}
