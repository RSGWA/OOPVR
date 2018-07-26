using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddressBoxController : MonoBehaviour
{

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
    private CurveLine curvedLine;



    float currentTime;

    bool movingToHand;
    bool inHand = false;
    bool swapping = false;
    bool infoSelected = false;

    // Use this for initialization
    void Start()
    {
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
    void Update()
    {
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

        if (inHand)
        {
            addressValue.transform.rotation = Hand.transform.rotation;
        }

        if(addressValue.parent != Hand && addressValue.parent != transform)
        {
            //curvedLine.HideCurve();
        }

    }


    public void PickUpAddress()
    {
        options.Deselect();
        ToHands();
    }

    public void ToHands()
    {
        currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        curvedLine.ShowCurve();
        //if hand has no object, Pick up the value
        if (objInHand == null)
        {

            // Set up animation
            addressValue.parent = Hand.transform;
            curves = AnimationUtility.moveToParent(addressValue, 0, 0, 0);

            movingToHand = true;

            // Disable RigidBody if present on component
            if (addressValue.GetComponent<Rigidbody>() != null)
            {
                Destroy(addressValue.GetComponent<Rigidbody>());
            }

            // Disable current variable in hand
            GetComponent<BoxCollider>().enabled = false;

            Hand.GetComponent<HandController>().setObjInHand(addressValue.gameObject);
            inHand = true;
        }
        else
        {

            info.SetInformation("\nPlease assign the address to the correct container to complete instantiation of the instance");

        }

    }

    void createAdressValue()
    {
        //addressValue = Instantiate(address, transform.position, Quaternion.identity , transform.parent);
        addressValue = Instantiate(address, address.transform.position, address.transform.rotation, transform);

        curvedLine = transform.GetComponent<CurveLine>();

        curvedLine.CreateCurveTo(addressValue);

        //Renderer rend = addressValue.GetComponent<Renderer>();
        //rend.material = Resources.Load("GhostText") as Material;
        addressValue.GetComponent<BoxCollider>().enabled = false;

    }

    public void InfoButton()
    {
        string valueType = transform.GetChild(0).tag;
        string varName = transform.GetComponent<TextMesh>().text;
        info.SetInformation(varName + "\nThis is a Value of Type " + valueType + ".\nYou may assign it to a variable or parameter container of type " + valueType);

    }

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

    public CurveLine getCurvedLine()
    {
        return curvedLine;
    }
}
