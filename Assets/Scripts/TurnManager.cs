using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	public Agent[] gameAgents;
	public int activeAgentIndex = 0;

	// Use this for initialization
	void Start () {
		// Start the first turn
		gameAgents[activeAgentIndex].TurnStart();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameAgents [activeAgentIndex].isTurnOver) {
			activeAgentIndex++;
			if (activeAgentIndex >= gameAgents.Length) {
				activeAgentIndex = 0;
			}
			gameAgents [activeAgentIndex].TurnStart ();
		}
	}
}
