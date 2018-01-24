using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueController : MonoBehaviour
{

    private GameObject Hand;
    private GameObject[] vars;
    private GameObject MessageCanvas;
	private GameObject objInHand;

	private Transform ghost;
	private Transform objInHandGhost;

	private AnimationCurve[] curves;
	private AnimationCurve[] curvesSwap;

    float currentTime;

    bool movingToHand;
    bool inHand = false;
	bool swapping = false;

    void Start()
    {
        Hand = GameObject.Find("Hand");
        vars = GameObject.FindGameObjectsWithTag("Value");
        movingToHand = false;
        currentTime = 0;
        MessageCanvas = GameObject.Find("MessageCanvas");
		AnimationCurve[] curves = new AnimationCurve[3];
    }

    void Update()
    {
		if (movingToHand) {
			transform.localPosition = new Vector3 (
				curves [0].Evaluate (Time.time - currentTime), 
				curves [1].Evaluate (Time.time - currentTime),
				curves [2].Evaluate (Time.time - currentTime));

			// Signals end of animation
			if (Time.time - currentTime > AnimatorController.ANIM_LENGTH) {
				movingToHand = false;
			}
		} else if (swapping) {
			transform.localPosition = new Vector3 (
				curves [0].Evaluate (Time.time - currentTime), 
				curves [1].Evaluate (Time.time - currentTime),
				curves [2].Evaluate (Time.time - currentTime));

			objInHand.transform.localPosition = new Vector3 (
				curvesSwap[0].Evaluate (Time.time - currentTime), 
				curvesSwap [1].Evaluate (Time.time - currentTime),
				curvesSwap [2].Evaluate (Time.time - currentTime));

			// Signals end of animation
			if (Time.time - currentTime > AnimatorController.ANIM_LENGTH) {
				swapping = false;
				objInHand.transform.parent = objInHandGhost.transform.parent;
				Destroy (objInHandGhost.gameObject);
			}
		}

		if (inHand) {
			transform.rotation = Hand.transform.rotation;
		}
    }

    public void ToHands()
    {
		currentTime = Time.time;
        objInHand = Hand.GetComponent<HandController>().getObjInHand();

        //if hand has no object, Pick up the value
        if (objInHand == null)
        {

			// Create Ghost copy to leave behind
			createValueGhost();

			// Set up animation
            transform.parent = Hand.transform;
			curves = AnimatorController.moveToParent(transform, 0, 0, 0);

            movingToHand = true;

            // Disable RigidBody if present on component
            if (GetComponent<Rigidbody>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }

            // Disable current variable in hand
            GetComponent<BoxCollider>().enabled = false;

			Hand.GetComponent<HandController>().setObjInHand(this.gameObject);
			inHand = true;
        }
        else
        {
			// Swap values
			swap ();
		}

    }

	void swap() {
		if (objInHand.tag == "Value") {

			// Put value in hand back where it was
			objInHandGhost = objInHand.GetComponent<ValueController> ().getGhost ();
			objInHand.transform.rotation = objInHandGhost.transform.rotation;
			objInHand.transform.parent = objInHandGhost.transform;
	
			// Animate moving value in hand back to its original position
			curvesSwap = AnimatorController.moveToParent (objInHand.transform, 0, 0, 0);

			// Create Ghost and place new Value in Hand
			createValueGhost();

			transform.parent = Hand.transform;

			// Animate value being gazed at being brought to hand
			curves = AnimatorController.moveToParent (transform, 0, 0, 0);

			Hand.GetComponent<HandController>().setObjInHand(this.gameObject);

			enableVars (true);
			inHand = true;
			swapping = true;
		} else if (objInHand.tag == "VariableBox") {
			// Dont swap if variable box in hand
			// TODO: Show two values cannot be swapped
		}
	}

	void createValueGhost() {
		ghost = Instantiate(transform, transform.position, transform.rotation, transform.parent);
		Renderer rend = ghost.GetComponent<Renderer>();
		rend.material = Resources.Load("GhostText") as Material;
		ghost.GetComponent<BoxCollider>().enabled = false;
	}

	public void removeGhost() {
		Destroy(ghost.gameObject);
	}

    public void setInHand(bool b)
    {
        inHand = b;
    }

    void enableVars(bool enable)
    {
        foreach (GameObject var in vars)
        {
            var.GetComponent<BoxCollider>().enabled = enable;
        }
    }

	public Transform getGhost() {
		return ghost;
	}
}
