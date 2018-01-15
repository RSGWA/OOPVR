using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialGaze : MonoBehaviour
{
    public float NumberOfSecondsForSelection = 3f;
    public Transform RadialProgress;
    private float counterTimer = 0;
    private static string variableType = "";
    private Transform platformPos;

    private GameObject player;

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
        RadialProgress.GetComponent<Image>().fillAmount = counterTimer / NumberOfSecondsForSelection;

        if (counterTimer >= NumberOfSecondsForSelection)
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
        platformPos = obj.transform;
	
        switch (obj.tag)
        {
            case "Variable":
                variableType = obj.transform.GetChild(0).tag;
                // Pick up variable
                obj.GetComponent<VariableController>().ToHands();
                break;
            case "Value":
                // Pick up Value
                obj.GetComponent<ValueController>().ToHands();
                break;
            case "Box":
                // Place in box
                obj.GetComponent<BoxController>().boxAction();
                break;
            case "VariableBox":
                // Place in box
                obj.GetComponent<VariableBoxController>().boxAction();
                break;
            case "Platform":
                obj.GetComponent<PlatformController>().ToPlatform(platformPos);
                obj.GetComponent<PlatformController>().ControlPlatform(obj, variableType);
                break;
            case "Move":
                player.GetComponent<PlayerController>().moveTo(obj);
                break;
            default:
                break;
        }
    }
}
