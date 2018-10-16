using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

/*
 * This class define actions to the in game menu functions 
 * 
 * */

public class InGameMenu : MonoBehaviour {

	float distanceFromCamera = 1.6f;
	ActivityController ac;
	public Material dimSky;

	// Use this for initialization
	void Start () {
		ac = GameObject.Find ("ActivityController").GetComponent<ActivityController> ();
	}
	
	// Update is called once per frame
	void Update () {
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
		RenderSettings.skybox = dimSky;
		RenderSettings.ambientIntensity = 0.3f;
	}
}
