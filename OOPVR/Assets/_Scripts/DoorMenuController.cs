using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenuController : MonoBehaviour
{

    private string ActivityName;

    private List<GameObject> inactiveDoors = new List<GameObject>();

    Transform activeDoorOptionMenuPanel;
    GameObject GoIntoButton, InfoButton,Return, activeGoInto, activeInfo, activeReturn;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        GetActiveDoor(scene.name);
        GetAllDoors();
    }
    // Use this for initialization
    void Start()
    {
        
        SetUpInActiveOptions();
        SetUpActiveOptions();
        
    }
    void SetUpInActiveOptions()
    {
        //Control options for other methods not in the activity
        foreach (GameObject method in inactiveDoors)
        {
            Transform doorExtOptionPanel = method.transform.Find("DoorExt/OptionMenu/Panel");
            GoIntoButton = doorExtOptionPanel.Find("GoIntoButton").gameObject;
            InfoButton = doorExtOptionPanel.Find("InfoButton").gameObject;
            Return = doorExtOptionPanel.Find("Return").gameObject;

            ShowButton(GoIntoButton, false);
            ShowButton(InfoButton, true);
            ShowButton(Return, false);

            GameObject doorIntDoorPanel = method.transform.Find("DoorInt/DoorPanel").gameObject;
            SetInteractive(doorIntDoorPanel, false);
        }
    }

    void SetUpActiveOptions()
    {
        //Control options for active activity
        if(ActivityName != "MultiMethod")
        {
            GameObject activityMethod = GameObject.Find(ActivityName);
            activeDoorOptionMenuPanel = activityMethod.transform.Find("Door/DoorExt/OptionMenu/Panel");
            activeGoInto = activeDoorOptionMenuPanel.Find("GoIntoButton").gameObject;
            activeInfo = activeDoorOptionMenuPanel.Find("InfoButton").gameObject;
            activeReturn = activeDoorOptionMenuPanel.Find("Return").gameObject;
            GameObject activeIntDoorPanel = activityMethod.transform.Find("Door/DoorInt/DoorPanel").gameObject;
            SetInteractive(activeIntDoorPanel, false);
        }
        

        if (ActivityName == "Constructor" || ActivityName == "SetName")
        {
            ShowButton(activeGoInto, false);
            ShowButton(activeInfo, true);
            ShowButton(activeReturn, false);
        }
        else if(ActivityName == "MultiMethod")
        {
            GameObject instance1 = GameObject.FindGameObjectWithTag("Instance1");
            GameObject instance2 = GameObject.FindGameObjectWithTag("Instance2");

            SetUpActiveMethod(instance1.transform.Find("SetName"));
            SetUpActiveMethod(instance1.transform.Find("GetName"));
            SetUpActiveMethod(instance2.transform.Find("GetName"));
        }
        else
        {
            ShowButton(activeGoInto, true);
            ShowButton(activeInfo, true);
            ShowButton(activeReturn, false);
        }
    }

    public void EnableDoorIndoorOptions(Transform optionMenu, bool key)
    {
        GameObject goInto = optionMenu.Find("Panel/GoIntoButton").gameObject;
        GameObject info = optionMenu.Find("Panel/InfoButton").gameObject;
        GameObject rtn = optionMenu.Find("Panel/Return").gameObject;

        if (key)
        {
            ShowButton(goInto, !key);
            ShowButton(info, !key);
            ShowButton(rtn, key);
        }
        else
        {
            SetUpInActiveOptions();
            SetUpActiveOptions();
        }
    }

    void GetActiveDoor(string activityName)
    {
        switch (activityName)
        {
            case "DefaultConstructorScene":
                ActivityName = "DefaultConstructor";
                break;
            case "2ParameterConstructor":
                ActivityName = "Constructor";
                break;
            case "SetNameActivity":
                ActivityName = "SetName";
                break;
            case "GetName":
                ActivityName = "GetName";
                break;
            case "IncrementAgeActivity":
                ActivityName = "IncrementAge";
                break;
            case "MultipleInstancesActivity":
                ActivityName = "Constructor";
                break;
            case "MultiInstancesMethodCallsActivity":
                ActivityName = "MultiMethod";
                break;
            default:
                break;
        }
    }

    void SetUpActiveMethod(Transform room)
    {
        GameObject goInto = room.Find("Door/DoorExt/OptionMenu/Panel/GoIntoButton").gameObject;
        GameObject info = room.Find("Door/DoorExt/OptionMenu/Panel/InfoButton").gameObject;
        GameObject rtn = room.Find("Door/DoorExt/OptionMenu/Panel/Return").gameObject;

        if(room.name == "Constructor" || room.name == "SetName")
        {
            ShowButton(goInto, false);
            ShowButton(info, true);
            ShowButton(rtn, false);
        }
        else
        {
            ShowButton(goInto, true);
            ShowButton(info, true);
            ShowButton(rtn, false);
        }

    }
    void GetAllDoors()
    {
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject doors in allDoors)
        {
            if (doors.name == "Door")
            {
                inactiveDoors.Add(doors);
            }
        }
    }

    void SetInteractive(GameObject doorPanel, bool key)
    {
        doorPanel.GetComponent<InteractiveItemGaze>().enabled = key;
    }

    public void ShowButton(GameObject button, bool key)
    {
        button.SetActive(key);
    }
}
