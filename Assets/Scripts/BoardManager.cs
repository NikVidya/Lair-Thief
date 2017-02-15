using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public int initialHeight = 10;
	public int columnCount = 5;

	public GameObject[] cellTypes;

	private Queue<GameObject>[] board_data; // An array of Queues of Prefabs. The prefabs should be the cells


	// Use this for initialization
	void Start () {
		// Initialize the board

		float cellScale = (Camera.main.orthographicSize * 2.0f * Camera.main.aspect) / (float)columnCount;

		board_data = new Queue<GameObject>[columnCount];
		for (int i = 0; i < columnCount; i++) {
			board_data [i] = new Queue<GameObject> ();
			for (int j = 0; j < initialHeight; j++) {
				GameObject cell = Instantiate (cellTypes[Constants.Board.Enums.GROUND_CELL], transform) as GameObject;
				cell.transform.position = new Vector2 (i * cellScale, j * cellScale);
				cell.transform.localScale = new Vector2 (cellScale, cellScale);
				board_data[i].Enqueue (cell);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
