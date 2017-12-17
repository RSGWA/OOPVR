using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public GameObject instance;

	private float waitTime = 0.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createInstance() {
		StartCoroutine (BuildInstance ());
		//GameObject instanceCopy;
		//Vector3 position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 2.0f, Camera.main.transform.position.z - 10.0f);
		//instanceCopy = Instantiate(instance, position, Quaternion.identity, Camera.main.transform);
		//instance.transform.parent = Camera.main.transform;
	}

	IEnumerator BuildInstance() {
		if (instance.transform.position.y >= 1.5f) {
			yield break;
		}
		yield return new WaitForSeconds (waitTime);
		instance.transform.localPosition += new Vector3 (0f, 0.1f, 0f);
		StartCoroutine (BuildInstance ());
	}
}
