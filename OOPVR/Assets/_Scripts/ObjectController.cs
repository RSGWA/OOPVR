using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public Transform Holder;  //Holder for holding the object
    public Rigidbody objct; //Object to be picked up and dropped

    public GameObject AllVariables;
    private Vector3 objScale;
    private Transform originalParent;

    void Start()
    {
        
        objScale = objct.transform.localScale;
    }

    public void InHands()
    {
        objct.transform.parent = Holder.transform.parent;
        objct.transform.position = Holder.transform.position;

        AllVariables.SetActive(true);
    }

    public void OnTheShelf()
    {
        objct.transform.parent = Holder.transform.parent;
        objct.transform.position = Holder.transform.position;
        objct.transform.rotation = Holder.transform.rotation;

        objct.transform.localScale = Holder.transform.localScale;

        AllVariables.SetActive(false);
    }
}
