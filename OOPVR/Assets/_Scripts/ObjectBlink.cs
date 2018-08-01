using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class ObjectBlink : MonoBehaviour {

	private Outline outline;
	private OutlineEffect outlineEffect;

	private GameObject objBlinking;

	private int originalOutlineColor;
	private static float BLINK_DELAY = 4f;

	IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		outlineEffect = GameObject.FindGameObjectWithTag ("Player").transform.Find ("Main Camera").GetComponent<OutlineEffect>();

		coroutine = glow ();
	}

	public void blinkObject(GameObject obj) {
		StartCoroutine (blink (obj));
	}

	IEnumerator blink(GameObject obj) 
	{
		yield return new WaitForSeconds (BLINK_DELAY);

		objBlinking = obj;

		if (objectWasSelected ()) {
			yield break;
		}

		originalOutlineColor = obj.GetComponent<Outline> ().color;

		outline = obj.GetComponent<Outline> ();
		obj.GetComponent<Outline> ().color = 2;
		outline.eraseRenderer = false;
		outline.enabled = true;

		StopCoroutine (coroutine);
		StartCoroutine (coroutine);
	}

	private bool objectWasSelected() {
		GameObject currentSelectedObj = objBlinking.GetComponent<InteractiveItemGaze> ().getCurrentSelectedObj ();
		if (currentSelectedObj == objBlinking) {
			return true;
		} else {
			return false;
		}
	}
		
	private void checkBlinkingObjectCurrentlySelected()
	{
		GameObject currentSelectedObj = objBlinking.GetComponent<InteractiveItemGaze> ().getCurrentSelectedObj ();
		if (currentSelectedObj == objBlinking) {
			objBlinking.GetComponent<Outline> ().color = originalOutlineColor;
			outlineEffect.fillAmount = 0f;

			StopCoroutine (coroutine);
			Debug.Log ("OBJECT BLINKING STOPPED");
		}
	}

	private IEnumerator glow() {

		while (true) {
			checkBlinkingObjectCurrentlySelected ();
			while (outlineEffect.fillAmount < 0.3f) 
			{
				outlineEffect.fillAmount += 0.4f * Time.deltaTime;
				checkBlinkingObjectCurrentlySelected ();
				yield return null;
			}

			while (outlineEffect.fillAmount > 0) 
			{
				outlineEffect.fillAmount -= 0.4f * Time.deltaTime;
				checkBlinkingObjectCurrentlySelected ();
				yield return null;
			}
		}

	}
}
