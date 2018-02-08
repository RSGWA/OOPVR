using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultConstructor : MonoBehaviour {

	InstanceController instance;
	Notepad notepad;
	PlayerController player;

	bool instanceCreated = false;
	bool constructorEntered = false;
	bool returned = false;

	List<string> objectives = new List<string>();

	void Awake() {
		instance = GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		objectives.Add ("Person p;");
		objectives.Add ("Person::Person() {");
		objectives.Add ("this->age = -1;");
		objectives.Add ("this->name = \"\";");
		objectives.Add ("}");
	}

	// Use this for initialization
	void Start () {
		notepad.enlargeCurrentObjective(objectives[0]);
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
		notepad.setTitle ("DEFAULT CONSTRUCTOR");
		notepad.enlargeCurrentObjective(objectives[1]);
		StartCoroutine ("checkConstructorEntered");
	}

	IEnumerator checkConstructorEntered() {
		while (!constructorEntered) {
			constructorEntered = player.isInRoom ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.reset ();
		notepad.enlargeCurrentObjective(objectives[2]);
		notepad.enlargeCurrentObjective(objectives[3]);
		StartCoroutine ("checkInstanceVarsSet");
	}

	IEnumerator checkInstanceVarsSet() {
		while (!instanceVariablesSet()) {
			yield return new WaitForSeconds (0.1f);
		}
		notepad.reset ();
		notepad.enlargeCurrentObjective(objectives[4]);
		StartCoroutine ("checkReturn");
	}

	IEnumerator checkReturn() {
		while (!returned) {
			returned = player.hasReturned ();
			yield return new WaitForSeconds (0.1f);
		}
		// Activity Finished
		notepad.endOfActivity();
	}

	bool instanceVariablesSet() {
		VariableBoxController ageBox = GameObject.Find ("Age_Instance").GetComponent<VariableBoxController> ();
		VariableBoxController nameBox = GameObject.Find ("Name_Instance").GetComponent<VariableBoxController> ();

		return (ageBox.isVarInBox() && nameBox.isVarInBox());
	}
}
