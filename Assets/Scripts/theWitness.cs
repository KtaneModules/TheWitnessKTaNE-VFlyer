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

	private int lastPress = 0;

	private bool[] interOn = new[] {false,false,false,false,false,false,false,false};

	private int symbolRandomizer = 0;

	private string currentLine;
	private string correctLine;
	private string alternativeLine;



	void Start () // Use this for initialization
	{
		_moduleId = _moduleIdCounter++;
		Module.OnActivate += Activate;

		var listobjects = new[] {tl,tr,tsl,tsm,tsr,ml,mr,bsl,bsm,bsr,bl,br};
		for (int x=0;x < listobjects.Length;x++)
		{
			listobjects [x].SetActive (false);
		}
		wireGray.SetActive (true);
		wireGreen.SetActive (false);
		wireRed.SetActive (false);

		lpiece_bl.transform.Rotate (new Vector3 (0, 90 * Random.Range (0, 4), 0));
		lpiece_br.transform.Rotate (new Vector3 (0, 90 * Random.Range (0, 4), 0));
		lpiece_tl.transform.Rotate (new Vector3 (0, 90 * Random.Range (0, 4), 0));
		lpiece_tr.transform.Rotate (new Vector3 (0, 90 * Random.Range (0, 4), 0));
		SetupSolution (Random.Range (1, 44));
	}

	void Activate()
	{
		Init();
		_lightsOn = true;
	}


	void Init()
	{
		currentLine = "1";
		//SetupSolution (Random.Range (1, 44));
	}

	void Awake(){
		submit.OnInteract += delegate ()
		{
			Check();
			updateLine (currentLine);
			return false;
		};

		for (int i = 0; i < 9; i++)
		{
			int k = i;
			btn[i].OnInteract += delegate ()
			{
				LineMaker(k);
				updateLine (currentLine);
				return false;
			};
		}
	}

	//for symbol setup (in puzzle#Array): 0-empty, 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter (order tl, tr, bl, br)
	void SetupSolution(int puzzleId){
		if (puzzleId >= 1 && puzzleId < 10) {
			correctLine = "14789";
			alternativeLine = "12369";

			symbolRandomizer = Random.Range (0, 20) * 4;
			int[] puzzle1Array = new int[] {3,4,4,3, 4,3,3,4, 3,6,4,3, 6,3,3,4, 3,4,6,3, 4,3,3,6, 4,6,3,4, 6,4,4,3, 4,3,6,4, 3,4,4,6, 1,1,1,1, 2,2,2,2, 5,3,3,6, 3,6,5,3, 6,3,3,5, 3,5,6,3, 5,4,4,6, 4,6,5,4, 6,4,4,5, 4,5,6,4};
			SetupSymbols (puzzle1Array [symbolRandomizer], puzzle1Array [symbolRandomizer + 1], puzzle1Array [symbolRandomizer + 2], puzzle1Array [symbolRandomizer + 3]);

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 4, 7, 8, 9' or '1, 2, 3, 6, 9'", _moduleId);

		} else if (puzzleId >= 10 && puzzleId < 18) {
			correctLine = "12569";

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections:  '1, 2, 5, 6, 9'", _moduleId);

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle2Array = new int[] {5,2,1,1, 5,1,2,2, 1,2,5,1, 2,1,5,2, 1,2,1,5, 2,1,2,5, 1,2,0,1, 2,1,0,2, 1,2,1,1, 2,1,2,2, 3,0,5,3, 5,0,3,3, 3,0,3,5, 4,0,5,4, 5,0,4,4, 4,0,4,5};
			SetupSymbols (puzzle2Array [symbolRandomizer], puzzle2Array [symbolRandomizer + 1], puzzle2Array [symbolRandomizer + 2], puzzle2Array [symbolRandomizer + 3]);
		} else if (puzzleId >= 18 && puzzleId < 26) {
			correctLine = "14589";
			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle3Array = new int[] {1,1,2,5, 2,2,1,5, 1,5,2,1, 2,5,1,2, 5,1,2,1, 5,2,1,2, 1,0,2,1, 2,0,1,2, 1,1,2,1, 2,2,1,2, 3,5,0,3, 3,3,0,5, 5,3,0,3, 4,5,0,4, 4,4,0,5, 5,4,0,4};
			SetupSymbols (puzzle3Array [symbolRandomizer], puzzle3Array [symbolRandomizer + 1], puzzle3Array [symbolRandomizer + 2], puzzle3Array [symbolRandomizer + 3]);

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections:  '1, 4, 5, 8, 9'", _moduleId);

		} else if (puzzleId >= 26 && puzzleId < 34) {
			correctLine = "1478569";
			alternativeLine = "1236589";

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle4Array = new int[] {1,5,1,2, 2,5,2,1, 5,1,1,2, 5,2,2,1, 1,1,5,2, 2,2,5,1, 0,1,1,2, 0,2,2,1, 1,1,1,2, 2,2,2,1, 5,3,3,0, 3,5,3,0, 3,3,5,0, 5,4,4,0, 4,5,4,0, 4,4,5,0};
			SetupSymbols (puzzle4Array [symbolRandomizer], puzzle4Array [symbolRandomizer + 1], puzzle4Array [symbolRandomizer + 2], puzzle4Array [symbolRandomizer + 3]);

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 4, 7, 8, 5, 6, 9' or '1, 2, 3, 6, 5, 8, 9'", _moduleId);

		} else if (puzzleId >= 34 && puzzleId < 42) {
			correctLine = "1254789";
			alternativeLine = "1452369";

			symbolRandomizer = Random.Range (0, 16) * 4;
			int[] puzzle5Array = new int[] {2,1,5,1, 1,2,5,2, 2,1,1,5, 1,2,2,5, 2,5,1,1, 1,5,2,2, 2,1,1,0, 1,2,2,0, 2,1,1,1, 1,2,2,2, 0,3,3,5, 0,3,5,3, 0,5,3,3, 0,4,4,5, 0,4,5,4, 0,5,4,4};
			SetupSymbols (puzzle5Array [symbolRandomizer], puzzle5Array [symbolRandomizer + 1], puzzle5Array [symbolRandomizer + 2], puzzle5Array [symbolRandomizer + 3]);

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 2, 5, 4, 7, 8, 9' or '1, 4, 5, 2, 3, 6, 9'", _moduleId);

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

			symbolRandomizer = Random.Range (0, 2) * 4;
			int[] puzzle7Array = new int[] { 1,1,2,2, 2,2,1,1 };
			SetupSymbols (puzzle7Array [symbolRandomizer], puzzle7Array [symbolRandomizer + 1], puzzle7Array [symbolRandomizer + 2], puzzle7Array [symbolRandomizer + 3]);

			Debug.LogFormat ("[The Witness #{0}] Correct line crosses these intersections: '1, 2, 3, 6, 5, 4, 7, 8, 9' or '1, 4, 5, 6, 9'", _moduleId);

		} else {
			Debug.LogFormat ("[The Witness #{0}] The module was unable to generate a puzzle with a reasonable solution. Please report this issue to VFlyer or bmo22xdd on Discord as this is a serious issue.", _moduleId);
			correctLine = "1";
			Debug.LogFormat ("[The Witness #{0}] To solve the module in this state without commands, press ONLY the submit button upon getting a board without a solution.", _moduleId);
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
		Debug.LogFormat ("[The Witness #{0}] Lblk = L Piece", _moduleId);
		Debug.LogFormat ("[The Witness #{0}] delr = Deleter", _moduleId);

		var SymbolsTLAll = new[] {bsquare_tl,wsquare_tl,sun1_tl,sun2_tl,lpiece_tl,deleter_tl};
		for (int v = 0; v < SymbolsTLAll.Length; v++)
		{
			if (v == Symboltl-1) {
				SymbolsTLAll [v].SetActive (true);
			}
			else
			{
				SymbolsTLAll [v].SetActive (false);
			}
		}
		var SymbolsTRAll = new[] {bsquare_tr,wsquare_tr,sun1_tr,sun2_tr,lpiece_tr,deleter_tr};
		for (int v = 0; v < SymbolsTRAll.Length; v++)
		{
			if (v == Symboltr-1) {
				SymbolsTRAll [v].SetActive (true);
			}
			else
			{
				SymbolsTRAll [v].SetActive (false);
			}
		}
		var SymbolsBLAll = new[] {bsquare_bl,wsquare_bl,sun1_bl,sun2_bl,lpiece_bl,deleter_bl};
		for (int v = 0; v < SymbolsBLAll.Length; v++)
		{
			if (v == Symbolbl-1) {
				SymbolsBLAll [v].SetActive (true);
			}
			else
			{
				SymbolsBLAll [v].SetActive (false);
			}
		}
		var SymbolsBRAll = new[] {bsquare_br,wsquare_br,sun1_br,sun2_br,lpiece_br,deleter_br};
		for (int v = 0; v < SymbolsBRAll.Length; v++)
		{
			if (v == Symbolbr-1) {
				SymbolsBRAll [v].SetActive (true);
			}
			else
			{
				SymbolsBRAll [v].SetActive (false);
			}
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
	//	Actual grid used for logging:
	//	
	//	1--2--3
	//	|  |  |
	//	|  |  |
	//	4--5--6
	//	|  |  |
	//	|  |  |
	//	7--8--9
	//

	void updateLine(string line)
	{
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
		if (Regex.IsMatch (line, "(12)"))
			tl.SetActive (true);

		if (Regex.IsMatch (line, "(23)") || Regex.IsMatch (line, "(32)"))
			tr.SetActive (true);

		if (Regex.IsMatch (line, "(14)"))
			tsl.SetActive (true);

		if (Regex.IsMatch (line, "(25)") || Regex.IsMatch (line, "(52)"))
			tsm.SetActive (true);

		if (Regex.IsMatch (line, "(36)") || Regex.IsMatch (line, "(63)"))
			tsr.SetActive (true);

		if (Regex.IsMatch (line, "(45)") || Regex.IsMatch (line, "(54)"))
			ml.SetActive (true);

		if (Regex.IsMatch (line, "(56)") || Regex.IsMatch (line, "(65)"))
			mr.SetActive (true);

		if (Regex.IsMatch (line, "(47)") || Regex.IsMatch (line, "(74)"))
			bsl.SetActive (true);

		if (Regex.IsMatch (line, "(58)") || Regex.IsMatch (line, "(85)"))
			bsm.SetActive (true);
		
		if (Regex.IsMatch (line, "(69)"))
			bsr.SetActive (true);

		if (Regex.IsMatch (line, "(78)") || Regex.IsMatch (line, "(87)"))
			bl.SetActive (true);

		if (Regex.IsMatch (line, "(89)"))
			br.SetActive (true);
	}

	void LineMaker(int num){

		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn[num].transform);

		if (!_lightsOn || _isSolved) return;

		if (num <= 0 || num - 1 >= interOn.Length) {

			for (int x = 0; x < interOn.Length; x++) {
				interOn [x] = false;
			}

			currentLine = "1";
			updateLine (currentLine);
			lastPress = 0;
		} else if (interOn [num - 1] || lastPress == 8)
			return;
		else {
			if ((Mathf.Abs (num - lastPress) == 1 && (num % 3 != 0 || lastPress % 3 != 0)) || Mathf.Abs (num - lastPress) == 3) {
				
				currentLine += (num + 1);
				interOn [num - 1] = true;
				//Debug.LogFormat ("[The Witness #{0}] Successfully Connected {1} to {2}", _moduleId,lastPress,num);
				lastPress = num;

			}
			else if (Mathf.Abs (num - lastPress) == 2 && (lastPress+1) % 3 != 0 && (lastPress+2)%3 != 0) {
				LineMaker (num - 1);
				currentLine += (num + 1);
				interOn [num - 1] = true;
				//Debug.LogFormat ("[The Witness #{0}] Successfully Connected {1} to {2}", _moduleId,lastPress,num);
				lastPress = num;
			}
			else if (Mathf.Abs(num - lastPress) == 6 && lastPress!=8) {
				LineMaker (num - 3);
				currentLine += (num + 1);
				interOn [num - 1] = true;
				//Debug.LogFormat ("[The Witness #{0}] Successfully Connected {1} to {2}", _moduleId,lastPress,num);
				lastPress = num;
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
			//Debug.LogFormat ("[The Witness #{0}] Command Processed as submit.", _moduleId);
			return new[] { submit };
		}
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
					//Debug.LogFormat ("[The Witness #{0}] Found button press as invalid. For your safety, this full command is voided because of: {1}", _moduleId, inputs [pos]);
					//var error = new System.FormatException("Detected an invalid button press. Your command has been voided.");
					//throw(error);
					return null;
				}


					
			}
			debugout.Trim();
			//Debug.LogFormat ("[The Witness #{0}] Command Processed as press {1}", _moduleId,debugout);
			return (output.Count > 0) ? output.ToArray() : null;
		}
		//Debug.LogFormat ("[The Witness #{0}] Unknown Command: {1}", _moduleId,command);
		return null;
	}

}

