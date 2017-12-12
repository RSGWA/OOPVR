using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	public GameObject gazeMenu;
	public GameObject instance;

	private bool menuOpen;
	private float waitTime = 0.05f;

	// Use this for initialization
	void Start () {
		SetGazedAt (false);
		gazeMenu.SetActive (false);
		menuOpen = false;
	}

	public void SetGazedAt(bool gazedAt) {
		if (menuOpen) {
			transform.localScale = new Vector3 (0.15f, 1, 0.11f);
			return;
		}

		if (gazedAt) {
			transform.localScale += new Vector3 (0.01f, 1, 0.01f);
		} else {
			transform.localScale = new Vector3 (0.14f, 1, 0.1f);
		}
	}

	public void OpenMenu() {
		gazeMenu.SetActive (true);
		menuOpen = true;
	}

	public void createInstance() {
		StartCoroutine (BuildInstance ());
	}

	IEnumerator BuildInstance() {
		if (instance.transform.position.y >= 1.5f) {
			yield break;
		}
		yield return new WaitForSeconds (waitTime);
		instance.transform.localPosition += new Vector3 (0f, 0.1f, 0f);
		StartCoroutine (BuildInstance ());
	}
}
