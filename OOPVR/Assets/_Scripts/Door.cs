using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

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
		GetComponent<BoxCollider> ().enabled = false;
		doorOpen = true;
	}

	public void closeDoor() {
		DoorControl ("Close");
		GetComponent<BoxCollider> ().enabled = true;
		doorOpen = false;
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

	public void enableDoor() {
		GetComponent<BoxCollider> ().enabled = true;
	}

	public void disableDoor() {
		GetComponent<BoxCollider> ().enabled = false;
	}

}
