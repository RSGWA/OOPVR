using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetName : ActivityController {

	public GameObject nameInstanceValue, ageInstanceValue, InstanceAddress;

	InstanceController instance;
	Notepad notepad;
	PlayerController player;
	HandController hand;

	GameObject getNameRoom;
	VariableBoxController instanceNameBox, instanceAgeBox, mainNameBox, instanceContainer;

	bool instanceCreated = false;
	bool methodEntered = false;
	bool returned = false;

	List<string> objectives = new List<string>();

	void Awake() {
		instance = GameObject.FindGameObjectWithTag ("Instance").GetComponent<InstanceController> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		hand = GameObject.FindGameObjectWithTag ("Hand").GetComponent<HandController> ();

		getNameRoom = GameObject.FindGameObjectWithTag ("GetName");
		instanceNameBox = GameObject.Find ("Name_InstanceBox").GetComponent<VariableBoxController> ();
		instanceNameBox.setBoxAssigned (true);
		instanceNameBox.setVariableBoxValue (nameInstanceValue);

        instanceAgeBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        instanceAgeBox.setBoxAssigned(true);
        instanceAgeBox.setVariableBoxValue(ageInstanceValue);

        instanceContainer = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        instanceContainer.setBoxAssigned(true);
        instanceContainer.setVariableBoxValue(InstanceAddress);

        mainNameBox = GameObject.Find ("Name_Variable").GetComponent<VariableBoxController> ();

        objectives.Add("p1->");
		objectives.Add ("getName()");
		objectives.Add ("this->name");
		objectives.Add ("return");
		objectives.Add ("string pName = p.getName();");
        objectives.Add("string pName");
    }

	// Use this for initialization
	void Start () {
       
		notepad.blinkObjective(objectives[0]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));
        instance.EnableMovePositions(false);
        StartCoroutine ("CheckPlayerOnInstanceArea");
	}

    IEnumerator CheckPlayerOnInstanceArea()
    {
        Vector3 onLand = GameObject.FindGameObjectWithTag("SetName").transform.Find("MovePoint").position;
        while (!player.checkPlayerPos(onLand))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instance.EnableMovePositions(true);
        notepad.blinkObjective(objectives[1]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/GetName/Door/DoorExt/DoorPanel"));
        StartCoroutine("checkMethodEntered");
    }

    IEnumerator checkMethodEntered() {
		while (!player.isInRoom ()) {
			yield return new WaitForSeconds (0.1f);
		}
		notepad.setActiveText (1);
		notepad.setTitle ("Get name");
		notepad.blinkObjective (objectives [2]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/Name_InstanceBox"));
		StartCoroutine ("checkNameInHand");
	}

	IEnumerator checkNameInHand() {
		while (!nameInHand ()) {
			yield return new WaitForSeconds (0.1f);
		}
		notepad.blinkObjective (objectives [3]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/GetName/Door/DoorExt/DoorPanel"));
        instance.transform.Find("GetName/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine ("checkReturned");
	}

	IEnumerator checkReturned() {
		while (!returned) {
			returned = player.hasReturned ();
			yield return new WaitForSeconds (0.1f);
		}
		notepad.setActiveText (0);
		notepad.setTitle ("Main");
		notepad.blinkObjective (objectives [4]);

        GameObject mainMovePoint = GameObject.Find("MainMovePoint");
        player.moveTo(mainMovePoint);
        StartCoroutine ("checkInMain");
	}

    IEnumerator checkInMain()
    {
        Vector3 mainMovePoint = GameObject.Find("MainMovePoint").transform.position;
        while (!player.checkPlayerPos(mainMovePoint))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[5]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Name_Variable"));
        StartCoroutine("checkNameAssigned");
    }


    IEnumerator checkNameAssigned() {
		while (!nameAssigned()) {
			yield return new WaitForSeconds (0.1f);
		}
		// Activity Finished
		PlayerPrefs.SetInt("GetNameComplete",1);
		PlayerPrefs.Save ();
		notepad.endOfActivity();
	}

	bool nameAssigned() {
		return mainNameBox.isVarInBox ();
	}

	bool playerInFrontOfMethod() {
		Transform roomMovePoint = getNameRoom.transform.Find ("MovePoint");

		return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);
	}

	bool nameInHand() {
		if (hand.getObjInHand () != null) {
			return hand.getObjInHand ().name == "Name_InstanceBox(Clone)";
		}
		return false;
	}
}