using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementAge : MonoBehaviour {

    //public GameObject IntValue;
    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool methodEntered = false;
    bool nameAssigned = false;
    bool returned = false;

    GameObject incrementAgeRoom;
    VariableBoxController instanceAgeBox;

    List<string> objectives = new List<string>();

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        incrementAgeRoom = GameObject.FindGameObjectWithTag("IncrementAge");
        instanceAgeBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        instanceAgeBox.setBoxAssigned(true);
        //instanceAgeBox.setVariableBoxValue(IntValue);

        objectives.Add("p.incrementAge();");
        //objectives.Add("incrementAge();");
        objectives.Add("void Person::incrementAge() {");
        objectives.Add("this->age++;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        // notepad.enlargeCurrentObjective(objectives[0]);
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkPlayerInFrontOfMethod");
    }

    IEnumerator checkPlayerInFrontOfMethod()
    {
        while (!playerInFrontOfMethod())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("IncrementAge");
       // notepad.enlargeCurrentObjective(objectives[1]);
        notepad.blinkObjective(objectives[1]);
        StartCoroutine("checkMethodEntered");
    }

   /* IEnumerator checkNameAssigned()
    {
        while (!checkNameAssigned)
        {
            //constructorEntered = player.isInRoom();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        notepad.enlargeCurrentObjective(objectives[2]);
        notepad.enlargeCurrentObjective(objectives[3]);
        StartCoroutine("checkInstanceVarsSet");
    }*/

    IEnumerator checkMethodEntered()
    {
        while (!methodEntered)
        {
            methodEntered = player.isInRoom();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        //notepad.enlargeCurrentObjective(objectives[2]);
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkAgeInstanceIncrement");
    }

    IEnumerator checkAgeInstanceIncrement()
    {
        while (!isAgeInstanceIncremented())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        notepad.enlargeCurrentObjective(objectives[3]);
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
        Transform roomMovePoint = incrementAgeRoom.transform.Find("MovePoint");

        return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);

    }
    bool isAgeInstanceIncremented()
    {
        VariableBoxController ageInstanceVariable = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();

        return ageInstanceVariable.isIncremented();
    }
}