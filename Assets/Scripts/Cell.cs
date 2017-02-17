using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
	public int x;
	public int y;
    
	public enum CellType {
		NONE, // Tiles of this type shouldn't be created. This is for specifying that in patterns
		ROCK,
        BOOST,
        FIST,
        GOLD,
        TRAP
	}

	public CellType type;
	public GameObject viz;

	public Cell(int x, int y, CellType type){
		this.x = x;
		this.y = y;
		this.type = type;
	}

	// Cell position x and y. Type of cell
	public Cell(int x, int y, CellType type, GameObject viz, Vector2 worldPos){
		this.type = type;
		this.viz = viz;
		UpdateCellPos (x, y, worldPos);
	}

	public void UpdateCellPos(int x, int y, Vector2 worldPos){
		this.x = x;
		this.y = y;
		if (viz != null) {
			viz.transform.position = worldPos;
		}
	}

	public bool IsTraversable(){
        return type == CellType.NONE || type == CellType.BOOST || type == CellType.FIST || type == CellType.GOLD || type == CellType.TRAP;
	}

	public static string GetResourcePath(CellType type) {
		switch (type) {
		    case CellType.ROCK:
                    return Constants.Board.ROCK_PREFAB_PATH;
            case CellType.BOOST:
                    return Constants.Board.BOOST_POWERUP_PREFAB_PATH;
            case CellType.FIST:
                return Constants.Board.BREAKER_POWERUP_PREFAB_PATH;

		}
		return "";
	}
}
