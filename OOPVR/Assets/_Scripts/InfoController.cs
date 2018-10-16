using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Attached to information canvas to show/hide information in regards to a selected object accordingly.
 * Moves the info canvas in front of player when called. 
 * 
 * */

public class InfoController : MonoBehaviour {

    private TextMeshProUGUI infoStatus;
    private GameObject canvas;
    private Transform camera;
    private Vector3 playerPos, infoPos;
    private Quaternion playerRotation;
    private PlayerController player;


    // Use this for initialization
    void Start()
    {
        infoStatus = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        canvas = GameObject.Find("InfoCanvas");
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        camera = GameObject.Find("Main Camera").transform;

        playerPos = player.transform.position;
        //playerRotation = player.transform.rotation;
        infoPos = new Vector3(0, 0.5f, 4);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        //playerRotation = player.transform.rotation;
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
        float yRotation = camera.eulerAngles.y;

        if(yRotation < -90 || yRotation > 90)
        {
            transform.SetPositionAndRotation((playerPos + camera.forward * 4), camera.rotation);
        }
        else
        {
            //transform.SetPositionAndRotation((playerPos + player.transform.forward * 4), playerRotation);
            transform.SetPositionAndRotation((playerPos + camera.forward * 4), camera.rotation);
        }
       
       

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