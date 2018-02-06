using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeOnHover : GvrAllEventsTrigger {

	Vector3 originalScale;

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public TriggerEvent OnPointerEnter(GameObject obj, PointerEventData data) {
		Debug.Log ("POINTER ENTERED");
		return null;
	}
}
