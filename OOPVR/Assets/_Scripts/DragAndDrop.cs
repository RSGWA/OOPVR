using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private GameObject Hand; 
	private GameObject[] boxes;

	private static GameObject objInHand;
	private static bool isObjInHand;

	AnimationCurve xCurve;
	AnimationCurve yCurve;
	AnimationCurve zCurve;
	Keyframe[] ks;

	//bool move;

	void Start()
	{
		Hand = GameObject.Find ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		setUpAnimation ();
		//move = false;
		enableBoxes (false);
		isObjInHand = false;
	}

	public void InHands()
	{
		//transform.parent = Hand.transform.parent;
		//transform.position = Hand.transform.position;
		moveToHand();
		Debug.Log (transform.position.x);
		Debug.Log (transform.position.y);
		Debug.Log (transform.position.z);
		objInHand = this.gameObject;
		isObjInHand = true;
		enableBoxes (true);
	}

	public void InBox()
	{
		objInHand.transform.parent = transform.parent;
		objInHand.transform.position = transform.position;
		objInHand.transform.rotation = transform.rotation;
		
		enableBoxes (false);
		isObjInHand = false;
	}

	private void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

	void setUpAnimation() 
	{
		ks = new Keyframe[2];

		Keyframe idle = new Keyframe (0,transform.position.x);
		Keyframe atHand = new Keyframe (100, Hand.transform.position.x);
		ks [0] = idle;
		ks [1] = atHand;

		xCurve = new AnimationCurve (ks);

		idle = new Keyframe (0, transform.position.y);
		atHand = new Keyframe (100, Hand.transform.position.y);
		ks [0] = idle;
		ks [1] = atHand;

		yCurve = new AnimationCurve (ks);

		idle = new Keyframe (0, transform.position.z);
		atHand = new Keyframe (100, Hand.transform.position.z);
		ks [0] = idle;
		ks [1] = atHand;

		zCurve = new AnimationCurve (ks);
	}

	void moveToHand() {
		transform.position = new Vector3 (xCurve.Evaluate (Time.time), yCurve.Evaluate (Time.time), zCurve.Evaluate (Time.time));
	}

}
