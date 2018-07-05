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

    GameObject instance1, instance2, ins1_GNDoor_goINTO, ins2_GNDoor_goINTO;
    Transform setNameIns1, getNameIns1, getNameIns2, getNameIns1DoorMenuPanel, getNameIns2DoorMenuPanel;
    VariableBoxController nameParamSNins1, INS1_NameInstanceBox, INS2_NameInstanceBox, INS1_AgeInstanceBox, INS2_AgeInstanceBox;
    VariableBoxController p1Variable, p2Variable;
    HandController hand;
    DoorMenuController doorControl;

    void Awake()
    {
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        doorControl = GameObject.Find("ActivityController").GetComponent<DoorMenuController>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandController>();

        instance1 = GameObject.FindGameObjectWithTag("Instance1");
        instance2 = GameObject.FindGameObjectWithTag("Instance2");

        setNameIns1 = instance1.transform.Find("SetName");
        getNameIns1 = instance1.transform.Find("GetName");
        getNameIns2 = instance2.transform.Find("GetName");

        getNameIns1DoorMenuPanel = getNameIns1.Find("Door/DoorExt/OptionMenu/Panel");
        getNameIns2DoorMenuPanel = getNameIns2.Find("Door/DoorExt/OptionMenu/Panel");

        ins1_GNDoor_goINTO = getNameIns1DoorMenuPanel.Find("GoIntoButton").gameObject;
        ins2_GNDoor_goINTO = getNameIns2DoorMenuPanel.Find("GoIntoButton").gameObject;

        nameParamSNins1 = setNameIns1.Find("ParametersPlatform/NameParameter/NameParameterBox").GetComponent<VariableBoxController>();

        initialiseInstanceVariables();
        

        p1Variable = GameObject.Find("p1 variablebox").GetComponent<VariableBoxController>();
        p2Variable = GameObject.Find("p2 variablebox").GetComponent<VariableBoxController>();





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

    void initialiseInstanceVariables()
    {

        INS1_NameInstanceBox = instance1.transform.Find("Name_InstanceBox").GetComponent<VariableBoxController>();
        INS1_NameInstanceBox.setBoxAssigned(true);
        INS1_NameInstanceBox.setVariableBoxValue(Ins1NameInstanceValue);

        INS2_NameInstanceBox = instance2.transform.Find("Name_InstanceBox").GetComponent<VariableBoxController>();
        INS2_NameInstanceBox.setBoxAssigned(true);
        INS2_NameInstanceBox.setVariableBoxValue(Ins2NameInstanceValue);

        //The following two (Age instance variables) are not used in this activity. But they are initialised anyways
        INS1_AgeInstanceBox = instance1.transform.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        INS1_AgeInstanceBox.setBoxAssigned(true);
        INS1_AgeInstanceBox.setVariableBoxValue(Ins1AgeInstanceValue);

        INS2_AgeInstanceBox = instance2.transform.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        INS2_AgeInstanceBox.setBoxAssigned(true);
        INS2_AgeInstanceBox.setVariableBoxValue(Ins2AgeInstanceValue);
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkInFrontSetName");
    }

    IEnumerator checkInFrontSetName()
    {
        while (!playerInFrontMethod(setNameIns1))
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
        while (!playerInFrontMethod(setNameIns1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        //activate gointo option for getName instance1
        doorControl.ShowButton(ins1_GNDoor_goINTO, true);

        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[4]);
        StartCoroutine("check_ins1_InsideGetName");
    }

    IEnumerator check_ins1_InsideGetName()
    {
        while (!player.isInRoom())
        {
            yield return new WaitForSeconds(0.1f);
        }

        notepad.setActiveText(2);
        notepad.setTitle("Get name");
        notepad.blinkObjective(objectives[5]);
        StartCoroutine("check_ins1_NameInHand");
    }

    IEnumerator check_ins1_NameInHand()
    {
        while (!nameInHand())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[6]);
        StartCoroutine("check_ins1_GetNameReturn");
    }

    IEnumerator check_ins1_GetNameReturn()
    {
        while (!playerInFrontMethod(getNameIns1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[7]);
        StartCoroutine("check_p1_NameAssigned");
    }

    IEnumerator check_p1_NameAssigned()
    {
        while (!variableAssigned(p1Variable))
        {
            yield return new WaitForSeconds(0.1f);
        }
        //activate goINTO button for getName instance 2
        doorControl.ShowButton(ins2_GNDoor_goINTO, true);

        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[8]);
        StartCoroutine("check_ins2_InsideGetName");
    }

    IEnumerator check_ins2_InsideGetName()
    {
        while (!player.isInRoom())
        {
            yield return new WaitForSeconds(0.1f);
        }

        notepad.setActiveText(2);
        notepad.setTitle("Get name");
        notepad.blinkObjective(objectives[9]);
        StartCoroutine("check_ins2_NameInHand");
    }

    IEnumerator check_ins2_NameInHand()
    {
        while (!nameInHand())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[10]);
        StartCoroutine("check_ins2_GetNameReturn");
    }

    IEnumerator check_ins2_GetNameReturn()
    {
        while (!playerInFrontMethod(getNameIns2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[11]);
        StartCoroutine("check_p2_NameAssigned");
    }

    IEnumerator check_p2_NameAssigned()
    {
        while (!variableAssigned(p2Variable))
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        //Activity Finished
        PlayerPrefs.SetInt("ConstructorWithParametersComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }


    bool playerInFrontMethod(Transform trans)
    {
        Transform roomMovePoint = trans.Find("MovePoint");
        return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);
    }

    bool nameParameterSet()
    {
        return nameParamSNins1.isVarInBox();
    }

    bool variableAssigned(VariableBoxController variable)
    {
        return variable.isVarInBox();
    }

    bool nameSet()
    {

        if (INS1_NameInstanceBox.transform.childCount > 3)
        {
            if (INS1_NameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>() != null)
            {
                if (INS1_NameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>().text.ToString() == "\"Pita\"")
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool nameInHand()
    {
        if (hand.getObjInHand() != null)
        {
            return hand.getObjInHand().name == "Name_InstanceBox(Clone)";
        }
        return false;
    }

    







}
