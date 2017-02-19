using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	
	public static float finalScore; // Final Score
	public Text scoreText; // The UI element that display's the player's score
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = string.Format("Score: {0}", Mathf.RoundToInt(finalScore));
	}
	
}
