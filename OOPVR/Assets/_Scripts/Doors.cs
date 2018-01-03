using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

	Animator animator;
	bool doorOpen;

	public int doorOpenIdleHash = Animator.StringToHash("Open Idle");

	void Start() {
		doorOpen = false;
		animator = GetComponent<Animator>();
	}

	private void DoorControl(string direction) {
		animator.SetTrigger(direction);
	}

	public void openDoor() {
		DoorControl ("Open");
	}

	public void closeDoor() {
		DoorControl ("Close");
	}

	public void ControlDoor() {
		if (doorOpen == false) {
			DoorControl ("Open");
			doorOpen = true;
		} else {
			DoorControl("Close");
			doorOpen = false;
		}
	}

	public void enableDoors() {
		GetComponent<BoxCollider> ().enabled = true;
	}

	public void disableDoors() {
		GetComponent<BoxCollider> ().enabled = false;
	}

	public void closeDoors() {
		DoorControl ("Close");
		doorOpen = false;
	}

}
