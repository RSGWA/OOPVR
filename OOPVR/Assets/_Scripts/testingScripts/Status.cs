using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Status : MonoBehaviour
{

    //private Image background;
    private Text messageStatus;
    private GameObject canvas;

    // Use this for initialization
    void Start()
    {
        messageStatus = GameObject.Find("MessageText").GetComponent<Text>();
        canvas = GameObject.Find("MessageCanvas");
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMessage(string msg)
    {
        messageStatus.text = msg;
        StartCoroutine(UpdateMessage());
    }

    private IEnumerator UpdateMessage()
    {
        EnableCanvas();
        yield return new WaitForSeconds(4);
        DisableCanvas();
    }

    private void EnableCanvas()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void DisableCanvas()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }
}
