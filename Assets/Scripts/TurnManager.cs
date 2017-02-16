using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	public Agent[] gameAgents;
	public int activeAgentIndex = 0;

	private bool firstTurn = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (firstTurn) {
			firstTurn = false;
			gameAgents [activeAgentIndex].TurnStart ();
		}

		if (gameAgents [activeAgentIndex].isTurnOver) {
			activeAgentIndex++;
			if (activeAgentIndex >= gameAgents.Length) {
				activeAgentIndex = 0;
			}
			gameAgents [activeAgentIndex].TurnStart ();
		}
	}

	public void EndCurrentTurn(){
		gameAgents [activeAgentIndex].TurnEnd ();
	}
}
