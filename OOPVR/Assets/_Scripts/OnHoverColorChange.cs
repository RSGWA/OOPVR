using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHoverColorChange : MonoBehaviour, IGvrPointerHoverHandler {

    public Material onHoverColor;
    private Material normalColor;

    

	// Use this for initialization
	void Start () {
        normalColor = GetComponent<Renderer>().material;
    }


    public void OnHover()
    {
        GetComponent<Renderer>().material = onHoverColor;
    }

   public void NotHover()
    {
        GetComponent<Renderer>().material = normalColor;
    }

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        //GetComponent<Renderer>().material.color = Color.green;
    }
}
