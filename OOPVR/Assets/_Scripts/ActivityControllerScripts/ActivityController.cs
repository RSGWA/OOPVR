using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActivityController : MonoBehaviour
{
	List<GameObject> objectsToBlink =  new List<GameObject> ();
	ObjectBlink objBlink;

	public void resetObjectsToBlink() {
		objectsToBlink.Clear ();
	}

	public void addObjectToBlink(GameObject obj) {
		objectsToBlink.Add(obj);
	}

	public void blinkObjects() {
		objBlink = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectBlink> ();
		foreach (GameObject obj in objectsToBlink) {
			objBlink.blinkObject (obj);
		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

