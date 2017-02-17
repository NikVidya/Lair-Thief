using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerup : Powerup, BoardPiece {
    void Start()
    {

    }
    void Update () {
		if (board.GetCell(player.cellPosX,player.cellPosY).type == Cell.CellType.BOOST)
        {
            Debug.Log("The powerup was got");
            OnTouch();
        }
	}
    void OnTouch()
    {
        // make sure to change this to add to inventory
        player.boosted = 5;
        // this powerup has to disappear
        board.ChangeCell(player.cellPosX, player.cellPosY, Cell.CellType.NONE);
        
    }

    public void HandleBoardAdvance(int distance)
    {
        throw new NotImplementedException();
    }
}
