using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	private GameObject Hand; 
	private GameObject[] boxes;

	private static GameObject objInHand;
	private static float ANIM_LENGTH = 1.2f;

	AnimationCurve xCurve;
	AnimationCurve yCurve;
	AnimationCurve zCurve;
	Keyframe[] ks;

	float currentTime = 0;

	bool moveToHand = false;

	void Start()
	{
		Hand = GameObject.Find ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		enableBoxes (false);
	}

	public void InHands()
	{
		transform.parent = Hand.transform;
		setUpAnimation();
		currentTime = Time.time;
		objInHand = this.gameObject;
		enableBoxes (true);
		moveToHand = true;
	}

	public void InBox()
	{
		moveToHand = false;
		objInHand.transform.parent = transform.parent;
		objInHand.transform.position = transform.position;
		objInHand.transform.rotation = transform.rotation;
		
		enableBoxes (false);
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

		ks[0] = new Keyframe (0,transform.position.x);
		ks[1] = new Keyframe (ANIM_LENGTH, Hand.transform.position.x);
		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.position.y);
		ks [1] = new Keyframe (ANIM_LENGTH, Hand.transform.position.y);
		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.position.z);
		ks [1] = new Keyframe (ANIM_LENGTH, Hand.transform.position.z);
		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;

	}

	void Update() {
		if (moveToHand) {
			transform.position = new Vector3 (xCurve.Evaluate(Time.time - currentTime), yCurve.Evaluate(Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time >= currentTime + ANIM_LENGTH) {
				moveToHand = false;
			}
		}
	}

}
