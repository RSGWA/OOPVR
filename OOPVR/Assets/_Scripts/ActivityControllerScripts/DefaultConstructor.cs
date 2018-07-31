using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultConstructor : MonoBehaviour
{

    InstanceController instance;
    Notepad notepad;
    PlayerController player;

    bool instanceCreated = false;
    bool constructorEntered = false;
    bool returned = false;

    List<string> objectives = new List<string>();

    VariableBoxController ageBox, nameBox, instanceBox;
    GameObject instanceContainer, mainMovePoint;
    AddressBoxController address;
    PlanksController entrance;
	ObjectBlink objBlink;

    Transform methodRoom;
    Vector3 inRoomPos, insConScale;


    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        instanceBox = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        address = GameObject.FindGameObjectWithTag("AddressBox").GetComponent<AddressBoxController>();

        ageBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        nameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        instanceContainer = GameObject.Find("InstanceContainer");
        insConScale = instanceContainer.transform.localScale;
        instanceContainer.transform.localScale = new Vector3(0, 0, 0);

        entrance = GameObject.FindGameObjectWithTag("Instance").transform.Find("DefaultConstructor/Planks").GetComponent<PlanksController>();
	
		objBlink = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectBlink> ();

        objectives.Add("new Person();");
        objectives.Add("Person::Person() {");
        objectives.Add("this->age = -1;");
        objectives.Add("this->name = \"\";");
        objectives.Add("}");
        objectives.Add("Person *p1 = new Person();");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
		objBlink.blinkObject (GameObject.Find ("Blueprint"));
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
            yield return new WaitForSeconds(3f);
        }

        notepad.setActiveText(1);
        notepad.setTitle("CONSTRUCTOR");
        notepad.blinkObjective(objectives[1]);
		objBlink.blinkObject (GameObject.Find ("Instance/Heptagon Instance/DefaultConstructor/Door/DoorExt/DoorPanel"));

        //Move player in front of constructor
        GameObject constrMovePoint = instance.transform.Find("DefaultConstructor/MovePoint").gameObject;
        movePlayerTo(constrMovePoint);
        StartCoroutine("checkConstructorEntered");
    }

    IEnumerator checkConstructorEntered()
    {
        inRoomPos = GameObject.FindGameObjectWithTag("ConstructorNoParameters").transform.Find("PlayerDest").position;
        while (!checkPlayerPos(inRoomPos))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkAgeSet");
    }

    IEnumerator checkAgeSet()
    {
        while (!ageSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[3]);
        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
    {
        while (!nameSet())
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[4]);
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
        notepad.blinkObjective(objectives[5]);

        retrieveAddress();
        
        StartCoroutine("checkPlayerInMain");
        
    }
    IEnumerator checkPlayerInMain()
    {
        yield return new WaitForSeconds(1.9f);
        instance.SetInstanceCompletion(true);       

        mainMovePoint = GameObject.Find("MainMovePoint");
        movePlayerTo(mainMovePoint);

        while (!checkPlayerPos(mainMovePoint.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instanceContainer.transform.localScale = insConScale;
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

    void movePlayerTo(GameObject pos)
    {
        
        player.moveTo(pos);
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

    bool checkPlayerPos(Vector3 againstPos)
    {
        if ((player.transform.position.x == againstPos.x) && (player.transform.position.z == againstPos.z))
        {
            return true;
        }
        return false;
    }

}
