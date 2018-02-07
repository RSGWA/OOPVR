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
    private bool inWorkingMethod = false;

    private AnimationCurve[] curves;

	float currentTime;
	bool playerMoving = false;

    private Transform currentWorkingDest = null;

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

			if (Time.time - currentTime > AnimationUtility.PLAYER_ANIM_LENGTH) {
				playerMoving = false;
			}
		}
	}

	public void backToOrigin() {
		// Animate player moving back to origin
		currentTime = Time.time;
		curves = AnimationUtility.movePlayer (transform, origin);
		playerMoving = true;

		Transform doorParent = currentRoom.transform.GetChild (0);

		doorParent.GetChild (0).GetComponent<Door> ().closeDoor ();
		doorParent.GetChild (1).GetComponent<Door> ().closeDoor ();

		returned = true;
	}

	public void moveTo(GameObject dest)
	{
        currentTime = Time.time;
        curves = AnimationUtility.movePlayer(transform, dest.transform.position);
        playerMoving = true;
        origin = dest.transform.position;
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

		Transform doorParent = currentRoom.transform.GetChild (0);

		while (!doorOpen) {
			yield return null;

			if (doorParent.GetComponentInChildren<Door>().isDoorFullyOpen()) {
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

		curves = AnimationUtility.movePlayer (transform, dest.position);
		playerMoving = true;

		inRoom = true;
	}

	void PlayerControl(string direction) {
		playerAnim.SetTrigger(direction);
	}

    public void setCurrentWorkingMethod(GameObject method)
    {
        this.currentWorkingDest = method.transform.Find("PlayerDest");
    }

    public bool isInWorkingMethod()
    {
        return inWorkingMethod;
    }

	public bool isInRoom() {
		return inRoom;
	}

	public bool hasReturned() {
		return returned;
	}
}
