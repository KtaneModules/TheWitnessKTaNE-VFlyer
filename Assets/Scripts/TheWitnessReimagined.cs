using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;



public class WitnessReImagined : MonoBehaviour {

	public KMAudio mAudio;
	public KMBombModule modSelf;
	public KMSelectable[] btn;
	public KMSelectable submit;
    public MeshRenderer[] symbolsAll;
    //for symbol setup: 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter
	private static int _moduleIdCounter = 1;
	private int _moduleId = 0;

	private int lastPress = 0;

    private bool[] interOn = new[] { false, false, false, false, false, false, false, false };

	private int symbolRandomizer = 0;

	public Material[] wireColors = new Material[3];
	public Material[] symbolMats = new Material[6];

	public int[] SymbolsActive;


	void QuickLogFormat(string value, object[] args)
    {
		QuickLog(string.Format(value, args));
    }

	void QuickLog(string value = "")
    {
		Debug.LogFormat("[The Witness Reimagined #{0}] {1}", _moduleId, value);
    }

	void Start () // Use this for initialization
	{
		_moduleId = _moduleIdCounter++;
    }

	bool CheckPath(IEnumerable<int> possiblePath)
    {
		return false;
    }

}

