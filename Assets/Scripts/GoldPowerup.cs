using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPowerup : Powerup {

    public float scorePerPickup = 10; // Remember, this is multiplied by the score multiplier

	// Update is called once per frame
	void Update () {
		if( board.GetCell(player.cellPosX, player.cellPosY).type == Cell.CellType.GOLD)
        {
            board.ChangeCell(player.cellPosX, player.cellPosY, Cell.CellType.NONE);
            OnTouch();
        }
	}

    void OnTouch()
    {
        Debug.Log("Give player score!");
        player.GainScore(scorePerPickup);
    }
}
