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

	private static float ANIM_LENGTH = 1f;

	float currentTime = 0;

	bool movingVarToBox = false;
	bool movingBoxToHand = false;
	bool movingBoxToBox = false;

	// Use this for initialization
	void Start () {
		Hand = GameObject.FindGameObjectWithTag ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		//enableBoxes (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (movingVarToBox) {
			objInHand.transform.localPosition = new Vector3 (xCurve.Evaluate (Time.time - currentTime), yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time > currentTime + ANIM_LENGTH) {
				movingVarToBox = false;
				objInHand.GetComponent<VariableController> ().setInHand (false);

				// Rotates object to stand up in box
				objInHand.transform.rotation = Quaternion.identity;

				objInHand.GetComponent<BoxCollider> ().enabled = true;
				objInHand.AddComponent<Rigidbody> ();
				//objInHand.GetComponent<Rigidbody> ().useGravity = false;
				//objInHand.GetComponent<Rigidbody> ().AddForce (0f, -1.3f, 0f, ForceMode.Impulse);
			}
		} else if (movingBoxToHand) {
			transform.localPosition = new Vector3 (xCurve.Evaluate (Time.time - currentTime), yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time > currentTime + ANIM_LENGTH) {
				movingBoxToHand = false;
			}
		} else if (movingBoxToBox) {
			objInHand.transform.localPosition = new Vector3 (xCurve.Evaluate (Time.time - currentTime), yCurve.Evaluate (Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time > currentTime + ANIM_LENGTH) {
				movingBoxToBox = false;

				// Rotates box upright
				objInHand.transform.rotation = transform.rotation;
				// Tip box
				Vector3 rot = objInHand.transform.rotation.eulerAngles;
				rot = new Vector3 (rot.x + 180, rot.y, rot.z);
				objInHand.transform.rotation = Quaternion.Euler (rot);
			}
		}

	}

	public void boxAction()
	{
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
					//enableBoxes (false);
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
		currentTime = Time.time;
	}

	void setUpVarToBoxAnimation() 
	{
		ks = new Keyframe[2];

		ks[0] = new Keyframe (0, objInHand.transform.localPosition.x);
		ks[1] = new Keyframe (ANIM_LENGTH, -0.5f);
		xCurve = new AnimationCurve (ks);
		xCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.y);
		ks [1] = new Keyframe (ANIM_LENGTH, 0);
		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

		ks [0] = new Keyframe (0, objInHand.transform.localPosition.z);
		ks [1] = new Keyframe (ANIM_LENGTH, 2.5f);
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

	public void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

}
