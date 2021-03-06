﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

/*
 * This is main script used to control actions for the OptionMenus of the different interactive components. It shows the menu accordingly
 * to what object is being selected and shows the options available to select from.
 * 
 * */
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
    private DoorMenuController doorOptions;

    private GameObject MainCamera;
    private PlayerController player;

    private Outline outline;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

    private Notepad notepad;

    bool isSelected;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        doorOptions = GameObject.Find("ActivityController").GetComponent<DoorMenuController>();

        selectedObject = transform.tag;
        if (selectedObject == "Door")
        {
            //used to filter the unused internal doors
            if (transform.parent.name != "DoorInt")
            {
                optionMenu = transform.parent.Find("OptionMenu");
                door = transform.parent.GetComponent<Door>();
                canvasGroup = optionMenu.GetComponent<CanvasGroup>();
            }

        }
        else
        {
            optionMenu = transform.Find("OptionMenu");
            canvasGroup = optionMenu.GetComponent<CanvasGroup>();
        }



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
        if (selectedObject == "Door" && transform.parent.name != "DoorInt")
        {
            if (player.isInRoom())
            {
                optionMenu.localPosition = new Vector3(0, 0, 0.1f);
                doorOptions.EnableDoorIndoorOptions(optionMenu, true);
            }
            else
            {
                optionMenu.localPosition = new Vector3(0, 0, -0.1f);
                doorOptions.EnableDoorIndoorOptions(optionMenu, false);
            }
        }
        targetPoint = new Vector3(MainCamera.transform.position.x, optionMenu.position.y, MainCamera.transform.position.z) - optionMenu.position;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        optionMenu.rotation = Quaternion.Slerp(optionMenu.rotation, targetRotation, Time.deltaTime * 2.0f);
    }

    public void ControlMenu()
    {
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

        //selectedItem.enabled = false;
        //EnableSelectedObject(false);

        ShowOptions();
        outline.eraseRenderer = false;
        outline.enabled = true;
        isSelected = true;

        // Specific actions to do depending on the object selected
        switch (selectedObject)
        {
            case "VariableBox":
                // Highlight code in notepad representing the selected variable
                foreach (string code in variableBox.code)
                {
                    notepad.highlightText(code, "green");
                }
                variableBox.peekValue(true);
                break;
            case "Door":
                notepad.highlightText(door.code, "purple");
                break;
            default:
                break;
        }
    }

    public void Deselect()
    {
        selectedItem.enabled = true;
        EnableSelectedObject(true);

        HideOptions();
        outline.eraseRenderer = true;
        outline.enabled = false;
        isSelected = false;

        // Deselecting actions
        switch (selectedObject)
        {
            case "VariableBox":
                foreach (string text in variableBox.code)
                {
                    notepad.unhighlightText(text);
                }
                variableBox.peekValue(false);
                break;
            case "Door":
                notepad.unhighlightText(door.code);
                break;
            default:
                break;
        }
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
                break;
            case "Door":
                if (!door.isDoorOpen())
                {
                    transform.GetComponent<Collider>().enabled = key;
                    door.GetComponent<Collider>().enabled = !key;
                }
                break;
            case "Return":
                transform.GetComponent<Collider>().enabled = key;
                break;
            case "Blueprint":
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



