using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenuController : MonoBehaviour
{

    private string ActivityName;

    private List<GameObject> inactiveDoors = new List<GameObject>();

    Transform activeDoorOptionMenuPanel;
    GameObject GoIntoButton, InfoButton, activeGoInto, activeInfo;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        GetActiveDoor(scene.name);
        GetAllDoors();
    }
    // Use this for initialization
    void Start()
    {

        foreach (GameObject method in inactiveDoors)
        {
            Transform doorExtOptionPanel = method.transform.GetChild(0).Find("OptionMenu").Find("Panel");
            GoIntoButton = doorExtOptionPanel.Find("GoIntoButton").gameObject;
            InfoButton = doorExtOptionPanel.Find("InfoButton").gameObject;

            ShowButton(GoIntoButton, false);
            ShowButton(InfoButton, true);

            GameObject doorIntDoorPanel = method.transform.GetChild(1).Find("DoorPanel").gameObject;
            SetInteractive(doorIntDoorPanel, false);
        }

        GameObject activityMethod = GameObject.Find(ActivityName);
        activeDoorOptionMenuPanel = activityMethod.transform.Find("Door").GetChild(0).Find("OptionMenu").Find("Panel");
        activeGoInto = activeDoorOptionMenuPanel.Find("GoIntoButton").gameObject;
        activeInfo = activeDoorOptionMenuPanel.Find("InfoButton").gameObject;

        GameObject activeIntDoorPanel = activityMethod.transform.Find("Door").transform.GetChild(1).Find("DoorPanel").gameObject;


        if (ActivityName == "Constructor" || ActivityName == "SetName")
        {
            ShowButton(activeGoInto, false);
            ShowButton(activeInfo, true);
            SetInteractive(activeIntDoorPanel, false);

        }
        else
        {
            ShowButton(activeGoInto, true);
            ShowButton(activeInfo, true);
            SetInteractive(activeIntDoorPanel, false);
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

    void ShowButton(GameObject button, bool key)
    {
        button.SetActive(key);
    }
}
