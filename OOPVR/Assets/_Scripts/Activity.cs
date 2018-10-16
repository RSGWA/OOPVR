using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using OOPVR;

/*
 * This class controls unlocking activities when completed by user. It loads activities accordingly to chosen activity in Main Menu.
 * 
 * */
public class Activity : MonoBehaviour
{

    private string activityTitle;
    private GameObject codePanel;
    private GameObject completedPanel;
    private Button playButton;
    private ButtonGaze gaze;

    static bool constructorNoParametersComplete, constructorWithParametersComplete,
    setNameComplete, getNameComplete, multipleInstancesComplete, multiInstanceMethodsComplete, incrementAgeComplete = false;

    // Use this for initialization
    void Awake()
    {
        activityTitle = this.name;
        codePanel = transform.Find("CodePanel").gameObject;
        completedPanel = transform.Find("CompletedPanel").gameObject;
        playButton = transform.Find("Panel/Play").GetComponent<Button>();
        gaze = transform.Find("Panel/Play").GetComponent<ButtonGaze>();

        playButton.interactable = false;
        gaze.enabled = false;

        switch (activityTitle)
        {
            case "ConstructorNoParameters":
                if (PlayerPrefs.GetInt("ConstructorNoParametersComplete") == 1)
                {
                    completeActivity();
                    constructorNoParametersComplete = true;
                }
                break;
            case "ConstructorWithParameters":
                if (PlayerPrefs.GetInt("ConstructorWithParametersComplete") == 1)
                {
                    completeActivity();
                    constructorWithParametersComplete = true;
                }
                break;
            case "SetName":
                if (PlayerPrefs.GetInt("SetNameComplete") == 1)
                {
                    completeActivity();
                    setNameComplete = true;
                }
                break;
            case "GetName":
                if (PlayerPrefs.GetInt("GetNameComplete") == 1)
                {
                    completeActivity();
                    getNameComplete = true;
                }
                break;
            case "IncrementAge":
                if (PlayerPrefs.GetInt("IncrementAgeComplete") == 1)
                {
                    completeActivity();
                    incrementAgeComplete = true;
                }
                break;
            case "MultipleInstances":
                if (PlayerPrefs.GetInt("MultipleInstances") == 1)
                {
                    completeActivity();
                    multipleInstancesComplete = true;
                }
                break;
            case "MultiInstancesMethodCalls":
                if (PlayerPrefs.GetInt("MultiInstancesMethodCalls") == 1)
                {
                    completeActivity();
                    multiInstanceMethodsComplete = true;
                }
                break;
            default:
                break;
        }
			
    }

    void Start()
    {
        codePanel.SetActive(false);

        switch (activityTitle)
        {
            case "ConstructorNoParameters":
                playButton.interactable = true;
                gaze.enabled = true;
                break;
            case "ConstructorWithParameters":
                Debug.Log(constructorNoParametersComplete);
                if (constructorNoParametersComplete)
                {
                    playButton.interactable = true;
                    gaze.enabled = true;
                }
                break;
            case "SetName":
            case "GetName":
                if (constructorWithParametersComplete)
                {
                    playButton.interactable = true;
                    gaze.enabled = true;
                }
                break;
            case "IncrementAge":
                if (setNameComplete && getNameComplete)
                {
                    playButton.interactable = true;
                    gaze.enabled = true;
                }
                break;
            case "MultipleInstances":
                if (incrementAgeComplete)
                {
                    playButton.interactable = true;
                    gaze.enabled = true;
                }
                break;
            case "MultiInstancesMethodCalls":
                if (multipleInstancesComplete)
                {
                    playButton.interactable = true;
                    gaze.enabled = true;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void completeActivity()
    {
        completedPanel.transform.Find("Text").GetComponent<Text>().text = "COMPLETED";
        completedPanel.GetComponent<Image>().color = Color.green;
    }

    void unlockNext()
    {
        switch (activityTitle)
        {
            case "ConstructorNoParameters":

                break;
            case "ConstructorWithParameters":

                break;
            case "SetName":

                break;
            case "GetName":

                break;
            case "IncrementAge":

                break;
            default:
                break;
        }
    }

    public void play()
    {
        switch (activityTitle)
        {
            case "ConstructorNoParameters":
                SceneManager.LoadScene("DefaultConstructorScene", LoadSceneMode.Single);
                break;
            case "ConstructorWithParameters":
                SceneManager.LoadScene("2ParameterConstructor", LoadSceneMode.Single);
                break;
            case "SetName":
                SceneManager.LoadScene("SetNameActivity", LoadSceneMode.Single);
                break;
            case "GetName":
                SceneManager.LoadScene("GetNameActivity", LoadSceneMode.Single);
                break;
            case "IncrementAge":
                SceneManager.LoadScene("IncrementAgeActivity", LoadSceneMode.Single);
                break;
            case "MultipleInstances":
                SceneManager.LoadScene("MultipleInstancesActivity", LoadSceneMode.Single);
                break;
            case "MultiInstancesMethodCalls":
                SceneManager.LoadScene("MultiInstancesMethodCallsActivity", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    public void showCode()
    {
        codePanel.SetActive(!codePanel.activeSelf);
    }
}
