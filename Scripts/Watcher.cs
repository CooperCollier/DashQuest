using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : Enemy {

	//--------------------------------------------------------------------------------

	[SerializeField]
  LayerMask detectEnemy;

  int totalTicks = 0;

	//--------------------------------------------------------------------------------

  public override void SpecificUpdate() {
      playerLocation = (Vector2) Player.getLocation();
		  currentLocation = (Vector2) transform.position;
		  Vector2 move = playerLocation - currentLocation;
		  Vector2 offset = enemyAvoid();
		  if (move.magnitude < aggroRadius) {
          if (totalTicks % 2000 > 500 && totalTicks % 2000 < 1000) {
             rigidbody2D.velocity = new Vector2((move.normalized.x + offset.normalized.x) * speed, 0);
          } else if (totalTicks % 2000 > 1500 && totalTicks % 2000 < 2000) {
             rigidbody2D.velocity = new Vector2(0, (move.normalized.y + offset.normalized.y) * speed);
          } else {
			       rigidbody2D.velocity = (move.normalized + offset.normalized) * speed;
          }
		  } else {
			    rigidbody2D.velocity = Vector2.zero;
		  }
      totalTicks += 1;
  }

    //--------------------------------------------------------------------------------

    Vector2 enemyAvoid() {
    	RaycastHit2D enemyAvoid = Physics2D.BoxCast(boxCollider2D.bounds.center,
                                              boxCollider2D.bounds.size * 5,
                                              0f,
                                              Vector2.up,
                                              1f,
                                              detectEnemy);
    	Vector2 offset = Vector2.zero;
    	if (enemyAvoid.collider != null) {
    		offset = (currentLocation - (Vector2) enemyAvoid.collider.gameObject.transform.position);
    	}
    	return offset;
    }

    //--------------------------------------------------------------------------------

}