using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : Agent {

	public BoardManager board;
    public PlayerAgent player;
    private bool playerTrapped = false;
    public int dangerDistance = 3; // The distance from the enemy to be considered endangered

    protected override void OnTurnStart(){
        if ((player.inDanger && Random.value < 0.5) || playerTrapped)
        {
            board.AdvanceBoard(2);
            playerTrapped = false;
        }
        else {
            board.AdvanceBoard(1);
        }
	}

	protected override void OnTurnUpdate(){

	}

	protected override void OnTurnEnd(){

	}
    public void TrapMove()
    {
        playerTrapped = true;
    }
}
