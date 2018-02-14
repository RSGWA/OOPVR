using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceVariablesMenuController : MonoBehaviour
{

    GameObject OptionMenuPanel;

    GameObject CopyButton, AssignButton, InfoButton, IncrementButton;

    Transform insideDConstructor, insideConstructor, insideSetName, insideGetName, insideIncrementAge;

    Transform Player;
    Transform outsideMethodPoint;
    Transform insideMethodPoint;

    private GameObject hiddenButton;

    bool playerInsideMethod = false;
    bool playerOnMovePoint = false;

    // Use this for initialization
    void Start()
    {
        OptionMenuPanel = transform.Find("OptionMenu").Find("Panel").gameObject;

        CopyButton = OptionMenuPanel.transform.Find("CopyButton").gameObject;
        AssignButton = OptionMenuPanel.transform.Find("AssignButton").gameObject;
        InfoButton = OptionMenuPanel.transform.Find("InfoButton").gameObject;
        IncrementButton = OptionMenuPanel.transform.Find("IncrementButton").gameObject;

        Player = GameObject.Find("Player").transform;

        insideDConstructor = transform.parent.Find("DefaultConstructor").Find("PlayerDest");
        insideConstructor = transform.parent.Find("Constructor").Find("PlayerDest");
        insideSetName = transform.parent.Find("SetName").Find("PlayerDest");
        insideGetName = transform.parent.Find("GetName").Find("PlayerDest");
        insideIncrementAge = transform.parent.Find("IncrementAge").Find("PlayerDest");
    }

    // Update is called once per frame
    void Update()
    {

        if (Player.position.x == insideDConstructor.position.x)
        {
            PlayerInsideDConstructor();


        }

        if (Player.position.x == insideConstructor.position.x)
        {
            PlayerInsideConstructor();

        }

        if (Player.position.x == insideSetName.position.x)
        {
            PlayerInsideSetName();

        }

        if (Player.position.x == insideGetName.position.x)
        {
            PlayerInsideGetName();

        }

        if (Player.position.x == insideIncrementAge.position.x)
        {
            PlayerInsideIncrementAge();

        }
    }

    void ShowButton(GameObject button, bool key)
    {
        button.SetActive(key);
    }


    void PlayerInsideDConstructor()
    {
        ShowButton(CopyButton, false);
        ShowButton(AssignButton, true);
        ShowButton(IncrementButton, false);
        ShowButton(InfoButton, true);

    }

    void PlayerInsideConstructor()
    {
        ShowButton(CopyButton, false);
        ShowButton(AssignButton, true);
        ShowButton(IncrementButton, false);
        ShowButton(InfoButton, true);
    }

    void PlayerInsideSetName()
    {
        if (transform.name == "Age_InstanceBox")
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, false);
            ShowButton(IncrementButton, false);
            ShowButton(InfoButton, true);
        }
        else
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, true);
            ShowButton(IncrementButton, false);
            ShowButton(InfoButton, true);
        }
       
    }

    void PlayerInsideGetName()
    {
        if (transform.name == "Age_InstanceBox")
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, false);
            ShowButton(IncrementButton, false);
            ShowButton(InfoButton, true);
        }
        else
        {
            ShowButton(CopyButton, true);
            ShowButton(AssignButton, false);
            ShowButton(IncrementButton, false);
            ShowButton(InfoButton, true);
        }
        
    }

    void PlayerInsideIncrementAge()
    {
        if(transform.name == "Age_InstanceBox")
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, false);
            ShowButton(IncrementButton, true);
            ShowButton(InfoButton, true);
        }
        else
        {
            ShowButton(CopyButton, false);
            ShowButton(AssignButton, false);
            ShowButton(IncrementButton, false);
            ShowButton(InfoButton, true);
        }
        
        
    }
}
