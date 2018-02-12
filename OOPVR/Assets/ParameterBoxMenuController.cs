using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterBoxMenuController : MonoBehaviour {

    GameObject OptionMenuPanel;

    Transform Player;
    Transform methodMovePoint;
    Transform insideMethodPoint;

    private GameObject hiddenButton;

    bool playerInsideMethod = false;
    bool playerOnMovePoint = false;

	// Use this for initialization
	void Start () {

        OptionMenuPanel = transform.Find("ParameterVariableBox").Find("OptionMenu").Find("Panel").gameObject;
        Player = GameObject.Find("Player").transform;

        insideMethodPoint = transform.parent.parent.Find("PlayerDest");
        methodMovePoint = transform.parent.parent.Find("MovePoint");

	}
	
	// Update is called once per frame
	void Update () {

        if ((Player.position.x >= methodMovePoint.position.x - 0.5) &&
            (Player.position.x <= methodMovePoint.position.x + 0.5))
        {

            playerOnMovePoint = true;

        }
        else
        {
            playerOnMovePoint = false;
        }

        if (Player.position.x == insideMethodPoint.position.x )
        {

            playerInsideMethod = true;

        }
        else
        {
            playerInsideMethod = false;
        }


        if (playerInsideMethod)
        {
            ShowButton(hiddenButton);
            //HideButton("AssignButton");
            
            
        }

        if (playerOnMovePoint)
        {
            HideButton("CopyButton");
            //ShowButton(hiddenButton);
        }
    }

    void HideButton(string button)
    {
        hiddenButton = OptionMenuPanel.transform.Find(button).gameObject;
        OptionMenuPanel.transform.Find(button).gameObject.SetActive(false);
    }

    void ShowButton(GameObject button)
    {
        button.SetActive(true);
    }

    

}
