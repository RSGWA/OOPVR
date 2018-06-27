using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInstances : MonoBehaviour {

    InstanceController instance1, instance2;
    Notepad notepad;
    PlayerController player;

    //bool instanceCreated = false;
    //bool constructorEntered = false;
    //bool returned = false;

    List<string> objectives = new List<string>();

    VariableBoxController ageBox1, nameBox1, ageBox2, nameBox2;

    void Awake()
    {
        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        instance1 = GameObject.FindGameObjectWithTag("Instance1").GetComponent<InstanceController>();
        instance2 = GameObject.FindGameObjectWithTag("Instance2").GetComponent<InstanceController>();

        ageBox1 = GameObject.Find("Age_InstanceBox1").GetComponent<VariableBoxController>();
        nameBox1 = GameObject.Find("Name_InstanceBox1").GetComponent<VariableBoxController>();

        ageBox2 = GameObject.Find("Age_InstanceBox2").GetComponent<VariableBoxController>();
        nameBox2 = GameObject.Find("Name_InstanceBox2").GetComponent<VariableBoxController>();

        objectives.Add("new Person");
        objectives.Add("\"Peter\"");
        objectives.Add("15");
        objectives.Add("this->name = name;");
        objectives.Add("this->age = age;");
        objectives.Add("}");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
