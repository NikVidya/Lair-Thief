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
		ROCK = CellFlags.IMPASSABLE
		// e.g. SAND = CellFlags.PASSABLE | CellFlags.SLOW
	}

	public CellType type;
	public GameObject viz;

	// Cell position x and y. Type of cell
	public Cell(int x, int y, CellType type, GameObject viz, Vector2 worldPos){
		this.type = type;
		this.viz = viz;
		updateCellPos (x, y, worldPos);
	}

	public void updateCellPos(int x, int y, Vector2 worldPos){
		this.x = x;
		this.y = y;
		viz.transform.position = worldPos;
	}

	public bool IsTraversable(){
		return ((int)type & (int)CellFlags.PASSABLE) > 0; // Check passable bit
	}

	public static string GetResourcePath(CellType type) {
		switch (type) {
		case CellType.ROCK:
			return Constants.Board.ROCK_PREFAB_PATH;
		}
		return "";
	}
}
