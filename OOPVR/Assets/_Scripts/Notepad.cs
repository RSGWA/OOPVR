using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OOPVR;
using TMPro;

public class Notepad : MonoBehaviour
{

    GameObject[] objectives;
    GameObject tracker;
    GameObject code;
    GameObject title;

    string defaultConstructorFilename = "defaultConstructor";
    string parameterConstructorFilename = "constructorWithParameter";
    string getNameFilename = "getName";
    string setNameFilename = "setName";
    string incrementAgeFilename = "incrementAge";
    string multiInstancesFilename = "multipleInstances";
    string multiInstancesMethodCallsFilename = "MultiInstancesMethodCalls";

    string activeText;
	string blinkedText;
	string originalUnhighlightedText;
	string originalBlinkedText;

    string objectivesEnlargedText;

    TextAsset textAsset;
    string textToParse;

    List<string> pages = new List<string>();

    int currentPage = 0;

    int fontSize = 105;

	IEnumerator coroutine;

    void Awake()
    {
        objectives = GameObject.FindGameObjectsWithTag("Objective");
        tracker = GameObject.FindGameObjectWithTag("Tracker");
        //setObjective (getObjective ("0"));
        code = this.transform.Find("Code").gameObject;
        title = this.transform.Find("Title").gameObject;

        // Depending on the current scene, display corresponding code on notepad
        Scene activeScene = SceneManager.GetActiveScene();

        switch (activeScene.name)
        {
            case "DefaultConstructorScene":
                textAsset = Resources.Load(defaultConstructorFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "2ParameterConstructor":
                textAsset = Resources.Load(parameterConstructorFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "GetName":
                textAsset = Resources.Load(getNameFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "SetNameActivity":
                textAsset = Resources.Load(setNameFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "IncrementAgeActivity":
                textAsset = Resources.Load(incrementAgeFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "MultipleInstancesActivity":
                textAsset = Resources.Load(multiInstancesFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            case "MultiInstancesMethodCallsActivity":
                textAsset = Resources.Load(multiInstancesFilename) as TextAsset;
                textToParse = textAsset.text;
                parseText(textToParse);
                break;
            default:
                break;
        }

        activeText = pages[0]; // Main
        setTitle("Main");

        setText(0);

		// Keep a copy of executing coroutine so it can be stopped
		coroutine = blink ();
    }

    void Update()
    {

    }

    void parseText(string text)
    {
        List<int> list = text.AllIndexsOf("<p>");
        List<int> startIndices = new List<int>();
        foreach (int index in list)
        {
            startIndices.Add(index + 3);
        }
        List<int> endIndices = text.AllIndexsOf("</p>");

        for (int i = 0; i < startIndices.Count; i++)
        {
            pages.Add(text.Substring(startIndices[i], endIndices[i] - startIndices[i]));
        }
    }

	public void highlightText(string text, string color)
	{
        int startIndex = activeText.IndexOf(text);
        int endIndex = startIndex + text.Length;

		// If code to highlight is not on current page
		if (startIndex == -1) {
			return;
		}

		activeText = activeText.Insert(endIndex, "</color>");
		activeText = activeText.Insert(startIndex, "<color=" + color + ">");

		startIndex = blinkedText.IndexOf(text);
		endIndex = startIndex + text.Length;

		blinkedText = blinkedText.Insert(endIndex, "</color>");
		blinkedText = blinkedText.Insert(startIndex, "<color=" + color + ">");

		setText (activeText);
    }

	public void unhighlightText(string text)
	{
		int startIndex = activeText.IndexOf(text);
		int endIndex = startIndex + text.Length;

		// If code to highlight is not on current page
		if (startIndex == -1) {
			return;
		}

		activeText = activeText.Insert(endIndex, "</color>");
		activeText = activeText.Insert(startIndex, "<color=black>");

		blinkedText = originalBlinkedText;

		setText (activeText);
	}

	public void blinkText(string text, string color) {
		blinkedText = activeText;

		int startIndex = blinkedText.IndexOf (text);
		int endIndex = startIndex + text.Length;

		// If code to highlight is not on current page
		if (startIndex == -1) {
			return;
		}

		blinkedText = blinkedText.Insert(endIndex, "</color>");
		blinkedText = blinkedText.Insert(startIndex, "<color=" + color + ">");

		// store original blinked text state
		originalBlinkedText = blinkedText;

		StopCoroutine (coroutine);
		StartCoroutine (coroutine);
	}

	IEnumerator blink() {
		
		while (true) {
			
			// Highlight
			setText(blinkedText);
			yield return new WaitForSeconds (0.25f);
			// Unhighlight
			setText(activeText);
			yield return new WaitForSeconds (0.25f);
		}
	}

    void setText(int pageNumber)
    {
		code.GetComponent<TextMeshProUGUI>().text = pages[pageNumber];
        currentPage = pageNumber;
    }

    void setText(string text)
    {
		code.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void setTitle(string text)
    {
		title.GetComponent<TextMeshProUGUI>().text = text;
    }

	public void blinkObjective(string objective) {
		reset ();
		blinkText (objective, "white");
	}

    public void reset()
    {
        activeText = pages[currentPage];
        setText(currentPage);
    }

    public void setActiveText(int pageNumber)
    {
        setText(pageNumber);
    }

    public void endOfActivity()
    {
		StopCoroutine (coroutine);
        //setTitle("Congratulations");
		//setText("Activity Completed" + System.Environment.NewLine +
        	//"Returning to Main Menu");
		appendText(System.Environment.NewLine + "RETURNING TO MAIN MENU");
		Invoke ("returnToMenu", 5f);
    }

	void appendText(string text)
	{
		code.GetComponent<TextMeshProUGUI> ().text = code.GetComponent<TextMeshProUGUI> ().text + text;
	}

	void returnToMenu() {
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}
}
