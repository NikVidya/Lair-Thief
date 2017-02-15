﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : Agent {

	public BoardManager board;

	public GameObject avatar;

	public GameObject movableHighlighter;

	private List<GameObject> highlights = new List<GameObject>();

	public int cellPosX = 3;
	public int cellPosY = 4;

	private int targetCellX = 3;
	private int targetCellY = 4;

	private IEnumerator moveCoroutine = null;

	// DEFINE MOVABLE AREA PATTERNS - Hardcoded = bad, but I can't think of another way to define these with Unity
	private static bool[,] NORMAL_MOVEMENT_REGION = new bool[4,3] {
		{true,  true,  true },
		{false, true,  false},
		{false, false, false},
		{false, false, false}
	};
	private static bool[,] ENDANGERED_MOVEMENT_REGION = new bool[4,3] {
		{true,  true,  true },
		{true,  true,  true },
		{false, true,  false},
		{false, false, false}
	};
	private static bool[,] BOOSTED_MOVEMENT_REGION = ENDANGERED_MOVEMENT_REGION;

	private static bool[,] BOOSTED_ENDANGERED_MOVEMENT_REGION = new bool[4,3] {
		{true,  true, true },
		{true,  true, true },
		{true,  true, true },
		{false, true, false}
	};

	public void Start(){
	}

	protected override void OnTurnStart() {
		Debug.Log ("Player turn: Start");
		avatar.transform.position = board.CellToWorldPos (cellPosX, cellPosY);
		avatar.transform.localScale = new Vector2(board.cellScale, board.cellScale);
		// Show the user what cells the player can move to
		for (int x = cellPosX - 1; x <= cellPosX + 1; x++) {
			for (int y = cellPosY; y < cellPosY + 3; y++) {
				if (IsInMovementPattern (x - cellPosX, y - cellPosY, NORMAL_MOVEMENT_REGION) && board.IsTraversable(x, y) ) {
					GameObject highlight = (GameObject)Instantiate (movableHighlighter, transform);
					highlight.transform.position = board.CellToWorldPos (x, y);
					highlight.transform.localScale = new Vector2 (board.cellScale, board.cellScale);
					highlights.Add (highlight);
				}
			}
		}
	}

	protected override void OnTurnUpdate() {
		if (Input.GetMouseButtonDown (0)) {
			int[] clickCell = board.WorldToCellPos (Input.mousePosition.x, Input.mousePosition.y);
			if (IsInMovementPattern (clickCell [0] - cellPosX, clickCell [1] - cellPosY, NORMAL_MOVEMENT_REGION) && board.IsTraversable (clickCell [0], clickCell [1])) {
				if (moveCoroutine != null) {
					StopCoroutine (moveCoroutine);
				}
				moveCoroutine = MoveAvatarTo (board.CellToWorldPos (clickCell [0], clickCell [1]), 1f);
				StartCoroutine (moveCoroutine);
				targetCellX = clickCell [0];
				targetCellY = clickCell [1];
			}
		}
	}

	protected override void OnTurnEnd() {
		Debug.Log ("Player Turn: End");
		// Clean up and make ready for handover to AI
		for (int i = 0; i < highlights.Count; i++) {
			GameObject.Destroy (highlights [i]);
		}
		highlights.Clear();
		// Update my actual position
		cellPosX = targetCellX;
		cellPosY = targetCellY;
	}

	IEnumerator MoveAvatarTo(Vector3 target, float moveTime)
	{
		float startTime = Time.time;
		while ((avatar.transform.position - target).magnitude > 0.01f) {
			float curTime = Time.time;
			avatar.transform.position = Vector2.Lerp (avatar.transform.position, target, (curTime - startTime) / moveTime);

			yield return null; // Yield control back to the rest of the engine so it can tick
		}
	}

	private bool IsInMovementPattern(int offsetX, int offsetY, bool[,] movementPattern){
		if (offsetX < -1 || offsetX >= 2 || offsetY < 0 || offsetY >= 4) {
			return false;
		}
		return movementPattern [offsetY , offsetX + 1];
	}
}