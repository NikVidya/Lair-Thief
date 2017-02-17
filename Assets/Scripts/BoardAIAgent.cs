using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAIAgent : Agent {

	// -----------------------

	public BoardManager board; // The board this AI agent operates on
	public PlayerAgent player; // The player agent this AI is aware of

	// -----------------------

	public TextAsset[] patternSources; // The source .csv files to use to generate new patterns

	private List<List<List<Cell.CellType>>> patterns = new List<List<List<Cell.CellType>>>(); // The in memory pattern pallet


	public void Start(){ // Parse all the patterns into data
		for (int i = 0; i < patternSources.Length; i++) {
			patterns.Add( ParsePatternFile(patternSources[i]) );
		}
	}

	private bool MakeValidPath(Cell fromCell){
		if (fromCell == null) {
			return false; // You reached a null cell. Probably the player is above the highest cells. That should only happen during initial generation.
		}
		if (fromCell.y >= board.boardHeight) {
			return true; // The path reached a cell in the top row, this is a valid path
		}
		int x = 0;

		List<Cell> rowCells = new List<Cell>();
		// fromCell is reachable, push that one
		rowCells.Add(fromCell);

		// Collect reachable cells to the left of fromCell
		x = fromCell.x - 1; // Start with the cell immediately to the left of the player.
		while (x >= 0 && board.IsTraversable (x, fromCell.y)) {
			rowCells.Add (board.GetCell (x, fromCell.y));
			x--; // Move to the left
		}

		// Collect reachable cells to the right of fromCell
		x = fromCell.x + 1; // Start with the cell immediately to the right of the player.
		while (x < board.columns && board.IsTraversable (x, fromCell.y)) {
			rowCells.Add (board.GetCell (x, fromCell.y));
			x++; // Move to the right
		}

		// Now, find the cells which have a path upwards
		List<Cell> columnCells = new List<Cell>();
		for (int i = 0; i < rowCells.Count; i++) {
			if (board.IsTraversable (rowCells [i].x, rowCells [i].y + 1)) { // Check if the cell above this one is traversable
				columnCells.Add (board.GetCell (rowCells [i].x, rowCells [i].y + 1));
			}
		}

		// Check if we need to make the path valid
		if (columnCells.Count <= 0) {
			// Choose a random cell to turn into a path
			Cell randCell = rowCells[ Random.Range(0, rowCells.Count) ];
			Cell newColCell = board.ChangeCell (randCell.x, randCell.y + 1, Cell.CellType.NONE );
			columnCells.Add (newColCell); // Add this to the list of cells to make a valid path for.
		}

		// TODO One could add a bit of randomization here. Clear a path above a random row cell, but don't add it to columnCells. It won't be guarenteed to be a path, and could be a misdirect.

		// Choose a random columnCells to further validate
		// TODO Choose multiple columnCells? Multiple paths that have enforced validation?
		// Recurse along the path
		return MakeValidPath( columnCells[ Random.Range(0, columnCells.Count) ] );
	}

	protected override void OnTurnStart(){
		Debug.Log ("Start BoardAIAgent Turn");

		// Fill the board until there is a row buffer of 5 rows at least
		while (board.NeedsRow (5)) {
			board.PushCellPattern (MakePattern ());
		}

		// Make sure there is a valid path!
		MakeValidPath( board.GetCell(player.cellPosX, player.cellPosY) );


		TurnEnd ();
	}

	protected override void OnTurnUpdate(){

	}

	protected override void OnTurnEnd(){

	}

	private List<Cell.CellType>[] MakePattern(){

		List<Cell.CellType>[] pattern = new List<Cell.CellType>[board.columns];

		int subX = 0;
		List<List<Cell.CellType>> subPattern = patterns[Random.Range(0, patterns.Count)];
		for (int x = 0; x < board.columns; x++, subX++) { // From left to right, begin filling the pattern
			if (subX >= subPattern.Count) {
				subX = 0; // Reset the subX index to the start of a new pattern
				subPattern = patterns[Random.Range(0, patterns.Count)];
			}
			pattern [x] = new List<Cell.CellType> (); // Initialize the list for this column
			for (int y = 0; y < subPattern [subX].Count; y++) { // For each cell in this column
				pattern[x].Add( subPattern[subX][y] );
			}
		}

		return pattern;
	}


	private List<List<Cell.CellType>> ParsePatternFile(TextAsset file){
		List<List<Cell.CellType>> parse = new List<List<Cell.CellType>>();

		// Split the file into lines
		string[] lines = file.text.Split('\n');
		for (int y = 0; y < lines.Length; y++) {
			string[] vals = lines [y].Split (',');
			for (int x = 0; x < vals.Length; x++) {
				if (x >= parse.Count) {
					parse.Add(new List<Cell.CellType> ());
				}
				parse [x].Add (ParseTypeFromInt (int.Parse (vals [x])));
			}
		}

		return parse;
	}

	private Cell.CellType ParseTypeFromInt(int typeInt){
		switch (typeInt) {
		case 0:
			return Cell.CellType.NONE;
		case 1:
			return Cell.CellType.ROCK;
        case 2:
            return Cell.CellType.BOOST;
		default:
			return Cell.CellType.NONE;
		}
	}
}
