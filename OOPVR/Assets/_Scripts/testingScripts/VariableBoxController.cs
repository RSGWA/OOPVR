using System.Collections;
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

    private AnimationCurve xRotCurve;
    private AnimationCurve yRotCurve;
    private AnimationCurve zRotCurve;

    private static float ANIM_LENGTH = 1.5f;
    private static float ANIM_DELAY = 0.5f;

    float currentTime = 0;

    bool movingVarToBox = false;
    bool movingBoxToHand = false;
    bool movingBoxToBox = false;
    bool tipBox = false;
    bool destroyValue = false;
    bool onParameter = false;
    bool paramReady = false;

    // Use this for initialization
    void Start()
    {
        Hand = GameObject.FindGameObjectWithTag("Hand");
        variableBoxes = GameObject.FindGameObjectsWithTag("VariableBox");
        MessageCanvas = GameObject.Find("MessageCanvas");

        //enableBoxes (false);
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

                tipBox = true;


                if (Time.time - currentTime > ANIM_LENGTH)
                {
                    Debug.Log("Animation Completed");
                    movingBoxToBox = false;

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
            transform.GetChild(2).transform.Translate(Vector3.up * 0.5f * (Time.deltaTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                Destroy(transform.GetChild(2).gameObject);
                destroyValue = false;
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
            print(parent + "  this is the PARENT");

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
            if (transform.childCount == 3) //&& !onParameter)
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

        }
        else
        {
            string varBoxType = transform.GetChild(0).tag;

            //player holding a Value
            if (objInHand.tag == "Value")
            {
                string valueType = objInHand.transform.GetChild(0).tag;
                // print("Value type  " + valueType + "  VarboxType == " + varBoxType);
                if (valueType == varBoxType)
                {
                    // A variableBox already contains a value
                    if (transform.childCount == 3)
                    {
                        Transform value = transform.GetChild(2);
                        if (value.GetComponent<Rigidbody>() != null)
                        {
                            Destroy(value.GetComponent<Rigidbody>());
                        }
                        destroyValue = true;

                    }

                    objInHand.transform.parent = transform;
                    Hand.GetComponent<HandController>().setObjInHand(null);

                    //Placing value into VariableBox
                    setUpVarToBoxAnimation();
                    movingVarToBox = true;

                    paramReady = true;

                }
                else
                {
                    //A visual effect to denote that the value's type inHand does not match variableBox Type
                    print("Value and VariableBox TYPES Mismatch");
                    MessageCanvas.GetComponent<Status>().SetMessage("TYPE MISMATCH: Cannot assign a Value of type "+ valueType + " to variable of type " + varBoxType);

                }

                //Player holding a VariableBox
            }
            else if (objInHand.tag == "VariableBox")
            {
                string handVarBoxType = objInHand.transform.GetChild(0).tag;

                if (varBoxType == handVarBoxType)
                {
                    //if  variable has a value assigned, update the value
                    if (transform.childCount == 3)
                    {
                        GameObject oldValue = transform.GetChild(2).gameObject;
                        Destroy(oldValue);
                    }

                    variableBoxValue = objInHand.transform.GetChild(2).gameObject;

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

    public void enableBoxes(bool enable)
    {
        foreach (GameObject box in variableBoxes)
        {
            box.GetComponent<BoxCollider>().enabled = enable;
        }
    }

}
