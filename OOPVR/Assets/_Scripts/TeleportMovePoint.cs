using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMovePoint : MonoBehaviour
{

    GameObject Player;
    GameObject glow;

    bool playerOnPoint = false;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        glow = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		if ((Player.transform.position.x >= transform.position.x - 0.5) &&
			(Player.transform.position.x <= transform.position.x + 0.5)) {
			DisableMovePoint();
		} 
		else
		{
			EnableMovePoint();
		}
		/*
        if (Player.transform.position.x == transform.position.x)
        {
            DisableMovePoint();
        }
        else
        {
            EnableMovePoint();
        }
        */

    }


    void EnableMovePoint()
    {
        //disable selection
        transform.GetComponent<Collider>().enabled = true;

        //disable glow
        glow.SetActive(true);

    }


    void DisableMovePoint()
    {
        transform.GetComponent<Collider>().enabled = false; //disable selection

        glow.SetActive(false); //disable glow
    }


	public void ShowMovePoint(bool b)
    {
        this.gameObject.SetActive(b);
    }
}
