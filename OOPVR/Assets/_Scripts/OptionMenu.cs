using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class OptionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Transform optionMenu;
    private InteractiveItemGaze selectedItem;
    private static InteractiveItemGaze itemHolder;
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

        if (selectedObject == "Door")
        {
            optionMenu = transform.parent.Find("OptionMenu");
            door = transform.parent.GetComponent<Door>();
        }
        else
        {
            optionMenu = transform.Find("OptionMenu");
        }

        canvasGroup = optionMenu.GetComponent<CanvasGroup>();

        selectedItem = transform.GetComponent<InteractiveItemGaze>();
        outline = transform.GetComponent<Outline>();
        outline.enabled = false;

        variableBox = transform.GetComponent<VariableBoxController>();
        valueControl = transform.GetComponent<ValueController>();


        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();

        MainCamera = GameObject.Find("Main Camera");

        isSelected = false;

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
        itemHolder = selectedItem;

        selectedItem.enabled = false;
        EnableSelectedObject(false);
        ShowOptions();
        outline.eraseRenderer = false;
        outline.enabled = true;
        isSelected = true;


    }

    public void Deselect()
    {
        itemHolder.enabled = true;
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
                valueControl.GetComponent<Collider>().enabled = key;
                break;
            case "VariableBox":
                variableBox.enableVariableBox(key);
                //notepad.highlightText (variableBox.code, "lime");
                break;
            case "Door":
                if (!door.isDoorOpen())
                {
                    transform.GetComponent<Collider>().enabled = key;
                }
                break;
            case "Return":
                transform.GetComponent<Collider>().enabled = key;
                break;
            default:
                break;
        }
    }

    public bool selected()
    {
        return isSelected;
    }

}
