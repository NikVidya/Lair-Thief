using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Submit") != 0)
        {
            //move the player to the x value of the target, then the y value
            //while (transform.position.x != target.position.x)
            //{
                //transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, transform.position.y), Time.deltaTime * 1);
				transform.position = Vector2.Lerp(transform.position, new Vector2(target.transform.position.x, target.transform.position.y), Time.deltaTime);
			//}
        }
    }
}
