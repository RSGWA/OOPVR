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
	private Transform doorInt, doorExt;

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
        //if (inRoom)
        //{
        //    enableMovePoints(false);
        //}
        //else
        //{
        //    enableMovePoints(true);
        //}
    }

    public void Return() { StartCoroutine("startReturnProcedure"); }

    IEnumerator startReturnProcedure()
    {
        Vector3 extMethodPos = currentRoom.transform.Find("MovePoint").transform.position;

        Door doorControl = currentRoom.transform.Find("Door/DoorExt").GetComponent<Door>();
        doorControl.openDoor();

        while (!doorControl.isDoorFullyOpen())
        {
            yield return new WaitForSeconds(0.1f);
        }
        currentTime = Time.time;
        curves = AnimationUtility.movePlayer(transform, extMethodPos);
        playerMoving = true;

        while (!checkPlayerPos(extMethodPos))
        {
            yield return new WaitForSeconds(0.1f);
        }
        doorExt.GetComponent<Door>().closeDoor();
        doorInt.GetComponent<Door>().closeDoor();
        returned = true;
        inRoom = false;
        
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

        doorInt = room.transform.Find("Door/DoorInt");
        doorExt = room.transform.Find("Door/DoorExt");

        if (!doorExt.GetComponent<Door>().isDoorOpen())
        {
            doorExt.GetComponent<Door>().openDoor();
        }

		//doorInt = room.transform.Find ("Door/DoorInt");
		doorInt.transform.Find ("DoorPanel").GetComponent<BoxCollider> ().enabled = false;

        //doorExt = room.transform.Find("Door/DoorExt");
        doorExt.transform.Find("DoorPanel").GetComponent<BoxCollider>().enabled = false;

		StartCoroutine ("check");
	}

	// Checks if door has fully opened before transporting player into room
	IEnumerator check() {
        while (!doorExt.GetComponent<Door>().isDoorFullyOpen())
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(movePlayer(currentRoom));
    }

	IEnumerator movePlayer(GameObject room) {

        Transform dest = room.transform.Find("PlayerDest");
        this.moveTo(dest.gameObject);

        while (!checkPlayerPos(dest.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        doorInt.GetComponent<Door>().openDoor();
        inRoom = true;
        StartCoroutine(handleDoorControl());
    }

    IEnumerator handleDoorControl()
    {
        while (!doorInt.GetComponent<Door>().isDoorFullyOpen())
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        doorExt.GetComponent<Door>().closeDoor();
        doorExt.GetComponent<Door>().disableDoor();
    }

    public bool checkPlayerPos(Vector3 againstPos)
    {
        if ((transform.position.x == againstPos.x) && (transform.position.z == againstPos.z))
        {
            return true;
        }
        return false;
    }

    void PlayerControl(string direction) {
		playerAnim.SetTrigger(direction);
	}

    public void setCurrentWorkingMethod(GameObject method)
    {
        this.currentWorkingDest = method.transform.Find("PlayerDest");
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
