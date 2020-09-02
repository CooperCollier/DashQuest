using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour {

	//--------------------------------------------------------------------------------

    /* IMPORTANT!!! startX must ALWAYS be smaller than endX, and startY must ALWAYS
     * be smaller than endY */

    /* Adjustable traits for each moving box. */

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

    /* As the moving box osscilates between the start & end points,
     * the 'arrived' variable osscilates between false and true. 
     * arrived is true means the box is going from end to start.
     * arrived is false means the box is going from start to end. */
	public Vector2 start;
	public Vector2 end;
	public bool arrived;

    /* Various utility objects. */
    public GameObject playerObj;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rigidbody2D;

    /* Player layermask. */
    [SerializeField]
    LayerMask detectPlayer;

    //--------------------------------------------------------------------------------

    void Start() {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        start = new Vector2(startX, startY);
        end = new Vector2(endX, endY);
        arrived = false;
        transform.position = start;
        playerObj = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
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
            if (startX != endX && checkPlayer()) {
                playerObj.SendMessage("Translate", move * speed);
            }
    	} else {
    		transform.Translate(-move * speed * Time.deltaTime);
            if (startX != endX && checkPlayer()) {
                playerObj.SendMessage("Translate", -move * speed);
            }
    	}
        
    }

    //--------------------------------------------------------------------------------

    /* This function makes the player move with the box if they're standing on top
     * of it, so the player doesn't just slide off. */
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
