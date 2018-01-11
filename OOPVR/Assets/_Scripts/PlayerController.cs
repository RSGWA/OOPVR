using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 originPosition;

	private Animator anim;
	Animator playerAnim;
	private bool doorOpen;
	private GameObject currentRoom;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		doorOpen = false;
		playerAnim = transform.GetChild(0).GetComponent<Animator>();
	}

	public void backToOrigin() {
		transform.position = originPosition;

		currentRoom.transform.GetChild (0).GetComponent<Door> ().closeDoor ();
		currentRoom.transform.GetChild (1).GetComponent<Door> ().closeDoor ();
	}

	public void moveTo(GameObject dest)
	{
		transform.position = new Vector3 (dest.transform.position.x, transform.position.y, dest.transform.position.z);
	}
		
	public void moveIntoRoom(GameObject room) {
		currentRoom = room;

		GameObject door = GameObject.FindGameObjectWithTag ("Door");
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
	}

	void PlayerControl(string direction) {
		playerAnim.SetTrigger(direction);
	}
}
