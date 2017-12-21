using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDController : MonoBehaviour {

	public GameObject Hand; 
	public GameObject Shelf; // Shelf to be placed on

	private static GameObject objInHand;

	void Start()
	{
		Hand = GameObject.Find ("Hand");
		Shelf.GetComponent<BoxCollider> ().enabled = false;
	}

	public void InHands()
	{
		transform.parent = Hand.transform.parent;
		transform.position = Hand.transform.position;
		objInHand = this.gameObject;
		Shelf.GetComponent<BoxCollider> ().enabled = true;
	}

	public void OnTheShelf(GameObject shelf)
	{
		Transform dropPosition = shelf.transform.GetChild (0);

		objInHand.transform.parent = dropPosition.transform.parent;
		objInHand.transform.position = dropPosition.position;
		objInHand.transform.rotation = dropPosition.transform.rotation;

		objInHand.transform.localScale = dropPosition.transform.localScale;
		
		Shelf.GetComponent<BoxCollider> ().enabled = false;
	}
}
