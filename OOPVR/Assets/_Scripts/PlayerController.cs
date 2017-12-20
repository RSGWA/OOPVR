using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 originPosition;

	private Animator anim;
	private bool doorOpen;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		Debug.Log (originPosition);
		doorOpen = false;
	}

	public void backToOrigin() {
		transform.position = originPosition;
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
				Debug.Log ("DOOR FINISHED OPENING");
				doorOpen = true;
			}		
		}
	}

	IEnumerator movePlayer(GameObject room) {
		while (!doorOpen) {
			yield return new WaitForSeconds (0.1f);
			Debug.Log (doorOpen);
		}
		// Move player to destination point
		Transform dest = room.transform.Find ("PlayerDest");
		transform.position = new Vector3 (dest.position.x, transform.position.y, dest.position.z);

		Debug.Log ("PLAYER MOVED INTO ROOM");
	}


}
