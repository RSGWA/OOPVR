using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class OptionMenu : MonoBehaviour {

    private CanvasGroup canvasGroup;
    private VariableBoxController variableBox;

    private Outline outline;

    bool isSelected;

    // Use this for initialization
    void Start()
    {
        canvasGroup = transform.Find("OptionMenu").GetComponent<CanvasGroup>();
        variableBox = transform.GetComponent<VariableBoxController>();
        outline = this.transform.GetComponent<Outline>();

        isSelected = false;
        outline.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {


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
