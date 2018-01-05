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

	bool movingToBox = false;

	// Use this for initialization
	void Start () {
		Hand = GameObject.FindGameObjectWithTag ("Hand");
		boxes = GameObject.FindGameObjectsWithTag ("Box");
		enableBoxes (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (movingToBox) {
			objInHand.transform.localPosition = new Vector3 (xCurve.Evaluate(Time.time - currentTime), yCurve.Evaluate(Time.time - currentTime), 
				zCurve.Evaluate (Time.time - currentTime));
			if (Time.time > currentTime + ANIM_LENGTH) {
				movingToBox = false;
				objInHand.GetComponent<VariableController> ().setInHand (false);

				// Rotates object to stand up in box
				objInHand.transform.rotation = Quaternion.identity;

				objInHand.GetComponent<BoxCollider> ().enabled = true;
				objInHand.AddComponent<Rigidbody> ();
				//objInHand.GetComponent<Rigidbody> ().useGravity = false;
				//objInHand.GetComponent<Rigidbody> ().AddForce (0f, -1.3f, 0f, ForceMode.Impulse);
			}
		}
	}

	public void ToBox()
	{
		
		//enableBoxes (false);

		objInHand = Hand.GetComponent<HandController> ().getObjInHand ();

		Hand.GetComponent<HandController> ().setObjInHand (null);

		objInHand.transform.parent = this.transform;
		setUpToBoxAnimation ();
		currentTime = Time.time;
		movingToBox = true;

	}

	void setUpToBoxAnimation() 
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

	public void enableBoxes(bool enable) 
	{
		foreach (GameObject box in boxes) {
			box.GetComponent<BoxCollider> ().enabled = enable;
		}
	}

}
