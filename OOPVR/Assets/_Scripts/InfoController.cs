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
        ShowInformation();

        //StartCoroutine(UpdateMessage());
    }

    private IEnumerator UpdateMessage()
    {
        ShowInformation();
        yield return new WaitForSeconds(5.5f);
        HideInformation();
    }

    public void ShowInformation()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Vector3 playerPos = player.transform.position;
        Vector3 infoPos = new Vector3(0, 0.5f, 5);
        transform.SetPositionAndRotation((playerPos + infoPos), player.transform.rotation);
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvas.GetComponent<CanvasGroup>().interactable = true;
        canvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void HideInformation()
    {
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvas.GetComponent<CanvasGroup>().interactable = false;
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    
}