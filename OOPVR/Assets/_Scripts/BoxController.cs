using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

	private GameObject Hand;
	private GameObject[] boxes;
	private GameObject objInHand;

	private AnimationCurve xCurve;
	private AnimationCurve yCurve;
	private AnimationCurve zCurve;
	private Keyframe[] ks;

	private AnimationCurve xRotCurve;
	private AnimationCurve yRotCurve;
	private AnimationCurve zRotCurve;

	private static float ANIM_LENGTH = 1.1f;

	float currentTime = 0;

	bool movingVarToBox = false;
	bool movingBoxToHand = false;
	bool movingBoxToBox = false;
	bool tipBox = false;

	bool variableInBox = false;

	// Use this for initialization
	void Start () {
		Hand = GameObject.FindGameObjectWithTag ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		//enableBoxes (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (movingVarToBox) {
			objInHand.transform.localPosition = new Vector3 (
				xCurve.Evaluate (Time.time - currentTime), 
				yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			
			if (Time.time - currentTime > ANIM_LENGTH) {
				movingVarToBox = false;
				objInHand.GetComponent<VariableController> ().setInHand (false);

				// Rotates object to stand up in box
				objInHand.transform.rotation = Quaternion.identity;

				objInHand.GetComponent<BoxCollider> ().enabled = true;
				objInHand.AddComponent<Rigidbody> ();
			
				variableInBox = true;
			}
		} else if (movingBoxToHand) {
			transform.localPosition = new Vector3 (
				xCurve.Evaluate (Time.time - currentTime), 
				yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			
			if (Time.time - currentTime > ANIM_LENGTH) {
				movingBoxToHand = false;
			}
		} else if (movingBoxToBox) {
			objInHand.transform.rotation = transform.rotation;
			objInHand.transform.localPosition = new Vector3 (
				xCurve.Evaluate (Time.time - currentTime), 
				yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));

			if (Time.time - currentTime > ANIM_LENGTH) {
				Debug.Log ("Animation Completed");
				movingBoxToBox = false;

				// Rotates box upright
				//objInHand.transform.rotation = transform.rotation;
				// Tip box
				tipBox = true;
				/*
				Vector3 rot = objInHand.transform.rotation.eulerAngles;
				rot = new Vector3 (rot.x + 180, rot.y, rot.z);
				objInHand.transform.rotation = Quaternion.Euler (rot);
				*/
			}
		}

		if (tipBox) {
			Vector3 to = new Vector3 (20, 20, 20);
			if (Vector3.Distance (objInHand.transform.eulerAngles, to) > 0.01f) {
				objInHand.transform.eulerAngles = Vector3.Lerp (objInHand.transform.rotation.eulerAngles, to, Time.deltaTime);
			} else {
				objInHand.transform.eulerAngles = to;
				tipBox = false;
			}
		}
			
	}

	public void boxAction()
	{
		currentTime = Time.time;
		objInHand = Hand.GetComponent<HandController> ().getObjInHand ();

		if (objInHand == null) {
			// Disable box in hand
			// GetComponent<BoxCollider>().enabled = false;

			transform.parent = Hand.transform;
			setUpBoxToHandAnimation ();
			Hand.GetComponent<HandController> ().setObjInHand (this.gameObject);
			movingBoxToHand = true;

		} else {
			string variableType = objInHand.transform.GetChild (0).tag;
			string boxType = transform.GetChild (0).tag;

			if (boxType == variableType) {
				Hand.GetComponent<HandController> ().setObjInHand (null);
				objInHand.transform.parent = this.transform;

				if (objInHand.tag == "Variable") {
					// Variable to box
					setUpVarToBoxAnimation ();
					movingVarToBox = true;

				} else if (objInHand.tag == "Box") {
					// Box to box
					setUpBoxToBoxAnimation ();
					movingBoxToBox = true;

				}
			} else {
				// Display type mismatch message or something
				enableBoxes (true);
			}
		}
	}

	void setUpVarToBoxAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0, objInHand.transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, 0);
		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);
		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 3.5f);
		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;
	}

	void setUpBoxToBoxAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0, objInHand.transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, 0);
		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);
		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 4f);
		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;
	}

	void setUpBoxToHandAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0,transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, 0);

		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);

		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);

		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;

	}

	void setUpVariableToHandAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0,transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, 0);

		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);

		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);

		zCurve = new AnimationCurve (ks);
		zCurve.postWrapMode = WrapMode.Once;

	}

	void rotateVarAnimation() 
	{
		Keyframe[] keys = new Keyframe[2];
		/*
		Vector3 rot = objInHand.transform.rotation.eulerAngles;
		rot = new Vector3 (rot.x + 180, rot.y, rot.z);
		objInHand.transform.rotation = Quaternion.Euler (rot);
		*/
		keys[0] = new Keyframe (0,objInHand.transform.rotation.x);
		keys[1] = new Keyframe (ANIM_LENGTH, 0);

		xRotCurve = new AnimationCurve (keys);
		xRotCurve.postWrapMode = WrapMode.Once;

		keys [0] = new Keyframe (0, objInHand.transform.rotation.y);
		keys [1] = new Keyframe (ANIM_LENGTH, 0);

		yRotCurve = new AnimationCurve (keys);
		yRotCurve.postWrapMode = WrapMode.Once;

		keys [0] = new Keyframe (0, objInHand.transform.rotation.z);
		keys [1] = new Keyframe (ANIM_LENGTH, 0);

		zRotCurve = new AnimationCurve (keys);
		zRotCurve.postWrapMode = WrapMode.Once;
	}

	public void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

	public bool isVarInBox() {
		return variableInBox;
	}
}
