using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerup : Powerup {
    void Start()
    {

    }
    void Update () {
		if (board.GetCell(player.cellPosX,player.cellPosY).type == Cell.CellType.BOOST)
        {
            Debug.Log("Boost powerup acquired");
            OnTouch();
        }
	}
    void OnTouch()
    {
        player.EnableBoosting(); // The player has obtained a boost item

        // Remove the cell from the board
        board.ChangeCell(player.cellPosX, player.cellPosY, Cell.CellType.NONE);
    }

    public void Activate()
    {
        player.StartBoosting();
    }
}
