using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterBoxMenuController : MonoBehaviour {

    GameObject OptionMenuPanel;

    GameObject CopyButton, AssignButton, InfoButton;

    Transform Player;
    Transform outsideMethodPoint;
    Transform insideMethodPoint;

    

    private GameObject hiddenButton;

    bool playerInsideMethod = false;
    bool playerOnMovePoint = false;

	// Use this for initialization
	void Start () {

        OptionMenuPanel = transform.GetChild(1).Find("OptionMenu").Find("Panel").gameObject;
        CopyButton = OptionMenuPanel.transform.Find("CopyButton").gameObject;
        AssignButton = OptionMenuPanel.transform.Find("AssignButton").gameObject;
        InfoButton = OptionMenuPanel.transform.Find("InfoButton").gameObject;

        Player = GameObject.Find("Player").transform;

        insideMethodPoint = transform.parent.parent.Find("PlayerDest");
        outsideMethodPoint = transform.parent.parent.Find("MovePoint");

	}
	
	// Update is called once per frame
	void Update () {

        if (Player.position.x == outsideMethodPoint.position.x)
        {
            PlayerOnMovePoint();


        }

        if (Player.position.x == insideMethodPoint.position.x )
        {
            PlayerInsideMethod();

        }
    }

    void ShowButton(GameObject button, bool key)
    {
        button.SetActive(key);
    }

    void PlayerOnMovePoint()
    {
        ShowButton(CopyButton, false);
        ShowButton(AssignButton, true);
        ShowButton(InfoButton, true);
    }

    void PlayerInsideMethod()
    {
        ShowButton(CopyButton, true);
        ShowButton(AssignButton, false);
        ShowButton(InfoButton, true);
    }




}
