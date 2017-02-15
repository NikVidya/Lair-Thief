using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour {

	public bool isTurnOver = true;

	public void TurnStart (){
		if (isTurnOver) {
			isTurnOver = false;
			OnTurnStart ();
		}
	}

	public void TurnEnd (){
		if (!isTurnOver) {
			isTurnOver = true;
			OnTurnEnd ();
		}
	}

	public void Update(){
		if (!isTurnOver) {
			OnTurnUpdate ();
		}
	}

	protected virtual void OnTurnUpdate() {}
	protected virtual void OnTurnStart () {}
	protected virtual void OnTurnEnd () {}
}
