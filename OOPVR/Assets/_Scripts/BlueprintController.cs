using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	public GameObject optionsMenu;
	public GameObject instance;

	private bool menuOpen;

	private Vector3 originalSize;
	private Vector3 highlightedSize;
	private Vector3 growth = new Vector3 (0.015f, 1, 0.015f);

	private Animator anim;

	private bool bpInPosition = false;

	// Use this for initialization
	void Start () {
		originalSize = transform.localScale;
		highlightedSize = transform.localScale += growth;
		SetGazedAt (false);
		optionsMenu.SetActive (false);
		menuOpen = false;
		anim = GetComponent<Animator> ();
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

	public void moveBlueprint() {
		optionsMenu.SetActive (false);
		BPControl ("Move");
		StartCoroutine ("check");
		StartCoroutine ("createInstance");
	}

	// Checks whether blueprint is in position for instance animations to begin
	IEnumerator check() {
		bpInPosition = false;

		while (!bpInPosition) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("BPInPosition")) {
				bpInPosition = true;
			}		
		}
	}

	IEnumerator createInstance() {
		while (!bpInPosition) {
			yield return new WaitForSeconds (0.1f);
		}
		instance.GetComponent<InstanceController> ().createInstance ();
	}

	void BPControl(string direction) {
		anim.SetTrigger(direction);
	}

	public void returnToOrigin() {
		this.gameObject.SetActive (false);
		BPControl ("Return");
	}
}
