using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour {

	//--------------------------------------------------------------------------------

    /* IMPORTANT: startX must ALWAYS be smaller than endX, and startY must ALWAYS
     be smaller than endY */

	public Vector2 start;
	public Vector2 end;
	public bool arrived;

	[SerializeField]
	public int speed;

	[SerializeField]
	public int startX;

	[SerializeField]
	public int startY;

	[SerializeField]
	public int endX;

	[SerializeField]
	public int endY;

    //--------------------------------------------------------------------------------

    void Start() {
        
        start = new Vector2(startX, startY);
        end = new Vector2(endX, endY);
        arrived = false;
        transform.position = start;

    }

    //--------------------------------------------------------------------------------

    void Update() {

    	Vector2 position = (Vector2) transform.position;

    	if ((position.x - end.x) > 0 || (position.y - end.y) > 0) {
    		arrived = true;
    	} else if ((position.x - start.x) < 0 || (position.y - start.y) < 0) {
    		arrived = false;
    	}

    	Vector2 move = (end - start).normalized;

    	if (!arrived) {
    		transform.Translate(move * speed * Time.deltaTime);
    	} else {
    		transform.Translate(-move * speed * Time.deltaTime);
    	}
        
    }

    //--------------------------------------------------------------------------------

}
