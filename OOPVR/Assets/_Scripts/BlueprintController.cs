using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	public GameObject optionsMenu;

	private bool menuOpen;

	// Use this for initialization
	void Start () {
		SetGazedAt (false);
		optionsMenu.SetActive (false);
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
		optionsMenu.SetActive (true);
		menuOpen = true;
	}
}
