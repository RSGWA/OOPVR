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
    AddressBoxController address;

    Transform methodRoom;
    Vector3 inRoomPos;


    void Awake()
    {
        instance = GameObject.FindGameObjectWithTag("Instance").GetComponent<InstanceController>();
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        instanceBox = GameObject.Find("InstanceContainer").GetComponent<VariableBoxController>();
        address = GameObject.FindGameObjectWithTag("AddressBox").GetComponent<AddressBoxController>();

        ageBox = GameObject.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        nameBox = GameObject.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        objectives.Add("new Person();");
        objectives.Add("Person::Person() {");
        objectives.Add("this->age = -1;");
        objectives.Add("this->name = \"\";");
        objectives.Add("}");
        objectives.Add("Person *p = new Person();");
    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
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
        notepad.setTitle("CONSTRUCTOR");
        notepad.blinkObjective(objectives[1]);
        StartCoroutine("checkConstructorEntered");
    }

    IEnumerator checkConstructorEntered()
    {

        while (!checkPlayerPos())
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
        if(address)
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

    bool checkPlayerPos()
    {
        methodRoom = GameObject.FindGameObjectWithTag("ConstructorNoParameters").transform;
        inRoomPos = methodRoom.Find("PlayerDest").position;

        if ((player.transform.position.x == inRoomPos.x) && (player.transform.position.z == inRoomPos.z))

        {
            return true;
        }
        return false;
    }
}
