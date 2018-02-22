using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor2Parameters : MonoBehaviour {

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

	VariableBoxController nameParameterBox, ageParameterBox, nameBox, ageBox;

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

        objectives.Add("new Person"); 
        objectives.Add("\"John\"");
        objectives.Add("20");
		objectives.Add("this->name = name;");
		objectives.Add("this->age = age;");
		objectives.Add ("}");
    }

    // Use this for initialization
    void Start()
    {
		notepad.blinkObjective (objectives [0]);
        StartCoroutine("checkInstanceCreated");
    }

    IEnumerator checkInstanceCreated()
    {
        while (!instanceCreated)
        {
            instanceCreated = instance.hasInstanceBeenCreated();
            yield return new WaitForSeconds(0.1f);
        }
		notepad.blinkObjective (objectives [1]);
        StartCoroutine("checkNameParameterSet");
    }

	IEnumerator checkNameParameterSet()
	{
		while (!nameParameterSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [2]);
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
		notepad.blinkObjective (objectives [3]);
		StartCoroutine("checkNameSet");
	}

	IEnumerator checkNameSet()
	{
		while (!nameSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [4]);
		StartCoroutine("checkAgeSet");
	}

	IEnumerator checkAgeSet()
	{
		while (!ageSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [5]);
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
		PlayerPrefs.SetInt("ConstructorWithParametersComplete",1);
		PlayerPrefs.Save ();
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
}
