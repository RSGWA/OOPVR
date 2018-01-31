using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorOneParameter : MonoBehaviour {

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

    // Use this for initialization
    void Start()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        StartCoroutine("checkInstanceCreated");
    }

    IEnumerator checkInstanceCreated()
    {
        while (!instanceCreated)
        {
            instanceCreated = instance.hasInstanceBeenCreated();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.updateObjective("1");
        StartCoroutine("checkConstructorEntered");
    }

    IEnumerator checkConstructorEntered()
    {
        while (!constructorEntered)
        {
            constructorEntered = player.isInRoom();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.updateObjective("2");
        StartCoroutine("checkInstanceVarsSet");
    }

    IEnumerator checkInstanceVarsSet()
    {
        while (!instanceVariablesSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.updateObjective("3");
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
    }

    bool instanceVariablesSet()
    {
        //BoxController ageBox = GameObject.Find("AgeBox").GetComponent<BoxController>();
        VariableBoxController nameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        return (nameBox.isVarInBox());
    }
}
