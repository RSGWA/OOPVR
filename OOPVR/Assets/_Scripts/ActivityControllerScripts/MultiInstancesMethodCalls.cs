using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInstancesMethodCalls : MonoBehaviour
{
    public GameObject Ins1NameInstanceValue, Ins1AgeInstanceValue, Ins2NameInstanceValue, Ins2AgeInstanceValue;

    InstanceController ic1, ic2;
    Notepad notepad;
    PlayerController player;

    List<string> objectives = new List<string>();

    GameObject instance1, instance2;
    Transform setNameIns1, getNameIns1, getNameIns2;
    VariableBoxController nameParamSNins1, obj1NameInstanceBox, obj1AgeInstanceBox ;

    void Awake()
    {
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        instance1 = GameObject.FindGameObjectWithTag("Instance1");
        instance2 = GameObject.FindGameObjectWithTag("Instance2");

        setNameIns1 = instance1.transform.Find("SetName");
        getNameIns1 = instance1.transform.Find("GetName");
        getNameIns2 = instance2.transform.Find("GetName");

        nameParamSNins1 = setNameIns1.Find("ParametersPlatform/NameParameter").GetComponent<VariableBoxController>();

        obj1NameInstanceBox = instance1.transform.Find("Name_InstanceBox").GetComponent<VariableBoxController>();
        obj1NameInstanceBox.setBoxAssigned(true);
        obj1NameInstanceBox.setVariableBoxValue(Ins1NameInstanceValue);




        objectives.Add("pete.setName"); //main
        objectives.Add("\"Pita\"");
        objectives.Add("this->name = name;"); //setName
        objectives.Add("}");

        objectives.Add("pete.getName()");//main
        objectives.Add("this->name");//getName
        objectives.Add("return");
        objectives.Add("string p1 = pete.getName();");//main

        objectives.Add("peter.getName()");
        objectives.Add("this->name");//getName
        objectives.Add("return");
        objectives.Add("string p2 = peter.getName();");//main

    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkInFrontSetName");
    }

    IEnumerator checkInFrontSetName()
    {
        while (!playerInFrontSetName())
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
        StartCoroutine("checkSetNameReturn");
    }

    IEnumerator checkSetNameReturn()
    {
        while (!playerInFrontSetName())
        {
            yield return new WaitForSeconds(0.1f);
        }

        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[4]);
        StartCoroutine("checkSetNameReturn");
    }

    bool playerInFrontSetName()
    {
        Transform roomMovePoint = setNameIns1.Find("MovePoint");
        return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);
    }

    bool nameParameterSet()
    {
        return nameParamSNins1.isVarInBox();
    }

    bool nameSet()
    {

        if (obj1NameInstanceBox.transform.childCount > 3)
        {
            if (obj1NameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>() != null)
            {
                if (obj1NameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>().text.ToString() == "\"Pita\"")
                {
                    return true;
                }
            }
        }
        return false;
    }



}
