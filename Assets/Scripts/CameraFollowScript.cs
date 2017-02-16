using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.x != target.position.x && transform.position.y != target.position.y)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10);
        }
	}
}
