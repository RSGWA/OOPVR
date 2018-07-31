using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class ObjectBlink : MonoBehaviour {

	private Outline outline;
	private OutlineEffect outlineEffect;

	private GameObject objBlinking;
	private int originalOutlineColor;

	IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		outlineEffect = GameObject.FindGameObjectWithTag ("Player").transform.Find ("Main Camera").GetComponent<OutlineEffect>();

		coroutine = glow ();
	}

	public void blinkObject(GameObject obj) 
	{
		objBlinking = obj;
		originalOutlineColor = obj.GetComponent<Outline> ().color;

		outline = obj.GetComponent<Outline> ();
		outline.eraseRenderer = false;
		outline.enabled = true;

		obj.GetComponent<Outline> ().color = 2;

		StopCoroutine (coroutine);
		StartCoroutine (coroutine);
	}

	private void checkBlinkingObjectSelected()
	{
		GameObject currentSelectedObj = objBlinking.GetComponent<InteractiveItemGaze> ().getCurrentSelectedObj ();
		if (currentSelectedObj == objBlinking) {
			objBlinking.GetComponent<Outline> ().color = originalOutlineColor;
			outlineEffect.fillAmount = 0f;

			StopCoroutine (coroutine);
		}
	}

	private IEnumerator glow() {

		while (true) {
			while (outlineEffect.fillAmount < 0.3f) 
			{
				outlineEffect.fillAmount += 0.4f * Time.deltaTime;
				checkBlinkingObjectSelected ();
				yield return null;
			}

			while (outlineEffect.fillAmount > 0) 
			{
				outlineEffect.fillAmount -= 0.4f * Time.deltaTime;
				checkBlinkingObjectSelected ();
				yield return null;
			}
		}

	}
}
