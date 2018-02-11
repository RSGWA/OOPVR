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
    private bool bpInPosition = false;
    bool infoSelected = false;

    // Use this for initialization
    void Start () {

        optionsMenu = transform.GetComponent<OptionMenu>();
		
		menuOpen = false;
		anim = GetComponent<Animator> ();
        info = GameObject.Find("InfoCanvas").GetComponent<InfoController>();
    }

    public void InstantiateButton()
    {
        optionsMenu.Deselect();
        moveBlueprint();
    }

	public void moveBlueprint() {
		//optionsMenu.SetActive (false);

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
        info.SetInformation("This is a Blueprint!! \nA blueprint represents a class in the programming context.\n PLEASE SELECT INFO AGAIN TO DESELECT!");

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
