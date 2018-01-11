using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterController : MonoBehaviour
{

    private GameObject doors;
    private GameObject paramVariableBox;
    private GameObject paramPlatform;


    // Use this for initialization
    void Start()
    {

        paramVariableBox = transform.GetChild(1).gameObject;
        paramPlatform = transform.GetChild(0).gameObject;
        doors = transform.parent.parent.Find("Doors").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        //int childCount = paramVariableBox.transform.childCount;
        //if (childCount == 3)
        //{
        //    //Light up parameter platform

        //    //Open Door
        //    if(doors != null)
        //    {
        //        doors.GetComponent<Doors>().ControlDoor();
        //    }
        //    print("Doors null");

        //    //Move Player into Method
        //}

    }

    public void ControlPlatform()
    {

        
    }

    public void OpenDoors()
    {

    }

}
