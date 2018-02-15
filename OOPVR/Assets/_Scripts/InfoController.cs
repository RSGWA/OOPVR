using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoController : MonoBehaviour {

    private TextMeshProUGUI infoStatus;
    private GameObject canvas;

    // Use this for initialization
    void Start()
    {
        infoStatus = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("InfoCanvas");
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInformation(string msg)
    {
        infoStatus.text = msg;
        StartCoroutine(UpdateMessage());
    }

    private IEnumerator UpdateMessage()
    {
        ShowInformation();
        yield return new WaitForSeconds(4.5f);
        HideInformation();
    }

    public void ShowInformation()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void HideInformation()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }
}