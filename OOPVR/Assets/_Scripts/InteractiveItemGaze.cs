using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveItemGaze : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private float NumberOfSecondsForSelection = 1f;
    private float counterTimer = 0f;

    private Transform RadialProgress;
    private GameObject player;
    private GameObject blueprint;

    private static GameObject currentSelectedObj;
    private static GameObject previousSelectedObj;


    private bool isEntered = false;


    // Use this for initialization
    void Start()
    {
        RadialProgress = GameObject.FindGameObjectWithTag("RadialProgress").transform;
        RadialProgress.GetComponent<Image>().fillAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        blueprint = GameObject.FindGameObjectWithTag("Blueprint");
    }

    // Update is called once per frame
    public void Update()
    {
        if (isEntered)
        {
            counterTimer += Time.deltaTime;
            RadialProgress.GetComponent<Image>().fillAmount = counterTimer / NumberOfSecondsForSelection;
            if (counterTimer >= NumberOfSecondsForSelection)
            {
                PerformActionOnGameObject();
                ResetCounter();
                isEntered = false;
            }
        }
        else
        {
            counterTimer = 0;
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
                SetSelectedObject(obj);
                break;
            case "VariableBox":
                // Bring up Options for VariableBox
                SetSelectedObject(obj);
                break;
            case "Move":
                player.GetComponent<PlayerController>().moveTo(obj);
                break;
            case "Blueprint":
                SetSelectedObject(obj);
                break;
            case "Door":
                SetSelectedObject(obj);
                break;
            case "Return":
                SetSelectedObject(obj);
                break;
            case "AddressBox":
                SetSelectedObject(obj);
                break;
            default:
                break;
        }
    }

    private void SetSelectedObject(GameObject obj)
    {

        if (currentSelectedObj == null)
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
    public void ResetCurrentSelectedObj()
    {
        currentSelectedObj = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEntered = false;
        RadialProgress.GetComponent<Image>().fillAmount = 0f;
    }

	public GameObject getCurrentSelectedObj()
	{
		return currentSelectedObj;
	}

	public GameObject getPreviousSelectedObj()
	{
		return previousSelectedObj;
	}
}