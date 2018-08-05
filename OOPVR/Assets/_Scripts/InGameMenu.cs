using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

	GameObject Notepad;
	ActivityController ac;

	// Use this for initialization
	void Start () {
		Notepad = GameObject.FindGameObjectWithTag ("Notepad");
		ac = GameObject.Find ("ActivityController").GetComponent<ActivityController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main.transform.rotation.eulerAngles.x > 62f & Camera.main.transform.eulerAngles.x < 90f) {
			Notepad.GetComponent<Image> ().enabled = false;
		} else {
			Notepad.GetComponent<Image> ().enabled = true;
		}
	}

	public void restartActivity() {
		string sceneName = SceneManager.GetActiveScene ().name;
		switch (sceneName)
		{
		case "DefaultConstructorScene":
			SceneManager.LoadScene("DefaultConstructorScene", LoadSceneMode.Single);
			break;
		case "2ParameterConstructor":
			SceneManager.LoadScene("2ParameterConstructor", LoadSceneMode.Single);
			break;
		case "SetNameActivity":
			SceneManager.LoadScene("SetNameActivity", LoadSceneMode.Single);
			break;
		case "GetNameActivity":
			SceneManager.LoadScene("GetNameActivity", LoadSceneMode.Single);
			break;
		case "IncrementAgeActivity":
			SceneManager.LoadScene("IncrementAgeActivity", LoadSceneMode.Single);
			break;
		case "MultipleInstancesActivity":
			SceneManager.LoadScene("MultipleInstancesActivity", LoadSceneMode.Single);
			break;
		case "MultiInstancesMethodCallsActivity":
			SceneManager.LoadScene("MultiInstancesMethodCallsActivity", LoadSceneMode.Single);
			break;
		default:
			break;
		}
	}

	public void exitActivity() {
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

	public void hint() {
		ac.blinkObjects ();
	}
}
