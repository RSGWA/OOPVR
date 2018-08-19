using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor2Parameters : ActivityController {


    public Transform[] MainAreaValues;
    private List<Vector3> initialScales = new List<Vector3>();

    InstanceController instance;
    Notepad notepad;
    PlayerController player;
    AddressBoxController address;
    GameObject instanceContainer, mainMovePoint, nameParamCopy, ageParamCopy;

    Vector3 insConScale;


    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

	VariableBoxController nameParameterBox, ageParameterBox, nameBox, ageBox, instanceBox;

    List<string> objectives = new List<string>();

    HandController hand;

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandController>();

        Transform parameterPlatform = GameObject.FindGameObjectWithTag ("ConstructorWithParameters").transform.Find ("ParametersPlatform");

		nameParameterBox = parameterPlatform.Find ("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
		ageParameterBox = parameterPlatform.Find ("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();

        ageBox = GameObject.Find ("Age_InstanceBox").GetComponent<VariableBoxController> ();
		nameBox = GameObject.Find ("Name_InstanceBox").GetComponent<VariableBoxController> ();
        instanceBox = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();

        address = GameObject.FindGameObjectWithTag("AddressBox").GetComponent<AddressBoxController>();
        mainMovePoint = GameObject.Find("MainMovePoint");

        instanceContainer = GameObject.Find("InstanceContainer");
        insConScale = instanceContainer.transform.localScale;
        instanceContainer.transform.localScale = new Vector3(0, 0, 0);

        objectives.Add("new Person");
        objectives.Add("Person(\"John\",20)");
        objectives.Add("\"John\"");
        objectives.Add("20");
        objectives.Add("name;");
        objectives.Add("this->name =");
        objectives.Add("age;");
        objectives.Add("this->age =");
		objectives.Add ("}");
        objectives.Add("Person *p1 =");


    }

    // Use this for initialization
    void Start()
    {
        setUpScales();
        notepad.blinkObjective (objectives [0]);
		addObjectToBlink (GameObject.Find ("Blueprint"));
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
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/Constructor/Door/DoorExt/DoorPanel"));

        GameObject constrMovePoint = instance.transform.Find("DefaultConstructor/MovePoint").gameObject;
        player.moveTo(constrMovePoint);
        StartCoroutine("checkInfrontOfConstructor");
    }

    IEnumerator checkInfrontOfConstructor()
    {
        Vector3 infrontConstructor = instance.transform.Find("Constructor/MovePoint").position;
        while (!player.checkPlayerPos(infrontConstructor))
        {
            yield return new WaitForSeconds(1f);
        }
        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(false);

        showMainAreaValues(true);
        notepad.blinkObjective(objectives[2]);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("John"));

        StartCoroutine("checkNameInHand");
    }

    IEnumerator checkNameInHand()
    {
        while (!nameValueInHand(GameObject.Find("John")))
        {
            yield return new WaitForSeconds(1f);
        }
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/Constructor/ParametersPlatform/NameParameter/NameParameterBox"));

        StartCoroutine("checkNameParameterSet");
    }

    IEnumerator checkNameParameterSet()
	{
		while (!nameParameterSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [3]);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("20"));


		StartCoroutine("checkAgeInHand");
	}

    IEnumerator checkAgeInHand()
    {
        while (!nameValueInHand(GameObject.Find("20")))
        {
            yield return new WaitForSeconds(1f);
        }
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/Constructor/ParametersPlatform/AgeParameter/AgeParameterBox"));

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
		notepad.blinkDuplicateObjective (objectives [4], 2);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/Constructor/ParametersPlatform/NameParameter/NameParameterBox"));

        instance.EnableMovePositions(false);
        DeactivateParamBox(ageParameterBox.transform);
        DeactivateInstanceBox(ageBox.transform);
        DeactivateInstanceBox(nameBox.transform);

		StartCoroutine("checkNameInHand2");
	}

    IEnumerator checkNameInHand2()
    {
        while (hand.getObjInHand() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[5]);
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/Name_InstanceBox"));

        instance.EnableMovePositions(false);
        DeactivateParamBox(nameParameterBox.transform);
        ActivateInstanceBox(nameBox.transform);

        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
	{
		while (!nameSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkDuplicateObjective (objectives [6], 2);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/Constructor/ParametersPlatform/AgeParameter/AgeParameterBox"));

        ActivateParamBox(ageParameterBox.transform);
        DeactivateInstanceBox(nameBox.transform);

		StartCoroutine("checkAgeInHand2");
	}

    IEnumerator checkAgeInHand2()
    {
        while (hand.getObjInHand() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[7]);
        resetObjectsToBlink();
        addObjectToBlink(GameObject.Find("Instance/Heptagon Instance/Age_InstanceBox"));

        DeactivateParamBox(ageParameterBox.transform);
        ActivateInstanceBox(ageBox.transform);
        StartCoroutine("checkAgeSet");
    }

    IEnumerator checkAgeSet()
	{
		while (!ageSet())
		{
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective (objectives [8]);
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/Constructor/Door/DoorExt/DoorPanel"));

        DeactivateInstanceBox(ageBox.transform);

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
        notepad.blinkObjective(objectives[9]);

		resetObjectsToBlink ();
		addObjectToBlink(GameObject.Find("InstanceContainer"));
			
        address.ToHands();

        StartCoroutine("checkPlayerInMain");
    }

    IEnumerator checkPlayerInMain()
    {
        yield return new WaitForSeconds(1.9f);
        instance.SetInstanceCompletion(true);

        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(true);
        player.moveTo(mainMovePoint);

        while (!player.checkPlayerPos(mainMovePoint.transform.position))
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

    bool nameValueInHand(GameObject value)
    {
        if (hand.getObjInHand() == value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void DeactivateParamBox(Transform box)
    {
        box.parent.GetComponent<ParameterBoxMenuController>().enabled = false;

        Transform panel = box.Find("OptionMenu/Panel");

        GameObject copy = panel.Find("CopyButton").gameObject;
        GameObject assign = panel.Find("AssignButton").gameObject;
        GameObject info = panel.Find("InfoButton").gameObject;

        copy.SetActive(false);
        assign.SetActive(false);
        info.SetActive(true);
        
    }

    void DeactivateInstanceBox(Transform box)
    {
        box.GetComponent<InstanceVariablesMenuController>().enabled = false;

        Transform panel = box.Find("OptionMenu/Panel");

        GameObject copy = panel.Find("CopyButton").gameObject;
        GameObject assign = panel.Find("AssignButton").gameObject;
        GameObject increment = panel.Find("IncrementButton").gameObject;
        GameObject info = panel.Find("InfoButton").gameObject;

        copy.SetActive(false);
        assign.SetActive(false);
        increment.SetActive(false);
        info.SetActive(true);

    }

    void ActivateInstanceBox(Transform box)
    {
        box.GetComponent<InstanceVariablesMenuController>().enabled = true;
    }


    void ActivateParamBox(Transform box)
    {
        box.parent.GetComponent<ParameterBoxMenuController>().enabled = true;
    }
}
