﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableBoxController : MonoBehaviour
{

    private GameObject Hand;
    private GameObject[] variableBoxes;
    private GameObject objInHand;
    private GameObject parameter;

    private GameObject MessageCanvas;
    private GameObject variableBoxValue;
    private GameObject newVarBoxValue;

    private Transform ghostObject;
    private static Transform originalObject;

    private AnimationCurve xCurve;
    private AnimationCurve yCurve;
    private AnimationCurve zCurve;
    private Keyframe[] ks;

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

    // Use this for initialization
    void Start()
    {
        Hand = GameObject.FindGameObjectWithTag("Hand");
        variableBoxes = GameObject.FindGameObjectsWithTag("VariableBox");
        MessageCanvas = GameObject.Find("MessageCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (movingVarToBox)
        {
            objInHand.transform.localPosition = new Vector3(
                xCurve.Evaluate(Time.time - currentTime),
                yCurve.Evaluate(Time.time - currentTime),
                zCurve.Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                movingVarToBox = false;
                objInHand.GetComponent<ValueController>().setInHand(false);

                // Rotates object to stand up in box
                objInHand.transform.rotation = Quaternion.identity;

                objInHand.GetComponent<BoxCollider>().enabled = true;
                objInHand.AddComponent<Rigidbody>();

				boxAssigned = true;
				variableBoxValue = objInHand;
            }
        }
        else if (movingBoxToHand)
        {
            ghostObject.localPosition = new Vector3(
                xCurve.Evaluate(Time.time - currentTime),
                yCurve.Evaluate(Time.time - currentTime),
                zCurve.Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                movingBoxToHand = false;
            }
        }
        else if (movingBoxToBox)
        {
            if (objInHand != null)
            {

                objInHand.transform.rotation = transform.rotation;
                objInHand.transform.localPosition = new Vector3(
                    xCurve.Evaluate(Time.time - currentTime),
                    yCurve.Evaluate(Time.time - currentTime),
                    zCurve.Evaluate(Time.time - currentTime));

                if (Time.time - currentTime > ANIM_LENGTH)
                {
                    movingBoxToBox = false;
					tipBox = true;
                }
            }

        }
        if (tipBox)
        {
            objInHand.transform.Rotate(Vector3.right * 100 * (Time.time - currentTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                newVarBoxValue.SetActive(true);
                tipBox = false;
            }

        }
        if (destroyValue)
        {
			variableBoxValue.transform.position = new Vector3 (
				variableBoxValue.transform.position.x, 
				yCurve.Evaluate (Time.time - currentTime), 
				variableBoxValue.transform.position.z);
			
			if (Time.time - currentTime > ANIM_LENGTH) {
				variableBoxValue.transform.parent = null;
				Destroy (variableBoxValue);
				destroyValue = false;
				varRemoved = true;
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
		objInHand = Hand.GetComponent<HandController> ().getObjInHand ();

		string parent = "";
		if (transform.parent != null)
		{
			parent = transform.parent.tag;
			print(parent + "  this is the PARENT");

			//VariableBox is OnParameter
			if (parent == "Parameter")
			{
				onParameter = true;
			}

			//Check for other variableboxes in different methods

		}

		if (objInHand == null) {
			//Check whether the variabeBox contains a value, only then it could be picked up
			if (boxAssigned) //&& !onParameter)
			{
				originalObject = transform;

				//create Ghost VariablBox
				ghostObject = Instantiate(transform, transform.position, transform.rotation, transform.parent);
				Renderer rend = ghostObject.GetComponent<Renderer>();
				rend.material = Resources.Load("HologramMaterial") as Material;

				//set parent of Ghost VariableBox to be Hand
				ghostObject.parent = Hand.transform;
				Hand.GetComponent<HandController>().setObjInHand(ghostObject.gameObject);

				//Animate
				setUpBoxToHandAnimation();
				movingBoxToHand = true;
			}
			else
			{
				//A visual effect to denote variableBox does not contain a value
				if (onParameter)
				{
					print("CANNOT PICK UP: VariableBox is a parameter.");
					MessageCanvas.GetComponent<Status>().SetMessage("CANNOT PICK UP: This is a Parameter");
				}
				else
				{
					print("CANNOT PICK UP: VariableBox has not been assigned a Value");
					MessageCanvas.GetComponent<Status>().SetMessage("CANNOT PICK UP: Uninitialised variable.");
				}

			}
		} else {
			if (boxAssigned) { // A variable is already assigned
				Debug.Log ("Variable already assigned a value");
				setUpRemovingVarAnimation ();
				destroyValue = true;
				StartCoroutine ("removeVariableAndAct");
			} else {
				action ();
			}
		}
	}

	void action() {
		string varBoxType = transform.GetChild (0).tag;

		if (objInHand.tag == "Value") 
		{
			string variableType = objInHand.transform.GetChild (0).tag;

			if (varBoxType == variableType) 
			{
				currentTime = Time.time;
				objInHand.transform.parent = this.transform;
			
				setUpVarToBoxAnimation ();
				movingVarToBox = true;
				variableBoxValue = objInHand;
				Hand.GetComponent<HandController> ().setObjInHand (null); // Object no longer in hand

				paramReady = true;
			}
			else 
			{
				//A visual effect to denote that the value's type inHand does not match variableBox Type
				print ("Value and VariableBox TYPES Mismatch");
				MessageCanvas.GetComponent<Status> ().SetMessage ("TYPE MISMATCH: Cannot assign a Value of type " + variableType + " to variable of type " + varBoxType);
			}
		} 
		else if (objInHand.tag == "VariableBox") 
		{
			string variableBoxTypeInHand = objInHand.transform.GetChild(0).tag;

			if (varBoxType == variableBoxTypeInHand)
			{
				//if  variable has a value assigned, update the value
				if (boxAssigned)
				{
					Destroy(variableBoxValue);
				}

				// Set variable value to value of that in the box in hand
				//variableBoxValue = objInHand.transform.GetChild(2).gameObject;

				//tip value of variable in hand to variable gazing at
				objInHand.transform.parent = transform;
				setUpBoxToBoxAnimation();
				movingBoxToBox = true;

				Hand.GetComponent<HandController>().setObjInHand(null);

				//Assign new value to new variable
				Destroy(objInHand, ANIM_LENGTH + 1f);
				newVarBoxValue = Instantiate(variableBoxValue, transform.position, Quaternion.identity, transform);
				newVarBoxValue.SetActive(false);
				Destroy(variableBoxValue);

			}
			else
			{
				//A visual effect to denote variableBox types mismatch
				print("VariableBoxes mismatch");
				MessageCanvas.GetComponent<Status>().SetMessage("TYPE MISMATCH: Variable Types mismatch");

			}
		}
	}

	IEnumerator removeVariableAndAct() {
		while (!varRemoved) {
			yield return new WaitForSeconds (0.1f);
		}
		action ();
		varRemoved = false;
	}

    void setUpVarToBoxAnimation()
    {
        ks = new Keyframe[2];

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.x);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);
        xCurve = new AnimationCurve(ks);
        xCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.y);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);
        yCurve = new AnimationCurve(ks);
        yCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.z);
        ks[1] = new Keyframe(ANIM_LENGTH, 3.5f);
        zCurve = new AnimationCurve(ks);
        zCurve.postWrapMode = WrapMode.Once;
    }

    void setUpBoxToBoxAnimation()
    {
        ks = new Keyframe[2];

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.x);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);
        xCurve = new AnimationCurve(ks);
        xCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.y);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);
        yCurve = new AnimationCurve(ks);
        yCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, objInHand.transform.localPosition.z);
        ks[1] = new Keyframe(ANIM_LENGTH, 4f);
        zCurve = new AnimationCurve(ks);
        zCurve.postWrapMode = WrapMode.Once;
    }

    void setUpBoxToHandAnimation()
    {
        ks = new Keyframe[2];

        ks[0] = new Keyframe(0, ghostObject.transform.localPosition.x);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        xCurve = new AnimationCurve(ks);
        xCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, ghostObject.transform.localPosition.y);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        yCurve = new AnimationCurve(ks);
        yCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, ghostObject.transform.localPosition.z);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        zCurve = new AnimationCurve(ks);
        zCurve.postWrapMode = WrapMode.Once;

    }

    void setUpVariableToHandAnimation()
    {
        ks = new Keyframe[2];

        ks[0] = new Keyframe(0, transform.localPosition.x);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        xCurve = new AnimationCurve(ks);
        xCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, transform.localPosition.y);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        yCurve = new AnimationCurve(ks);
        yCurve.postWrapMode = WrapMode.Once;

        ks[0] = new Keyframe(0, transform.localPosition.z);
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        zCurve = new AnimationCurve(ks);
        zCurve.postWrapMode = WrapMode.Once;

    }

	void setUpRemovingVarAnimation() 
	{
		ks = new Keyframe[2];

		ks [0] = new Keyframe (0, variableBoxValue.transform.position.y);
		ks [1] = new Keyframe (ANIM_LENGTH, variableBoxValue.transform.position.y + 1f);

		yCurve = new AnimationCurve (ks);
		yCurve.postWrapMode = WrapMode.Once;

	}

    public void enableBoxes(bool enable)
    {
        foreach (GameObject box in variableBoxes)
        {
            box.GetComponent<BoxCollider>().enabled = enable;
        }
    }

	public bool isVarInBox() {
		return boxAssigned;
	}

}
