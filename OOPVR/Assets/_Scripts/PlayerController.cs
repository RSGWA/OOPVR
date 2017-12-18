using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 originPosition;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		Debug.Log (originPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void backToOrigin() {
		transform.position = originPosition;
	}
}
