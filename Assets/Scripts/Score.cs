using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	
	public float score; // Final Score
    public float scoreMulti;
	public Text scoreText; // The UI element that display's the player's score
    public float scoreAdvance;
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);
	}
	
    public void AdvanceMultiplier()
    {
        score += scoreAdvance;
    }

    public void GainScore(float flatAmount)
    {
        score += flatAmount * scoreMulti;
        scoreText.text = string.Format("Score: {0}", Mathf.RoundToInt(score));
    }
	
}
