using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnController : MonoBehaviour {

    private GameObject hand;
    private PlayerController player;
    private InfoController info;
    private OptionMenu options;

    bool infoSelected = false;

    // Use this for initialization
    void Start () {
		hand = GameObject.FindGameObjectWithTag ("Hand");
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
        options = transform.GetComponent<OptionMenu>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            options.Deselect();
		}

        player.setInRoom(false);
        

    }

	// Update is called once per frame
	void Update () {
		
	}

    void enableMovePoints(bool trigger)
    {
        GameObject[] movePoints = GameObject.FindGameObjectsWithTag("Move");
        foreach (GameObject movePoint in movePoints)
        {
            movePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(trigger);
        }
    }

    public void InfoButton()
    {
        info.SetInformation("This is a <i>return</i> which is the exit point of a method or instance.\n" + 
            "It is usually represented in code by \"}\" meaning void or \"return\" meaning having the required object in Hand before returning.\n");

        

    }
}
