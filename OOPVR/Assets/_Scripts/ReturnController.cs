using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnController : MonoBehaviour {

	private GameObject hand;
    private InfoController info;

    bool infoSelected = false;

    // Use this for initialization
    void Start () {
		hand = GameObject.FindGameObjectWithTag ("Hand");
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
    }

	public void checkAndReturn() {
		string returnType = transform.GetChild (0).tag;
		GameObject objInHand = hand.GetComponent<HandController> ().getObjInHand ();

		string varType;

		if (objInHand == null) {
			varType = "Void";
		} else {
			varType = objInHand.transform.GetChild (0).tag;
		}

		if (varType == returnType) {
			// Exit room
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().backToOrigin();
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

    public void InfoButton()
    {
        info.SetInformation("This is a Return.\n" + "Imagine it like a return in a method/nIf void, you may return without a variable./n" +
           "PLEASE SELECT INFO AGAIN TO DESELECT!");

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
}
