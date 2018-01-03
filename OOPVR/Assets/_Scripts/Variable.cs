using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : MonoBehaviour {
	private AnimationCurve anim;
	AnimationCurve yAnim;
	AnimationCurve xAnim;

	private Keyframe[] ks;

	public GameObject Hand;

	void Start() {

		Hand = GameObject.FindGameObjectWithTag ("Hand");
		ks = new Keyframe[2];

		ks [0] = new Keyframe (0, transform.position.z);
		ks [1] = new Keyframe (3, Hand.transform.position.z);
		anim = new AnimationCurve (ks);

		ks [0] = new Keyframe (0, transform.position.y);
		ks [1] = new Keyframe (3, Hand.transform.position.y);
		yAnim = new AnimationCurve (ks);

		ks [0] = new Keyframe (0, transform.position.x);
		ks [1] = new Keyframe (3, Hand.transform.position.x);
		xAnim = new AnimationCurve (ks);
	}

	void Update() {
		transform.position = new Vector3 (xAnim.Evaluate(Time.time), yAnim.Evaluate(Time.time), anim.Evaluate(Time.time));
	}
}
