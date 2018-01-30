using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class OptionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private VariableBoxController variableBox;
    private Transform optionMenu;
    private GameObject MainCamera;

    private Outline outline;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

    bool isSelected;

    // Use this for initialization
    void Start()
    {
        optionMenu = transform.Find("OptionMenu");
        canvasGroup = optionMenu.GetComponent<CanvasGroup>();
        variableBox = transform.GetComponent<VariableBoxController>();
        outline = transform.GetComponent<Outline>();
        MainCamera = GameObject.Find("Main Camera");

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



        if (isSelected == false)
        {
            Select();
            //variableBox.enableVariableBox(false);

        }
        else
        {
            Deselect();
        }



    }

    public void Select()
    {
        ShowOptions();
        outline.eraseRenderer = false;
        outline.enabled = true;
        isSelected = true;


    }

    public void Deselect()
    {
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

}
