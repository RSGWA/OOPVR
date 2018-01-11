using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterController : MonoBehaviour
{
    public Material activeColor;
    private GameObject doors;
    private GameObject[] parameters;
    private Transform[] varBoxes;
    private int numOfParams;
    private static int count = 0;


    // Use this for initialization
    void Start()
    {
        doors = transform.parent.Find("Doors").gameObject;
        numOfParams = transform.childCount;
        parameters = new GameObject[numOfParams];
        varBoxes = new Transform[numOfParams];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ControlPlatform()
    {
        
        print("Control Platform -> Animate open Door and Move PLAYER");

        //Visual effect
        GetComponent<Renderer>().material = activeColor;

        //Open Door
        doors.GetComponent<Door>().ControlDoor();

        //Move Player


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
