using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 origin;

	private Animator anim;
	Animator playerAnim;
	private bool doorOpen;
	private GameObject currentRoom;
	private bool inRoom = false;
	private bool returned = false;

	private AnimationCurve[] curves;

	float currentTime;
	bool playerMoving = false;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		doorOpen = false;
		playerAnim = transform.GetChild(0).GetComponent<Animator>();
	}

	void Update() {
		if (playerMoving) {
			transform.position = new Vector3 (
				curves [0].Evaluate (Time.time - currentTime), 
				curves [1].Evaluate (Time.time - currentTime),
				curves [2].Evaluate (Time.time - currentTime));

			if (Time.time - currentTime > AnimatorController.ANIM_LENGTH) {
				playerMoving = false;
			}
		}
	}

	public void backToOrigin() {
		// Animate player moving back to origin
		currentTime = Time.time;
		curves = AnimatorController.movePlayer (transform, origin);
		playerMoving = true;

		currentRoom.transform.GetChild (0).GetComponent<Door> ().closeDoor ();
		currentRoom.transform.GetChild (1).GetComponent<Door> ().closeDoor ();

		returned = true;
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

			if (currentRoom.transform.GetChild(0).GetComponent<Door>().isDoorFullyOpen()) {
				doorOpen = true;
			}		
		}
	}

	IEnumerator movePlayer(GameObject room) {
		while (!doorOpen) {
			yield return new WaitForSeconds (0.1f);
		}
		// Animate player moving into room
		currentTime = Time.time;
		Transform dest = room.transform.Find("PlayerDest");

		curves = AnimatorController.movePlayer (transform, dest.position);
		playerMoving = true;

		inRoom = true;
	}

	void PlayerControl(string direction) {
		playerAnim.SetTrigger(direction);
	}

	public bool isInRoom() {
		return inRoom;
	}

	public bool hasReturned() {
		return returned;
	}
}
