using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBody : MonoBehaviour
{

	public Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    	Vector2 random = new Vector2(UnityEngine.Random.Range(-5, 5),
    								 UnityEngine.Random.Range(3, 8));
        rigidbody2D.velocity = random;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
