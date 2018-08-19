using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultConstructor : ActivityController
{

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

    List<string> objectives = new List<string>();

    VariableBoxController ageBox, nameBox, instanceBox;
    GameObject instanceContainer, mainMovePoint, emptyStringPickUp;
    AddressBoxController address;
    PlanksController entrance;
	BlueprintController bp;
	HandController hand;

	public GameObject ageValue, nameValue;

    Transform methodRoom;
    Vector3 inRoomPos, insConScale;

	private static float BLINK_DELAY = 4f;

    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		bp = GameObject.FindGameObjectWithTag ("Blueprint").GetComponent<BlueprintController> ();

        instanceBox = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        address = GameObject.FindGameObjectWithTag("AddressBox").GetComponent<AddressBoxController>();

        ageBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        nameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        instanceContainer = GameObject.Find("InstanceContainer");
        insConScale = instanceContainer.transform.localScale;
        instanceContainer.transform.localScale = new Vector3(0, 0, 0);

        emptyStringPickUp = instance.transform.Find("DefaultConstructor/\" \"/OptionMenu/Panel/PickUpButton").gameObject;

        mainMovePoint = GameObject.Find("MainMovePoint");

        entrance = GameObject.FindGameObjectWithTag("Instance").transform.Find("DefaultConstructor/Planks").GetComponent<PlanksController>();
	
		hand = GameObject.FindGameObjectWithTag ("Hand").GetComponent<HandController> ();

        objectives.Add("new Person();");
        objectives.Add("Person::Person() {");
		objectives.Add("0");
        objectives.Add("this->age =");
        objectives.Add("\" \"");
		objectives.Add("this->name =");
        objectives.Add("}");
        objectives.Add("Person *p1 =");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
		addObjectToBlink (GameObject.Find ("Blueprint"));
        emptyStringPickUp.SetActive(false);
		//objBlink.blinkObject (GameObject.Find ("Blueprint"));
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
            yield return new WaitForSeconds(1.5f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("CONSTRUCTOR");
        notepad.blinkObjective(objectives[1]);

		//objBlink.blinkObject (GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/Door/DoorExt/DoorPanel"));
		resetObjectsToBlink();
		addObjectToBlink(GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/Door/DoorExt/DoorPanel"));
        //Move player in front of constructor
        GameObject constrMovePoint = instance.transform.Find("DefaultConstructor/MovePoint").gameObject;
        player.moveTo(constrMovePoint);
        mainMovePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(false);
        StartCoroutine("checkConstructorEntered");
    }

    IEnumerator checkConstructorEntered()
    {
        inRoomPos = GameObject.FindGameObjectWithTag("ConstructorNoParameters").transform.Find("PlayerDest").position;
        while (!player.checkPlayerPos(inRoomPos))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instance.EnableMovePositions(false);

        notepad.blinkObjective(objectives[2]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/0"));
		StartCoroutine("checkAgeValueInHand");
    }

	IEnumerator checkAgeValueInHand() {

		while (!ageValueInHand ()) {
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective(objectives[3]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/Age_InstanceBox"));
		StartCoroutine("checkAgeSet");
	}

    IEnumerator checkAgeSet()
    {
        while (!ageSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        emptyStringPickUp.SetActive(true);
        notepad.blinkObjective(objectives[4]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/\" \""));
        StartCoroutine("checkNameValueInHand");
    }

	IEnumerator checkNameValueInHand() {

		while (!nameValueInHand ()) {
			yield return new WaitForSeconds(0.1f);
		}
		notepad.blinkObjective(objectives[5]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/Name_InstanceBox"));
		StartCoroutine("checkNameSet");
	}

    IEnumerator checkNameSet()
    {
        while (!nameSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[6]);
		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/Door/DoorExt/DoorPanel"));

        //Enable Door selection for exit
        instance.transform.Find("DefaultConstructor/Door/DoorExt").GetComponent<Door>().enableDoor();
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

        retrieveAddress();
        
        StartCoroutine("checkPlayerInMain");
        
    }

    IEnumerator checkPlayerInMain()
    {
        yield return new WaitForSeconds(1.9f);
        instance.SetInstanceCompletion(true);       

        //mainMovePoint = GameObject.Find("MainMovePoint");
        player.moveTo(mainMovePoint);
        instance.EnableMovePositions(false);

        while (!player.checkPlayerPos(mainMovePoint.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instanceContainer.transform.localScale = insConScale;

		resetObjectsToBlink ();
		addObjectToBlink (GameObject.Find ("InstanceContainer"));

        StartCoroutine("checkInstanceContainer");
    }

    IEnumerator checkInstanceContainer()
    {
        while (!instanceSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Activity Finished
        PlayerPrefs.SetInt("ConstructorNoParametersComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }

    void retrieveAddress()
    {
        address.ToHands();
    }

    bool ageSet()
    {
        return ageBox.isVarInBox();
    }

    bool nameSet()
    {
        return nameBox.isVarInBox();
    }

    bool instanceSet()
    {
        return instanceBox.isVarInBox();
    }

	bool ageValueInHand()
	{
		if (hand.getObjInHand () == ageValue) {
			return true;
		} else {
			return false;
		}
	}

	bool nameValueInHand()
	{
		if (hand.getObjInHand () == nameValue) {
			return true;
		} else {
			return false;
		}
	}

}
