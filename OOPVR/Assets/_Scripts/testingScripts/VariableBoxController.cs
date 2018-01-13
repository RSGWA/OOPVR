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
    bool returnBox = false;
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
            objInHand.transform.rotation = transform.rotation;
            objInHand.transform.localPosition = new Vector3(
                xCurve.Evaluate(Time.time - currentTime),
                yCurve.Evaluate(Time.time - currentTime),
                zCurve.Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                Debug.Log("Animation Completed");
                movingBoxToBox = false;

                setUpBoxReturnAnimation();
                returnBox = true;

                // Rotates box upright
                //objInHand.transform.rotation = transform.rotation;
                // Tip box
                //tipBox = true;
                /*
                Vector3 rot = objInHand.transform.rotation.eulerAngles;
                rot = new Vector3(rot.x + 180, rot.y, rot.z);
                objInHand.transform.rotation = Quaternion.Euler(rot);
                */
            }
        }
        if (returnBox)
        {
            objInHand.transform.localPosition = new Vector3(
                xCurve.Evaluate(Time.time - currentTime),
                yCurve.Evaluate(Time.time - currentTime),
                zCurve.Evaluate(Time.time - currentTime));

            if (Time.time - currentTime > ANIM_LENGTH)
            {
                returnBox = false;
            }

        }
        if (tipBox)
        {
            Vector3 to = new Vector3(20, 20, 20);
            if (Vector3.Distance(objInHand.transform.eulerAngles, to) > 0.01f)
            {
                objInHand.transform.eulerAngles = Vector3.Lerp(objInHand.transform.rotation.eulerAngles, to, Time.deltaTime);
                
            }
            else
            {
                objInHand.transform.eulerAngles = to;
                tipBox = false;
            }
        }

        if(onParameter && paramReady)
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
            if(transform.childCount == 3) //&& !onParameter)
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
                    MessageCanvas.GetComponent<Status>().SetMessage("CANNOT PICK UP:  VariableBox is a parameter.");
                }
                else
                {
                    print("CANNOT PICK UP: VariableBox has not been assigned a Value");
                    MessageCanvas.GetComponent<Status>().SetMessage("CANNOT PICK UP: VariableBox has not been assigned a Value");
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
                    objInHand.transform.parent = transform;
                    Hand.GetComponent<HandController>().setObjInHand(null);

                    //Placing value into VariableBox
                    setUpVarToBoxAnimation();
                    movingVarToBox = true;
                    //rotateVarAnimation();

                    paramReady = true;

                }
                else
                {
                    //A visual effect to denote that the value's type inHand does not match variableBox Type
                    print("Value and VariableBox TYPES Mismatch");
                    MessageCanvas.GetComponent<Status>().SetMessage("Value and VariableBox TYPES Mismatch");
                    
                }

                //Player holding a VariableBox
            }else if (objInHand.tag == "VariableBox")
            {
                string handVarBoxType = objInHand.transform.GetChild(0).tag;
                
                if(varBoxType == handVarBoxType)
                {
                    //check whether the variableBox we gazing at contain a value
                    if (transform.childCount == 3)
                    {
                        GameObject oldValue = transform.GetChild(2).gameObject;
                        Destroy(oldValue);
                    }

                    GameObject hand = objInHand.transform.parent.gameObject;
                    GameObject variableBoxValue = objInHand.transform.GetChild(2).gameObject;

                   
                    


                    //Below block is a work in progress
                    //**************************************************
                    objInHand.transform.parent = transform;

                    setUpBoxToBoxAnimation();
                    movingBoxToBox = true;

                    objInHand.transform.parent = hand.transform;
                    //setUpBoxReturnAnimation();
                    //returnBox = true;

                    Instantiate(variableBoxValue, transform.position, Quaternion.identity, transform);
                    Hand.GetComponent<HandController>().setObjInHand(null);
                    Destroy(originalObject.GetChild(2).gameObject,1f); //Destroy the original value where ghost is from
                    Destroy(objInHand);
                    
                    //
                    //***************************************************

                }
                else
                {
                    //A visual effect to denote variableBox types mismatch
                    print("VariableBoxes mismatch");
                    MessageCanvas.GetComponent<Status>().SetMessage("VariableBoxes Mismatch");
                   
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

    void setUpBoxReturnAnimation()
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
        ks[1] = new Keyframe(ANIM_LENGTH, 0);

        zCurve = new AnimationCurve(ks);
        zCurve.postWrapMode = WrapMode.Once;

    }

    void rotateVarAnimation()
    {
        Keyframe[] keys = new Keyframe[2];
        /*
		Vector3 rot = objInHand.transform.rotation.eulerAngles;
		rot = new Vector3 (rot.x + 180, rot.y, rot.z);
		objInHand.transform.rotation = Quaternion.Euler (rot);
		*/
        keys[0] = new Keyframe(0, objInHand.transform.rotation.x);
        keys[1] = new Keyframe(ANIM_LENGTH, 0);

        xRotCurve = new AnimationCurve(keys);
        xRotCurve.postWrapMode = WrapMode.Once;

        keys[0] = new Keyframe(0, objInHand.transform.rotation.y);
        keys[1] = new Keyframe(ANIM_LENGTH, 0);

        yRotCurve = new AnimationCurve(keys);
        yRotCurve.postWrapMode = WrapMode.Once;

        keys[0] = new Keyframe(0, objInHand.transform.rotation.z);
        keys[1] = new Keyframe(ANIM_LENGTH, 0);

        zRotCurve = new AnimationCurve(keys);
        zRotCurve.postWrapMode = WrapMode.Once;
    }

    public void enableBoxes(bool enable)
    {
        foreach (GameObject box in variableBoxes)
        {
            box.GetComponent<BoxCollider>().enabled = enable;
        }
    }

}
