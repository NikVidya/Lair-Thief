using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : Agent {

	public BoardManager board;

	protected override void OnTurnStart(){
		board.AdvanceBoard(1);
	}

	protected override void OnTurnUpdate(){

	}

	protected override void OnTurnEnd(){

	}
}
