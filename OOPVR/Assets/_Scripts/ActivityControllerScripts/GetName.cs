using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetName : ActivityController {

	public GameObject nameInstanceValue, ageInstanceValue, InstanceAddress;

	InstanceController instance;
	Notepad notepad;
	PlayerController player;
	HandController hand;
    Button goToAddr;
   
    Transform setNameRoom;
	GameObject getNameRoom , mainMovePoint;
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
        goToAddr = instanceContainer.transform.Find("OptionMenu/Panel/GoToAddress").GetComponent<Button>();
        
        mainNameBox = GameObject.Find ("Name_Variable").GetComponent<VariableBoxController> ();

        mainMovePoint = GameObject.Find("MainMovePoint");
        setNameRoom = instance.transform.Find("SetName");

        objectives.Add("p1->");
		objectives.Add ("getName()");
		objectives.Add ("this->name");
		objectives.Add ("return");
		objectives.Add ("string pName = p.getName();");
        objectives.Add("string pName");
    }

	// Use this for initialization
	void Start () {

        notepad.blinkDuplicateObjective(objectives[0], 2);
        resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));

        instance.SetInstanceCompletion(true);
        instance.EnableMovePositions(false);

        StartCoroutine ("CheckPlayerOnInstanceArea");
	}

    IEnumerator CheckPlayerOnInstanceArea()
    {
               
        while (!playerInFrontMethod(setNameRoom))
        {
            yield return new WaitForSeconds(0.1f);
        }

        instance.EnableMovePositions(true);
        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(false);

        goToAddr.interactable = false;
        goToAddr.transform.GetComponent<ButtonGaze>().enabled = false;

        notepad.blinkObjective(objectives[1]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/GetName/Door/DoorExt/DoorPanel"));
        StartCoroutine("checkMethodEntered");
    }

    IEnumerator checkMethodEntered() {
		while (!player.isInRoom ()) {
			yield return new WaitForSeconds (0.1f);
		}
        instance.EnableMovePositions(false);

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

        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        player.moveTo(mainMovePoint);
        StartCoroutine ("checkInMain");
	}

    IEnumerator checkInMain()
    {
        while (!player.checkPlayerPos(mainMovePoint.transform.position))
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

    bool playerInFrontMethod(Transform trans)
    {
        Transform roomMovePoint = trans.Find("MovePoint");
        return (player.checkPlayerPos(roomMovePoint.position));
    }

    bool nameInHand() {
		if (hand.getObjInHand () != null) {
			return hand.getObjInHand ().name == "Name_InstanceBox(Clone)";
		}
		return false;
	}
}