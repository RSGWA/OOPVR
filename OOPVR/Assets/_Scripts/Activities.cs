using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Activities : MonoBehaviour {
	
	public GameObject Next;
	public GameObject Previous;
	public GameObject Title;

	private List<string> activityTitles;

	int currentActivity;

	// Use this for initialization
	void Awake () {

		/*
		if (PlayerPrefs.GetInt ("ActivityMenuActive") == 1) {
			this.gameObject.SetActive (true);
			currentActivity = PlayerPrefs.GetInt ("CurrentActivity");
		} else {
			this.gameObject.SetActive (false);
			currentActivity = 0;
		}
		*/

		//this.gameObject.SetActive (false);

		//Load ();

		activityTitles = new List<string> ();

		activityTitles.Add ("Default Constructor");
		activityTitles.Add ("Constructor with Parameters");
		activityTitles.Add ("Get name");
		activityTitles.Add ("Set name");
		activityTitles.Add ("Increment age");

		setActivityTitle (currentActivity);
	}

	// Update is called once per frame
	void Update () {

	}

	public void showActivityMenu() 
	{
		currentActivity = 0;
		setActivityTitle (currentActivity);
		this.gameObject.SetActive (true);
		Previous.SetActive (false);
		Next.SetActive (true);
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
		
		SaveScene ();

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
		case "Increment age":
			SceneManager.LoadScene ("IncrementAgeActivity", LoadSceneMode.Single);
			break;
		default:
			break;
		}
	}

	public void SaveScene() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/mainMenuScene.dat");

		SceneData data = new SceneData ();
		data.activityMenuOpen = this.gameObject.activeSelf;
		data.currentActivity = currentActivity;
		data.prevActive = Previous.activeSelf;
		data.nextActive = Next.activeSelf;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load() {
		if (File.Exists (Application.persistentDataPath + "/mainMenuScene.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/mainMenuScene.dat", FileMode.Open);
			SceneData data = (SceneData)bf.Deserialize (file);
			file.Close ();

			this.gameObject.SetActive (data.activityMenuOpen);
			currentActivity = data.currentActivity;
			Previous.SetActive (data.prevActive);
			Next.SetActive (data.nextActive);
		}
	}
		
	void OnApplicationQuit() {
		resetScene ();
	}

	// Reset scene to initial values - used when application has quit
	void resetScene() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/mainMenuScene.dat");

		SceneData data = new SceneData ();
		data.activityMenuOpen = false;
		data.currentActivity = 0;

		bf.Serialize (file, data);
		file.Close ();
	}

	[Serializable]
	class SceneData
	{
		public bool activityMenuOpen;
		public int currentActivity;
		public bool prevActive;
		public bool nextActive;
	}

}