using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NotepadPivot : MonoBehaviour {

	public float distanceFromCamera = 0.8f;
	float tiltSpeed = 5f;
	float tiltAngle = -35f;

	// Use this for initialization
	void Start () {
		StartCoroutine (tiltNotepad ());
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Camera.main.transform.position.x + Camera.main.transform.forward.x * distanceFromCamera, 
			transform.position.y, 
			Camera.main.transform.position.z + Camera.main.transform.forward.z * distanceFromCamera);

		transform.eulerAngles = new Vector3 (0, 
			Camera.main.transform.eulerAngles.y, 
			0);

		StartCoroutine (tiltNotepad ());
	}

	IEnumerator tiltNotepad() {
		while (Camera.main.transform.rotation.eulerAngles.x > 7f & Camera.main.transform.eulerAngles.x < 90f) {
			transform.rotation = Quaternion.Lerp (transform.rotation, 
				Quaternion.Euler (new Vector3 (tiltAngle, Camera.main.transform.eulerAngles.y, 0)), 
				tiltSpeed * Time.deltaTime);
			yield return null;
		}
			
		transform.rotation = Quaternion.Euler (new Vector3 (0, Camera.main.transform.eulerAngles.y, 0));
		yield return null;
	}
}
