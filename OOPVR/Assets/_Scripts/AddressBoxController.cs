using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressBoxController : MonoBehaviour {

    private GameObject Hand;
    private GameObject[] vars;
    private GameObject objInHand;

    private Transform addressValue;
    private Transform objInHandGhost;
    private Transform address;

    private AnimationCurve[] curves;
    private AnimationCurve[] curvesSwap;

    private OptionMenu options;
    private InfoController info;
    private Status MessageCanvas;

    float currentTime;

    bool movingToHand;
    bool inHand = false;
    bool swapping = false;
    bool infoSelected = false;

    // Use this for initialization
    void Start () {
        Hand = GameObject.Find("Hand");
        vars = GameObject.FindGameObjectsWithTag("Value");
        movingToHand = false;
        currentTime = 0;
        MessageCanvas = GameObject.Find("MessageCanvas").GetComponent<Status>();
        AnimationCurve[] curves = new AnimationCurve[3];
        options = transform.GetComponent<OptionMenu>();
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
        address = transform.Find("address");
        createAdressValue();
    }
	
	// Update is called once per frame
	void Update () {
        if (movingToHand)
        {
            addressValue.localPosition = new Vector3(
                curves[0].Evaluate(Time.time - currentTime),
                curves[1].Evaluate(Time.time - currentTime),
                curves[2].Evaluate(Time.time - currentTime));

            // Signals end of animation
            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                movingToHand = false;
            }
        }
        else if (swapping)
        {
            addressValue.localPosition = new Vector3(
                curves[0].Evaluate(Time.time - currentTime),
                curves[1].Evaluate(Time.time - currentTime),
                curves[2].Evaluate(Time.time - currentTime));

            objInHand.transform.localPosition = new Vector3(
                curvesSwap[0].Evaluate(Time.time - currentTime),
                curvesSwap[1].Evaluate(Time.time - currentTime),
                curvesSwap[2].Evaluate(Time.time - currentTime));

            // Signals end of animation
            if (Time.time - currentTime > AnimationUtility.ANIM_LENGTH)
            {
                swapping = false;
                objInHand.transform.parent = objInHandGhost.transform.parent;
                Destroy(objInHandGhost.gameObject);
            }
        }

        if (inHand)
        {
            transform.rotation = Hand.transform.rotation;
        }
    }


    public void PickUpButton()
    {
        options.Deselect();
        ToHands();
    }

    public void InfoButton()
    {
        string valueType = transform.GetChild(0).tag;
        string varName = transform.GetComponent<TextMesh>().text;
        info.SetInformation(varName + "\nThis is a Value of Type " + valueType + ".\nYou may assign it to a variable or parameter container of type " + valueType);

    }

    public void ToHands()
    {
        currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();


        //if hand has no object, Pick up the value
        if (objInHand == null)
        {

            // Create Ghost copy to leave behind
            //createAdressValue();

            // Set up animation
            addressValue.parent = Hand.transform;
            curves = AnimationUtility.moveToParent(addressValue, 0, 0, 0);

            movingToHand = true;

            // Disable RigidBody if present on component
            if (GetComponent<Rigidbody>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }

            // Disable current variable in hand
            GetComponent<BoxCollider>().enabled = false;

            Hand.GetComponent<HandController>().setObjInHand(addressValue.gameObject);
            inHand = true;
        }
        else
        {
            // Swap values
            swap();
            //info.SetInformation(varName + "\nThis is a Value of Type " + valueType + ".\nYou may assign it to a variable or parameter container of type " + valueType);

        }

    }

    void swap()
    {
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        if (objInHand.tag == "Value")
        {

            // Put value in hand back where it was
            //objInHandGhost = objInHand.GetComponent<ValueController>().getGhost();
            //objInHand.transform.rotation = objInHandGhost.transform.rotation;
            //objInHand.transform.parent = objInHandGhost.transform;

            // Animate moving value in hand back to its original position
            //curvesSwap = AnimationUtility.moveToParent(objInHand.transform, 0, 0, 0);

            // Create Ghost and place new Value in Hand
            //createAdressValue();

            addressValue.parent = Hand.transform;

            // Animate value being gazed at being brought to hand
            curves = AnimationUtility.moveToParent(addressValue, 0, 0, 0);

            Hand.GetComponent<HandController>().setObjInHand(addressValue.gameObject);

            enableVars(true);
            inHand = true;
            swapping = true;

        }
        else if (objInHand.tag == "VariableBox")
        {
            // Dont swap if variable box in hand
            // TODO: Show two values cannot be swapped
            MessageCanvas.SetMessage("CANNOT PICK UP: You are currently holding a Variable.");
        }
    }

    void createAdressValue()
    {
        addressValue = Instantiate(address, transform.position, transform.rotation, transform.parent);
        //Renderer rend = addressValue.GetComponent<Renderer>();
        //rend.material = Resources.Load("GhostText") as Material;
        addressValue.GetComponent<BoxCollider>().enabled = false;
       
    }


    //public void removeGhost()
    //{
    //    Destroy(ghost.gameObject);
    //}

    public void setInHand(bool b)
    {
        inHand = b;
    }

    public void enableVars(bool enable)
    {
        foreach (GameObject var in vars)
        {
            var.GetComponent<BoxCollider>().enabled = enable;
        }
    }

    //public Transform getGhost()
    //{
    //    return ghost;
    //}

}
