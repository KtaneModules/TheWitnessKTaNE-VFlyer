using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;




public class theWitness : MonoBehaviour {

	public KMAudio Audio;
	public KMBombModule Module;
	public KMModSettings modSettings;
	public KMSelectable[] btn;
	public KMSelectable submit;
	public GameObject tl, tr, tsl, tsm, tsr, ml, mr, bsl, bsm, bsr, bl, br, bsquare_tl, bsquare_tr, bsquare_bl, bsquare_br, wsquare_tl, wsquare_tr, wsquare_bl, wsquare_br, sun1_tl, sun1_tr, sun1_bl, sun1_br, sun2_tl, sun2_tr, sun2_bl, sun2_br, deleter_tl, deleter_tr, deleter_bl, deleter_br, lpiece_tl, lpiece_tr, lpiece_bl, lpiece_br, wireGray, wireGreen, wireRed;
	//for symbol setup: 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter

	private static int _moduleIdCounter = 1;
	private int _moduleId = 0;
	private bool _isSolved = false;
	private bool _lightsOn = false;

	private int puzzleId = 0;

	private bool isFirst = true;
	private int lastPress = 0;
	private bool tmOn = false;
	private bool trOn = false;
	private bool mlOn = false;
	private bool mmOn = false;
	private bool mrOn = false;
	private bool blOn = false;
	private bool bmOn = false;
	private bool brOn = false;

	private int symbolRandomizer = 0;

	private string currentLine;
	private string correctLine;
	private string alternativeLine;

	// Use this for initialization
	void Start ()
	{
		_moduleId = _moduleIdCounter++;
		Module.OnActivate += Activate;

		tl.SetActive (false);
		tr.SetActive (false);
		tsl.SetActive (false);
		tsm.SetActive (false);
		tsr.SetActive (false);
		ml.SetActive (false);
		mr.SetActive (false);
		bsl.SetActive (false);
		bsm.SetActive (false);
		bsr.SetActive (false);
		bl.SetActive (false);
		br.SetActive (false);
		bsquare_tl.SetActive (false);
		bsquare_tr.SetActive (false);
		bsquare_bl.SetActive (false);
		bsquare_br.SetActive (false);
		wsquare_tl.SetActive (false);
		wsquare_tr.SetActive (false);
		wsquare_bl.SetActive (false);
		wsquare_br.SetActive (false);
		sun1_tl.SetActive (false);
		sun1_tr.SetActive (false);
		sun1_bl.SetActive (false);
		sun1_br.SetActive (false);
		sun2_tl.SetActive (false);
		sun2_tr.SetActive (false);
		sun2_bl.SetActive (false);
		sun2_br.SetActive (false);
		deleter_tl.SetActive (false);
		deleter_tr.SetActive (false);
		deleter_bl.SetActive (false);
		deleter_br.SetActive (false);
		lpiece_tl.SetActive (false);
		lpiece_tr.SetActive (false);
		lpiece_bl.SetActive (false);
		lpiece_br.SetActive (false);

		wireGray.SetActive (true);
		wireGreen.SetActive (false);
		wireRed.SetActive (false);
	}

	void Activate()
	{
		Init();
		_lightsOn = true;
	}


	void Init()
	{
		puzzleId = Random.Range (1, 44);
		Debug.LogFormat ("[The Witness #{0}] Puzzle ID {1}", _moduleId, puzzleId);

		currentLine = "";

		SetupSolution ();
	}

	void Awake(){
		submit.OnInteract += delegate ()
		{
			Check();
			return false;
		};

		for (int i = 0; i < 9; i++)
		{
			int j = i;
			btn[i].OnInteract += delegate ()
			{
				LineMaker(j);
				return false;
			};
		}
	}

	//for symbol setup (in puzzle#Array): 0-empty, 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter (order tl, tr, bl, br)
	void SetupSolution(){
		if (puzzleId >= 1 && puzzleId < 10) {
			correctLine = "chkl";
			alternativeLine = "abej";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tsl, bsl, bl, br' or 'tl, tr, tsr, bsr'", _moduleId);

			symbolRandomizer = Random.Range (0, 20) * 4;
			int[] puzzle1Array = new int[] { 3,4,4,3, 4,3,3,4, 3,6,4,3, 6,3,3,4, 3,4,6,3, 4,3,3,6, 4,6,3,4, 6,4,4,3, 4,3,6,4, 3,4,4,6, 1,1,1,1, 2,2,2,2, 5,3,3,6, 3,6,5,3, 6,3,3,5, 3,5,6,3, 5,4,4,6, 4,6,5,4, 6,4,4,5, 4,5,6,4 };
			SetupSymbols (puzzle1Array [symbolRandomizer], puzzle1Array [symbolRandomizer + 1], puzzle1Array [symbolRandomizer + 2], puzzle1Array [symbolRandomizer + 3]);
		}

		if (puzzleId >= 10 && puzzleId < 18) {
			correctLine = "adgj";
			alternativeLine = "zzzz";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tr, tsm, mr, bsr'", _moduleId);

			symbolRandomizer = Random.Range (0,16) * 4;
			int[] puzzle2Array = new int[] { 5,2,1,1, 5,1,2,2, 1,2,5,1, 2,1,5,2, 1,2,1,5, 2,1,2,5, 1,2,0,1, 2,1,0,2, 1,2,1,1, 2,1,2,2, 3,0,5,3, 5,0,3,3, 3,0,3,5, 4,0,5,4, 5,0,4,4, 4,0,4,5 };
			SetupSymbols (puzzle2Array [symbolRandomizer], puzzle2Array [symbolRandomizer + 1], puzzle2Array [symbolRandomizer + 2], puzzle2Array [symbolRandomizer + 3]);
		}

		if (puzzleId >= 18 && puzzleId < 26) {
			correctLine = "cfil";
			alternativeLine = "zzzz";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tsl, ml, bsm, br'", _moduleId);

			symbolRandomizer = Random.Range (0,16) * 4;
			int[] puzzle3Array = new int[] { 1,1,2,5, 2,2,1,5, 1,5,2,1, 2,5,1,2, 5,1,2,1, 5,2,1,2, 1,0,2,1, 2,0,1,2, 1,1,2,1, 2,2,1,2, 3,5,0,3, 3,3,0,5, 5,3,0,3, 4,5,0,4, 4,4,0,5, 5,4,0,4 };
			SetupSymbols (puzzle3Array [symbolRandomizer], puzzle3Array [symbolRandomizer + 1], puzzle3Array [symbolRandomizer + 2], puzzle3Array [symbolRandomizer + 3]);
		}

		if (puzzleId >= 26 && puzzleId < 34) {
			correctLine = "chkigj";
			alternativeLine = "abegil";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tsl, bsl, bl, bsm, mr, bsr' or 'tl, tr, tsr, mr, bsm, br'", _moduleId);

			symbolRandomizer = Random.Range (0,16) * 4;
			int[] puzzle4Array = new int[] { 1,5,1,2, 2,5,2,1, 5,1,1,2, 5,2,2,1, 1,1,5,2, 2,2,5,1, 0,1,1,2, 0,2,2,1, 1,1,1,2, 2,2,2,1, 5,3,3,0, 3,5,3,0, 3,3,5,0, 5,4,4,0, 4,5,4,0, 4,4,5,0 };
			SetupSymbols (puzzle4Array [symbolRandomizer], puzzle4Array [symbolRandomizer + 1], puzzle4Array [symbolRandomizer + 2], puzzle4Array [symbolRandomizer + 3]);
		}

		if (puzzleId >= 34 && puzzleId < 42) {
			correctLine = "adfhkl";
			alternativeLine = "cfdbej";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tl, tsm, ml, bsl, bl, br' or 'tsl, ml, tsm, tr, tsr, bsr'", _moduleId);

			symbolRandomizer = Random.Range (0,16) * 4;
			int[] puzzle5Array = new int[] { 2,1,5,1, 1,2,5,2, 2,1,1,5, 1,2,2,5, 2,5,1,1, 1,5,2,2, 2,1,1,0, 1,2,2,0, 2,1,1,1, 1,2,2,2, 0,3,3,5, 0,3,5,3, 0,5,3,3, 0,4,4,5, 0,4,5,4, 0,5,4,4 };
			SetupSymbols (puzzle5Array [symbolRandomizer], puzzle5Array [symbolRandomizer + 1], puzzle5Array [symbolRandomizer + 2], puzzle5Array [symbolRandomizer + 3]);
		}

		if (puzzleId == 42) {
			correctLine = "adil";
			alternativeLine = "chkidbej";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tl, tsm, bsm, br' or 'tsl, bsl, bl, bsm, tsm, tr, tsr, bsr'", _moduleId);

			symbolRandomizer = Random.Range (0,2) * 4;
			int[] puzzle6Array = new int[] { 1,2,1,2, 2,1,2,1 };
			SetupSymbols (puzzle6Array [symbolRandomizer], puzzle6Array [symbolRandomizer + 1], puzzle6Array [symbolRandomizer + 2], puzzle6Array [symbolRandomizer + 3]);
		}

		if (puzzleId == 43) {
			correctLine = "abegfhkl";
			alternativeLine = "cfgj";

			Debug.LogFormat ("[The Witness #{0}] Correct line is 'tl, tr, tsr, mr, ml, bsl, bl, br' or 'tsl, ml, mr, bsr'", _moduleId);

			symbolRandomizer = Random.Range (0,2) * 4;
			int[] puzzle7Array = new int[] { 1,1,2,2, 2,2,1,1 };
			SetupSymbols (puzzle7Array [symbolRandomizer], puzzle7Array [symbolRandomizer + 1], puzzle7Array [symbolRandomizer + 2], puzzle7Array [symbolRandomizer + 3]);
		}
	}



	void SetupSymbols(int Symboltl, int Symboltr, int Symbolbl, int Symbolbr){
		Debug.LogFormat ("[The Witness #{0}] Symbols shown are {1}, {2}, {3}, {4}", _moduleId, Symboltl, Symboltr, Symbolbl, Symbolbr);
		Debug.LogFormat ("[The Witness #{0}] 0-empty, 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter (order tl, tr, bl, br)", _moduleId);

		if (Symboltl == 1) {
			bsquare_tl.SetActive (true);
		} else if (Symboltl == 2) {
			wsquare_tl.SetActive (true);
		} else if (Symboltl == 3) {
			sun1_tl.SetActive (true);
		} else if (Symboltl == 4) {
			sun2_tl.SetActive (true);
		} else if (Symboltl == 5) {
			lpiece_tl.SetActive (true);
		} else if (Symboltl == 6) {
			deleter_tl.SetActive (true);
		}

		if (Symboltr == 1) {
			bsquare_tr.SetActive (true);
		} else if (Symboltr == 2) {
			wsquare_tr.SetActive (true);
		} else if (Symboltr == 3) {
			sun1_tr.SetActive (true);
		} else if (Symboltr == 4) {
			sun2_tr.SetActive (true);
		} else if (Symboltr == 5) {
			lpiece_tr.SetActive (true);
		} else if (Symboltr == 6) {
			deleter_tr.SetActive (true);
		}
	
		if (Symbolbl == 1) {
			bsquare_bl.SetActive (true);
		} else if (Symbolbl == 2) {
			wsquare_bl.SetActive (true);
		} else if (Symbolbl == 3) {
			sun1_bl.SetActive (true);
		} else if (Symbolbl == 4) {
			sun2_bl.SetActive (true);
		} else if (Symbolbl == 5) {
			lpiece_bl.SetActive (true);
		} else if (Symbolbl == 6) {
			deleter_bl.SetActive (true);
		}

		if (Symbolbr == 1) {
			bsquare_br.SetActive (true);
		} else if (Symbolbr == 2) {
			wsquare_br.SetActive (true);
		} else if (Symbolbr == 3) {
			sun1_br.SetActive (true);
		} else if (Symbolbr == 4) {
			sun2_br.SetActive (true);
		} else if (Symbolbr == 5) {
			lpiece_br.SetActive (true);
		} else if (Symbolbr == 6) {
			deleter_br.SetActive (true);
		}
	}

		

	void Check(){

		if (!_lightsOn || _isSolved) return;

		Audio.PlayGameSoundAtTransform (KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
		submit.AddInteractionPunch ();

		if (correctLine == currentLine) {

			Debug.LogFormat ("[The Witness #{0}] Expected line: {1} or {2}, input line: {3}.", _moduleId, correctLine, alternativeLine, currentLine);
			Debug.LogFormat ("[The Witness #{0}] Module defused. Well done :)", _moduleId);

			Audio.PlaySoundAtTransform ("disarmed", Module.transform);
			wireGray.SetActive (false);
			wireGreen.SetActive (true);
			Module.HandlePass (); 
			_isSolved = true;

		} else if (alternativeLine == currentLine) {

			Debug.LogFormat ("[The Witness #{0}] Expected line: {1} or {2}, input line: {3}.", _moduleId, correctLine, alternativeLine, currentLine);
			Debug.LogFormat ("[The Witness #{0}] Module defused. Well done :)", _moduleId);

			Audio.PlaySoundAtTransform ("disarmed", Module.transform);
			wireGray.SetActive (false);
			wireGreen.SetActive (true);
			Module.HandlePass ();
			_isSolved = true;

		} else {

			Debug.LogFormat ("[The Witness #{0}] Expected line: {1} or {2}, input line: {3}. Strike!", _moduleId, correctLine, alternativeLine, currentLine);

			Module.HandleStrike ();

			StartCoroutine(RedWireHandle());

			tl.SetActive (false);
			tr.SetActive (false);
			tsl.SetActive (false);
			tsm.SetActive (false);
			tsr.SetActive (false);
			ml.SetActive (false);
			mr.SetActive (false);
			bsl.SetActive (false);
			bsm.SetActive (false);
			bsr.SetActive (false);
			bl.SetActive (false);
			br.SetActive (false);

			tmOn = false;
			trOn = false;
			mlOn = false;
			mmOn = false;
			mrOn = false;
			blOn = false;
			bmOn = false;
			brOn = false;

			currentLine = "";
			lastPress = 0;
			isFirst = true;
		}
	}

	IEnumerator RedWireHandle(){
		wireGray.SetActive (false);
		wireRed.SetActive (true);
		yield return new WaitForSeconds(1);
		wireGray.SetActive (true);
		wireRed.SetActive (false);
	}

	void LineMaker(int num){
					
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn[num].transform);

		if (!_lightsOn || _isSolved) return;

		if (num == 0) {
			tl.SetActive (false);
			tr.SetActive (false);
			tsl.SetActive (false);
			tsm.SetActive (false);
			tsr.SetActive (false);
			ml.SetActive (false);
			mr.SetActive (false);
			bsl.SetActive (false);
			bsm.SetActive (false);
			bsr.SetActive (false);
			bl.SetActive (false);
			br.SetActive (false);

			tmOn = false;
			trOn = false;
			mlOn = false;
			mmOn = false;
			mrOn = false;
			blOn = false;
			bmOn = false;
			brOn = false;

			currentLine = "";
			lastPress = 0;
			isFirst = true;
		}

		if (num == 1) {

			if (tmOn == true)
				return;

			if (isFirst == true) {
				tl.SetActive (true);
				currentLine += "a";
				isFirst = false;
				tmOn = true;
				lastPress = 1;

			} else if (lastPress == 4) {
				tsm.SetActive (true);
				currentLine += "d";
				tmOn = true;
				lastPress = 1;
			} else if (lastPress == 2) {
				tr.SetActive (true);
				currentLine += "b";
				tmOn = true;
				lastPress = 1;
			}
		}

		if (num == 2) {
			
			if (trOn == true)
				return;

			if (lastPress == 1) {
				tr.SetActive (true);
				currentLine += "b";
				trOn = true;
				lastPress = 2;
			} else if (lastPress == 5) {
				tsr.SetActive (true);
				currentLine += "e";
				trOn = true;
				lastPress = 2;
			}
		}

		if (num == 3) {
			
			if (mlOn == true)
				return;

			if (isFirst == true) {
				tsl.SetActive (true);
				currentLine += "c";
				isFirst = false;
				mlOn = true;
				lastPress = 3;
			} else if (lastPress == 4) {
				ml.SetActive (true);
				currentLine += "f";
				mlOn = true;
				lastPress = 3;
			} else if (lastPress == 6) {
				bsl.SetActive (true);
				currentLine += "h";
				mlOn = true;
				lastPress = 3;
			}
		}

		if (num == 4) {

			if (mmOn == true)
				return;

			if (lastPress == 1) {
				tsm.SetActive (true);
				currentLine += "d";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 3) {
				ml.SetActive (true);
				currentLine += "f";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 5) {
				mr.SetActive (true);
				currentLine += "g";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 7) {
				bsm.SetActive (true);
				currentLine += "i";
				mmOn = true;
				lastPress = 4;
			}
		}

		if (num == 5) {
			
			if (mrOn == true)
				return;

			if (lastPress == 2) {
				tsr.SetActive (true);
				currentLine += "e";
				mrOn = true;
				lastPress = 5;
			} else if (lastPress == 4) {
				mr.SetActive (true);
				currentLine += "g";
				mrOn = true;
				lastPress = 5;
			}
		}

		if (num == 6) {
			
			if (blOn == true)
				return;

			if (lastPress == 3) {
				bsl.SetActive (true);
				currentLine += "h";
				blOn = true;
				lastPress = 6;
			} else if (lastPress == 7) {
				bl.SetActive (true);
				currentLine += "k";
				blOn = true;
				lastPress = 6;
			}
		}

		if (num == 7) {
			if (bmOn == true)
				return;

			if (lastPress == 6) {
				bl.SetActive (true);
				currentLine += "k";
				bmOn = true;
				lastPress = 7;
			} else if (lastPress == 4) {
				bsm.SetActive (true);
				currentLine += "i";
				bmOn = true;
				lastPress = 7;
			}
		}

		if (num == 8) {

			if (brOn == true)
				return;

			if (lastPress == 5) {
				bsr.SetActive (true);
				currentLine += "j";
				brOn = true;
				lastPress = 8;
			} else if (lastPress == 7) {
				br.SetActive (true);
				currentLine += "l";
				brOn = true;
				lastPress = 8;
			}
		}
	}

	//TWITCH PLAYS SETUP HERE
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"To press the grid buttons use !{0} press 1 [number from 1 to 9, 1 top-left corner, 9 bottom-right corner, in reading order]. To submit use !{0} submit";
#pragma warning restore 414

	KMSelectable[] ProcessTwitchCommand(string command){
		command = command.ToLowerInvariant ().Trim ();

		if (command == "submit")
			return new[] { submit };
		else if (Regex.IsMatch (command, @"^press +\d$")) {
			command = command.Substring(5).Trim();
			return new[] { btn [int.Parse (command [0].ToString ()) - 1] };
		}

		return null;
	}

}

