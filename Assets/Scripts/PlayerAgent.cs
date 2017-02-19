using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAgent : Agent, BoardPiece {


    // -----------------------------------

	public BoardManager board; // The board the player is playing on

	public GameObject avatar; // The visual indicator of the player's position

	public GameObject movableHighlighter; // The prefab to use to indicate the player's movable region
	

    public int boostTurns; // The number of turns a boost lasts for

    public EnemyAgent enemyAgent; // The enemy the player is escaping from

    public Button boostButton; // The button the player click's to enable boosting

    public Button breakButton; // Player clicks this to break the block ahead of them

    public Text scoreText; // The UI element that display's the player's score

    public float scoreMulti = 10f; // Base multiplier for the score
    public float scoreAdvance = 0.5f; // Amount to increase the score for each screen traveled

    // -----------------------------------

    public float currentScore; // The player's current score
    private int rowsTraversedOnBoard; // Each board is board.visibleRows height.


	private List<GameObject> highlights = new List<GameObject>();

	public int cellPosX = 3;
	public int cellPosY = 4;

	private int targetCellX = 3;
	private int targetCellY = 4;

	private IEnumerator moveCoroutine = null;

    private bool[,] movementRegion;

    public bool inDanger;

    public bool CanBoost = false, CanPunch = false; // Hold the state of the player's 'inventory'. Player can only hold one of each

    public int boostedTurnsRemaining = 0;

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

    private bool[,] activeMovementRegion = NORMAL_MOVEMENT_REGION;

    public void Start(){
		board.RegisterPiece (this);
	}

    protected override void OnTurnStart()
    {
		if (cellPosY < 0){
			Application.LoadLevel("GameOver");
		}
        Debug.Log ("Player turn: Start");
		avatar.transform.position = board.CellToWorld (cellPosX, cellPosY);
		avatar.transform.localScale = new Vector2(board.cellScale, board.cellScale);
        showMoveRegion();
	}

    private void showMoveRegion()
    {
        for (int i = 0; i < highlights.Count; i++)
        {
            GameObject.Destroy(highlights[i]);
        }
        highlights.Clear();

        UpdateEndangered();
        UpdateMovementType();
        // Show the user what cells the player can move to
        for (int x = cellPosX - 1; x <= cellPosX + 1; x++)
        {
            for (int y = cellPosY; y < cellPosY + 4; y++)
            {
                if (IsReachable(x - cellPosX, y - cellPosY, activeMovementRegion) && board.IsTraversable(x, y))
                {
                    GameObject highlight = (GameObject)Instantiate(movableHighlighter, transform);
                    highlight.transform.position = board.CellToWorld(x, y);
                    highlight.transform.localScale = new Vector2(board.cellScale, board.cellScale);
                    highlights.Add(highlight);
                }
            }
        }
    }

    protected override void OnTurnUpdate()
    {
        int[] clickCell;
        if (Input.GetMouseButtonDown(0))
        {
            clickCell = board.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            clickCell = new int[2];
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            clickCell[0] = targetCellX;
            clickCell[1] = targetCellY + 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            clickCell[0] = targetCellX;
            clickCell[1] = targetCellY - 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            clickCell[0] = targetCellX + 1;
            clickCell[1] = targetCellY;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            clickCell[0] = targetCellX - 1;
            clickCell[1] = targetCellY;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnEnd();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartBoosting();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BreakBlock();
        }

        if (IsReachable(clickCell[0] - cellPosX, clickCell[1] - cellPosY, activeMovementRegion) && board.IsTraversable(clickCell[0], clickCell[1]))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = MoveAvatarTo(board.CellToWorld(clickCell[0], clickCell[1]), 1f);
            StartCoroutine(moveCoroutine);
            targetCellX = clickCell[0];
            targetCellY = clickCell[1];
        }
    }


    private void MoveAvatarByAmount(int xDirection, int yDirection)
    {
        if (IsReachable(targetCellX + xDirection - cellPosX, targetCellY + yDirection - cellPosY, activeMovementRegion) && board.IsTraversable(targetCellX + xDirection, targetCellY + yDirection))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = MoveAvatarTo(board.CellToWorld(targetCellX + xDirection, targetCellY + yDirection), 1f);
            StartCoroutine(moveCoroutine);
            targetCellX = targetCellX + xDirection;
            targetCellY = targetCellY + yDirection;
            Debug.Log("should have moved the avatar");
        }
    }

	protected override void OnTurnEnd() {
		Debug.Log ("Player Turn: End");
		// Clean up and make ready for handover to AI
		for (int i = 0; i < highlights.Count; i++) {
			GameObject.Destroy (highlights [i]);
		}
		highlights.Clear();
        // Update how far the player has traveled and add score
        int dist = targetCellY - cellPosY;
        rowsTraversedOnBoard += dist;
        if(rowsTraversedOnBoard > board.visibleRows)
        {
            rowsTraversedOnBoard = 0; // Reset, new board
            enemyAgent.dangerDistance++; // Enemy gets more dangerous
            scoreMulti += scoreAdvance; // Advance the score multiplier
        }
        // Add score for distance travelled
        currentScore += dist * scoreMulti;
		Score.finalScore = currentScore;
        scoreText.text = string.Format("Score: {0}", Mathf.RoundToInt(currentScore));

		// Update my actual position
		cellPosX = targetCellX;
		cellPosY = targetCellY;
        // iterate boosted so that powerup can run out
        if (boostedTurnsRemaining > 0)
        {
            boostedTurnsRemaining--;
        }
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

	private bool IsReachable(int offsetX, int offsetY, bool[,] movementPattern){
		if (	IsInMovementPattern (offsetX, offsetY, movementPattern) 
			&& 	cellPosX + offsetX >= 0 && cellPosX + offsetX < board.columns 
			&& 	cellPosY + offsetY >= 0 && cellPosY + offsetY < board.visibleRows	
		){
			// Starting from x/y
			// Move down until you hit a rock, or have reached the player's y
			while (cellPosY + offsetY > cellPosY) {
				if (!board.IsTraversable (cellPosX + offsetX, cellPosY + offsetY)) {
					return false; // We hit a rock, the player wouldn't be able to move past it
				}
				offsetY--;
			}
			return true;
		} else {
			return false;
		}
	}

    // tests if player is in danger zone
    public void UpdateEndangered()
    {
        inDanger = cellPosY < enemyAgent.dangerDistance;
    }

    public void UpdateMovementType()
    {
        if (cellPosY < enemyAgent.dangerDistance )
        {
            if (boostedTurnsRemaining > 0)
            {
                activeMovementRegion = BOOSTED_ENDANGERED_MOVEMENT_REGION;
            }
            else
            {
                activeMovementRegion = ENDANGERED_MOVEMENT_REGION;
            }
        }else
        {
            if (boostedTurnsRemaining > 0)
            {
                activeMovementRegion = BOOSTED_MOVEMENT_REGION;
            }else
            {
                activeMovementRegion = NORMAL_MOVEMENT_REGION;
            }
        }
    }

    public void EnableBoosting()
    {
        CanBoost = true;
        boostButton.interactable = true;
    }
    public void EnableBreaking()
    {
        CanPunch = true;
        breakButton.interactable = true;
    }

    public void StartBoosting()
    {
        if (CanBoost)
        {
            Debug.Log("Player started thier boost");
            activeMovementRegion = BOOSTED_MOVEMENT_REGION;
            boostedTurnsRemaining = boostTurns;
            CanBoost = false; // Player consumed the boost item
            boostButton.interactable = false;
            showMoveRegion();
        }
    }
    public void BreakBlock()
    {
        if (CanPunch)
        {
            Debug.Log("Player just used Break Powerup and broke a block");
            board.ChangeCell(targetCellX, targetCellY + 1, Cell.CellType.NONE);
            CanPunch = false;
            breakButton.interactable = false;
            showMoveRegion();
        }
    }

    public void HandleBoardAdvance(int distance){
		cellPosY -= distance;
		targetCellY -= distance;
		avatar.transform.position = board.CellToWorld (cellPosX, cellPosY);
    }

    public void GainScore(float score)
    {
        currentScore += score * scoreMulti;
        scoreText.text = string.Format("Score: {0}", Mathf.RoundToInt(currentScore));
    }
}
