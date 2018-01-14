using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultConstructor : MonoBehaviour {

	InstanceController instance;
	Notepad notepad;
	PlayerController player;

	bool instanceCreated = false;
	bool constructorEntered = false;
	bool instanceVariablesChecked = false;
	bool checkReturn = false;

	// Use this for initialization
	void Start () {
		instance = GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!instanceCreated) {
			if (instance.hasInstanceBeenCreated ()) {
				notepad.updateObjective ();
				instanceCreated = true;
			}
		}

		if (!constructorEntered) {
			if (player.isInRoom()) {
				notepad.updateObjective ();
				constructorEntered = true;
			}
		}

		if (!instanceVariablesChecked) {
			if (instanceVariablesSet()) {
				notepad.updateObjective ();
				instanceVariablesChecked = true;
			}
		}

		if (!checkReturn) {
			if (player.hasReturned()) {
				notepad.updateObjective ();
				checkReturn = true;
			}
		}

	}

	bool instanceVariablesSet() {
		BoxController ageBox = GameObject.Find ("AgeBox").GetComponent<BoxController> ();
		BoxController nameBox = GameObject.Find ("NameBox").GetComponent<BoxController> ();

		return (ageBox.isVarInBox() && nameBox.isVarInBox());
	}
}
