﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetName : MonoBehaviour {

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

        objectives.Add("Person p = new Person();");
        //objectives.Add("p.setName(\"Gilbert\");");
        objectives.Add("void Person::setName(const string& name) {");
        objectives.Add("this->name = name;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start()
    {
        notepad.enlargeCurrentObjective(objectives[0]);
        StartCoroutine("checkInstanceCreated");
    }

    IEnumerator checkInstanceCreated()
    {
        while (!instanceCreated)
        {
            instanceCreated = instance.hasInstanceBeenCreated();
            if (player.isInRoom())
            {
                instanceCreated = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("SetName");
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
        StartCoroutine("checkInstanceVarsSet");
    }

    IEnumerator checkInstanceNameAssigned()
    {
        while (!instanceNameAssigned())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        notepad.enlargeCurrentObjective(objectives[3]);
        StartCoroutine("checkReturn");
    }

   /* IEnumerator checkInstanceVarsSet()
    {
        while (!instanceVariablesSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.reset();
        notepad.enlargeCurrentObjective(objectives[3]);
        StartCoroutine("checkReturn");
    }*/

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

    /*bool instanceVariablesSet()
    {
        VariableBoxController ageBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        VariableBoxController nameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        return (ageBox.isVarInBox() && nameBox.isVarInBox());
    }*/

    bool instanceNameAssigned()
    {
        VariableBoxController nameParameter = GameObject.FindGameObjectWithTag("Parameter").transform.GetChild(1).GetComponent<VariableBoxController>();

        return nameParameter.isVarInBox();
    }
}