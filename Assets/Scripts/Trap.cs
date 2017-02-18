using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    public PlayerAgent player;
    public BoardManager board;
    public EnemyAgent enemy;
    // Update is called once per frame
    void Update()
    {
        if (board.GetCell(player.cellPosX, player.cellPosY).type == Cell.CellType.TRAP)
        {
            Debug.Log("Player is trapped!");
            OnTouch();
        }
    }

    void OnTouch()
    {
        // makes the enemy move two instead of one
        enemy.TrapMove();
        // removes this cell
        board.ChangeCell(player.cellPosX, player.cellPosY, Cell.CellType.NONE);
    }
}
