using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OOPVR;

public class Notepad : MonoBehaviour {

	GameObject[] objectives;
	GameObject tracker;
	GameObject code;
	GameObject title;

	string mainFilename = "main";
	string defaultConstructorFilename = "defaultConstructor";

	public const string MAIN = "Main";
	public const string DEFAULT_CONSTRUCTOR = "Default Constructor";
	public const string OBJECTIVE_MARKER = "[";

	string activeText;

	TextAsset main;
	TextAsset defaultConstructor;

	string mainText; 
	string defaultConstructorText;

	List<string> pages = new List<string>();

	int currentPage = 0;

	void Awake() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		//setObjective (getObjective ("0"));
		code = this.transform.Find ("Code").gameObject;
		title = this.transform.Find ("Title").gameObject;

		// Load in text from textfile
		main = Resources.Load (mainFilename) as TextAsset;
		defaultConstructor = Resources.Load (defaultConstructorFilename) as TextAsset;

		defaultConstructorText = defaultConstructor.text;
		mainText = main.text;

		parseText (defaultConstructorText);

		activeText = pages[0]; // Main
		setTitle ("Main");

		//removeObjectiveMarkers (ref mainText);
		//removeObjectiveMarkers (ref defaultConstructorText);

		setText (0);
		//setText (defaultConstructorText);
	}

	void Update() {
		
	}

	void parseText(string text) {

		List<int> list = text.AllIndexsOf ("<p>");
		List<int> startIndices = new List<int>();
		foreach (int index in list) {
			startIndices.Add (index + 3);
		}
		List<int> endIndices = text.AllIndexsOf ("</p>");

		for (int i = 0; i < startIndices.Count; i++) {
			pages.Add (text.Substring (startIndices [i], endIndices [i] - startIndices [i]));
		}
	}

	public void highlightText(string text, string color) {
		activeText = pages [currentPage];

		int startIndex = activeText.IndexOf (text);
		int	endIndex = startIndex + text.Length;

		activeText = activeText.Insert (endIndex, "</color></b>");
		activeText = activeText.Insert (startIndex, "<b><color=" + color + ">");
		setText (activeText);
	}

	void setText(int pageNumber) {
		code.GetComponent<Text> ().text = pages [pageNumber];
		currentPage = pageNumber;
	}

	void setText(string text) {
		code.GetComponent<Text> ().text = text;
	}

	void setTitle(string text) {
		title.GetComponent<Text> ().text = text;
	}

	public void updateObjective(string objNumber) {
		//setObjective (getObjective (objNumber));
	}

	public void highlightCurrentObjective(string objective) {
		highlightText (objective, "white");
	}

	public void resetHighlight() {
		activeText = pages [currentPage];
		setText (currentPage);
	}

	void setObjective(GameObject objective) {
		/*
		tracker.transform.localPosition = new Vector3 (
			tracker.transform.localPosition.x, 
			objective.transform.localPosition.y,
			tracker.transform.localPosition.z);
			*/
	}

	GameObject getObjective(string number) {
		foreach (GameObject objective in objectives) {
			if (objective.name == number) {
				return objective;
			}
		}
		return null;
	}

	void removeObjectiveMarkers(ref string text) {
		List<int> indexes = text.AllIndexsOf (OBJECTIVE_MARKER);

		// Indices reversed so indices of markers remain the same after first removal
		indexes.Reverse ();

		foreach (int index in indexes) {
			text = text.Remove (index,3);
		}
	}

	public void setActiveText(int pageNumber) {
		activeText = pages [pageNumber];
		setText (pageNumber);
	}

	public void endOfActivity() {
		setTitle ("Congratulations");
		setText ("Activity Completed");
	}
}
