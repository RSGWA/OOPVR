using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour {

	GameObject[] objectives;
	GameObject tracker;
	int currentObjective = 0;

	void Start() {
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		tracker = GameObject.FindGameObjectWithTag ("Tracker");
		setObjective ();
	}

	void Update() {
		
	}

	public void updateObjective() {
		currentObjective++;
		if (currentObjective > objectives.Length) {
			// Activity Finished
		} else {
			setObjective ();
		}
	}

	void setObjective() {
		tracker.transform.localPosition = new Vector3 (
			tracker.transform.localPosition.x, 
			objectives [currentObjective].transform.localPosition.y,
			tracker.transform.localPosition.z);
	}
		

}
