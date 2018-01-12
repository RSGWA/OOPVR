using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour {

	GameObject objective;
	string[] objectives;
	int currentObjective = 0;

	void Start() {
		objective = GameObject.FindGameObjectWithTag ("Objective");

		objectives = new string[4];
		objectives[0] = "Create instance";
		objectives[1] = "Enter default constructor";
		objectives[2] = "Set name and age";
		objectives[3] = "Return";

		setObjective (objectives [currentObjective]);
	}

	void Update() {
		
	}

	public void updateObjective() {
		currentObjective++;
		if (currentObjective > 3) {
			Debug.Log ("Activity Finished");
		} else {
			setObjective (objectives [currentObjective]);
		}
	}

	void setObjective(string text) {
		objective.GetComponent<Text> ().text = text;
	}

}
