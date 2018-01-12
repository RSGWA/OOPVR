using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanceController : MonoBehaviour {
	
	public GameObject player;
	public Material transparent;
	public Material tinted;

	private Animator anim;

	bool instanceCreated = false;
	bool instanceLowered = false;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		transform.localScale = new Vector3 (0, 0, 0);
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createInstance() {
		this.gameObject.SetActive (true);
		InstanceControl ("Create");
		InstanceControl ("Lower");
		StartCoroutine ("checkInstanceCreated");
		StartCoroutine ("returnBlueprint");
		StartCoroutine ("checkInstanceLowered");
		StartCoroutine ("updateObjective");
	}

	// Checks if instance has finished being created so blueprint can be returned
	// to its original position
	IEnumerator checkInstanceCreated() {
		instanceCreated = false;

		while (!instanceCreated) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("InstanceCreated")) {
				instanceCreated = true;
			}	
		}
	}

	IEnumerator checkInstanceLowered() {
		instanceLowered = false;

		while (!instanceLowered) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("InstanceCreatedIdle")) {
				instanceLowered = true;
			}	
		}
	}

	IEnumerator returnBlueprint() {
		while (!instanceCreated) {
			yield return new WaitForSeconds (0.1f);
		}

		GameObject.FindGameObjectWithTag ("Blueprint").GetComponent<BlueprintController> ().returnToOrigin ();

	}

	IEnumerator updateObjective() {
		while (!instanceLowered) {
			yield return new WaitForSeconds (0.1f);
		}

		// Update objectives if necessary

	}

	void InstanceControl(string direction) {
		anim.SetTrigger(direction);
	}

	public void makeTransparent() {
		GetComponent<Renderer> ().material = transparent;
	}

	public void makeTinted() {
		GetComponent<Renderer> ().material = tinted;
	}
}