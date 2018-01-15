using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour {

	GameObject[] objectives;
	GameObject tracker;

	void Start() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		setObjective (getObjective ("0"));
	}

	void Update() {
		
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
