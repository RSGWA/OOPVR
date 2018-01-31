using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class OptionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Transform optionMenu;
    private string selectedObject;

    private VariableBoxController variableBox;
    private ValueController valueControl;
	private Door door;
    private ReturnController returnControl;

    private GameObject MainCamera;

    private Outline outline;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

	private Notepad notepad;

    bool isSelected;

    // Use this for initialization
    void Start()
    {
        selectedObject = transform.tag;

       if(selectedObject == "Door")
        {
            optionMenu = transform.parent.Find("OptionMenu");
        }
        else
        {
            optionMenu = transform.Find("OptionMenu");
        }
        
        outline = transform.GetComponent<Outline>();

        canvasGroup = optionMenu.GetComponent<CanvasGroup>();

        variableBox = transform.GetComponent<VariableBoxController>();
        valueControl = transform.GetComponent<ValueController>();
        
        MainCamera = GameObject.Find("Main Camera");

		door = transform.parent.GetComponent<Door> ();
		notepad = GameObject.FindGameObjectWithTag ("Notepad").GetComponent<Notepad> ();

        isSelected = false;
        outline.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = new Vector3(MainCamera.transform.position.x, optionMenu.position.y, MainCamera.transform.position.z) - optionMenu.position;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        optionMenu.rotation = Quaternion.Slerp(optionMenu.rotation, targetRotation, Time.deltaTime * 2.0f);
    }

    public void ControlMenu()
    {
        //some way of disabling the optionMenu when another object is selected

        print(transform.name + "  this is the NAME");

        if (isSelected == false)
        {
            Select();
        }
        else
        {
            Deselect();
        }

    }

    public void Select()
    {
        EnableSelectedObject(false);
        ShowOptions();
        outline.eraseRenderer = false;
        outline.enabled = true;
        isSelected = true;


    }

    public void Deselect()
    {
        EnableSelectedObject(true);
        HideOptions();
        outline.eraseRenderer = true;
        outline.enabled = false;
        isSelected = false;
    }

    void ShowOptions()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    void HideOptions()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    void EnableSelectedObject(bool key)
    {
        switch (selectedObject)
        {
            case "Value":
                valueControl.enableVars(key);
                break;
			case "VariableBox":
				variableBox.enableVariableBox (key);
				notepad.highlightText (variableBox.code, "lime");
                break;
			case "Door":
				if (!door.isDoorOpen ()) {
					transform.GetComponent<Collider> ().enabled = key;
				}
                break;
            case "Return":
                transform.GetComponent<Collider>().enabled = key;
                break;
            default:
                break;
        }
    }

	public bool selected() {
		return isSelected;
	}

}
