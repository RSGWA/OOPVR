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

	private static float ANIM_LENGTH = 1.1f;

	float currentTime = 0;

	bool movingVarToBox = false;
	bool movingBoxToHand = false;
	bool movingBoxToBox = false;

	bool removingVariable = false;
	bool varRemoved = false;

	bool tipBox = false;

	bool boxAssigned = false;
	GameObject variableInBox;

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
			
				boxAssigned = true;
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

				tipBox = true;
			}
		} else if (removingVariable) {
			variableInBox.transform.position = new Vector3 (
				variableInBox.transform.position.x, 
				yCurve.Evaluate (Time.time - currentTime), 
				variableInBox.transform.position.z);
			if (Time.time - currentTime > ANIM_LENGTH) {
				variableInBox.transform.parent = null;
				Destroy (variableInBox);
				removingVariable = false;
				varRemoved = true;
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
			transform.parent = Hand.transform;
			setUpBoxToHandAnimation ();
			Hand.GetComponent<HandController> ().setObjInHand (this.gameObject);
			movingBoxToHand = true;

		} else {
			
			if (boxAssigned) { // A variable is already assigned
				Debug.Log ("Variable already assigned");
				setUpRemovingVarAnimation ();
				removingVariable = true;
				StartCoroutine ("removeVariableAndAct");
			} else {
				action ();
			}
		}
	}

	void action() {
		string variableType = objInHand.transform.GetChild (0).tag;
		string boxType = transform.GetChild (0).tag;

		currentTime = Time.time;
		objInHand.transform.parent = this.transform;
		if (boxType == variableType) {
			if (objInHand.tag == "Variable") {
				// Variable to box
				setUpVarToBoxAnimation ();
				movingVarToBox = true;
				variableInBox = objInHand;
				Hand.GetComponent<HandController> ().setObjInHand (null); // Object no longer in hand
			} else if (objInHand.tag == "Box") {
				// Copying value of one box to another
				setUpBoxToBoxAnimation ();
				movingBoxToBox = true;

			} else {
				// Display type mismatch message or something
				enableBoxes (true);
			}
		}
	}

	IEnumerator removeVariableAndAct() {
		while (!varRemoved) {
			yield return new WaitForSeconds (0.1f);
		}
		action ();
		varRemoved = false;
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

	void setUpRemovingVarAnimation() 
	{
		ks = new Keyframe[2];

		ks [0] = new Keyframe (0, variableInBox.transform.position.y);
		ks [1] = new Keyframe (ANIM_LENGTH, variableInBox.transform.position.y + 1f);

		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

	}

	public void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

	public bool isVarInBox() {
		return boxAssigned;
	}
}
