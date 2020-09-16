using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muncher : Enemy {

    //--------------------------------------------------------------------------------

	/* The muncher walks from xStart to xEnd, then from xEnd to xStart, and 
	 * constantly repeats. xStart MUST be less than xEnd. Direction is true
	 * if the muncher is moving to the right. */

	[SerializeField]
	int xStart;

	[SerializeField]
	int xEnd;

    bool direction;

    //--------------------------------------------------------------------------------

    public override void SpecificUpdate() {

    	if (totalTicks < 5) {
    		transform.position = new Vector2(xStart, currentLocation.y);
    	}

    	/* Move from xStart to xEnd, and back again. */
    	Vector2 move;
        if (direction) {
        	move = (new Vector2(xEnd, currentLocation.y) - currentLocation);
        } else {
        	move = (new Vector2(xStart, currentLocation.y) - currentLocation);
        }
        rigidbody2D.velocity = move.normalized * speed;

        /* Change direction upon reaching xStart or xEnd. */
        if (currentLocation.x > xEnd) {
        	direction = false;
        	gameObject.SendMessage("MuncherLeft");
        } else if (currentLocation.x < xStart) {
        	direction = true;
        	gameObject.SendMessage("MuncherRight");
        }
    }

    //--------------------------------------------------------------------------------

}
