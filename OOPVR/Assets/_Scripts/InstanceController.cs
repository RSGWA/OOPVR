using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceController : MonoBehaviour {

	public Material inactiveMaterial;
	public Material gazedAtMaterial;
	public GameObject player;

	// Use this for initialization
	void Start () {
		setGazedAt (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material = gazedAt ? gazedAtMaterial : inactiveMaterial;
	}

	public void movePlayer() {
		player.transform.position = new Vector3 (-2.0f, 1.2f, 6.71f);
		GetComponent<BoxCollider> ().enabled = false;
	}
}
