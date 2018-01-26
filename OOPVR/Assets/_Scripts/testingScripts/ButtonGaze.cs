using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonGaze : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

    public float activationTime = 1f;
    private Slider gazeProgress;
    private Button button;

    private bool isEntered = false;

    float timeElapsed;


    // Use this for initialization
    void Start()
    {
        gazeProgress = transform.Find("Slider").GetComponent<Slider>();
        button = transform.GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isEntered)
        {
            timeElapsed += Time.deltaTime;
            gazeProgress.value = Mathf.Clamp(timeElapsed / activationTime, 0, 1);
            if (timeElapsed >= activationTime)
            {
                timeElapsed = 0;
                button.onClick.Invoke();
                gazeProgress.value = 0;
                isEntered = false;
            }
        }
        else
        {
            timeElapsed = 0;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isEntered = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEntered = false;
        gazeProgress.value = 0f;
    }

    
}
