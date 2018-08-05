using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiInstancesMethodCalls : MonoBehaviour
{
    public GameObject Ins1NameInstanceValue, Ins1AgeInstanceValue, Ins2NameInstanceValue, Ins2AgeInstanceValue;

    InstanceController ic1, ic2;
    Notepad notepad;
    PlayerController player;

    List<string> objectives = new List<string>();

    GameObject instance1, instance2 ;
    GameObject mainMovePos;
    Transform setNameIns1, getNameIns1, getNameIns2, getNameIns1DoorMenuPanel, getNameIns2DoorMenuPanel;
    VariableBoxController nameParamSNins1, INS1_NameInstanceBox, INS2_NameInstanceBox, INS1_AgeInstanceBox, INS2_AgeInstanceBox;
    VariableBoxController p1Name, p2Name, instanceContainer1, instanceContainer2;
    Transform nameValue;
    HandController hand;
    DoorMenuController doorControl;
    Vector3 insConScale, varConScale, valueScale;
    Button ins1_GNDoor_goINTO, ins2_GNDoor_goINTO;

    void Awake()
    {
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        doorControl = GameObject.Find("ActivityController").GetComponent<DoorMenuController>();
        mainMovePos = GameObject.Find("MainMovePoint");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandController>();

        instance1 = GameObject.FindGameObjectWithTag("Instance1");
        instance2 = GameObject.FindGameObjectWithTag("Instance2");


        ic1 = instance1.GetComponent<InstanceController>();
        ic2 = instance2.GetComponent<InstanceController>();

        setNameIns1 = instance1.transform.Find("SetName");
        getNameIns1 = instance1.transform.Find("GetName");
        getNameIns2 = instance2.transform.Find("GetName");

        getNameIns1DoorMenuPanel = getNameIns1.Find("Door/DoorExt/OptionMenu/Panel");
        getNameIns2DoorMenuPanel = getNameIns2.Find("Door/DoorExt/OptionMenu/Panel");

        ins1_GNDoor_goINTO = getNameIns1DoorMenuPanel.Find("GoIntoButton").GetComponent<Button>();
        ins2_GNDoor_goINTO = getNameIns2DoorMenuPanel.Find("GoIntoButton").GetComponent<Button>();


        nameParamSNins1 = setNameIns1.Find("ParametersPlatform/NameParameter/NameParameterBox").GetComponent<VariableBoxController>();

        initialiseInstanceVariables();
        insConScale = instanceContainer2.transform.localScale;
        instanceContainer2.transform.localScale = new Vector3(0, 0, 0);

        p1Name = GameObject.Find("Name_Variable1").GetComponent<VariableBoxController>();
        p2Name = GameObject.Find("Name_Variable2").GetComponent<VariableBoxController>();

        varConScale = p1Name.transform.localScale;
        p1Name.transform.localScale = new Vector3(0, 0, 0);
        p2Name.transform.localScale = new Vector3(0, 0, 0);

        nameValue = GameObject.Find("Junior").transform;
        valueScale = nameValue.localScale;
        nameValue.localScale = new Vector3(0, 0, 0);


        objectives.Add("p1->setName"); //main
        objectives.Add("\"Junior\"");
        objectives.Add("this->name = name;"); //setName
        objectives.Add("}");

        objectives.Add("p1->");//main- duplicate
        objectives.Add("getName()");
        objectives.Add("this->name");//getName
        objectives.Add("return");
        objectives.Add("string p1Name =");//main

        objectives.Add("p2->");
        objectives.Add("getName()"); //-duplicate
        objectives.Add("this->name");//getName
        objectives.Add("return");
        objectives.Add("string p2Name =");//main

    }

    void initialiseInstanceVariables()
    {
        //initialising instance containers
        instanceContainer1 = GameObject.Find("InstanceContainer1").GetComponent<VariableBoxController>();
        instanceContainer1.setBoxAssigned(true);
        instanceContainer1.setVariableBoxValue(instanceContainer1.transform.Find("address").gameObject);

        instanceContainer2 = GameObject.Find("InstanceContainer2").GetComponent<VariableBoxController>();
        instanceContainer2.setBoxAssigned(true);
        instanceContainer2.setVariableBoxValue(instanceContainer2.transform.Find("address").gameObject);

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
        ic1.EnableMovePositions(false);
        ic2.EnableMovePositions(false);
        ins1_GNDoor_goINTO.interactable = false;

        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkInFrontSetName");
    }

    IEnumerator checkInFrontSetName()
    {
        while (!playerInFrontMethod(setNameIns1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        ic1.EnableMovePositions(true);
        nameValue.localScale = valueScale;
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(false);
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
        instance1.transform.Find("SetName/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine("checkSetNameReturn");
    }

    IEnumerator checkSetNameReturn()
    {
        while (!playerInFrontMethod(setNameIns1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        player.moveTo(mainMovePos);
        ic1.EnableMovePositions(false);
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        notepad.setActiveText(0);
        notepad.setTitle("Main");
        //notepad.blinkObjective(objectives[4]);
        notepad.blinkDuplicateObjective(objectives[4], 2);
        StartCoroutine("checkPlayerInMain");
    }

    IEnumerator checkPlayerInMain()
    {
        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        ins1_GNDoor_goINTO.interactable = true;
        StartCoroutine("checkPlayerOnInstance1");
    }

    IEnumerator checkPlayerOnInstance1()
    {
        while (!player.checkPlayerPos(setNameIns1.Find("MovePoint").position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        ic1.EnableMovePositions(true);
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(false);
        notepad.blinkObjective(objectives[5]);
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
        notepad.blinkObjective(objectives[6]);
        StartCoroutine("check_ins1_NameInHand");
    }

    IEnumerator check_ins1_NameInHand()
    {
        while (!nameInHand())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[7]);
        instance1.transform.Find("GetName/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
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
        notepad.blinkObjective(objectives[8]);
        player.moveTo(mainMovePos); //move player to main
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        ic1.EnableMovePositions(false);
        StartCoroutine("checkPlayerBackInMain");
    }

    IEnumerator checkPlayerBackInMain()
    {
        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        p1Name.transform.localScale = varConScale;
        StartCoroutine("check_p1_NameAssigned");
    }

    IEnumerator check_p1_NameAssigned()
    {
        while (!variableAssigned(p1Name))
        {
            yield return new WaitForSeconds(0.1f);
        }

        //show instance container 2
        instanceContainer2.transform.localScale = insConScale;
        notepad.blinkObjective(objectives[9]);
        StartCoroutine("checkPlayerOnInstance2");
    }

    IEnumerator checkPlayerOnInstance2()
    {
        while (!player.checkPlayerPos(getNameIns2.Find("MovePoint").position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        ic2.EnableMovePositions(true);
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(false);
        notepad.blinkDuplicateObjective(objectives[10], 2);
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
        notepad.blinkObjective(objectives[11]);
        StartCoroutine("check_ins2_NameInHand");
    }

    IEnumerator check_ins2_NameInHand()
    {
        while (!nameInHand())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[12]);
        instance2.transform.Find("GetName/Door/DoorExt").GetComponent<Door>().enableDoor(); //Enable door for return
        StartCoroutine("check_ins2_GetNameReturn");
    }

    IEnumerator check_ins2_GetNameReturn()
    {
        while (!playerInFrontMethod(getNameIns2))
        {
            yield return new WaitForSeconds(0.1f);
        }

        player.moveTo(mainMovePos);
        mainMovePos.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        ic2.EnableMovePositions(false);
        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[13]);

        StartCoroutine("checkPlayerFinalMain");
    }

    IEnumerator checkPlayerFinalMain()
    {
        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        p2Name.transform.localScale = varConScale;
        StartCoroutine("check_p2_NameAssigned");
    }

    IEnumerator check_p2_NameAssigned()
    {
        while (!variableAssigned(p2Name))
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Activity Finished
        PlayerPrefs.SetInt("MultiInstancesMethodCalls", 1);
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
                if (INS1_NameInstanceBox.gameObject.transform.GetChild(3).GetComponent<TextMesh>().text.ToString() == "\"Junior\"")
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
