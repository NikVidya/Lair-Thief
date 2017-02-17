using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerPowerup : Powerup {
	
	// Update is called once per frame
	void Update () {
        if (board.GetCell(player.cellPosX, player.cellPosY).type == Cell.CellType.FIST)
        {
            Debug.Log("Breaker powerup acquired");
            OnTouch();
        }
    }
    
    void OnTouch()
    {
        player.EnableBreaking();
        // removes this cell
        board.ChangeCell(player.cellPosX, player.cellPosY, Cell.CellType.NONE);
    }

    public void Activate()
    {
        player.BreakBlock();
    }
}
