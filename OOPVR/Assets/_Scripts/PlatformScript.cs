using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    private Animator _anim;
    GameObject _go = null;
    bool active = false;

	// Use this for initialization
	void Start () {

        _anim = GetComponent<Animator>();
        _go = GameObject.FindGameObjectWithTag("Door");


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Control()
    {
        if (!active)
        {
            _anim.SetTrigger("Activate");
            /*if (_go != null)
            {
                _go.GetComponent<Doors>().ControlDoor();
            }*/
            active = true;
        }
        else
        {
            _anim.SetTrigger("Deactivate");
            active = false;
        }

    }

}
