using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

	Animator animator;
	bool doorOpen;

	void Start() {
		doorOpen = false;
		animator = GetComponent<Animator>();
	}

	void DoorControl(string direction) {
		animator.SetTrigger(direction);
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
}
