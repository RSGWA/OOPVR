using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetName : MonoBehaviour
{

    public GameObject NameInstanceValue , AgeInstanceValue, InstanceAddress;

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool methodEntered = false;
    bool nameAssigned = false;
    bool returned = false;

    GameObject setNameRoom;
    VariableBoxController nameParameterBox, nameInstanceBox, ageInstanceBox, instanceContainer;

    List<string> objectives = new List<string>();

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

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

        objectives.Add("p1.");
        objectives.Add("setName");
        objectives.Add("\"Gilbert\"");
        objectives.Add("this->name = name;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("CheckPlayerOnInstanceArea");
    }

    IEnumerator CheckPlayerOnInstanceArea()
    {
        Vector3 onLand = GameObject.FindGameObjectWithTag("ConstructorWithParameters").transform.Find("MovePoint").position;
        while (!player.checkPlayerPos(onLand))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[1]);
        StartCoroutine("checkPlayerInFrontOfMethod");
    }

    IEnumerator checkPlayerInFrontOfMethod()
    {
        Vector3 frontOfSetName = setNameRoom.transform.Find("MovePoint").position;
        while (!player.checkPlayerPos(frontOfSetName))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkNameParameterSet");
    }

    IEnumerator checkNameParameterSet()
    {
        while (!nameParameterSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("Set name");
        notepad.blinkObjective(objectives[3]);
        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
    {
        while (!nameSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[4]);
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
        GameObject mainMovePoint = GameObject.Find("MainMovePoint");
        player.moveTo(mainMovePoint);
        StartCoroutine("checkInMain");
    }

    IEnumerator checkInMain()
    {
        Vector3 mainMovePoint = GameObject.Find("MainMovePoint").transform.position;
        while (!player.checkPlayerPos(mainMovePoint))
        {
            yield return new WaitForSeconds(4f);
        }
        // Activity Finished
        PlayerPrefs.SetInt("SetNameComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
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
}