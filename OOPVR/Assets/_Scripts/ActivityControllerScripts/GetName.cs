using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetName : MonoBehaviour {

	public GameObject name;

	InstanceController instance;
	Notepad notepad;
	PlayerController player;

	bool instanceCreated = false;
	bool methodEntered = false;
	bool returned = false;

	List<string> objectives = new List<string>();

	void Awake() {
		instance = GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		objectives.Add ("Person p = new Person(\"Gilbert\", 14)");
		objectives.Add ("string Person::getName() {");
		objectives.Add ("return this->name;");
		objectives.Add ("string pName = p.getName();");
	}

	// Use this for initialization
	void Start () {
		notepad.enlargeCurrentObjective(objectives[0]);
		GameObject.Find ("Name_Instance").GetComponent<VariableBoxController> ().setBoxAssigned(true);
		GameObject.Find ("Name_Instance").GetComponent<VariableBoxController> ().setVariableBoxValue (name);
		StartCoroutine ("checkInstanceCreated");
	}

	IEnumerator checkInstanceCreated() {
		while (!instanceCreated) {
			instanceCreated = instance.hasInstanceBeenCreated ();
			if (player.isInRoom ()) {
				instanceCreated = true;
			}
			yield return new WaitForSeconds (0.1f);
		}
		notepad.setActiveText (1);
		notepad.setTitle ("GET NAME");
		notepad.enlargeCurrentObjective(objectives[1]);
		StartCoroutine ("checkMethodEntered");
	}

	IEnumerator checkMethodEntered() {
		while (!methodEntered) {
			methodEntered = player.isInRoom ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.reset ();
		notepad.enlargeCurrentObjective(objectives[2]);
		StartCoroutine ("checkReturned");
	}

	IEnumerator checkReturned() {
		while (!returned) {
			returned = player.hasReturned ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.setActiveText (0);
		notepad.setTitle ("Main");
		notepad.reset ();
		notepad.enlargeCurrentObjective(objectives[3]);
		StartCoroutine ("checkNameAssigned");
	}

	IEnumerator checkNameAssigned() {
		while (!nameAssigned()) {
			yield return new WaitForSeconds (0.1f);
		}
		// Activity Finished
		notepad.endOfActivity();
	}

	bool nameAssigned() {
		VariableBoxController nameBox = GameObject.Find ("Name_Variable").GetComponent<VariableBoxController> ();

		return nameBox.isVarInBox ();
	}
}
