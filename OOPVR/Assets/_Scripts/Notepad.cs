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

	void Awake() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		//setObjective (getObjective ("0"));
		code = this.transform.Find ("Code").gameObject;
		title = this.transform.Find ("Title").gameObject;

		main = Resources.Load (mainFilename) as TextAsset;
		defaultConstructor = Resources.Load (defaultConstructorFilename) as TextAsset;

		defaultConstructorText = defaultConstructor.text;
		mainText = main.text;

		activeText = MAIN;
		setTitle (activeText);

		//removeObjectiveMarkers (ref mainText);
		//removeObjectiveMarkers (ref defaultConstructorText);

		setText (mainText);
		//setText (defaultConstructorText);
	}

	void Update() {
		
	}

	public void highlightText(string text, string color) {
		switch (activeText) {
		case MAIN:
			int startIndex = mainText.IndexOf (text);
			int	endIndex = startIndex + text.Length;

			mainText = mainText.Insert (endIndex, "</color></b>");
			mainText = mainText.Insert (startIndex, "<b><color=" + color + ">");
			setText (mainText);
			break;
		case DEFAULT_CONSTRUCTOR:
			startIndex = defaultConstructorText.IndexOf (text);
			endIndex = startIndex + text.Length;

			defaultConstructorText = defaultConstructorText.Insert (endIndex, "</color></b>");
			defaultConstructorText = defaultConstructorText.Insert (startIndex, "<b><color=" + color + ">");
			setText (defaultConstructorText);
			break;
		default:
			break;
		}
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

		// Reset color to black
		switch (activeText) {
		case MAIN:
			mainText = main.text;
			setText (mainText);
			break;
		case DEFAULT_CONSTRUCTOR:
			defaultConstructorText = defaultConstructor.text;
			setText (defaultConstructor.text);
			break;
		default:
			break;
		}

		highlightText (objective, "white");
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

	public void setActiveText(string text) {
		activeText = text;
		// Change title to match code
		setTitle(activeText);
		switch (text) {
		case MAIN:
			setText (mainText);
			break;
		case DEFAULT_CONSTRUCTOR:
			setText (defaultConstructorText);
			break;
		default:
			break;
		}
	}

	public void endOfActivity() {
		setTitle ("Congratulations");
		setText ("Activity Completed");
	}
}
