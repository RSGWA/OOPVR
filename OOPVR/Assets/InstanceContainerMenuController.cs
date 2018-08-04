using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanceContainerMenuController : MonoBehaviour {

    GameObject OptionMenuPanel;

    GameObject CopyButton, AssignButton, InfoButton, GotoAddress;

    Transform Player;
    Transform outsideMethodPoint;
    Transform insideMethodPoint;

    private GameObject hiddenButton;

    bool playerInsideMethod = false;
    bool playerOnMovePoint = false;
    string ActivityName;

    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        GetActivity(scene.name);
    }
    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        OptionMenuPanel = transform.Find("OptionMenu/Panel").gameObject;

        CopyButton = OptionMenuPanel.transform.Find("CopyButton").gameObject;
        AssignButton = OptionMenuPanel.transform.Find("AssignButton").gameObject;
        GotoAddress = OptionMenuPanel.transform.Find("GoToAddress").gameObject;
        InfoButton = OptionMenuPanel.transform.Find("InfoButton").gameObject;

        if (ActivityName == "DefaultConstructor" || ActivityName == "Constructor" || ActivityName == "MultiInstanceConstructor")
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, true);
            ShowButton(GotoAddress, false);
            ShowButton(InfoButton, true);
        }
        else
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, false);
            ShowButton(GotoAddress, true);
            ShowButton(InfoButton, true);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetActivity(string activityName)
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
                ActivityName = "MultiInstanceConstructor";
                break;
            case "MultiInstancesMethodCallsActivity":
                ActivityName = "MultiInstanceMethod";
                break;
            default:
                break;
        }
    }
    void ShowButton(GameObject button, bool key)
    {
        button.SetActive(key);
    }
}
