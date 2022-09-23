using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class TheWitnessReimagined : MonoBehaviour {

	public KMAudio mAudio;
	public KMBombModule modSelf;
	public KMSelectable btnBase;
	public KMSelectable selfSelectable;
	public Transform btnsContainer;
	public MeshRenderer[] symbolsAll;
	public MeshRenderer wireRenderer, backingRenderer;
	public LineRenderer startLineRenderer, endLineRenderer, lineTraceRenderer, lineCenterTraceRenderer;
	//for symbol setup: 1-bsquare, 2-wsquare, 3-sun1, 4-sun2, 5-lpiece, 6-deleter
	private static int _moduleIdCounter = 1;
	private int _moduleId = 0;

	List<int> curPathDrawn;

	WitnessPuzzle currentPuzzle;
	

	public Material[] wireColors = new Material[3];
	public Material[] symbolMats = new Material[6];

	private int startIdx, endIdx;
	private bool _is3x3, interactable = false;

	List<List<int>> possibleValidPaths = new List<List<int>>();
	public TextAsset[] debugPuzzles;
	void QuickLogFormat(string value, params object[] args)
    {
		QuickLog(string.Format(value, args));
    }
	void QuickLogDebugFormat(string value, params object[] args)
    {
		QuickLogDebug(string.Format(value, args));
    }

	void QuickLog(string value = "")
    {
		Debug.LogFormat("[The Witness Reimagined #{0}] {1}", _moduleId, value);
    }
	void QuickLogDebug(string value = "")
    {
		Debug.LogFormat("<The Witness Reimagined #{0}> {1}", _moduleId, value);
    }

	void Start () // Use this for initialization
	{
		_moduleId = _moduleIdCounter++;
		curPathDrawn = new List<int>();
		Generate2x2Empty();
    }
	void Generate3x3Empty()
    {
		_is3x3 = true;
		var offsetValues3x3 = new[] { -1f, -1 / 3f, 1 / 3f, 1f };
		var allSelectables = new List<KMSelectable>() { btnBase };
		for (int x = 0; x < 4; x++)
		{
			for (int y = 0; y < 4; y++)
			{
				if (x == 0 && y == 0) continue;
				var clonedObject = Instantiate(btnBase);
				clonedObject.transform.parent = btnsContainer;
				clonedObject.name = btnBase.name + "(" + (3 * x + y).ToString() + ")";
				clonedObject.transform.localPosition = new Vector3(offsetValues3x3[y], 0, offsetValues3x3[x]);
				clonedObject.transform.localScale = btnBase.transform.localScale;
				clonedObject.Highlight.transform.localScale = btnBase.Highlight.transform.localScale;
				allSelectables.Add(clonedObject.GetComponent<KMSelectable>());
			}
		}

	}
	void Generate2x2Empty()
    {
		currentPuzzle = new WitnessPuzzle();
		var allSelectables = new List<KMSelectable>() { btnBase };
		var offsetValues = new[] { -1f, 0, 1f };
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
				if (x == 0 && y == 0) continue;
                var clonedObject = Instantiate(btnBase);
				clonedObject.transform.parent = btnsContainer;
				clonedObject.name = btnBase.name + "(" + (3 * x + y).ToString() + ")";
				clonedObject.transform.localPosition = new Vector3(offsetValues[y], 0, offsetValues[x]);
				clonedObject.transform.localScale = btnBase.transform.localScale;
				clonedObject.Highlight.transform.localScale = btnBase.Highlight.transform.localScale;
				allSelectables.Add(clonedObject.GetComponent<KMSelectable>());
			}
        }
		selfSelectable.Children = allSelectables.ToArray();
		selfSelectable.UpdateChildren();

		startLineRenderer.SetPositions(Enumerable.Repeat(new Vector3(offsetValues[startIdx % 3], offsetValues[startIdx / 3], 0), 2).ToArray());
        lineCenterTraceRenderer.SetPositions(Enumerable.Repeat(new Vector3(offsetValues[startIdx % 3], offsetValues[startIdx / 3], 0), 2).ToArray());
		var modifiedEndPoints = Enumerable.Repeat(new Vector3(offsetValues[endIdx % 3], offsetValues[endIdx / 3], 0), 2).ToArray();
		for (var x = 0; x < modifiedEndPoints.Length; x++)
        {
			modifiedEndPoints[x] += x == 0 ? Vector3.zero : (Vector3.right + Vector3.down) * 0.25f;
		}
		endLineRenderer.SetPositions(modifiedEndPoints);
		// Assign Selectables;
        for (var x = 0; x < allSelectables.Count; x++)
        {
			int y = x;
			allSelectables[x].OnInteract += delegate {
				if (interactable)
				{
					mAudio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, allSelectables[y].transform);
					if (y == startIdx && !curPathDrawn.Any())
					{
						curPathDrawn.Add(y);
					}
					else if (curPathDrawn.Any() && endIdx == curPathDrawn.Last())
					{
						
						if (possibleValidPaths.Any(a => a.SequenceEqual(curPathDrawn)))
						{
							QuickLogFormat("The drawn path [{0}] is one of the provided solutions. Module disarmed.", curPathDrawn.Select(a => a + 1).Join(","));
							modSelf.HandlePass();
							StartCoroutine(AnimateSolve());
							mAudio.PlaySoundAtTransform("disarmed", transform);
							interactable = false;
							return false;
						}
						else
						{
							QuickLogFormat("The drawn path [{0}] is not one of the provided solutions. Does it meet the symbol rules?", curPathDrawn.Select(a => a + 1).Join(","));
							if (CheckPath(possiblePath: curPathDrawn, logStatus: true))
							{
								QuickLog("The answer is yes. Module disarmed.");
								modSelf.HandlePass();
								StartCoroutine(AnimateSolve());
								mAudio.PlaySoundAtTransform("disarmed", transform);
								interactable = false;
							}
							else
                            {
								modSelf.HandleStrike();
								curPathDrawn.Clear();
                            }
							return false;
						}
					}
					else if (curPathDrawn.Any())
					{
						curPathDrawn.Clear();
					}
					UpdateLine();
				}
				return false;
			};
			allSelectables[x].OnHighlight += delegate {
				if (interactable)
				{
					if (curPathDrawn.Contains(startIdx))
					{
						if (!curPathDrawn.Contains(y))
						{
							var lastSpotX = curPathDrawn.LastOrDefault() % 3;
							var lastSpotY = curPathDrawn.LastOrDefault() / 3;
							var hlSpotX = y % 3;
							var hlSpotY = y / 3;
							var deltaModifierChecksAdjacent = new Dictionary<int, bool>()
							{
								{ -1, lastSpotX > 0 },
								{ 1, lastSpotX < 2 },
								{ -3, lastSpotY > 0 },
								{ 3, lastSpotY < 2 },
							};
							var passChecksAdjacent = false;
							foreach (KeyValuePair<int, bool> deltaModifier in deltaModifierChecksAdjacent)
							{
								passChecksAdjacent |= deltaModifier.Value && y - curPathDrawn.LastOrDefault() == deltaModifier.Key;
							}
							if (passChecksAdjacent)
							{
								curPathDrawn.Add(y);
							}
						}
						else if (curPathDrawn.Count > 1 && curPathDrawn.ElementAt(curPathDrawn.Count - 2) == y)
							curPathDrawn.RemoveAt(curPathDrawn.Count - 1);
					}
					UpdateLine();
				}
			};
		}

		startIdx = 6;
		endIdx = 2;
		StartCoroutine(GeneratePuzzle());
	}

	void UpdateLine()
    {
		if (curPathDrawn.Any())
        {
			var startEndPosAll = new List<Vector3>();
			var offsetValues2x2 = new[] { -1f, 0, 1f };
            var offsetValues3x3 = new[] { -1f, -1 / 3f, 1 / 3f, 1f };
			if (_is3x3)
            {

            }
			else
            {
				for (var x = 0; x < curPathDrawn.Count; x++)
                {
					var curX = curPathDrawn[x] % 3;
					var curY = curPathDrawn[x] / 3;
					startEndPosAll.Add(new Vector3(offsetValues2x2[curX], offsetValues2x2[curY], 0));
					if (x + 1 >= curPathDrawn.Count && curPathDrawn[x] == endIdx)
					{
						//startEndPosAll.AddRange(Enumerable.Repeat(new Vector3(offsetValues2x2[curX], offsetValues2x2[curY], 0), 2));
						startEndPosAll.Add(new Vector3(offsetValues2x2[curX] + 0.25f, offsetValues2x2[curY] - 0.25f, 0));
					}
				}
            }
			lineCenterTraceRenderer.enabled = true;
			lineTraceRenderer.positionCount = startEndPosAll.Count;
			lineTraceRenderer.SetPositions(startEndPosAll.ToArray());
        }
		else
		{
			lineCenterTraceRenderer.enabled = false;
			lineTraceRenderer.positionCount = 0;
		}

    }

	IEnumerator GeneratePuzzle()
    {
		var firstColor = backingRenderer.material.color;
		backingRenderer.material.color = Color.black;
		// Generate every single path from the start of the grid to the end of the grid.
		var allPossibleEndingPaths = new List<IEnumerable<int>>();
		var currentDrawnPaths = new List<IEnumerable<int>>();
		currentDrawnPaths.Add(new List<int> { startIdx });
		while (currentDrawnPaths.Any())
        {
			var nextDrawnPaths = new List<IEnumerable<int>>();
			for (var x = 0; x < currentDrawnPaths.Count; x++)
            {
				var curPathToTrace = currentDrawnPaths[x];
				var lastSpotX = curPathToTrace.LastOrDefault() % (_is3x3 ? 4 : 3);
				var lastSpotY = curPathToTrace.LastOrDefault() / (_is3x3 ? 4 : 3);
				var deltaModifierChecksAdjacent = new Dictionary<int, bool>()
					{
						{ -1, lastSpotX > 0 },
						{ 1, lastSpotX < (_is3x3 ? 3 : 2) },
						{ -(_is3x3 ? 4 : 3), lastSpotY > 0 },
						{ _is3x3 ? 4 : 3, lastSpotY < (_is3x3 ? 3 : 2) },
					};
				foreach (KeyValuePair<int,bool> deltaModifier in deltaModifierChecksAdjacent)
                {
					var newValueAfterDelta = curPathToTrace.LastOrDefault() + deltaModifier.Key;
					if (deltaModifier.Value && !curPathToTrace.Contains(newValueAfterDelta))
					{
						var validPath = curPathToTrace.Concat(new[] { newValueAfterDelta });
						if (validPath.LastOrDefault() == endIdx)
							allPossibleEndingPaths.Add(validPath);
						else
							nextDrawnPaths.Add(validPath);
					}
                }
				yield return null;
			}
			currentDrawnPaths.Clear();
			currentDrawnPaths.AddRange(nextDrawnPaths);
			yield return null;
        }
		QuickLogDebugFormat("All possible paths: {0}", allPossibleEndingPaths.Select(a => "[" + a.Join(",") + "]").Join(";"));
		
		var iterationCount = 0;
		do
		{
			// Generate a random puzzle with the valid symbols.
			var offsetValuesAll2x2 = new[] { -1f, -0.5f, 0, 0.5f, 1f };
			var offsetValuesAll3x3 = new[] { -1f, -2 / 3f, -1 / 3f, 0, 1 / 3f, 2 / 3f, 1f };
			var generatedSymbols = new WitnessSymbol[_is3x3 ? 49 : 25];
			var generatedMetadata = new string[_is3x3 ? 49 : 25];
			for (var x = 0; x < generatedSymbols.Length; x++)
			{
				var curX = x % (_is3x3 ? 7 : 5);
				var curY = x / (_is3x3 ? 7 : 5);
			}

			// Check if any of the provided paths 
			for (var x = 0; x < allPossibleEndingPaths.Count; x++)
            {
				var curPath = allPossibleEndingPaths[x];

				if (CheckPath(curPath))
					possibleValidPaths.Add(curPath.ToList());
            }
			yield return null;
		}
		while (!possibleValidPaths.Any() && iterationCount < 2);

		interactable = true;

		var flickerTimings = new[] { 0.1f, 0.5f, 0.1f, 0.4f, 0.1f, 0.3f };
        for (var x = 0; x < flickerTimings.Length; x++)
        {
			yield return new WaitForSeconds(flickerTimings[x]);
			backingRenderer.material.color = x % 2 == 0 ? Color.black : firstColor;
		}
		backingRenderer.material.color = firstColor;
		yield break;
    }

	IEnumerator AnimateSolve()
    {
		wireRenderer.material = wireColors[1];
		for (float t = 0; t < 1f; t += Time.deltaTime * 1)
        {
			var transformingColor = new Color(t, t, 1f);

			lineCenterTraceRenderer.startColor = transformingColor;
			lineCenterTraceRenderer.endColor = transformingColor;
			lineTraceRenderer.startColor = transformingColor;
			lineTraceRenderer.endColor = transformingColor;
			yield return null;
		}
		lineCenterTraceRenderer.startColor = Color.white;
		lineCenterTraceRenderer.endColor = Color.white;
		lineTraceRenderer.startColor = Color.white;
		lineTraceRenderer.endColor = Color.white;
		
	}

	bool CheckPath(IEnumerable<int> possiblePath, bool logStatus = false)
    {
		bool allCorrect = true;
		List<List<int>> groupedSymbolIdxes = new List<List<int>>();
		var connectionsIdx = new int[_is3x3 ? 49 : 25];


		return allCorrect;
    }

}

