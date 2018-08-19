using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

	private static GameObject objInHand;

	// Use this for initialization
	void Start () {
		objInHand = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject getObjInHand() {
		return objInHand;
	}

	public void setObjInHand(GameObject obj) {
		objInHand = obj;
	}
}
