using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanksController : MonoBehaviour {

    Transform Door, DoorExt;
    Vector3 planksScale;


	// Use this for initialization
	void Awake () {


        Door = transform.parent.Find("Door");
        DoorExt = Door.Find("DoorExt");
        transform.localScale = new Vector3(0, 0, 0);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnablePlanks(bool key)
    {
        if (key)
        {
            //Door.GetComponent<Collider>().enabled = key;
            DoorExt.GetComponent<Collider>().enabled = key;

            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
           // Door.GetComponent<Collider>().enabled = true;
            DoorExt.GetComponent<Collider>().enabled = key;

            transform.localScale = new Vector3(0, 0, 0);
        }

        
    }
}
