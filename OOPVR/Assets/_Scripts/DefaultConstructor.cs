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
		notepad.highlightCurrentObjective (objectives [0]);
		StartCoroutine ("checkInstanceCreated");
	}

	IEnumerator checkInstanceCreated() {
		while (!instanceCreated) {
			instanceCreated = instance.hasInstanceBeenCreated ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.setActiveText (Notepad.DEFAULT_CONSTRUCTOR);
		notepad.resetHighlight ();
		notepad.highlightCurrentObjective (objectives [1]);
		StartCoroutine ("checkConstructorEntered");
	}

	IEnumerator checkConstructorEntered() {
		while (!constructorEntered) {
			constructorEntered = player.isInRoom ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.resetHighlight ();
		notepad.highlightCurrentObjective (objectives [2]);
		notepad.highlightCurrentObjective (objectives [3]);
		StartCoroutine ("checkInstanceVarsSet");
	}

	IEnumerator checkInstanceVarsSet() {
		while (!instanceVariablesSet()) {
			yield return new WaitForSeconds (0.1f);
		}
		notepad.resetHighlight ();
		notepad.highlightCurrentObjective (objectives [4]);
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
		VariableBoxController ageBox = GameObject.Find ("Age_InstanceBox").GetComponent<VariableBoxController> ();
		VariableBoxController nameBox = GameObject.Find ("Name_InstanceBox").GetComponent<VariableBoxController> ();

		return (ageBox.isVarInBox() && nameBox.isVarInBox());
	}
}
