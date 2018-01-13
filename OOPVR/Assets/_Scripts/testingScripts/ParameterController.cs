using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterController : MonoBehaviour
{
    public Material activeColor;
    private GameObject doorExt, doorInt;
    private GameObject Player;
    private GameObject[] parameters;
    private Transform[] varBoxes;
    private int numOfParams;
    private Material normalColor;
    private static int count = 0;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        doorExt = transform.parent.Find("DoorExt").gameObject;
        doorInt = transform.parent.Find("DoorInt").gameObject;
        numOfParams = transform.childCount;
        parameters = new GameObject[numOfParams];
        varBoxes = new Transform[numOfParams];
        normalColor = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ControlPlatform()
    {
        
        print("Control Platform -> Animate open Door and Move PLAYER");

        StartCoroutine(Control());
        

    }

    private IEnumerator Control()
    {
        yield return new WaitForSeconds(1.5f);
        //Visual effect
        GetComponent<Renderer>().material = activeColor;
        //GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(.5f);
       
       
        //Open Door
        doorExt.GetComponent<Door>().openDoor();
        doorInt.GetComponent<Door>().openDoor();
        yield return new WaitForSeconds(2);

       
        //Move Player
        GameObject roomObject = transform.parent.gameObject;
        GetComponent<Renderer>().material = normalColor;
        Player.GetComponent<PlayerController>().moveIntoRoom(roomObject);
        //GetComponent<Light>().enabled = false;
    }

    public void addVariableBox(Transform vb)
    {
        if(numOfParams == 1)
        {
            ControlPlatform();
        }
        else
        {
            varBoxes[count] = vb;
            count++;

            if(numOfParams == count)
            {
                ControlPlatform();
            }
        }
    }

}
