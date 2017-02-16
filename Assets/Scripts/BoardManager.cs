using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	// --~~== Prefab Settings ==~~--
	public int columns = 7;			// # of columns on the screen - Board can't be wider
	public int visibleRows = 6;		// # of rows on the screen - Board *can* extend beyond this - Limits where the player can reach
	public int rowOffset = 3;		// # of rows from the bottom to offset the board - make room for the monster

	public GameObject cam; 		// The camera to move when showing the advancement of the board

	public TurnManager turnManager; // The manager handling who's turn it is
	// -----------------------------

	public float cellScale { get; private set; } // Scaling factor for cell GameObjects

	private List<Cell> board = new List<Cell>(); // The actual data for the board
	private List<BoardPiece> pieces = new List<BoardPiece>();

	private int boardHeight = 0; // The highest y value of cells inserted into the grid

	private IEnumerator moveCoroutine = null; // The coroutine used to animate the camera

	private Vector3 camOrigin; // The original position of the camera

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

	public void AdvanceBoard(int distance){
		if (moveCoroutine != null) {
			StopCoroutine (moveCoroutine);
		}
		moveCoroutine = AdvanceCamera (distance, 1f);
		StartCoroutine (moveCoroutine);
	}

	IEnumerator AdvanceCamera(int distance, float moveTime)
	{
		float startTime = Time.time;

		Vector3 target = cam.transform.position + (Vector3)CellToWorld(0, distance);

		while ((cam.transform.position - target ).magnitude > 0.01f) {
			float curTime = Time.time;
			cam.transform.position = Vector3.Lerp (cam.transform.position, target, (curTime - startTime) / moveTime);

			yield return null; // Yield control back to the rest of the engine so it can tick
		}
		UpdateBoardAfterAdvance (distance); // Actually trigger the board to move
	}

	// Does not animate!
	public void UpdateBoardAfterAdvance(int distance){
		// Reset the camera position
		cam.transform.position = camOrigin;

		// Update all Cell positions
		for (int i = 0; i < board.Count; i++) {
			Cell cell = board [i];
			cell.UpdateCellPos (cell.x, cell.y - distance, CellToWorld (cell.x, cell.y - distance));
			// Check if the piece fell off the board
			if (cell.y < 0) {
				GameObject.DestroyImmediate (cell.viz);
				board.Remove (cell);
				i--; // Fix the index into the array because shits shifted
			}
		}
		// Update the board height
		boardHeight -= distance;

		// Update all registered movers
		for (int i = 0; i < pieces.Count; i++) {
			pieces [i].HandleBoardAdvance (distance);
		}

		turnManager.EndCurrentTurn ();
	}

	public void RegisterPiece(BoardPiece piece){
		pieces.Add (piece);
	}

	// --~~== End API Functions ==~~--

	// --~~== START ==~~--
	public void Start(){
		// Initialize board parameters
		cellScale = 1; //(Camera.main.orthographicSize * 2.0f * Camera.main.aspect) / (float)columns; // Base the cell scale off of the horizontal resoulution of the camera, and the number of columns
		visibleRows = Mathf.CeilToInt( (Camera.main.orthographicSize * 2.0f) / cellScale) - rowOffset; // Determine how many rows are visible to the player. (Max rows visible - offset)
		camOrigin = cam.transform.position;
	}
}
