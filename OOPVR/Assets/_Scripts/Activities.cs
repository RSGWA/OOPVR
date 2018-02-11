using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Activities : MonoBehaviour {
	
	public GameObject Next;
	public GameObject Previous;
	public GameObject Title;

	private List<string> activityTitles;

	int currentActivity;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
		currentActivity = 0;

		activityTitles = new List<string> ();

		activityTitles.Add ("Default Constructor");
		activityTitles.Add ("Constructor with Parameters");
		activityTitles.Add ("Get name");
		activityTitles.Add ("Set name");

		setActivityTitle (currentActivity);
	}

	// Update is called once per frame
	void Update () {

	}

	public void showActivityMenu() {
		this.gameObject.SetActive (true);
		Previous.SetActive (false);
	}

	public void nextActiviy() {

		currentActivity++;

		if (currentActivity >= activityTitles.Count - 1) {
			Next.SetActive (false);
		}

		if (!Previous.activeSelf) {
			Previous.SetActive (true);
		}

		setActivityTitle (currentActivity);

	}

	public void previousActivity() {
		
		currentActivity--;

		if (currentActivity <= 0) {
			Previous.SetActive (false);
		}

		if (!Next.activeSelf) {
			Next.SetActive (true);
		}

		setActivityTitle (currentActivity);
	}

	void setActivityTitle(int activity) {
		Title.GetComponent<Text> ().text = activityTitles [activity];
	}

	public void playActivity() {
		
		switch (activityTitles [currentActivity]) {
		case "Default Constructor":
			SceneManager.LoadScene ("DefaultConstructorScene", LoadSceneMode.Single);
			break;
		case "Constructor with Parameters":
			SceneManager.LoadScene ("2ParameterConstructor", LoadSceneMode.Single);
			break;
		case "Get name":
			SceneManager.LoadScene ("GetName", LoadSceneMode.Single);
			break;
		case "Set name":
			SceneManager.LoadScene ("SetNameActivity", LoadSceneMode.Single);
			break;
		default:
			break;
		}
	}

}