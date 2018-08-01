﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor2Parameters : MonoBehaviour {


    public Transform[] MainAreaValues;
    private List<Vector3> initialScales = new List<Vector3>();

    InstanceController instance;
    Notepad notepad;
    PlayerController player;
    AddressBoxController address;
    GameObject instanceContainer;

    Vector3 insConScale;


    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

	VariableBoxController nameParameterBox, ageParameterBox, nameBox, ageBox, instanceBox;

    List<string> objectives = new List<string>();

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		Transform parameterPlatform = GameObject.FindGameObjectWithTag ("ConstructorWithParameters").transform.Find ("ParametersPlatform");

		nameParameterBox = parameterPlatform.Find ("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
		ageParameterBox = parameterPlatform.Find ("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();

		ageBox = GameObject.Find ("Age_InstanceBox").GetComponent<VariableBoxController> ();
		nameBox = GameObject.Find ("Name_InstanceBox").GetComponent<VariableBoxController> ();
        instanceBox = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();

        address = GameObject.FindGameObjectWithTag("AddressBox").GetComponent<AddressBoxController>();

        instanceContainer = GameObject.Find("InstanceContainer");
        insConScale = instanceContainer.transform.localScale;
        instanceContainer.transform.localScale = new Vector3(0, 0, 0);

        objectives.Add("new Person");
        objectives.Add("Person(\"John\",20)");
        objectives.Add("\"John\"");
        objectives.Add("20");
		objectives.Add("this->name = name;");
		objectives.Add("this->age = age;");
		objectives.Add ("}");
        objectives.Add("Person *p1 = new Person(\"John\",20);");


    }

    // Use this for initialization
    void Start()
    {
        setUpScales();
        notepad.blinkObjective (objectives [0]);
        StartCoroutine("checkInstanceCreated");
    }


    IEnumerator checkInstanceCreated()
    {
        while (!instanceCreated)
        {
            instanceCreated = instance.hasInstanceBeenCreated();
            yield return new WaitForSeconds(2f);
        }

		notepad.blinkObjective (objectives [1]);

        GameObject constrMovePoint = instance.transform.Find("DefaultConstructor/MovePoint").gameObject;
        player.moveTo(constrMovePoint);
        StartCoroutine("checkInfrontOfConstructor");
    }

    IEnumerator checkInfrontOfConstructor()
    {
        Vector3 infrontConstructor = instance.transform.Find("Constructor/MovePoint").position;
        while (!checkPlayerPos(infrontConstructor))
        {
            yield return new WaitForSeconds(1f);
        }

        showMainAreaValues(true);
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkNameParameterSet");
    }

    IEnumerator checkNameParameterSet()
	{
		while (!nameParameterSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [3]);
		StartCoroutine("checkAgeParameterSet");
	}

	IEnumerator checkAgeParameterSet()
	{
		while (!ageParameterSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.setActiveText (1);
		notepad.setTitle ("CONSTRUCTOR");
		notepad.blinkObjective (objectives [4]);
		StartCoroutine("checkNameSet");
	}

	IEnumerator checkNameSet()
	{
		while (!nameSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [5]);
		StartCoroutine("checkAgeSet");
	}

	IEnumerator checkAgeSet()
	{
		while (!ageSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [6]);
        //Enable Door selection for exit
        instance.transform.Find("Constructor/Door/DoorExt").GetComponent<Door>().enableDoor();
        StartCoroutine("checkReturn");
	}

    IEnumerator checkReturn()
    {
        while (!returned)
        {
            returned = player.hasReturned();
            yield return new WaitForSeconds(0.1f);
        }
 
        notepad.setActiveText(0);
        notepad.setTitle("Main");
        notepad.blinkObjective(objectives[7]);

        address.ToHands();

        StartCoroutine("checkPlayerInMain");
    }

    IEnumerator checkPlayerInMain()
    {
        yield return new WaitForSeconds(1.9f);
        instance.SetInstanceCompletion(true);

        GameObject mainMovePoint = GameObject.Find("MainMovePoint");
        player.moveTo(mainMovePoint);

        while (!checkPlayerPos(mainMovePoint.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instanceContainer.transform.localScale = insConScale;
        StartCoroutine("checkInstanceContainer");
    }

    IEnumerator checkInstanceContainer()
    {
        while (!instanceBox.isVarInBox())
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Activity Finished
        PlayerPrefs.SetInt("ConstructorWithParametersComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }



    bool nameParameterSet() {
		return nameParameterBox.isVarInBox ();
	}

	bool ageParameterSet() {
		return ageParameterBox.isVarInBox ();
	}

	bool nameSet() {
		return nameBox.isVarInBox ();
	}

	bool ageSet() {
		return ageBox.isVarInBox ();
	}

    bool checkPlayerPos(Vector3 againstPos)
    {
        if ((player.transform.position.x == againstPos.x) && (player.transform.position.z == againstPos.z))
        {
            return true;
        }
        return false;
    }

    void setUpScales()
    {
        for (int i = 0; i < MainAreaValues.Length; i++)
        {
            initialScales.Add(MainAreaValues[i].localScale);
            MainAreaValues[i].localScale = new Vector3(0, 0, 0);
        }
    }
    void showMainAreaValues(bool key)
    {

        if (key)
        {
            for (int i = 0; i < MainAreaValues.Length; i++)
            {
                    MainAreaValues[i].localScale = initialScales[i];
            }
        }
        else
        {
            for (int i = 0; i < MainAreaValues.Length; i++)
            {
                MainAreaValues[i].localScale = new Vector3(0, 0, 0);
            }
        }

    }
}
