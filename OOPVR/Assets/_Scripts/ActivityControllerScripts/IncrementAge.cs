using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementAge : ActivityController
{

    public GameObject AgeInstanceValue, NameInstanceValue, InstanceAddress;

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool methodEntered = false;
    bool returned = false;

    GameObject mainMovePos;
    GameObject incrementAgeRoom;
    VariableBoxController instanceAgeBox, instanceNameBox, instanceContainer;

    List<string> objectives = new List<string>();

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        mainMovePos = GameObject.Find("MainMovePoint");

        incrementAgeRoom = GameObject.FindGameObjectWithTag("IncrementAge");
        instanceAgeBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        instanceAgeBox.setBoxAssigned(true);
        instanceAgeBox.setVariableBoxValue(AgeInstanceValue);

        instanceNameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();
        instanceNameBox.setBoxAssigned(true);
        instanceNameBox.setVariableBoxValue(NameInstanceValue);

        instanceContainer = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        instanceContainer.setBoxAssigned(true);
        instanceContainer.setVariableBoxValue(InstanceAddress);

        objectives.Add("p1->");
        objectives.Add("incrementAge();");
        objectives.Add("this->age++;");
        objectives.Add("}");
		objectives.Add("p1->"); // NOTE: Should this be john.incrementAge() or incrementAge()
        objectives.Add("incrementAge();");
        objectives.Add("this->age++;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkDuplicateObjective(objectives[0], 2);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));
        instance.EnableMovePositions(false);
        StartCoroutine("CheckPlayerOnInstanceArea");
    }

    IEnumerator CheckPlayerOnInstanceArea()
    {
        Vector3 onLand = GameObject.FindGameObjectWithTag("GetName").transform.Find("MovePoint").position;
        while (!player.checkPlayerPos(onLand))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instance.EnableMovePositions(true);
        notepad.blinkObjective(objectives[1]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/IncrementAge/Door/DoorExt/DoorPanel"));
        StartCoroutine("checkMethodEntered");
    }

    IEnumerator checkMethodEntered()
    {
        while (!player.isInRoom())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("IncrementAge");
        notepad.blinkObjective(objectives[2]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/Age_InstanceBox"));
        StartCoroutine("checkAgeInstanceIncrement");
    }

    IEnumerator checkAgeInstanceIncrement()
    {
        while (!isAgeInstanceIncremented())
        {
            yield return new WaitForSeconds(0.1f);
        }

        notepad.reset();
        notepad.blinkObjective(objectives[3]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/IncrementAge/Door/DoorExt/DoorPanel"));
        instance.transform.Find("IncrementAge/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine("checkFirstIncrementReturn");
    }

    IEnumerator checkFirstIncrementReturn()
    {
        while (!playerInFrontOfMethod())
        {
            yield return new WaitForSeconds(0.1f);
        }
        resetIncrement(); //resets the isIncremented when player exits
        player.setInRoom(false); //resets isInRoom to false as player exits
        methodEntered = player.isInRoom();

        player.moveTo(mainMovePos);
        instance.EnableMovePositions(false);

        StartCoroutine("CheckPlayerInMain");
    }

    IEnumerator CheckPlayerInMain()
    {
        Vector3 mainPos = GameObject.Find("MainMovePoint").transform.position;
        while (!player.checkPlayerPos(mainPos))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(0);
        notepad.setTitle("MAIN");
        notepad.blinkDuplicateObjective(objectives[4], 3);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));

        instance.EnableMovePositions(false);
        StartCoroutine("CheckPlayerOnInstance");
    }

    IEnumerator CheckPlayerOnInstance()
    {
        Vector3 onLand = GameObject.FindGameObjectWithTag("GetName").transform.Find("MovePoint").position;
        while (!player.checkPlayerPos(onLand))
        {
            yield return new WaitForSeconds(0.1f);
        }

        instance.EnableMovePositions(true);
        notepad.blinkDuplicateObjective(objectives[5], 2);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/IncrementAge/Door/DoorExt/DoorPanel"));

        StartCoroutine("methodEnteredTwice");
    }

    IEnumerator methodEnteredTwice()
    {
        Vector3 inside = incrementAgeRoom.transform.Find("PlayerDest").position;
        while (!player.checkPlayerPos(inside))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("IncrementAge");
        notepad.blinkObjective(objectives[6]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/Age_InstanceBox"));
        StartCoroutine("checkSecondIncrement");
    }

    IEnumerator checkSecondIncrement()
    {
        while (!isAgeInstanceIncremented())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        notepad.blinkObjective(objectives[7]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/IncrementAge/Door/DoorExt/DoorPanel"));

        instance.transform.Find("IncrementAge/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine("checkReturn");
    }

    IEnumerator checkReturn()
    {
        while (!playerInFrontOfMethod())
        {
            yield return new WaitForSeconds(0.1f);
        }

        player.moveTo(mainMovePos);
        StartCoroutine("checkBackInMain");
    }

    IEnumerator checkBackInMain()
    {
        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Activity Finished
        PlayerPrefs.SetInt("IncrementAgeComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }

    bool playerInFrontOfMethod()
    {
        Transform roomMovePoint = incrementAgeRoom.transform.Find("MovePoint");

        return (player.checkPlayerPos(roomMovePoint.position));

    }
    bool isAgeInstanceIncremented()
    {
        VariableBoxController ageInstanceVariable = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();

        return ageInstanceVariable.isIncremented();
    }
    void resetIncrement()
    {
        VariableBoxController ageInstanceVariable = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        ageInstanceVariable.resetIncrement();
    }
}