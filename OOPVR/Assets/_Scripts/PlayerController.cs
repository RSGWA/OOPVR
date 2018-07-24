using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector3 origin;

	private Animator anim;
	Animator playerAnim;
	private bool doorOpen;
	private GameObject currentRoom;
    GameObject[] movePoints;

    private bool inRoom = false;
	private bool returned = false;
    private bool inWorkingMethod = false;
	private GameObject doorInt;

    private AnimationCurve[] curves;

	float currentTime;
	bool playerMoving = false;

    private Transform currentWorkingDest = null;

    // Use this for initialization
    void Start () {
		origin = transform.position;
		doorOpen = false;
		playerAnim = transform.GetChild(0).GetComponent<Animator>();
        movePoints = GameObject.FindGameObjectsWithTag("Move");
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
        if (inRoom)
        {
            enableMovePoints(false);
        }
        else
        {
            enableMovePoints(true);
        }
        
	}

	public void backToOrigin() {
		// Animate player moving back to origin
		currentTime = Time.time;
		curves = AnimationUtility.movePlayer (transform, origin);
		playerMoving = true;

        StartCoroutine("closeDoor");

		returned = true;
		inRoom = false;

	}

    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(1f);

        Transform doorParent = currentRoom.transform.GetChild(0);

        doorParent.GetChild(0).GetComponent<Door>().closeDoor();
        doorParent.GetChild(1).GetComponent<Door>().closeDoor();
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

		doorInt = room.transform.Find ("Door").Find ("DoorInt").gameObject;
		doorInt.transform.Find ("DoorPanel").GetComponent<BoxCollider> ().enabled = false;

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

        // Removes glow of movepoint outside room so player cannot leave using it
        //GameObject[] movePoints = GameObject.FindGameObjectsWithTag("Move");
        //foreach (GameObject movePoint in movePoints) {
        //	movePoint.GetComponent<TeleportMovePoint> ().ShowMovePoint (false);
        //}

        yield return new WaitForSeconds(1f);
		// Open interior door once player has been moved completely into room
		doorInt.GetComponent<Door> ().openDoor ();
	}

    void enableMovePoints(bool trigger)
    {
        foreach (GameObject movePoint in movePoints)
        {
            movePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(trigger);
        }
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

    public void setInRoom(bool b)
    {
        inRoom = b;
    }

	public bool hasReturned() {
		return returned;
	}

    public Vector3 getCurrentPosition()
    {
        return this.transform.position;
    }
}
