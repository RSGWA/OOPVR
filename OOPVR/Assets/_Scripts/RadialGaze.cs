using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialGaze : MonoBehaviour {

	public float NumberOfSecondsForSelection = 3f;
	public Transform RadialProgress;
	private float counterTimer = 0;

	// Use this for initialization
	void Start () {

		RadialProgress.GetComponent<Image>().fillAmount = 0;
		this.enabled = false;

	}

	// Update is called once per frame
	public void Update () {

		counterTimer += Time.deltaTime;
		RadialProgress.GetComponent<Image>().fillAmount = counterTimer/NumberOfSecondsForSelection;

		if (counterTimer >= NumberOfSecondsForSelection) {
			PerformActionOnGameObject ();
			ResetCounter ();
		}
	}

	public void ResetCounter() {
		counterTimer = 0f;
		RadialProgress.GetComponent<Image>().fillAmount = counterTimer;
	}

	public void PerformActionOnGameObject() {
		GameObject obj = GvrPointerInputModule.CurrentRaycastResult.gameObject;
		switch (obj.tag) {
		case "Variable":
			// Pick up variable
			obj.GetComponent<DragAndDrop> ().InHands ();
			break;
		case "Box":
			// Place in box
			obj.GetComponent<DragAndDrop> ().InBox ();
			break;
		default:
			break;
		}
	}
}
