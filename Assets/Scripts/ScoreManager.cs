using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private Dictionary<string, int> scores;
	// Use this for initialization
	void Start () {
		
	}
    void Init()
    {
        if (scores == null)
        {
            scores = new Dictionary<string, int>();
        }
    }

    int GetScore(string player)
    {
        Init();
        if (scores.ContainsKey(player) == false)
        {
            return 0;
        }
        return scores[player];
    }

	void SetScore(string player, int value)
    {
        Init();
        scores[player] = value;
    }

    void IncrementScore(string player)
    {
        Init();
        scores[player] = scores[player] + 1;
    }
}
