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

	//string defaultConstructorFilename = "defaultConstructor";
    string defaultConstructorFilename = "constructorWithParameter";

    string activeText;
	string highlightedText;

	string objectivesEnlargedText;

	TextAsset defaultConstructor;

	string defaultConstructorText;

	List<string> pages = new List<string>();

	int currentPage = 0;

	int fontSize = 160;

	void Awake() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		//setObjective (getObjective ("0"));
		code = this.transform.Find ("Code").gameObject;
		title = this.transform.Find ("Title").gameObject;

		// Load in text from textfile
		defaultConstructor = Resources.Load (defaultConstructorFilename) as TextAsset;

		defaultConstructorText = defaultConstructor.text;

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

		highlightedText = activeText;

		int startIndex = activeText.IndexOf (text);
		int	endIndex = startIndex + text.Length;

		activeText = activeText.Insert (endIndex, "</color>");
		activeText = activeText.Insert (startIndex, "<color=" + color + ">");
		setText (activeText);
	}

	public void enlargeText(string text) {

		int startIndex = activeText.IndexOf (text);
		int	endIndex = startIndex + text.Length;

        activeText = activeText.Insert (endIndex, "</size></b>");
		activeText = activeText.Insert (startIndex, "<b><size="+ fontSize +">");
		setText (activeText);
	}

	void setText(int pageNumber) {
		code.GetComponent<Text> ().text = pages [pageNumber];
		currentPage = pageNumber;
	}

	void setText(string text) {
		code.GetComponent<Text> ().text = text;
	}

	public void setTitle(string text) {
		title.GetComponent<Text> ().text = text;
	}

	public void highlightCurrentObjective(string objective) {
		highlightText (objective, "white");
	}

	public void enlargeCurrentObjective(string objective) {
		enlargeText (objective);
	}

	public void reset() {
		activeText = pages [currentPage];
		setText (currentPage);
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
		List<int> indexes = text.AllIndexsOf ("");

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
