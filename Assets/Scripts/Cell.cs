using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
	public int x;
	public int y;

	[System.Flags] private enum CellFlags
	{
		INVALID 	= 0,
		IMPASSABLE 	= 1,
		PASSABLE 	= 2
		// e.g. SLOW = 3
	}
	public enum CellType {
		NONE = CellFlags.PASSABLE, // Tiles of this type shouldn't be created. This is for specifying that in patterns
		ROCK = CellFlags.IMPASSABLE,
        NONE_BOOST = CellFlags.PASSABLE
		// e.g. SAND = CellFlags.PASSABLE | CellFlags.SLOW
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
		return ((int)type & (int)CellFlags.PASSABLE) > 0; // Check passable bit
	}

	public static string GetResourcePath(CellType type) {
		switch (type) {
		    case CellType.ROCK:
                    return Constants.Board.ROCK_PREFAB_PATH;
            case CellType.NONE_BOOST:
                    return Constants.Board.POWERUP_PREFAB_PATH;

		}
		return "";
	}
}
