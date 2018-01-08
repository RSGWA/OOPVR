using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    private Animator _anim;
    private GameObject Hand;
    private GameObject objInHand;
    bool active = false;
    private GameObject parameterPosition;

    // Use this for initialization
    void Start()
    {

        _anim = GetComponent<Animator>();
        Hand = GameObject.FindGameObjectWithTag("Hand");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Control()
    {
        if (!active)
        {
            _anim.SetTrigger("Activate");
            active = true;
        }
        else
        {
            _anim.SetTrigger("Deactivate");
            active = false;
        }

    }

    public void ToPlatform(Transform platformPos)
    {
        objInHand = Hand.GetComponent<HandController>().getObjInHand();
        

        Transform platform = platformPos.Find("Platform");
        parameterPosition = platform.Find("ParamPos").gameObject;

        objInHand.transform.parent = parameterPosition.transform.parent;
        objInHand.transform.position = parameterPosition.transform.position;
        objInHand.transform.rotation = parameterPosition.transform.rotation;

    }

    public void ControlPlatform(GameObject paramPlatform, string variableType)
    {
        GameObject parentPlatform = paramPlatform.transform.parent.gameObject;
        int parameterCount = paramPlatform.transform.parent.transform.childCount;

        Transform paramType = paramPlatform.transform.GetChild(0).Find("ParamType");

        if (parameterCount == 1)
        {
            //Checks parameter type matches variable type
            if (paramType.tag == variableType)
            {
                //***FIRE DOOR ANIMATION HERE****
                Control();
            }
        }

        GameObject[] allParameterPlatforms = new GameObject[parameterCount];
        int correctParams = 0;

        //Checks to see if all other parameters have been assigned a CORRECT variable
        for (int i = 0; i < parameterCount; i++)
        {
            Transform parameterPlatform = parentPlatform.transform.GetChild(i);
            //Flag that Parameter has a CORRECT variable assigned to it
            allParameterPlatforms[i] = parameterPlatform.gameObject;

            int childCounts = parameterPlatform.GetChild(0).childCount;

            print("childCounts == " + childCounts);

            // check whether variable is ON THE PLATFORM
            if (childCounts == 5) 
            {
                Transform parameterType = parameterPlatform.GetChild(0).GetChild(0);
                Transform varType = parameterPlatform.GetChild(0).GetChild(4).GetChild(0);

                //check whether the CORRECT VARIABLE is on the RIGHT PLATFORM
                if (parameterType.tag == varType.tag) 
                {
                    correctParams = correctParams + 1;
                }
            }

        }
        print("Correct Params == " + correctParams + "      ParameterCount == " + parameterCount);

        if (correctParams == parameterCount)
        {
            foreach (GameObject platform in allParameterPlatforms)
            {
                //***FIRE DOOR ANIMATION HERE****
                platform.GetComponent<Animator>().SetTrigger("Activate"); // activating the platforms
                platform.GetComponent<BoxCollider>().enabled = false;
                platform.GetComponent<CapsuleCollider>().enabled = false;

            }
            
        }

    }

}
