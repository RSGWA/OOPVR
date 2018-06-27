using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInstances : MonoBehaviour
{

    InstanceController ic1, ic2;
    Notepad notepad;
    PlayerController player;

    bool instance1Created = false, instance2Created = false;
    //bool constructorEntered = false;
    //bool returned = false;

    List<string> objectives = new List<string>();

    VariableBoxController ageBox1, nameBox1, nameParameterBox1, ageParameterBox1, nameParameterBox2, ageParameterBox2, ageBox2, nameBox2;
    GameObject instance1ConstructorRoom, instance2ConstructorRoom , instance1, instance2;
    

    void Awake()
    {

        


        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        instance1 = GameObject.FindGameObjectWithTag("Instance1");
        instance2 = GameObject.FindGameObjectWithTag("Instance2");

        instance1ConstructorRoom = instance1.transform.Find("Constructor").gameObject;
        instance2ConstructorRoom = instance2.transform.Find("Constructor").gameObject;

        ic1 = instance1.GetComponent<InstanceController>();
        ic2 = instance2.GetComponent<InstanceController>();

        ageBox1 = GameObject.Find("Age_InstanceBox1").GetComponent<VariableBoxController>();
        nameBox1 = GameObject.Find("Name_InstanceBox1").GetComponent<VariableBoxController>();

        ageBox2 = GameObject.Find("Age_InstanceBox2").GetComponent<VariableBoxController>();
        nameBox2 = GameObject.Find("Name_InstanceBox2").GetComponent<VariableBoxController>();

        Transform parameterPlatform1 = instance1ConstructorRoom.transform.Find("ParametersPlatform");
        Transform parameterPlatform2 = instance2ConstructorRoom.transform.Find("ParametersPlatform");

        nameParameterBox1 = parameterPlatform1.Find("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
        ageParameterBox1 = parameterPlatform1.Find("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();

        nameParameterBox2 = parameterPlatform1.Find("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
        ageParameterBox2 = parameterPlatform1.Find("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();


        objectives.Add("new Person");
        objectives.Add("\"Peter\"");
        objectives.Add("15");
        objectives.Add("this->name = name;");
        objectives.Add("this->age = age;");
        objectives.Add("}");

        objectives.Add("new Person");
        objectives.Add("\"Peter\"");
        objectives.Add("15");
        objectives.Add("this->name = name;");
        objectives.Add("this->age = age;");
        objectives.Add("}");

    }

    // Use this for initialization
    void Start()
    {
        notepad.blinkObjective(objectives[0]);
        StartCoroutine("checkInstanceCreated");
    }

    IEnumerator checkInstanceCreated()
    {
        while (!instance1Created)
        {
            instance1Created = ic1.hasInstanceBeenCreated();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[1]);
        StartCoroutine("checkNameParameterSet");
    }

    IEnumerator checkNameParameterSet()
    {
        while (!nameParameterSet(1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[2]);
        StartCoroutine("checkAgeParameterSet");
    }

    IEnumerator checkAgeParameterSet()
    {
        while (!ageParameterSet(1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("CONSTRUCTOR");
        notepad.blinkObjective(objectives[3]);
        StartCoroutine("checkNameSet");
    }

    IEnumerator checkNameSet()
    {
        while (!nameSet(1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[4]);
        StartCoroutine("checkAgeSet");
    }

    IEnumerator checkAgeSet()
    {
        while (!ageSet(1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[5]);
        StartCoroutine("checkInstance1Return");
    }

    IEnumerator checkInstance1Return()
    {
        while (!outsideOfMethod(1))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(0);
        notepad.setTitle("MAIN");
        notepad.blinkObjective(objectives[6]);
        StartCoroutine("checkInstance2Created");
    }

    IEnumerator checkInstance2Created()
    {
        while (!instance2Created)
        {
            instance2Created = ic2.hasInstanceBeenCreated();
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[7]);
        StartCoroutine("checkNameParameterSet2");
    }

    IEnumerator checkNameParameterSet2()
    {
        while (!nameParameterSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[8]);
        StartCoroutine("checkAgeParameterSet2");
    }

    IEnumerator checkAgeParameterSet2()
    {
        while (!ageParameterSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.setActiveText(1);
        notepad.setTitle("CONSTRUCTOR");
        notepad.blinkObjective(objectives[9]);
        StartCoroutine("checkNameSet2");
    }

    IEnumerator checkNameSet2()
    {
        while (!nameSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[10]);
        StartCoroutine("checkAgeSet2");
    }

    IEnumerator checkAgeSet2()
    {
        while (!ageSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[11]);
        StartCoroutine("checkReturn");
    }

    IEnumerator checkReturn()
    {
        while (!outsideOfMethod(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Activity Finished
        PlayerPrefs.SetInt("ConstructorWithParametersComplete", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();

    }


    bool outsideOfMethod(int num)
    {
        Transform roomMovePoint;
        if (num == 1)
        {
            roomMovePoint = instance1ConstructorRoom.transform.Find("MovePoint");
            return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);
        }
        else
        {
            roomMovePoint = instance2ConstructorRoom.transform.Find("MovePoint");
            return (player.transform.position.x == roomMovePoint.position.x) && (player.transform.position.z == roomMovePoint.position.z);
        }

    }


    bool nameParameterSet(int num)
    {
        if (num == 1)
        {
            return nameParameterBox1.isVarInBox();
        }
        else
        {
            return nameParameterBox2.isVarInBox();
        }

    }

    bool ageParameterSet(int num)
    {
        if (num == 1)
        {
            return ageParameterBox1.isVarInBox();
        }
        else
        {
            return ageParameterBox2.isVarInBox();
        }

    }

    bool nameSet(int num)
    {
        if (num == 1)
        {
            return nameBox1.isVarInBox();
        }
        else
        {
            return nameBox2.isVarInBox();
        }
    }

    bool ageSet(int num)
    {
        if (num == 1)
        {
            return ageBox1.isVarInBox();
        }
        else
        {
            return ageBox2.isVarInBox();
        }
    }
}
