﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	Animator animator;
    private InfoController info;
    private OptionMenu options;
    bool doorOpen;
    bool infoSelected = false;
    
	Transform DoorPanel;

    void Start() {

		DoorPanel = transform.Find ("DoorPanel");

		doorOpen = false;
		animator = GetComponent<Animator>();
        options = transform.Find("DoorPanel").GetComponent<OptionMenu>();
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
    }

	private void DoorControl(string direction) {
		animator.SetTrigger(direction);
	}

	public void openDoor() {
        
        DoorControl ("Open");
		DoorPanel.GetComponent<BoxCollider> ().enabled = false;
		doorOpen = true;
        //Deselect the door outlines before opening
        options.Deselect();
    }

	public void closeDoor() {
		DoorControl ("Close");
		DoorPanel.GetComponent<BoxCollider> ().enabled = true;
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
		DoorPanel.GetComponent<BoxCollider> ().enabled = true;
	}

	public void disableDoor() {
		DoorPanel.GetComponent<BoxCollider> ().enabled = false;
	}

    public bool isDoorFullyOpen()
    {
		return GetComponent<Animator>().GetCurrentAnimatorStateInfo (0).IsName ("DoorOpenIdle");
    }

	public bool isDoorOpen()
	{
		return doorOpen;
	}

    public void infoButton()
    {
        info.SetInformation("This is a Door.\nIt allows you to 'GO INTO' a method and perform Operations\nPLEASE SELECT INFO AGAIN TO DESELECT!");

        if (!infoSelected)
        {
            info.ShowInformation();
            infoSelected = true;
        }
        else
        {
            info.HideInformation();
            infoSelected = false;
        }
    }
		
}
