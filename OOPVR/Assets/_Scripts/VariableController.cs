using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour {

	private GameObject Hand; 
	private GameObject[] vars;

	private AnimationCurve xCurve;
	private AnimationCurve yCurve;
	private AnimationCurve zCurve;
	private Keyframe[] ks;

	private static float ANIM_LENGTH = 1.0f;

	float currentTime;

	bool movingToHand;
	bool inHand = false;

	void Start () {
		Hand = GameObject.Find ("Hand");
		vars = GameObject.FindGameObjectsWithTag ("Variable");
		movingToHand = false;
		currentTime = 0;
		transform.Rotate (Vector3.right * Time.deltaTime);
	}

	void Update () {
		if (movingToHand) {
			transform.localPosition = new Vector3 (xCurve.Evaluate(Time.time - currentTime), yCurve.Evaluate(Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			// Signals end of animation
			if (Time.time - currentTime > ANIM_LENGTH) {
				movingToHand = false;
			}
		}

		if (inHand) {
			transform.rotation = Hand.transform.rotation;
		}
	}

	public void ToHands()
	{
		currentTime = Time.time;
		GameObject objInHand = Hand.GetComponent<HandController> ().getObjInHand ();

		// Checking if there is already a variable in hand - if so, swap them
		if (objInHand != null) {
			objInHand.transform.parent = transform.parent;
			transform.parent = Hand.transform;

			objInHand.transform.position = transform.position;
			transform.position = Hand.transform.position;
			enableVars (true);
		} else { 
			transform.parent = Hand.transform;
			setUpVariableToHandAnimation ();

			movingToHand = true;

			// Enable Boxes
			if (GameObject.FindGameObjectWithTag ("Box") != null) {
				GameObject.FindGameObjectWithTag ("Box").GetComponent<VariableBoxController> ().enableBoxes (true);
			}

			// Disable RigidBody if present on component
			if (GetComponent<Rigidbody> () != null) {
				Destroy (GetComponent<Rigidbody> ());
			}

			// Disable current variable in hand
			GetComponent<BoxCollider> ().enabled = false;
		}

		Hand.GetComponent<HandController> ().setObjInHand (this.gameObject);
		inHand = true;

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

	public void setInHand(bool b) {
		inHand = b;
	}

	void enableVars(bool enable) 
	{
		foreach (GameObject var in vars) {
			var.GetComponent<BoxCollider> ().enabled = enable;
		}
	}
}
