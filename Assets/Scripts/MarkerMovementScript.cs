using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMovementScript : MonoBehaviour {
    public float moveDist;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            transform.Translate(0, 1, 0);
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            transform.Translate(0, -1, 0);
        }
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            transform.Translate(-1, 0, 0);
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            transform.Translate(1, 0, 0);
        }
    }
}
