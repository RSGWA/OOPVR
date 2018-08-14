using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class VariableBoxController : MonoBehaviour
{

	public List<string> code;

    private GameObject Hand;
    public GameObject MainCamera;
    private GameObject[] variableBoxes;
    private GameObject objInHand;
    private GameObject parameter;

    private GameObject variableBoxValue;
    private GameObject newVarBoxValue;

    private GameObject plusOne;
    private GameObject valueToIncrement;

    private Status MessageCanvas;
    private InfoController info;
	private OptionMenu options;

    private string variableType;
    private AddressBoxController addressBox;


    // Only one Ghost Object can exist at a time so all boxes must have a
    // reference to it and the original
    private static Transform ghostObject;
    private static Transform originalObject;

    private AnimationCurve[] curves;

    private static float ANIM_LENGTH = 1.5f;

    float currentTime = 0;

    bool movingVarToBox = false;
    bool movingBoxToHand = false;
    bool movingBoxToBox = false;
    bool tipBox = false;
    bool destroyValue = false;
    bool onParameter = false;
    bool paramReady = false;

    bool boxAssigned = false;
    bool varRemoved = false;
    bool containerSelected = false;

    bool infoSelected = false;
    bool preIncrementSelection = false;
    bool incrementSelected = false;
    bool postIncrementSelection = false;
    bool incremented = false;
    bool isParameter = false;
    bool animatePeekUp = false;
    bool animatePeekDown = false;





    // Use this for initialization
    void Start()
    {
        Hand = GameObject.FindGameObjectWithTag("Hand");
        variableBoxes = GameObject.FindGameObjectsWithTag("VariableBox");
        MessageCanvas = GameObject.Find("MessageCanvas").GetComponent<Status>();
        info = GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoController>();
        options = transform.GetComponent<OptionMenu>();
        MainCamera = GameObject.Find("Main Camera");

        checkVariableBoxKind();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingVarToBox)
        {
            objInHand.transform.localPosition = new Vector3(
                curves[0].Evaluate(Time.time - currentTime),
                curves[1].Evaluate(Time.time - currentTime),
                curves[2].Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                movingVarToBox = false;
                objInHand.GetComponent<InteractiveItemGaze>().enabled = false;

                
                objInHand.GetComponent<ValueController>().setInHand(false);
               


                // Rotates object to stand up in box
                objInHand.transform.rotation = Quaternion.identity;

                objInHand.GetComponent<BoxCollider>().enabled = true;
                objInHand.AddComponent<Rigidbody>();

                //Hand.GetComponent<HandController>().setObjInHand(null);

                boxAssigned = true;
                variableBoxValue = objInHand;
                
                variableBoxValue.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                if (variableType == "Address")
                {
                    addressBox.GetComponent<AddressBoxController>().setInHand(false);
                    addressBox.GetComponent<AddressBoxController>().getCurvedLine().HideCurve();
                    //options.Select ();
                    
                }

            }
        }
        else if (movingBoxToHand)
        {
            ghostObject.localPosition = new Vector3(
                curves[0].Evaluate(Time.time - currentTime),
                curves[1].Evaluate(Time.time - currentTime),
                curves[2].Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                movingBoxToHand = false;
				//options.Select ();
            }
        }
        else if (movingBoxToBox)
        {
            objInHand.transform.rotation = transform.rotation;
            objInHand.transform.localPosition = new Vector3(
                curves[0].Evaluate(Time.time - currentTime),
                curves[1].Evaluate(Time.time - currentTime),
                curves[2].Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                movingBoxToBox = false;
                tipBox = true;

				//options.Select ();
            }
        }
        if (boxAssigned)
        {
            Renderer rend = transform.GetComponent<Renderer>();
            rend.material = Resources.Load("VarAssigned") as Material;

        }
        if (tipBox)
        {
            if (variableBoxValue.GetComponent<Rigidbody>() == null)
            {
                variableBoxValue.AddComponent<Rigidbody>();
               
                variableBoxValue.transform.rotation = Quaternion.identity;
                variableBoxValue.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }

            objInHand.transform.Rotate(Vector3.down * 180);
            variableBoxValue.transform.parent = this.transform;
            Destroy(objInHand, 1.5f);
            tipBox = false;
        }
        if (destroyValue)
        {
            variableBoxValue.transform.Translate(Vector3.up * 0.85f * Time.deltaTime);

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                variableBoxValue.transform.parent = null;
                Destroy(variableBoxValue);
                destroyValue = false;
                varRemoved = true;
            }

        }
        if (preIncrementSelection)
        {
            

            Vector3 targetPoint = new Vector3(MainCamera.transform.position.x, valueToIncrement.transform.position.y, MainCamera.transform.position.z) - valueToIncrement.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            valueToIncrement.transform.rotation = Quaternion.Slerp(valueToIncrement.transform.rotation, targetRotation, Time.deltaTime * 2.0f);

            valueToIncrement.transform.Translate(Vector3.up * 0.7f * Time.deltaTime);

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                Vector3 newPosition = new Vector3(valueToIncrement.transform.position.x, valueToIncrement.transform.position.y + 0.6f * AnimationUtility.ANIM_LENGTH, valueToIncrement.transform.position.z);
                valueToIncrement.transform.position = newPosition;
                preIncrementSelection = false;

            }
        }
        if (incrementSelected)
        {

            plusOne.transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
            

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                
                Destroy(plusOne);
                incrementSelected = false;
            }

        }

        if (postIncrementSelection)
        {
            Vector3 targetPoint = new Vector3(MainCamera.transform.position.x, valueToIncrement.transform.position.y, MainCamera.transform.position.z) - valueToIncrement.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            valueToIncrement.transform.rotation = Quaternion.Slerp(valueToIncrement.transform.rotation, targetRotation, Time.deltaTime * 2.0f);

            valueToIncrement.transform.Translate(Vector3.down * 0.7f * Time.deltaTime);

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                postIncrementSelection = false;

            }
        }

        if (animatePeekUp)
        {
            if(variableBoxValue.GetComponent<Rigidbody>() != null)
            {
                variableBoxValue.transform.GetComponent<Rigidbody>().useGravity = false;
            }
            
            //Rotate the Value to face the player
            Vector3 targetPoint = new Vector3(MainCamera.transform.position.x, variableBoxValue.transform.position.y, MainCamera.transform.position.z) - variableBoxValue.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            variableBoxValue.transform.rotation = Quaternion.Slerp(variableBoxValue.transform.rotation, targetRotation, Time.deltaTime * 2.0f);


            variableBoxValue.transform.Translate(Vector3.up * 0.1f * Time.deltaTime);
            

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                animatePeekUp = false;

            }
        }
        if (animatePeekDown)
        {
            if (variableBoxValue.GetComponent<Rigidbody>() != null)
            {
                variableBoxValue.transform.GetComponent<Rigidbody>().useGravity = false;
            }
                       
            variableBoxValue.transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
            

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                variableBoxValue.transform.rotation = Quaternion.identity;
                variableBoxValue.transform.localPosition = new Vector3(0, 0, 0);
                animatePeekDown = false;

            }
        }


        if (onParameter && paramReady)
        {
            //Control Platform
            transform.parent.parent.GetComponent<ParameterController>().addVariableBox(transform);
            onParameter = false;
            paramReady = false;
        }
    }

    void checkVariableBoxKind()
    {
        string parent = "";
        if (transform.parent != null)
        {
            parent = transform.parent.tag;

            //VariableBox is OnParameter
            if (parent == "Parameter")
            {
                onParameter = true;
                isParameter = true;
            }

            //Check for other variableboxes in different areas of an Instance or Main

        }
    }

    public void boxAction()
    {
        currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        if (objInHand == null)
        {
            //Check whether the variabeBox contains a value, only then it could be picked up
            if (boxAssigned) //&& !onParameter)
            {
                originalObject = transform;

                //create Ghost VariableBox
                ghostObject = Instantiate(transform, transform.position, transform.rotation, transform.parent);
                Renderer rend = ghostObject.GetComponent<Renderer>();
                rend.material = Resources.Load("HologramMaterial") as Material;

                //set parent of Ghost VariableBox to be Hand
                ghostObject.parent = Hand.transform;
                Hand.GetComponent<HandController>().setObjInHand(ghostObject.gameObject);

                VariableBoxController ghostObj = ghostObject.GetComponent<VariableBoxController>();
                // Set variable box value in the script so it can be accessed later
                ghostObj.setVariableBoxValue(ghostObject.GetChild(3).gameObject);

                if (ghostObj.getVariableBoxValue().GetComponent<Rigidbody>() != null)
                {
                    Destroy(ghostObj.getVariableBoxValue().GetComponent<Rigidbody>());
                }

                // Disable ghost object in hand
                ghostObj.enableVariableBox(false);

                // Set up animation - Ghost Object to Hand
                //setUpBoxToHandAnimation();
                curves = AnimationUtility.moveToParent(ghostObject.transform, 0, 0, 0);
                movingBoxToHand = true;
            }
            else
            {
                //A visual effect to denote variableBox does not contain a value
                if (onParameter)
                {
                    print("CANNOT PICK UP: VariableBox is a parameter.");
                    MessageCanvas.SetMessage("CANNOT PICK UP: This is a Parameter");
                }
                else
                {
                    print("CANNOT PICK UP: VariableBox has not been assigned a Value");
                    MessageCanvas.SetMessage("CANNOT PICK UP: Uninitialised variable.");
                }

            }
        }
        else
        {
            if (boxAssigned)
            {
                // Rotate variable in box upright for animation
                variableBoxValue.transform.rotation = Quaternion.identity;
               Destroy(variableBoxValue.GetComponent<Rigidbody>());
                destroyValue = true;
                StartCoroutine("removeVariableAndAct");
            }
            else
            {
                AssignAction();
            }
        }
    }

    public void CopyButton()
    {
        currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        if (objInHand == null)
        {
            //Check whether the variabeBox contains a value, only then it could be picked up
            if (boxAssigned)
            {
                options.Deselect(); //Deselecting the optionsMenu

                variableBoxValue.transform.localPosition = new Vector3(0, 0, 0);

                originalObject = transform;

                //create Ghost VariableBox
                ghostObject = Instantiate(transform, transform.position, transform.rotation, transform.parent);
                Renderer rend = ghostObject.GetComponent<Renderer>();
                rend.material = Resources.Load("HologramMaterial") as Material;


                if (isParameter)
                {
                    ghostObject.Find("Labels").GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    ghostObject.Find("Labels").GetChild(1).gameObject.SetActive(false);
                }

                //set parent of Ghost VariableBox to be Hand
                ghostObject.parent = Hand.transform;

                //Disable the instance variable menu controller for the ghostObject
                if(ghostObject.name == "Name_InstanceBox(Clone)" | ghostObject.name == "Age_InstanceBox(Clone)")
                {
                    ghostObject.GetComponent<InstanceVariablesMenuController>().enabled = false;
                }
               
                Hand.GetComponent<HandController>().setObjInHand(ghostObject.gameObject);

                VariableBoxController ghostObj = ghostObject.GetComponent<VariableBoxController>();
                // Set variable box value in the script so it can be accessed later
                ghostObj.setVariableBoxValue(ghostObject.GetChild(3).gameObject);

                if (ghostObj.getVariableBoxValue().GetComponent<Rigidbody>() != null)
                {
                    Destroy(ghostObj.getVariableBoxValue().GetComponent<Rigidbody>());
                }

                // Disable ghost object in hand
                ghostObj.enableVariableBox(false);

                // Set up animation - Ghost Object to Hand
                //setUpBoxToHandAnimation();
                curves = AnimationUtility.moveToParent(ghostObject.transform, 0, 0, 0);
                movingBoxToHand = true;
            }
            else
            {
                //A visual effect to denote variableBox does not contain a value
                if (onParameter)
                {
                    print("CANNOT PICK UP: VariableBox is a parameter.");
                    MessageCanvas.SetMessage("CANNOT PICK UP: This is a Parameter");
                }
                else
                {
                    print("CANNOT PICK UP: VariableBox has not been assigned a Value");
                    MessageCanvas.SetMessage("CANNOT PICK UP: Uninitialised variable.");
                }

            }
        }
        else
        {

            if (objInHand.tag == "Value")
            {
                MessageCanvas.SetMessage("Please place the Value in hand back before picking up the Variable");
            }
            else if (objInHand.tag == "VariableBox")
            {
                //!!!!!!!!!!!!This is up for discussion...whether we get rid of the variable in hand and make copy of the current variable!!!!!!!!!!!
                MessageCanvas.SetMessage("Can only have one Variable in hand at a time");
            }
        }
    }


    public void AssignButton()
    {
        objInHand = Hand.GetComponent<HandController>().getObjInHand();
        currentTime = Time.time;
        string varBoxType = transform.GetChild(0).tag;

        if (objInHand == null)
        {
            MessageCanvas.SetMessage("CANNOT ASSIGN: No Value has been picked up, please pick up a value of type " + varBoxType);
        }
        else
        {

            if (boxAssigned)
            {
                //Destroy the current variable value
                variableBoxValue.transform.rotation = Quaternion.identity;
                Destroy(variableBoxValue.GetComponent<Rigidbody>());
                destroyValue = true;
                StartCoroutine("removeVariableAndAct");
            }
            else
            {
                AssignAction();
            }

        }

    }

    void AssignAction()
    {
        string varBoxType = transform.GetChild(0).tag;
        currentTime = Time.time;

        if (objInHand.tag == "Value")
        {
            variableType = objInHand.transform.GetChild(0).tag;

            if (varBoxType == variableType)
            {
                options.Deselect();

                objInHand.transform.parent = this.transform;

                curves = AnimationUtility.moveToParent(objInHand.transform, 0, 0, 3.5f);

                //Only remove the ghost value for all other apart from Address type
                if(variableType == "Address")
                {
                    addressBox = GameObject.Find("Mailbox").GetComponent<AddressBoxController>();
                   // addressBox = GameObject.Find("AddressBox").GetComponent<AddressBoxController>();
                }
                else
                {
                    // Remove ghost of variable
                    objInHand.GetComponent<ValueController>().removeGhost();
                }
                
                
                movingVarToBox = true;
                variableBoxValue = objInHand;
                Hand.GetComponent<HandController>().setObjInHand(null); // Object no longer in hand

                paramReady = true;
            }
            else
            {
                //A visual effect to denote that the value's type inHand does not match variableBox Type
                print("Value and VariableBox TYPES Mismatch");
                MessageCanvas.SetMessage("TYPE MISMATCH: Cannot assign a value of type " + variableType + " to variable of type " + varBoxType);
            }
        }
        else if (objInHand.tag == "VariableBox")
        {
            string variableBoxTypeInHand = objInHand.transform.GetChild(0).tag;

            if (varBoxType == variableBoxTypeInHand)
            {
                options.Deselect();
                // Set up animation
                objInHand.transform.parent = transform;
                //setUpBoxToBoxAnimation();
                curves = AnimationUtility.moveToParent(objInHand.transform, 0, 0, 4f);

                movingBoxToBox = true;

                Hand.GetComponent<HandController>().setObjInHand(null);

                boxAssigned = true;

                // Set new variable box value
                variableBoxValue = ghostObject.GetComponent<VariableBoxController>().getVariableBoxValue();

                //ghostObject.GetComponent<VariableBoxController> ().getVariableBoxValue ().transform.parent = transform;


            }
            else
            {
                //A visual effect to denote variableBox types mismatch
                print("VariableBoxes mismatch");
                MessageCanvas.SetMessage("TYPE MISMATCH: Variable Types mismatch");
            }
        }
    }

    public void GOTOaddress(GameObject address)
    {
        options.Deselect();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.moveTo(address);
    }

    public void InfoButton()
    {
        string varBoxType = transform.GetChild(0).tag;
        string varName = transform.Find("Labels").GetChild(0).GetComponent<TextMesh>().text;

        if (isVarInBox())
        {
            string value = variableBoxValue.GetComponent<TextMesh>().text;

            if (isParameter)
            {
                info.SetInformation(varName + " = " + value + "\nThis is a Parameter Variable Container which take values of type " + varBoxType + ".\nCurrent value is " + value);
            }
            else
            {
                info.SetInformation(varName + " = " + value + "\nThis is a Variable Container of type " + varBoxType + ".\nCurrent value is " + value);
            }
        }
        else
        {
            if (isParameter)
            {
                info.SetInformation(varName + "\nThis is a Parameter Variable Container of type " + varBoxType + ".\nPlease assign a value of type " + varBoxType + " to this Parameter.");
            }
            else
            {
                info.SetInformation(varName + "\nThis is a Variable Container of type " + varBoxType + ".\nPlease assign a value of type " + varBoxType + " to this variable.");
            }
        }
    }

    public void IncrementFunction()
    {
        if (!incremented)
        {
            options.Deselect();
            
            StartCoroutine("AnimateIncrement");
            incremented = true;
        }
        else
        {
            MessageCanvas.SetMessage("CANNOT PERFORM ACTION: Increment++ can only be called once when inside the method!");
        }
    }

    IEnumerator AnimateIncrement()
    {
        currentTime = Time.time;
        variableBoxValue.transform.localPosition = new Vector3(0, 0, 0);
        valueToIncrement = transform.GetChild(3).gameObject;
        preIncrementSelection = true;

        while (preIncrementSelection)
        {
            yield return new WaitForSeconds(0.1f);
        }
        currentTime = Time.time;
        Vector3 pos = transform.position + Vector3.up * 0.8f;
        plusOne = Instantiate((Resources.Load("1increment", typeof(GameObject)) as GameObject), pos, Hand.transform.rotation, transform.parent.parent);

        string value = valueToIncrement.GetComponent<TextMesh>().text;
        int oldValue = int.Parse(value);
        int newValue = oldValue + 1;
        valueToIncrement.GetComponent<TextMesh>().text = newValue.ToString();
        incrementSelected = true;

        currentTime = Time.time;
        Vector3 newPos = transform.position + Vector3.up * 0.8f;
        valueToIncrement.transform.position = newPos;
       
        postIncrementSelection = true;

    }

    public void peekValue(bool key)
    {
        currentTime = Time.time;
        if (boxAssigned)
        {
            if (key)
            {
                animatePeekUp = true;
            }
            else
            {
                animatePeekDown = true;
            }
            
        }

    }

    IEnumerator removeVariableAndAct()
    {
        options.Deselect();
        while (!varRemoved)
        {
            yield return new WaitForSeconds(0.1f);
        }
        AssignAction();
        varRemoved = false;
    }

    public bool isVarInBox()
    {
        return boxAssigned;
    }

    public GameObject getVariableBoxValue()
    {
        return variableBoxValue;
    }

    public void setVariableBoxValue(GameObject obj)
    {
        variableBoxValue = obj;
    }

    public void enableVariableBox(bool enable)
    {
        BoxCollider[] colliders = GetComponents<BoxCollider>();

        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = enable;
        }
        //this is used to check whether the box is selected
        containerSelected = !enable;

    }

	// For testing/debugging purposes
	public void setBoxAssigned(bool b) {
		boxAssigned = b;
	}

    public bool isIncremented()
    {
        return incremented;
    }

    public void resetIncrement()
    {
        incremented = false;
    }

    public bool isSelected()
    {
        return transform.GetComponent<OptionMenu>().selected();
    }
}