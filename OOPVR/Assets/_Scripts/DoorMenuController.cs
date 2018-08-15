using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenuController : MonoBehaviour
{

    string ActivityName;

    List<GameObject> inactiveDoors = new List<GameObject>();

    Transform activeDoorOptionMenuPanel;
    GameObject GoIntoButton, InfoButton,Return, activeGoInto, activeInfo, activeReturn;
    bool isMultiInstance;

    void Awake()
    {

        
    }
    // Use this for initialization
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MultiInstancesMethodCallsActivity" || scene.name == "MultipleInstancesActivity")
        {
            isMultiInstance = true;
        }
        else
        {
            isMultiInstance = false;
        }

        GetActivityName(scene.name);
        GetAllDoors();

        ActivateOptions();
        SetUpInActiveOptions();
        
    }

    void ActivateOptions()
    {
            foreach (GameObject method in inactiveDoors)
            {
                Transform doorExtOptionPanel = method.transform.Find("DoorExt/OptionMenu/Panel");
                GoIntoButton = doorExtOptionPanel.Find("GoIntoButton").gameObject;
                InfoButton = doorExtOptionPanel.Find("InfoButton").gameObject;
                Return = doorExtOptionPanel.Find("Return").gameObject;

                ShowButton(GoIntoButton, true);
                ShowButton(InfoButton, true);
                ShowButton(Return, false);

                GameObject doorIntDoorPanel = method.transform.Find("DoorInt/DoorPanel").gameObject;
                SetInteractive(doorIntDoorPanel, false);
            }                    
    }

    void SetUpInActiveOptions()
    {
        if (isMultiInstance)
        {
            Transform instance1 = GameObject.FindGameObjectWithTag("Instance1").transform;
            Transform instance2 = GameObject.FindGameObjectWithTag("Instance2").transform;

            if(ActivityName == "MultiMethod")
            {
                SetUpInActiveMethod(instance1.Find("IncrementAge"));
                SetUpInActiveMethod(instance1.Find("Constructor"));
                SetUpInActiveMethod(instance1.Find("DefaultConstructor"));
                SetUpInActiveMethod(instance1.Find("SetName"));

                SetUpInActiveMethod(instance2.Find("IncrementAge"));
                SetUpInActiveMethod(instance2.Find("Constructor"));
                SetUpInActiveMethod(instance2.Find("DefaultConstructor"));
                SetUpInActiveMethod(instance2.Find("SetName"));
            }
            else
            {
                SetUpInActiveMethod(instance1.Find("Constructor"));
                SetUpInActiveMethod(instance1.Find("DefaultConstructor"));

                SetUpInActiveMethod(instance2.Find("Constructor"));
                SetUpInActiveMethod(instance2.Find("DefaultConstructor"));
            }
            
        }
        else
        {
            Transform instance = GameObject.FindGameObjectWithTag("Instance").transform;

            if (ActivityName == "GetName")
            {
                SetUpInActiveMethod(instance.Find("SetName"));
                SetUpInActiveMethod(instance.Find("IncrementAge"));
            }
            if (ActivityName == "IncrementAge")
            {
                SetUpInActiveMethod(instance.Find("SetName"));
                SetUpInActiveMethod(instance.Find("GetName"));
            }
            if (ActivityName == "DefaultConstructor")
            {
                //SetUpInActiveMethod(instance.Find("SetName"));
                //SetUpInActiveMethod(instance.Find("GetName"));
                //SetUpInActiveMethod(instance.Find("IncrementAge"));
                SetUpInActiveMethod(instance.Find("Constructor"));
            }
            if (ActivityName == "Constructor")
            {
                //SetUpInActiveMethod(instance.Find("SetName"));
                //SetUpInActiveMethod(instance.Find("GetName"));
                //SetUpInActiveMethod(instance.Find("IncrementAge"));
                SetUpInActiveMethod(instance.Find("Constructor"));
                SetUpInActiveMethod(instance.Find("DefaultConstructor"));
            }
            if (ActivityName == "SetName")
            {
                SetUpInActiveMethod(instance.Find("SetName"));
                SetUpInActiveMethod(instance.Find("GetName"));
                SetUpInActiveMethod(instance.Find("IncrementAge"));
            }
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
            ActivateOptions();
            SetUpInActiveOptions();
        }
    }

    void SetUpInActiveMethod(Transform room)
    {
        GameObject goInto = room.Find("Door/DoorExt/OptionMenu/Panel/GoIntoButton").gameObject;
        GameObject info = room.Find("Door/DoorExt/OptionMenu/Panel/InfoButton").gameObject;
        GameObject rtn = room.Find("Door/DoorExt/OptionMenu/Panel/Return").gameObject;

        ShowButton(goInto, false);
        ShowButton(info, true);
        ShowButton(rtn, false);

    }

    void GetActivityName(string activityName)
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
            case "GetNameActivity":
                ActivityName = "GetName";
                break;
            case "IncrementAgeActivity":
                ActivityName = "IncrementAge";
                break;
            case "MultipleInstancesActivity":
                ActivityName = "MultiConstructor";
                break;
            case "MultiInstancesMethodCallsActivity":
                ActivityName = "MultiMethod";
                break;
            default:
                break;
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
