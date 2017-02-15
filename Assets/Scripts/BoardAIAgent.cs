using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAIAgent : Agent {
	public BoardManager board;

	public TextAsset[] patternSources;

	private List<List<List<Cell.CellType>>> patterns = new List<List<List<Cell.CellType>>>();

	public void Start(){ // Parse all the patterns into data
		for (int i = 0; i < patternSources.Length; i++) {
			patterns.Add( ParsePatternFile(patternSources[i]) );
		}
	}

	protected override void OnTurnStart(){
		Debug.Log ("Start BoardAIAgent Turn");

		// Fill the board until there is a row buffer of 5 rows at least
		while (board.NeedsRow (5)) {
			board.PushCellPattern (MakePattern ());
		}

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
		default:
			return Cell.CellType.NONE;
		}
	}
}
