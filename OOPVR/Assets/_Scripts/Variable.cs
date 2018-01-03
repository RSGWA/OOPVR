using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : MonoBehaviour {
	private AnimationCurve anim;
	private Keyframe[] ks;

	public GameObject Hand;

	void Start() {
		ks = new Keyframe[50];
		Keyframe beginning = new Keyframe (0, transform.position.x);
		int i = 0;
		while (i < ks.Length) {
			ks [i] = new Keyframe (i, i * i);
			i++;
		}
		anim = new AnimationCurve (ks);
	}

	void Update() {
		transform.position = new Vector3 (Time.time, anim.Evaluate (Time.time), 0);
	}
}
