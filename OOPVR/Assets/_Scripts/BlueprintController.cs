using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using UnityEngine;

public class BlueprintController : MonoBehaviour {

	private OptionMenu optionsMenu;
	public GameObject instance;

	private bool menuOpen;

	private Vector3 originalSize;
	private Vector3 highlightedSize;
	private Vector3 growth = new Vector3 (0.015f, 1, 0.015f);

	private Animator anim;
    private InfoController info;
    private Status MessageCanvas;
    private bool bpInPosition = false;
    bool infoSelected = false;
    bool alreadyInstantiated = false;

    // Use this for initialization
    void Start () {

        optionsMenu = transform.GetComponent<OptionMenu>();
		
		menuOpen = false;
		anim = GetComponent<Animator> ();
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
        MessageCanvas = GameObject.Find("MessageCanvas").GetComponent<Status>();
    }

    public void InstantiateButton()
    {
        if (alreadyInstantiated)
        {
            MessageCanvas.SetMessage("CANNOT PERFORM ACTION : This Activity only allows you to instantiate ONE instance from the blueprint");
        }
        else
        {
            optionsMenu.Deselect();
            moveBlueprint();
        }
        
    }

	public void moveBlueprint() {
		
		// Disable collider so gaze timer doesnt activate
		GetComponent<BoxCollider>().enabled = false;

		BPControl ("Move");
		StartCoroutine ("check");
		StartCoroutine ("createInstance");
	}

	// Checks whether blueprint is in position for instance animations to begin
	IEnumerator check() {
		bpInPosition = false;

		while (!bpInPosition) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("BPInPosition")) {
				bpInPosition = true;
			}		
		}
	}

	IEnumerator createInstance() {
		while (!bpInPosition) {
			yield return new WaitForSeconds (0.1f);
		}
		instance.GetComponent<InstanceController> ().createInstance ();
	}

	void BPControl(string direction) {
		anim.SetTrigger(direction);
	}

	public void returnToOrigin() {
		this.gameObject.SetActive (false);
		BPControl ("Return");
	}

    public void InfoButton()
    {
        string valueType = transform.GetChild(0).tag;
        string varName = transform.name;
        info.SetInformation("This is a BLUEPRINT, a representation of a CLASS in the programming context!! \nThis Blueprint allow you to INSTANTIATE an INSTANCE of the class it represents.\n ");
    }
}
