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
    GameObject instanceContainer;
    AddressBoxController address;

    Transform methodRoom;
    Vector3 inRoomPos, insConScale, mainMovePoint;


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
        yield return new WaitForSeconds(4f);

        //Move player in front of constructor automatically
        movePlayerAutomatically();
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
        mainMovePoint = GameObject.Find("MainMovePoint").transform.position;

        while (!checkPlayerPos(mainMovePoint))
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

    void movePlayerAutomatically()
    {
        GameObject constrMovePoint = instance.transform.Find("DefaultConstructor/MovePoint").gameObject;
        player.moveTo(constrMovePoint);
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

    bool checkPlayerPos(Vector3 againstPos)
    {
        if ((player.transform.position.x == againstPos.x) && (player.transform.position.z == againstPos.z))
        {
            return true;
        }
        return false;
    }
}
