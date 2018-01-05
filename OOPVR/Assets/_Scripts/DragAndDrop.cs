using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private GameObject Hand; 
	private GameObject[] boxes;

	private static GameObject objInHand;
	private static Vector3 origRot;
	private static float ANIM_LENGTH = 1.2f;

	AnimationCurve xCurve;
	AnimationCurve yCurve;
	AnimationCurve zCurve;
	Keyframe[] ks;

	Quaternion originalRot;

	float currentTime = 0;

	bool moveToHand = false;
	bool inHand = false;

	void Start()
	{
		Hand = GameObject.Find ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		enableBoxes (false);
	}

	public void ToHands()
	{
		originalRot = transform.rotation;
		transform.parent = Hand.transform;
		currentTime = Time.time;
		setUpVariableToHandAnimation();
		objInHand = this.gameObject;
		enableBoxes (true);
		moveToHand = true;
		inHand = true;
	}

	public void ToBox()
	{
		inHand = false;

		objInHand.transform.parent = transform.parent;
		//objInHand.transform.position = transform.position;
		
		enableBoxes (false);
	}

	private void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

	void setUpVariableToHandAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0,transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, 0);
		//Debug.Log ("TARGET X: " + Hand.transform.localPosition.x);
		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);
		//Debug.Log ("TARGET Y: " + Hand.transform.localPosition.y);
		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);
		//Debug.Log ("TARGET Z: " + Hand.transform.localPosition.z);
		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;

	}

	void Update() {
		if (moveToHand) {
			transform.localPosition = new Vector3 (xCurve.Evaluate(Time.time - currentTime), yCurve.Evaluate(Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));

			if (Time.time > currentTime + ANIM_LENGTH) {
				moveToHand = false;
			}
		}

		if (inHand) {
			transform.rotation = Hand.transform.rotation;
		}
	}

}
