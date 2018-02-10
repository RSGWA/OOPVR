using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementAge : MonoBehaviour {

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool methodEntered = false;
    bool nameAssigned = false;
    bool returned = false;

    List<string> objectives = new List<string>();

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        objectives.Add("p.incrementAge();");
        //objectives.Add("p.setName(\"Gilbert\");");
        objectives.Add("void Person::incrementAge() {");
        objectives.Add("this->age++;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        notepad.enlargeCurrentObjective(objectives[0]);
        StartCoroutine("checkPlayerPosition");
    }

    IEnumerator checkPlayerPosition()
    {
        while (!movedToPoint())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("IncrementAge");
        notepad.enlargeCurrentObjective(objectives[1]);
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
        notepad.enlargeCurrentObjective(objectives[2]);
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

    bool movedToPoint()
    {
        Transform movePoint = GameObject.Find("MovePointIncrementAge").transform;
        if(movePoint.position.x == player.transform.position.x)
        {
            return true;
        }
        return false;
    }
    bool isAgeInstanceIncremented()
    {
        VariableBoxController ageInstanceVariable = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();

        return ageInstanceVariable.isIncremented();
    }
}