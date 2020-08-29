using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : Enemy {

	//--------------------------------------------------------------------------------

    public override void SpecificUpdate() {
        playerLocation = (Vector2) Player.getLocation();
		currentLocation = (Vector2) transform.position;
		Vector2 move = playerLocation - currentLocation;
		if (move.magnitude < aggroRadius) {
			rigidbody2D.velocity = move.normalized * speed;
		} else {
			rigidbody2D.velocity = Vector2.zero;
		}
    }

    //--------------------------------------------------------------------------------

}