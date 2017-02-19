using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
