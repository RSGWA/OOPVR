using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	public GameObject optionsMenu;

	private bool menuOpen;

	private Vector3 originalSize;
	private Vector3 highlightedSize;
	private Vector3 growth = new Vector3 (0.015f, 1, 0.015f);

	// Use this for initialization
	void Start () {
		originalSize = transform.localScale;
		highlightedSize = transform.localScale += growth;
		SetGazedAt (false);
		optionsMenu.SetActive (false);
		menuOpen = false;
	}

	public void SetGazedAt(bool gazedAt) {
		if (menuOpen) {
			transform.localScale = highlightedSize;
			return;
		}

		if (gazedAt) {
			transform.localScale = highlightedSize;
		} else {
			transform.localScale = originalSize;
		}
	}

	public void OpenMenu() {
		optionsMenu.SetActive (true);
		menuOpen = true;
	}
}
