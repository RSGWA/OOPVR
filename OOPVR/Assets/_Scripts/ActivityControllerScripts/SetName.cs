using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is main script to manipulate the actions (steps) needed to be carried out 
 * to complete the SetName activity
 * 
 * */
public class SetName : ActivityController
{

    public GameObject NameInstanceValue , AgeInstanceValue, InstanceAddress;

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool methodEntered = false;
    bool nameAssigned = false;
    bool returned = false;

    GameObject setNameRoom, mainMovePoint;
    VariableBoxController nameParameterBox, nameInstanceBox, ageInstanceBox, instanceContainer;
    Vector3 onLand, frontOfSetName;
    Transform setNameRm, constructorRoom;

    List<string> objectives = new List<string>();
    HandController hand;

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandController>();

        setNameRoom = GameObject.FindGameObjectWithTag("SetName");
        Transform parameterPlatform = setNameRoom.transform.Find("ParametersPlatform");
        nameParameterBox = parameterPlatform.Find("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
        nameInstanceBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();
        nameInstanceBox.setBoxAssigned(true);
        nameInstanceBox.setVariableBoxValue(NameInstanceValue);

        ageInstanceBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        ageInstanceBox.setBoxAssigned(true);
        ageInstanceBox.setVariableBoxValue(AgeInstanceValue);

        instanceContainer = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        instanceContainer.setBoxAssigned(true);
        instanceContainer.setVariableBoxValue(InstanceAddress);

        mainMovePoint = GameObject.Find("MainMovePoint");
        onLand = GameObject.FindGameObjectWithTag("ConstructorWithParameters").transform.Find("MovePoint").position;
        frontOfSetName = setNameRoom.transform.Find("MovePoint").position;

        setNameRm = instance.transform.Find("SetName");
        constructorRoom = instance.transform.Find("Constructor");

        objectives.Add("p1->");
        objectives.Add("setName");
        objectives.Add("\"Gilbert\"");
        objectives.Add("name;");
        objectives.Add("this->name =");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        instance.SetInstanceCompletion(true);
        instance.EnableMovePositions(false);

        notepad.blinkObjective(objectives[0]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));
        StartCoroutine("CheckPlayerOnInstanceArea");
    }

    IEnumerator CheckPlayerOnInstanceArea()
    {
        
        while (!playerInFrontMethod(constructorRoom))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instance.EnableMovePositions(true);
        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(false);

        notepad.blinkObjective(objectives[1]);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/SetName/Door/DoorExt/DoorPanel"));
        StartCoroutine("checkPlayerInFrontOfMethod");
    }

    IEnumerator checkPlayerInFrontOfMethod()
    {
        while (!playerInFrontMethod(setNameRm))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[2]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Gilbert"));
        StartCoroutine("checkNameInHand");
    }

    IEnumerator checkNameInHand()
    {
        while (!nameValueInHand(GameObject.Find("Gilbert")))
        {
            yield return new WaitForSeconds(1f);
        }
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/SetName/ParametersPlatform/NameParameter/NameParameterBox"));

        StartCoroutine("checkNameParameterSet");
    }

    IEnumerator checkNameParameterSet()
    {
        while (!nameParameterSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        instance.EnableMovePositions(false);
        notepad.setActiveText(1);
        notepad.setTitle("Person");
        notepad.blinkDuplicateObjective(objectives[3], 2);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/SetName/ParametersPlatform/NameParameter/NameParameterBox"));
        StartCoroutine("checkNameInHand2");
    }

    IEnumerator checkNameInHand2()
    {
        while (hand.getObjInHand() == null)
        {
            yield return new WaitForSeconds(1f);
        }
        DeactivateParamBox(nameParameterBox.transform);
        ActivateInstanceBox(nameInstanceBox.transform);

        notepad.blinkObjective(objectives[4]);
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/Name_InstanceBox"));

        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
    {
        while (!nameSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        DeactivateInstanceBox(nameInstanceBox.transform);
        notepad.blinkObjective(objectives[5]);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/SetName/Door/DoorExt/DoorPanel"));

        instance.transform.Find("SetName/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine("checkReturn");
    }

    IEnumerator checkReturn()
    {
        while (!returned)
        {
            returned = player.hasReturned();
            yield return new WaitForSeconds(0.1f);
        }
        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        player.moveTo(mainMovePoint);
        StartCoroutine("checkInMain");
    }

    IEnumerator checkInMain()
    {
        while (!player.checkPlayerPos(mainMovePoint.transform.position))
        {
            yield return new WaitForSeconds(4f);
        }
        // Activity Finished
        PlayerPrefs.SetInt("SetNameComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }

    bool playerInFrontMethod(Transform trans)
    {
        Transform roomMovePoint = trans.Find("MovePoint");
        return (player.checkPlayerPos(roomMovePoint.position));
    }

    bool nameParameterSet()
    {
        return nameParameterBox.isVarInBox();
    }

    bool nameSet()
    {

        if (nameInstanceBox.transform.childCount > 3)
        {
            if (nameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>() != null)
            {
                if (nameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>().text.ToString() == "\"Gilbert\"")
                {
                    return true;
                }
            }  
        }
        return false;
    }

    bool nameValueInHand(GameObject value)
    {
        if (hand.getObjInHand() == value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void DeactivateParamBox(Transform box)
    {
        box.parent.GetComponent<ParameterBoxMenuController>().enabled = false;

        Transform panel = box.Find("OptionMenu/Panel");

        GameObject copy = panel.Find("CopyButton").gameObject;
        GameObject assign = panel.Find("AssignButton").gameObject;
        GameObject info = panel.Find("InfoButton").gameObject;

        copy.SetActive(false);
        assign.SetActive(false);
        info.SetActive(true);

    }
    void ActivateParamBox(Transform box)
    {
        box.parent.GetComponent<ParameterBoxMenuController>().enabled = true;
    }

    void DeactivateInstanceBox(Transform box)
    {
        box.GetComponent<InstanceVariablesMenuController>().enabled = false;

        Transform panel = box.Find("OptionMenu/Panel");

        GameObject copy = panel.Find("CopyButton").gameObject;
        GameObject assign = panel.Find("AssignButton").gameObject;
        GameObject increment = panel.Find("IncrementButton").gameObject;
        GameObject info = panel.Find("InfoButton").gameObject;

        copy.SetActive(false);
        assign.SetActive(false);
        increment.SetActive(false);
        info.SetActive(true);

    }
    void ActivateInstanceBox(Transform box)
    {
        box.GetComponent<InstanceVariablesMenuController>().enabled = true;
    }
}