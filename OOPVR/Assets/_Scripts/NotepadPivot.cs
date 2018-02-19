using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NotepadPivot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (-30, Camera.main.transform.rotation.eulerAngles.y, 0);

		if (Camera.main.transform.rotation.eulerAngles.y > -10 && Camera.main.transform.rotation.eulerAngles.y < 10) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (0, transform.localPosition.y, 0.5f), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 35 && Camera.main.transform.rotation.eulerAngles.y < 55) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (0.35f, transform.localPosition.y, 0.35f), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 80 && Camera.main.transform.rotation.eulerAngles.y < 100) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (0.5f, transform.localPosition.y, 0), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 125 && Camera.main.transform.rotation.eulerAngles.y < 145) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (0.35f, transform.localPosition.y, -0.35f), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 160 && Camera.main.transform.rotation.eulerAngles.y < 190) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (0, transform.localPosition.y, -0.5f), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 215 && Camera.main.transform.rotation.eulerAngles.y < 235) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (-0.35f, transform.localPosition.y, -0.35f), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 260 && Camera.main.transform.rotation.eulerAngles.y < 280) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (-0.5f, transform.localPosition.y, 0), 2 * Time.deltaTime);
		}

		if (Camera.main.transform.rotation.eulerAngles.y > 305 && Camera.main.transform.rotation.eulerAngles.y < 325) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (-0.35f, transform.localPosition.y, 0.35f), 2 * Time.deltaTime);
		}
	}
}
