using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class plays the animations (open or close) for the doors and displays 
 * door information accordingly to selected door.
 * 
 * */
public class Door : MonoBehaviour
{

    public string code;

    Animator animator;
    Transform DoorPanel;

    private InfoController info;
    private OptionMenu options;
    private string methodName;
    bool doorOpen;
    bool infoSelected = false;



    void Start()
    {
        
        DoorPanel = transform.Find("DoorPanel");
        doorOpen = false;

        animator = GetComponent<Animator>();

        if (transform.name != "DoorInt")
        {
            options = transform.Find("DoorPanel").GetComponent<OptionMenu>();
        }
        
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();

        methodName = transform.parent.parent.name;
    }

    private void DoorControl(string direction)
    {
        animator.SetTrigger(direction);
    }

    public void openDoor()
    {

        DoorControl("Open");
        DoorPanel.GetComponent<BoxCollider>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        doorOpen = true;
        //Deselect the door outlines before opening
        if(transform.name != "DoorInt")
        {
            options.Deselect();
        }
       
    }

    public void closeDoor()
    {
        DoorControl("Close");
        DoorPanel.GetComponent<BoxCollider>().enabled = true;
        doorOpen = false;
    }

    public void ControlDoor()
    {
        if (doorOpen == false)
        {
            DoorControl("Open");
            doorOpen = true;
        }
        else
        {
            DoorControl("Close");
            doorOpen = false;
        }
    }

    public void enableDoor()
    {
        DoorPanel.GetComponent<BoxCollider>().enabled = true;
        DoorPanel.GetComponent<InteractiveItemGaze>().enabled = true;
    }

    public void disableDoor()
    {
        DoorPanel.GetComponent<BoxCollider>().enabled = false;
        DoorPanel.GetComponent<InteractiveItemGaze>().ResetCurrentSelectedObj();
        DoorPanel.GetComponent<InteractiveItemGaze>().enabled = false;

    }

    public bool isDoorFullyOpen()
    {
        return transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DoorOpenIdle");
    }

    public bool isDoorOpen()
    {
        return doorOpen;
    }

    string getDoorInfoDescription(string methodName)
    {
        
        string description = "";
        switch (methodName)
        {
            case "DefaultConstructor":
                description = "This is the ENTRY POINT into \"Constructor With NO Parameters\" \nThis method allows you to initialise this Instance. ";
                break;
            case "Constructor":
                description = "This is the ENTRY POINT into this Method \nThis method allows you to initialise this Instance with provided values\nPlease assign the CORRECT values into the parameter variables to ACTIVATE the Door";
                break;
            case "SetName":
                description = "This is the ENTRY POINT into \"SetName\" \nThis method allows you to SET the NAME of this Instance.\nThis Door is controlled by the Parameter Platform.Please assign the CORRECT name to the parameter variable to \'go into\' this method";
                break;
            case "GetName":
                description = "This is the ENTRY POINT into \"GetName\" \nThis method GETS you the NAME of this Instance";
                break;
            case "IncrementAge":
                description = "This is the ENTRY POINT into \"IncrementeAge\" \nThis method INCREMENTS the AGE  of this Instance by ONE";
                break;
            default:
                break;
        }

        return description;
    }

    string getMethodName(string methodName)
    {
        string mName = "";
        switch (methodName)
        {
            case "DefaultConstructor":
                mName = "Constructor with NO Parameters";
                break;
            case "Constructor":
                mName = "Constructor with Parameters";
                break;
            case "SetName":
                mName = "SET NAME";
                break;
            case "GetName":
                mName = "GET NAME";
                break;
            case "IncrementAge":
                mName = "INCREMENT AGE";
                break;
            default:
                break;
        }
        return mName;

    }

    public void checkAndReturn()
    {
        string returnType = transform.parent.parent.Find("ReturnType").tag;
        GameObject objInHand = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandController>().getObjInHand();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        string varType;

        if (objInHand == null)
        {
            varType = "Void";
        }
        else
        {
            varType = objInHand.transform.GetChild(0).tag;
        }

        if (varType == returnType)
        {
            // Exit room
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Return();
            options.Deselect();
        }

        player.setInRoom(false);


    }
    public void infoButton()
    {
        options.Deselect();
        string nameMethod = getMethodName(methodName);
        string desc = getDoorInfoDescription(methodName);
        info.SetInformation(nameMethod + "\nThis is a DOOR. " + desc);

    }

}
