using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Activity : MonoBehaviour {

	private string activityTitle;
	private GameObject codePanel;

	// Use this for initialization
	void Awake () {
		activityTitle = this.name;
		codePanel = transform.Find ("CodePanel").gameObject;
	}

	void Start() {
		codePanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void play() {
		switch (activityTitle) {
		case "ConstructorNoParameters":
			SceneManager.LoadScene ("DefaultConstructorScene", LoadSceneMode.Single);
			break;
		case "ConstructorWithParameters":
			SceneManager.LoadScene ("2ParameterConstructor", LoadSceneMode.Single);
			break;
		case "SetName":
			SceneManager.LoadScene ("SetNameActivity", LoadSceneMode.Single);
			break;
		case "GetName":
			SceneManager.LoadScene ("GetName", LoadSceneMode.Single);
			break;
		case "IncrementAge":
			SceneManager.LoadScene ("IncrementAgeActivity", LoadSceneMode.Single);
			break;
		default:
			break;
		}
	}

	public void showCode() {
		codePanel.SetActive (!codePanel.activeSelf);
	}
}
