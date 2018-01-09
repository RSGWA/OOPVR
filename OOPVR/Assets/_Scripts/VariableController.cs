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
	private static GameObject objInHand;

	float currentTime;

	bool movingToHand;
	bool inHand = false;

	// Use this for initialization
	void Start () {
		Hand = GameObject.Find ("Hand");
		movingToHand = false;
		currentTime = 0;
	}
	
	// Update is called once per frame

	void Update () {
		if (movingToHand) {
			transform.localPosition = new Vector3 (xCurve.Evaluate(Time.time - currentTime), yCurve.Evaluate(Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time > currentTime + ANIM_LENGTH) {
				movingToHand = false;
			}
		}

		if (inHand) {
			transform.rotation = Hand.transform.rotation;
		}
	}

	public void ToHands()
	{
		transform.parent = Hand.transform;

		Hand.GetComponent<HandController> ().setObjInHand (this.gameObject);
		setUpVariableToHandAnimation ();

		currentTime = Time.time;
		movingToHand = true;
		inHand = true;

		// Enable Boxes
		GameObject.FindGameObjectWithTag ("Box").GetComponent<BoxController>().enableBoxes(true);

		// Disable variable
		GetComponent<BoxCollider>().enabled = false;
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

}
