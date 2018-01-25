using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour {

	GameObject[] objectives;
	GameObject tracker;
	GameObject code;

	string mainFilename = "main";
	string defaultConstructorFilename = "defaultConstructor";

	TextAsset main;
	TextAsset defaultConstructor;

	string mainText; 
	string defaultConstructorText;

	void Start() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		setObjective (getObjective ("0"));
		code = this.transform.Find ("Code").gameObject;

		main = Resources.Load (mainFilename) as TextAsset;
		defaultConstructor = Resources.Load (defaultConstructorFilename) as TextAsset;

		defaultConstructorText = defaultConstructor.text;
		mainText = main.text;

		setText (mainText);
		setText (defaultConstructorText);
	}

	void Update() {
		
	}

	public void highlightText(string text, string color) {
		int startIndex = mainText.IndexOf (text);
		int	endIndex = startIndex + text.Length;

		mainText = mainText.Insert (endIndex, "</color>");
		mainText = mainText.Insert (startIndex, "<color=" + color + ">");
	}

	void setText(string text) {
		code.GetComponent<Text> ().text = text;
	}
	public void updateObjective(string objNumber) {
		setObjective (getObjective (objNumber));
	}

	void setObjective(GameObject objective) {
		tracker.transform.localPosition = new Vector3 (
			tracker.transform.localPosition.x, 
			objective.transform.localPosition.y,
			tracker.transform.localPosition.z);
	}

	GameObject getObjective(string number) {
		foreach (GameObject objective in objectives) {
			if (objective.name == number) {
				return objective;
			}
		}
		return null;
	}
		

}
