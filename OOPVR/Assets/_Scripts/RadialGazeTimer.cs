using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialGazeTimer : MonoBehaviour {


    
    public float NumberOfSecondsForSelection = 3f;
    public Transform RadialProgress;
    public string place;

    public GameObject ToDoAction;
    private float counterTimer = 0;

    // Use this for initialization
    void Start () {

        RadialProgress.GetComponent<Image>().fillAmount = 0;

    }
	
	// Update is called once per frame
	public void Update () {
        
        counterTimer += Time.deltaTime;
        RadialProgress.GetComponent<Image>().fillAmount = counterTimer/NumberOfSecondsForSelection;

        if (counterTimer >= NumberOfSecondsForSelection)
        {
            PerformActionOnGameObject();

        }
        
    }

    public void ResetCounter()
    {
        counterTimer = 0f;
        RadialProgress.GetComponent<Image>().fillAmount = counterTimer;
    }


    public void PerformActionOnGameObject()
    {
        if(place == "HAND")
        {
            ResetCounter();
            ToDoAction.GetComponent<ObjectController>().InHands();
        }
        else if (place == "SHELF")
        {
            ResetCounter();
            ToDoAction.GetComponent<ObjectController>().OnTheShelf();
        }

        
    }
}
