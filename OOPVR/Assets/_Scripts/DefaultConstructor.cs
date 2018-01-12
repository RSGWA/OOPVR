using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultConstructor : MonoBehaviour {

	InstanceController instance;
	Notepad notepad;

	bool instanceCreated = false;

	// Use this for initialization
	void Start () {
		instance = GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();
	}
	
	// Update is called once per frame
	void Update () {
		instanceCreated = instance.hasInstanceBeenCreated ();

		if (instanceCreated) {
			notepad.updateObjective ();
			instanceCreated = false;
		}
	}
}
