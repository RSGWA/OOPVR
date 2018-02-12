using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class VariableBoxController : MonoBehaviour
{

	public List<string> code;

    private GameObject Hand;
    private GameObject[] variableBoxes;
    private GameObject objInHand;
    private GameObject parameter;

    private GameObject variableBoxValue;
    private GameObject newVarBoxValue;

    private GameObject plusOne;

    private Status MessageCanvas;
    private InfoController info;
	private OptionMenu options;

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

    bool infoSelected = false;
    bool incrementSelected = false;
    bool incremented = false;

    // Use this for initialization
    void Start()
    {
        Hand = GameObject.FindGameObjectWithTag("Hand");
        variableBoxes = GameObject.FindGameObjectsWithTag("VariableBox");
        MessageCanvas = GameObject.Find("MessageCanvas").GetComponent<Status>();
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
        options = transform.GetComponent<OptionMenu>();
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
                objInHand.GetComponent<ValueController>().setInHand(false);

                // Rotates object to stand up in box
                objInHand.transform.rotation = Quaternion.identity;

                objInHand.GetComponent<BoxCollider>().enabled = true;
                objInHand.AddComponent<Rigidbody>();

                boxAssigned = true;
                variableBoxValue = objInHand;

				//options.Select ();
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
        if (tipBox)
        {
            if (variableBoxValue.GetComponent<Rigidbody>() == null)
            {
                variableBoxValue.AddComponent<Rigidbody>();
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
        if (incrementSelected)
        {

            plusOne.transform.Translate(Vector3.up * 0.85f * Time.deltaTime);

            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                
                Destroy(plusOne);
                incrementSelected = false;
               
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

    public void boxAction()
    {
        currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        string parent = "";
        if (transform.parent != null)
        {
            parent = transform.parent.tag;

            //VariableBox is OnParameter
            if (parent == "Parameter")
            {
                onParameter = true;
            }

            //Check for other variableboxes in different methods

        }

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

        //Check whether it is on a Parameter
        string parent = "";
        if (transform.parent != null)
        {
            parent = transform.parent.tag;

            //VariableBox is OnParameter
            if (parent == "Parameter")
            {
                onParameter = true;
            }

            //Check for other variableboxes in different methods

        }


        if (objInHand == null)
        {
            //Check whether the variabeBox contains a value, only then it could be picked up
            if (boxAssigned) //&& !onParameter)
            {
                options.Deselect(); //Deselecting the optionsMenu

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

        //Check whether it is on a Parameter
        string parent = "";
        if (transform.parent != null)
        {
            parent = transform.parent.tag;

            //VariableBox is OnParameter
            if (parent == "Parameter")
            {
                onParameter = true;
            }

            //Check for other variableboxes in different methods

        }

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
            string variableType = objInHand.transform.GetChild(0).tag;

            if (varBoxType == variableType)
            {
                options.Deselect();

                objInHand.transform.parent = this.transform;

                curves = AnimationUtility.moveToParent(objInHand.transform, 0, 0, 3.5f);

                // Remove ghost of variable
                objInHand.GetComponent<ValueController>().removeGhost();
                
                movingVarToBox = true;
                variableBoxValue = objInHand;
                Hand.GetComponent<HandController>().setObjInHand(null); // Object no longer in hand

                paramReady = true;
            }
            else
            {
                //A visual effect to denote that the value's type inHand does not match variableBox Type
                print("Value and VariableBox TYPES Mismatch");
                MessageCanvas.SetMessage("TYPE MISMATCH: Cannot assign a Value of type " + variableType + " to variable of type " + varBoxType);
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

    public void InfoButton()
    {
        string varBoxType = transform.GetChild(0).tag;
        string varName = transform.name;
        info.SetInformation("This is a variable.\n " + varName + " = " + varBoxType + "!!\nThis " +
            "is just an exampe of showing how info works.\n PLEASE SELECT INFO AGAIN TO DESELECT!");

        if (!infoSelected)
        {
            info.ShowInformation();
            infoSelected = true;
        }
        else
        {
            info.HideInformation();
            infoSelected = false;
        }



    }

    public void IncrementFunction()
    {
        options.Deselect();
        currentTime = Time.time;
        //have an animation of +1 that comes out of the box to signify an Increment
        plusOne = Instantiate((Resources.Load("1increment", typeof(GameObject))as GameObject) , transform.position, Hand.transform.rotation, transform.parent.parent);

        incrementSelected = true;
        incremented = true;

        //later- get the assigned value to this int variable and increment it
        GameObject intValue = transform.GetChild(3).gameObject;

    }

    IEnumerator removeVariableAndAct()
    {
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
    }

	// For testing/debugging purposes
	public void setBoxAssigned(bool b) {
		boxAssigned = b;
	}

    public bool isIncremented()
    {
        return incremented;
    }
}