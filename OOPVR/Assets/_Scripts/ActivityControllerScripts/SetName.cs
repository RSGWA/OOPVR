using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetName : MonoBehaviour
{

    public GameObject NameInstanceValue , AgeInstanceValue;

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool methodEntered = false;
    bool nameAssigned = false;
    bool returned = false;

    GameObject setNameRoom;
    VariableBoxController nameParameterBox, nameInstanceBox, ageInstanceBox;

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

        objectives.Add("setName");
        objectives.Add("\"Gilbert\"");
        objectives.Add("this->name = name;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkPlayerInFrontOfMethod");
    }

    IEnumerator checkPlayerInFrontOfMethod()
    {
        while (!playerInFrontOfMethod())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[1]);
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
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
    {
        while (!nameSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[3]);
        StartCoroutine("checkReturn");
    }

    IEnumerator checkReturn()
    {
        while (!returned)
        {
            returned = player.hasReturned();
            yield return new WaitForSeconds(0.1f);
        }
        // Activity Finished
        notepad.endOfActivity();
    }

    bool playerInFrontOfMethod()
    {
        Transform roomMovePoint = setNameRoom.transform.Find("MovePoint");

        return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);

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