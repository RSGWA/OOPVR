using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 originPosition;

	private Animator anim;
	Animator playerAnim;
	private bool doorOpen;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		Debug.Log (originPosition);
		doorOpen = false;
		playerAnim = transform.GetChild(0).GetComponent<Animator>();
	}

	public void backToOrigin() {
		transform.position = originPosition;

		//GameObject.Find ("InstancePrefab").GetComponent<InstanceController> ().makeTinted ();
		GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ().makeTinted ();
		GameObject door = GameObject.Find ("Door");
		door.GetComponent<Doors> ().enableDoors ();
		door.GetComponent<Doors> ().closeDoors ();
	}

	public void moveIntoRoom(GameObject room) {
		//Animator animator = doorScript.getAnimator ();

		GameObject door = GameObject.Find ("Door");
		anim = door.GetComponent<Animator> ();

		StartCoroutine ("check");
		StartCoroutine (movePlayer(room));
	}

	// Checks if door has fully opened before transporting player into room
	IEnumerator check() {
		doorOpen = false;

		while (!doorOpen) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("DoorOpenIdle")) {
				doorOpen = true;
			}		
		}
	}

	IEnumerator movePlayer(GameObject room) {
		while (!doorOpen) {
			yield return new WaitForSeconds (0.1f);
		}
		// Move player to destination point
		//PlayerControl("InstanceCreated");
		Transform dest = room.transform.Find ("PlayerDest");
		transform.position = new Vector3 (dest.position.x, transform.position.y, dest.position.z);

		//GameObject.Find ("InstancePrefab").GetComponent<InstanceController> ().makeTransparent ();
		GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ().makeTransparent ();

		GameObject door = GameObject.Find("Door");
		door.GetComponent<Doors> ().disableDoors ();
	}

	void PlayerControl(string direction) {
		playerAnim.SetTrigger(direction);
	}
}
