using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public int initialHeight = 8;
	public int columnCount = 5;

	public Cell[] cellTypes;

	public float cellScale { get; private set; }

	private List<Cell>[] board_data; // An array of Queues of Prefabs. The prefabs should be the cells
	private int maxReachableHeight = 8;

	// Returns whether or not the position is traversable
	public bool IsTraversable(int x, int y){
		if (x < 0 || x >= columnCount || y < 0 || y >= maxReachableHeight) {
			return false;
		} else {
			return board_data [x] [y].IsPlayerTraversable ();
		}
	}

	public void UpdateCellPositions(){
		for (int i = 0; i < columnCount; i++) {
			for (int j = 0; j < initialHeight; j++) {
				Cell cell = board_data [i] [j];
				cell.transform.position = new Vector2 (i * cellScale, j * cellScale);
				cell.transform.localScale = new Vector2 (cellScale, cellScale);
			}
		}
	}

	public Vector2 CellToWorldPos(int x, int y){
		return new Vector2 (x * cellScale, y * cellScale);
	}

	public int[] WorldToCellPos(float pixX, float pixY){
		Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return new int[2] { (int)Mathf.Floor (worldPos.x / cellScale), (int)Mathf.Floor (worldPos.y / cellScale) };
	}

	// Use this for initialization
	void Start () {
		// Initialize the board
		cellScale = (Camera.main.orthographicSize * 2.0f * Camera.main.aspect) / (float)columnCount;

		maxReachableHeight = (int)Mathf.Floor( (float) ( (Camera.main.orthographicSize * 2.0f) / cellScale) );

		board_data = new List<Cell>[columnCount];
		for (int i = 0; i < columnCount; i++) {
			board_data [i] = new List<Cell> ();
			for (int j = 0; j < initialHeight; j++) {
				Cell cell = (Cell) Instantiate (cellTypes[Constants.Board.Enums.GROUND_CELL], transform);
				cell.transform.position = new Vector2 (i * cellScale, j * cellScale);
				cell.transform.localScale = new Vector2 (cellScale, cellScale);
				board_data[i].Add(cell);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
