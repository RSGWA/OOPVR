using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	public GameObject gazeMenu;
	public GameObject instance;

	private bool menuOpen;

	// Use this for initialization
	void Start () {
		SetGazedAt (false);
		gazeMenu.SetActive (false);
		menuOpen = false;
		instance.GetComponent<Renderer> ().enabled = false;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
	}

	public void SetGazedAt(bool gazedAt) {
		if (menuOpen) {
			transform.localScale = new Vector3 (0.15f, 1, 0.11f);
			return;
		}

		if (gazedAt == true) {
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
		instance.GetComponent<Renderer> ().enabled = true;
		instance.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
	}
}
