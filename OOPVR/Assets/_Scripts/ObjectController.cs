using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public Transform Holder;  //Holder for holding the object
    public Rigidbody objct; //Object to be picked up and dropped

    public GameObject AllVariables;
    private Vector3 objScale;

    void Start()
    {
        objScale = objct.transform.localScale;
    }

    public void InHands()
    {
        objct.transform.parent = Holder.transform;
        objct.transform.position = Holder.transform.position;

        AllVariables.SetActive(true);
    }

    public void OnTheShelf()
    {
        //objct.transform.parent = null;
        objct.transform.parent = Holder.transform;
        objct.transform.localPosition = Holder.transform.localPosition;
        objct.transform.rotation = Holder.transform.rotation;

        objct.transform.localScale = objScale;

        AllVariables.SetActive(false);
    }
}
