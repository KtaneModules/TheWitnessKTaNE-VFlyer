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
		Debug.LogFormat ("[The Witness #{0}] Generated Puzzle ID {1}", _moduleId, puzzleId);

		currentLine = "1";

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
			correctLine = "14789";
			alternativeLine = "12369";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 4, 7, 8, 9' or '1, 2, 3, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 20) * 4;
			int[] puzzle1Array = new int[] {3,4,4,3, 4,3,3,4, 3,6,4,3, 6,3,3,4, 3,4,6,3, 4,3,3,6, 4,6,3,4, 6,4,4,3, 4,3,6,4, 3,4,4,6, 1,1,1,1, 2,2,2,2, 5,3,3,6, 3,6,5,3, 6,3,3,5, 3,5,6,3, 5,4,4,6, 4,6,5,4, 6,4,4,5, 4,5,6,4};
			SetupSymbols (puzzle1Array [symbolRandomizer], puzzle1Array [symbolRandomizer + 1], puzzle1Array [symbolRandomizer + 2], puzzle1Array [symbolRandomizer + 3]);
		} else if (puzzleId >= 10 && puzzleId < 18) {
			correctLine = "12569";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections:  '1, 2, 5, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle2Array = new int[] {5,2,1,1, 5,1,2,2, 1,2,5,1, 2,1,5,2, 1,2,1,5, 2,1,2,5, 1,2,0,1, 2,1,0,2, 1,2,1,1, 2,1,2,2, 3,0,5,3, 5,0,3,3, 3,0,3,5, 4,0,5,4, 5,0,4,4, 4,0,4,5};
			SetupSymbols (puzzle2Array [symbolRandomizer], puzzle2Array [symbolRandomizer + 1], puzzle2Array [symbolRandomizer + 2], puzzle2Array [symbolRandomizer + 3]);
		} else if (puzzleId >= 18 && puzzleId < 26) {
			correctLine = "14589";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections:  '1, 4, 5, 8, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle3Array = new int[] {1,1,2,5, 2,2,1,5, 1,5,2,1, 2,5,1,2, 5,1,2,1, 5,2,1,2, 1,0,2,1, 2,0,1,2, 1,1,2,1, 2,2,1,2, 3,5,0,3, 3,3,0,5, 5,3,0,3, 4,5,0,4, 4,4,0,5, 5,4,0,4};
			SetupSymbols (puzzle3Array [symbolRandomizer], puzzle3Array [symbolRandomizer + 1], puzzle3Array [symbolRandomizer + 2], puzzle3Array [symbolRandomizer + 3]);
		} else if (puzzleId >= 26 && puzzleId < 34) {
			correctLine = "1478569";
			alternativeLine = "1236589";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 4, 7, 8, 5, 6, 9' or '1, 2, 3, 6, 5, 8, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle4Array = new int[] {1,5,1,2, 2,5,2,1, 5,1,1,2, 5,2,2,1, 1,1,5,2, 2,2,5,1, 0,1,1,2, 0,2,2,1, 1,1,1,2, 2,2,2,1, 5,3,3,0, 3,5,3,0, 3,3,5,0, 5,4,4,0, 4,5,4,0, 4,4,5,0};
			SetupSymbols (puzzle4Array [symbolRandomizer], puzzle4Array [symbolRandomizer + 1], puzzle4Array [symbolRandomizer + 2], puzzle4Array [symbolRandomizer + 3]);
		} else if (puzzleId >= 34 && puzzleId < 42) {
			correctLine = "1254789";
			alternativeLine = "1452369";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 2, 5, 4, 7, 8, 9' or '1, 4, 5, 2, 3, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle5Array = new int[] {2,1,5,1, 1,2,5,2, 2,1,1,5, 1,2,2,5, 2,5,1,1, 1,5,2,2, 2,1,1,0, 1,2,2,0, 2,1,1,1, 1,2,2,2, 0,3,3,5, 0,3,5,3, 0,5,3,3, 0,4,4,5, 0,4,5,4, 0,5,4,4};
			SetupSymbols (puzzle5Array [symbolRandomizer], puzzle5Array [symbolRandomizer + 1], puzzle5Array [symbolRandomizer + 2], puzzle5Array [symbolRandomizer + 3]);
		} else if (puzzleId == 42) {
			correctLine = "12589";
			alternativeLine = "147852369";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 2, 5, 8, 9' or '1, 4, 7, 8, 5, 2, 3, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 2) * 4;
			int[] puzzle6Array = new int[] { 1,2,1,2, 2,1,2,1 };
			SetupSymbols (puzzle6Array [symbolRandomizer], puzzle6Array [symbolRandomizer + 1], puzzle6Array [symbolRandomizer + 2], puzzle6Array [symbolRandomizer + 3]);
		} else if (puzzleId == 43) {
			correctLine = "123654789";
			alternativeLine = "14569";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 2, 3, 6, 5, 4, 7, 8, 9' or '1, 4, 5, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 2) * 4;
			int[] puzzle7Array = new int[] { 1,1,2,2, 2,2,1,1 };
			SetupSymbols (puzzle7Array [symbolRandomizer], puzzle7Array [symbolRandomizer + 1], puzzle7Array [symbolRandomizer + 2], puzzle7Array [symbolRandomizer + 3]);
		} else {
			Debug.LogFormat ("[The Witness #{0}] The module was unable to generate a puzzle with a solution. Please report this issue to bmo22xdd or VFlyer on Discord as this is a serious issue.", _moduleId);
			correctLine = "1";
			Debug.LogFormat ("[The Witness #{0}] For your safety, press ONLY the submit button upon getting a board without a solution.", _moduleId);
		}
	}



	void SetupSymbols(int Symboltl, int Symboltr, int Symbolbl, int Symbolbr){

		var SymbolsTHalf = new[] { "  ", "ks" , "ws" , "1s" , "2s" , "Lb", "de" };
		var SymbolsBHalf = new[] { "  ", "qu" , "qu" , "un" , "un" , "lk", "lr" };

		Debug.LogFormat ("[The Witness #{0}] Generated Puzzle:", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] 1--2--3", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] |{1}|{2}|", _moduleId,SymbolsTHalf[Symboltl],SymbolsTHalf[Symboltr]);
		Debug.LogFormat ("[The Witness #{0}] |{1}|{2}|", _moduleId,SymbolsBHalf[Symboltl],SymbolsBHalf[Symboltr]);
		Debug.LogFormat ("[The Witness #{0}] 4--5--6", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] |{1}|{2}|", _moduleId,SymbolsTHalf[Symbolbl],SymbolsTHalf[Symbolbr]);
		Debug.LogFormat ("[The Witness #{0}] |{1}|{2}|", _moduleId,SymbolsBHalf[Symbolbl],SymbolsBHalf[Symbolbr]);
		Debug.LogFormat ("[The Witness #{0}] 7--8--9", _moduleId);

		Debug.LogFormat ("[The Witness #{0}] ksqu = Black Square", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] wsqu = White Square", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] 1sun = Sun 1", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] 2sun = Sun 2", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] lblk = L Piece", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] delr = Deleter", _moduleId);

		var SymbolsTLAll = new[] {bsquare_tl,wsquare_tl,sun1_tl,sun2_tl,lpiece_tl,deleter_tl};
		if (Symboltl >= 1&&Symboltl <= 6) {
			SymbolsTLAll [Symboltl - 1].SetActive (true);
		}
		var SymbolsTRAll = new[] {bsquare_tr,wsquare_tr,sun1_tr,sun2_tr,lpiece_tr,deleter_tr};
		if (Symboltr >= 1&&Symboltr <= 6) {
			SymbolsTRAll [Symboltr - 1].SetActive (true);
		}
		var SymbolsBLAll = new[] {bsquare_bl,wsquare_bl,sun1_bl,sun2_bl,lpiece_bl,deleter_bl};
		if (Symbolbl >= 1&&Symbolbl<=6) {
			SymbolsBLAll [Symbolbl - 1].SetActive (true);
		}
		var SymbolsBRAll = new[] {bsquare_br,wsquare_br,sun1_br,sun2_br,lpiece_br,deleter_br};
		if (Symbolbr >= 1&&Symbolbr<=6) {
			SymbolsBRAll [Symbolbr - 1].SetActive (true);
		}
	}

		

	void Check(){

		if (!_lightsOn || _isSolved) return;

		Audio.PlayGameSoundAtTransform (KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
		submit.AddInteractionPunch ();
		if ( alternativeLine != null) {
			Debug.LogFormat ("[The Witness #{0}] Inputted line: {3}. Expected line: {1} or {2}", _moduleId, correctLine, alternativeLine, currentLine);
		}
		else
		{
			Debug.LogFormat ("[The Witness #{0}] Inputted line: {2}. Expected line: {1}", _moduleId, correctLine, currentLine);
		}
		if (correctLine == currentLine || alternativeLine == currentLine) {
			Debug.LogFormat ("[The Witness #{0}] That is correct. Module defused.", _moduleId);

			Audio.PlaySoundAtTransform ("disarmed", Module.transform);
			wireGray.SetActive (false);
			wireGreen.SetActive (true);
			Module.HandlePass (); 
			_isSolved = true;

		} else {
			

			Debug.LogFormat ("[The Witness #{0}] That is not correct. Strike!", _moduleId);

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

			currentLine = "1";
			lastPress = 0;
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

		//	
		//	The table is the following:
		//	0-1-2
		//	| | |
		//	3-4-5
		//	| | |
		//	6-7-8
		//	
		//	TL refers to 0-1
		//	TR refers to 1-2
		//	TSL refers to 0-3
		//	TSM refers to 1-4
		//	TSR refers to 2-5
		//	ML refers to 3-4
		//	MR refers to 4-5
		//	BSL refers to 3-6
		//	BSM refers to 4-7
		//	BSR refers to 5-8
		//	BL refers to 6-7
		//	BR refers to 7-8
		//	

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

			currentLine = "1";
			lastPress = 0;
		}

		if (num == 1) {

			if (tmOn == true)
				return;
			// Distance = 1
			if (lastPress == 0) {
				tl.SetActive (true);
				currentLine += "2";
				tmOn = true;
				lastPress = 1;
			} else if (lastPress == 4) {
				tsm.SetActive (true);
				currentLine += "2";
				tmOn = true;
				lastPress = 1;
			} else if (lastPress == 2) {
				tr.SetActive (true);
				currentLine += "2";
				tmOn = true;
				lastPress = 1;
			} //Distance = 2
			else if (lastPress == 7) {
				LineMaker(4);
				tsm.SetActive (true);
				currentLine += "2";
				tmOn = true;
				lastPress = 1;
			}
		}

		if (num == 2) {
			
			if (trOn == true)
				return;
			// Distance = 1
			if (lastPress == 1) {
				tr.SetActive (true);
				currentLine += "3";
				trOn = true;
				lastPress = 2;
			} else if (lastPress == 5) {
				tsr.SetActive (true);
				currentLine += "3";
				trOn = true;
				lastPress = 2;
			}//Distance = 2
			else if (lastPress == 0) {
				LineMaker(1);
				tr.SetActive (true);
				currentLine += "3";
				trOn = true;
				lastPress = 2;
			}

		}

		if (num == 3) {
			
			if (mlOn == true)
				return;
			//Distance = 1
			if (lastPress == 0) {
				tsl.SetActive (true);
				currentLine += "4";
				mlOn = true;
				lastPress = 3;
			} else if (lastPress == 4) {
				ml.SetActive (true);
				currentLine += "4";
				mlOn = true;
				lastPress = 3;
			} else if (lastPress == 6) {
				bsl.SetActive (true);
				currentLine += "4";
				mlOn = true;
				lastPress = 3;
			} // Distance = 2
			else if (lastPress == 5) {
				LineMaker(4);
				ml.SetActive (true);
				currentLine += "4";
				mlOn = true;
				lastPress = 3;
			}
		}

		if (num == 4) {

			if (mmOn == true)
				return;
			// Distance = 1
			if (lastPress == 1) {
				tsm.SetActive (true);
				currentLine += "5";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 3) {
				ml.SetActive (true);
				currentLine += "5";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 5) {
				mr.SetActive (true);
				currentLine += "5";
				mmOn = true;
				lastPress = 4;
			} else if (lastPress == 7) {
				bsm.SetActive (true);
				currentLine += "5";
				mmOn = true;
				lastPress = 4;
			}
		}

		if (num == 5) {
			
			if (mrOn == true)
				return;
			// Distance = 1
			if (lastPress == 2) {
				tsr.SetActive (true);
				currentLine += "6";
				mrOn = true;
				lastPress = 5;
			} else if (lastPress == 4) {
				mr.SetActive (true);
				currentLine += "6";
				mrOn = true;
				lastPress = 5;
			}// Distance = 2
			else if (lastPress == 3) {
				LineMaker(4);
				mr.SetActive (true);
				currentLine += "6";
				mrOn = true;
				lastPress = 5;
			}
		}

		if (num == 6) {
			
			if (blOn == true)
				return;
			// Distance = 1
			if (lastPress == 3) {
				bsl.SetActive (true);
				currentLine += "7";
				blOn = true;
				lastPress = 6;
			} else if (lastPress == 7) {
				bl.SetActive (true);
				currentLine += "7";
				blOn = true;
				lastPress = 6;
			}// Distance = 2
			else if (lastPress == 0) {
				LineMaker(3);
				bsl.SetActive (true);
				currentLine += "7";
				blOn = true;
				lastPress = 6;
			}
		}

		if (num == 7) {
			if (bmOn == true)
				return;
			// Distance = 1
			if (lastPress == 6) {
				bl.SetActive (true);
				currentLine += "8";
				bmOn = true;
				lastPress = 7;
			} else if (lastPress == 4) {
				bsm.SetActive (true);
				currentLine += "8";
				bmOn = true;
				lastPress = 7;
			}// Distance = 2
			else if (lastPress == 1) {
				LineMaker(4);
				bsm.SetActive (true);
				currentLine += "8";
				bmOn = true;
				lastPress = 7;
			}
		}

		if (num == 8) {

			if (brOn == true)
				return;
			// Distance = 1
			if (lastPress == 5) {
				bsr.SetActive (true);
				currentLine += "9";
				brOn = true;
				lastPress = 8;
			} else if (lastPress == 7) {
				br.SetActive (true);
				currentLine += "9";
				brOn = true;
				lastPress = 8;
			}// Distance = 2
			else if (lastPress == 6) {
				LineMaker(7);
				br.SetActive (true);
				currentLine += "9";
				bmOn = true;
				lastPress = 8;
			}
			else if (lastPress == 2) {
				LineMaker(5);
				bsr.SetActive (true);
				currentLine += "9";
				bmOn = true;
				lastPress = 8;
			}
		}
	}

	//TWITCH PLAYS SETUP HERE
#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"To press the grid buttons use !{0} press 1 [number from 1 to 9, 1 top-left corner, 9 bottom-right corner, in reading order]. Button presses can be chained. To submit use !{0} submit";
#pragma warning restore 414

	KMSelectable[] ProcessTwitchCommand(string command){
		command = command.ToLowerInvariant ().Trim ();
		if (command == "submit")
		{
			Debug.LogFormat ("[The Witness #{0}] Command Processed as submit.", _moduleId);
			return new[] { submit };
		}
//		else if (Regex.IsMatch (command, @"^press +\d$")) {
//			command = command.Substring(5).Trim();
//			Debug.LogFormat ("[The Witness #{0}] Command Processed as press {1}.", _moduleId,command);
//			return new[] { btn [int.Parse (command [0].ToString ()) - 1] };
//		}
		else if (Regex.IsMatch (command, @"^press(\s\d)+$")) {
			command = command.Substring(5).Trim();
			var inputs = command.Split (new[] { ' ', ' ', '|', '&' });
			var debugout = "";
			var output = new List<KMSelectable> {};
			for (int pos=0;pos<inputs.Length;pos++)
			{
				var buttonToPress = int.Parse (inputs [pos]);
				if (buttonToPress >=1 && buttonToPress <=9) {
					debugout += inputs[pos]+" ";
					output.Add(btn[int.Parse(inputs[pos])-1]);
				} else
				{
					Debug.LogFormat ("[The Witness #{0}] Found button press as invalid. For your safety, this full command is voided because of: {1}", _moduleId, inputs [pos]);
					return null;
				}


					
			}
			debugout.Trim();
			Debug.LogFormat ("[The Witness #{0}] Command Processed as press {1}", _moduleId,debugout);
			return (output.Count > 0) ? output.ToArray() : null;
		}
		Debug.LogFormat ("[The Witness #{0}] Unknown Command: {1}", _moduleId,command);
		return null;
	}

}

