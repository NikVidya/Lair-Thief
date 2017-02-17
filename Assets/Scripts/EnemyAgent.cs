using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : Agent {

	public BoardManager board;
    public PlayerAgent player;

    protected override void OnTurnStart(){
        if (player.IsEndangered(player.danger) && Random.value < 0.5)
        {
            board.AdvanceBoard(2);
        }
        else {
            board.AdvanceBoard(1);
        }
	}

	protected override void OnTurnUpdate(){

	}

	protected override void OnTurnEnd(){

	}
}
