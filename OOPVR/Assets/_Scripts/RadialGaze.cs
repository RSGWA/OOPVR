using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialGaze : MonoBehaviour
{
    
    public Transform RadialProgress;

    private float NumberOfSecondsForSelection = 1f;
    private float counterTimer = 0f;

    private GameObject player;
    private static GameObject currentSelectedObj;
    private static GameObject previousSelectedObj;

	public float extendTime;

    // Use this for initialization
    void Start()
    {
        RadialProgress.GetComponent<Image>().fillAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        this.enabled = false;
    }

    // Update is called once per frame
    public void Update()
    {

        counterTimer += Time.deltaTime;
		RadialProgress.GetComponent<Image>().fillAmount = counterTimer / (NumberOfSecondsForSelection + extendTime);

		if (counterTimer >= (NumberOfSecondsForSelection + extendTime))
        {
            PerformActionOnGameObject();
            ResetCounter();
        }
    }

    public void ResetCounter()
    {
        counterTimer = 0f;
        RadialProgress.GetComponent<Image>().fillAmount = counterTimer;
    }

    public void PerformActionOnGameObject()
    {
        GameObject obj = GvrPointerInputModule.CurrentRaycastResult.gameObject;
        switch (obj.tag)
        {
            case "Value":
                //Bring up options for Value
                //obj.GetComponent<OptionMenu>().ControlMenu();
                SetSelectedObject(obj);
                break;
            case "VariableBox":
                // Bring up Options for VariableBox
                //obj.GetComponent<OptionMenu>().ControlMenu();
                SetSelectedObject(obj);
                break;
            case "Move":
                player.GetComponent<PlayerController>().moveTo(obj);
                break;
            case "Door":
                //obj.GetComponent<OptionMenu>().ControlMenu();
                SetSelectedObject(obj);
                break;
            case "Return":
                //obj.GetComponent<OptionMenu>().ControlMenu();
                SetSelectedObject(obj);
                break;
			case "Next":
				obj.GetComponentInParent<Activities> ().nextActiviy ();
				break;
			case "Previous":
				obj.GetComponentInParent<Activities> ().previousActivity ();
				break;
            default:
                break;
        }

		// Stops radial gaze restarting timer after an action is started
		this.enabled = false;
    }

    private void SetSelectedObject(GameObject obj)
    {

        if(currentSelectedObj == null)
        {
            currentSelectedObj = obj;
            previousSelectedObj = null;
            currentSelectedObj.GetComponent<OptionMenu>().Select();
        }
        else
        {
                previousSelectedObj = currentSelectedObj;
                currentSelectedObj = obj;

                //Deselect previousSO
                previousSelectedObj.GetComponent<OptionMenu>().Deselect();

                //Select currentSO
                currentSelectedObj.GetComponent<OptionMenu>().Select();

        }
        
        


    }
}
