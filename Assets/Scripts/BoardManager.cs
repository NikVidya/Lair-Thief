using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	// --~~== Prefab Settings ==~~--
	public int columns = 7;			// # of columns on the screen - Board can't be wider
	public int visibleRows = 6;		// # of rows on the screen - Board *can* extend beyond this - Limits where the player can reach
	public int rowOffset = 3;		// # of rows from the bottom to offset the board - make room for the monster
	// -----------------------------

	public float cellScale { get; private set; } // Scaling factor for cell GameObjects

	private List<Cell> board = new List<Cell>(); // The actual data for the board

	private int boardHeight = 0; // The highest y value of cells inserted into the grid

	// --~~== API Functions ==~~--

	public bool IsTraversable(int x, int y){
		// A bit of an ugly solution here. Iterate through cells on the board looking for one that's at this location
		for(int i=0; i<board.Count; i++){
			if (board[i].x == x && board[i].y == y) {
				return board [i].IsTraversable ();
			}
		}
		// Loop would have terminated if a cell was found, so
		return true; // There was no cell in the list at that location. Assume that the player can move there.
	}

	public Vector2 CellToWorld(int x, int y){
		return new Vector2 (x * cellScale, y * cellScale);
	}

	public int[] WorldToCell(Vector3 pos){
		return new int[2] { Mathf.FloorToInt (pos.x / cellScale), Mathf.FloorToInt (pos.y / cellScale) };
	}

	public void PushCellPattern(List<Cell.CellType>[] pattern){
		int heightAdd = 0;
		for (int x = 0; x < pattern.Length; x++) {
			int rowHeight = 0;
			for (int y = 0; y < pattern[x].Count; y++) {
				if (pattern[x][y] != Cell.CellType.NONE) {
					GameObject viz = Instantiate (Resources.Load (Cell.GetResourcePath (pattern [x][y]), typeof(GameObject))) as GameObject;
					viz.transform.localScale = new Vector3 (cellScale, cellScale, cellScale);
					Cell toPush = new Cell (x, boardHeight + y, pattern [x][y], viz, CellToWorld (x, boardHeight + y));
					board.Add (toPush);
				}
				rowHeight++;
			}
			if (rowHeight > heightAdd) {
				heightAdd = rowHeight;
			}
		}
		boardHeight += heightAdd;
	}

	public bool NeedsRow(int buffer){
		return boardHeight < (visibleRows + buffer);
	}

	// --~~== End API Functions ==~~--

	// --~~== START ==~~--
	public void Start(){
		// Initialize board parameters
		cellScale = (Camera.main.orthographicSize * 2.0f * Camera.main.aspect) / (float)columns; // Base the cell scale off of the horizontal resoulution of the camera, and the number of columns
		visibleRows = Mathf.CeilToInt( (Camera.main.orthographicSize * 2.0f) / cellScale) - rowOffset; // Determine how many rows are visible to the player. (Max rows visible - offset)
	}
}
