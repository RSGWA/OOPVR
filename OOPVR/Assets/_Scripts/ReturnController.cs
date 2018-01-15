using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnController : MonoBehaviour {

	private GameObject hand;

	// Use this for initialization
	void Start () {
		hand = GameObject.FindGameObjectWithTag ("Hand");
	}

	public void checkAndReturn() {
		string returnType = transform.GetChild (0).tag;
		GameObject objInHand = hand.GetComponent<HandController> ().getObjInHand ();

		string varType;

		if (objInHand == null) {
			varType = "Void";
		} else {
			varType = objInHand.transform.GetChild (0).tag;
		}

		if (varType == returnType) {
			// Exit room
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().backToOrigin();
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
}
