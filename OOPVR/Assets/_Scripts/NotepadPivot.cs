using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadPivot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (0, Camera.main.transform.rotation.eulerAngles.y, 0);
	}
}
