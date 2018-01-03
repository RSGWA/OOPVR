using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private GameObject Hand; 
	private GameObject[] boxes;

	private static GameObject objInHand;
	private static bool isObjInHand;

	void Start()
	{
		Hand = GameObject.Find ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		enableBoxes (false);
		isObjInHand = false;
	}

	void Update() {
		if (isObjInHand) {
			// Object follow reticle
		}
	}

	public void InHands()
	{
		transform.parent = Hand.transform.parent;
		transform.position = Hand.transform.position;
		objInHand = this.gameObject;
		isObjInHand = true;
		enableBoxes (true);
	}

	public void InBox()
	{
		objInHand.transform.parent = transform.parent;
		objInHand.transform.position = transform.position;
		objInHand.transform.rotation = transform.rotation;
		objInHand.transform.localScale = transform.localScale;
		
		enableBoxes (false);
		isObjInHand = false;
	}

	private void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

}
