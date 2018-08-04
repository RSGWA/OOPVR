using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInstances : MonoBehaviour
{
    public Transform[] Instance1Values, Instance2Values;

    private List<Vector3> initialScales = new List<Vector3>();

    InstanceController ic1, ic2;
    Notepad notepad;
    PlayerController player;

    bool instance1Created = false, instance2Created = false;

    List<string> objectives = new List<string>();

    VariableBoxController ageBox1, nameBox1, nameParameterBox1, ageParameterBox1, nameParameterBox2, ageParameterBox2, ageBox2, nameBox2, instanceBox1, instanceBox2;
    GameObject instance1ConstructorRoom, instance2ConstructorRoom , instance1, instance2;
    GameObject i1constructorMovePoint, i2constructorMovePoint;
    GameObject blueprint, mainMovePos;
    AddressBoxController address1, address2;
    Vector3 insConScale;
    

    void Awake()
    {
        blueprint = GameObject.FindGameObjectWithTag("Blueprint");

        notepad = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Notepad>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        mainMovePos = GameObject.Find("MainMovePoint");

        instance1 = GameObject.FindGameObjectWithTag("Instance1");
        instance2 = GameObject.FindGameObjectWithTag("Instance2");

        instance1ConstructorRoom = instance1.transform.Find("Constructor").gameObject;
        instance2ConstructorRoom = instance2.transform.Find("Constructor").gameObject;

        i1constructorMovePoint = instance1ConstructorRoom.transform.Find("MovePoint").gameObject;
        i2constructorMovePoint = instance2ConstructorRoom.transform.Find("MovePoint").gameObject;

        ic1 = instance1.GetComponent<InstanceController>();
        ic2 = instance2.GetComponent<InstanceController>();

        instanceBox1 = GameObject.Find("InstanceContainer1").GetComponent<VariableBoxController>();
        instanceBox2 = GameObject.Find("InstanceContainer2").GetComponent<VariableBoxController>();
        

        ageBox1 = instance1.transform.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        nameBox1 = instance1.transform.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        ageBox2 = instance2.transform.Find("Age_InstanceBox").GetComponent<VariableBoxController>();
        nameBox2 = instance2.transform.Find("Name_InstanceBox").GetComponent<VariableBoxController>();

        Transform parameterPlatform1 = instance1ConstructorRoom.transform.Find("ParametersPlatform");
        Transform parameterPlatform2 = instance2ConstructorRoom.transform.Find("ParametersPlatform");

        nameParameterBox1 = parameterPlatform1.Find("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
        ageParameterBox1 = parameterPlatform1.Find("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();

        nameParameterBox2 = parameterPlatform2.Find("NameParameter/NameParameterBox").GetComponent<VariableBoxController>();
        ageParameterBox2 = parameterPlatform2.Find("AgeParameter/AgeParameterBox").GetComponent<VariableBoxController>();

        address1 = GameObject.Find("Plot of Land 1/Mailbox").GetComponent<AddressBoxController>();
        address2 = GameObject.Find("Plot of Land 2/Mailbox").GetComponent<AddressBoxController>();

        insConScale = instanceBox1.transform.localScale;
        instanceBox1.transform.localScale = new Vector3(0, 0, 0);
        instanceBox2.transform.localScale = new Vector3(0, 0, 0); 

        setUpScales(Instance1Values);
        setUpScales(Instance2Values);

        objectives.Add("new Person");
        objectives.Add("\"Peter\"");
        objectives.Add("15");
        objectives.Add("this->name = name;");
        objectives.Add("this->age = age;");
        objectives.Add("}");
        objectives.Add("Person *p1");

        objectives.Add("new Person");
        objectives.Add("\"Peter\"");
        objectives.Add("15");
        objectives.Add("this->name = name;");
        objectives.Add("this->age = age;");
        objectives.Add("}");
        objectives.Add("Person *p2");

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
        blueprint.SetActive(false); //hide blueprint to force player to work on first instance
        player.moveTo(i1constructorMovePoint);  //automatically move player to constructor of instance
       
        notepad.blinkObjective(objectives[1]);
        StartCoroutine("checkPlayerOnInstance");
    }

    IEnumerator checkPlayerOnInstance()
    {
        while (!player.checkPlayerPos(i1constructorMovePoint.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        showInstanceValues(Instance1Values, true);
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
        //enableMovePoints(false);

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
        instance1.transform.Find("Constructor/Door/DoorExt").GetComponent<Door>().enableDoor();
        StartCoroutine("checkInstance1Return");
    }

    IEnumerator checkInstance1Return()
    {
        while (!outsideOfMethod(1))
        {
            yield return new WaitForSeconds(0.1f);
        }

        address1.ToHands();
   
        notepad.setActiveText(0);
        notepad.setTitle("MAIN");
        notepad.blinkObjective(objectives[6]);
        //notepad.blinkDuplicateObjective(objectives[6], 2);
        StartCoroutine("checkPlayerInMain");
    }

    IEnumerator checkPlayerInMain()
    {
        yield return new WaitForSeconds(1.9f);
        ic1.SetInstanceCompletion(true);

        player.moveTo(mainMovePos);

        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }

        instanceBox1.transform.localScale = insConScale;
        StartCoroutine("checkInstance1Container");
    }

    IEnumerator checkInstance1Container()
    {
        while (!instanceBox1.isVarInBox())
        {
            yield return new WaitForSeconds(0.1f);
        }
        blueprint.SetActive(true);
        notepad.blinkDuplicateObjective(objectives[7], 2);
        StartCoroutine("checkInstance2Created");

    }

    IEnumerator checkInstance2Created()
    {
        while (!instance2Created)
        {
            instance2Created = ic2.hasInstanceBeenCreated();
            yield return new WaitForSeconds(0.1f);
        }
        //automatically move player to constructor of instance
        player.moveTo(i2constructorMovePoint);
        StartCoroutine("checkPlayerOnInstance2");
    }

    IEnumerator checkPlayerOnInstance2()
    {
        while (!player.checkPlayerPos(i2constructorMovePoint.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        showInstanceValues(Instance2Values, true);
        notepad.blinkDuplicateObjective(objectives[8], 2);
        StartCoroutine("checkNameParameterSet2");
    }

    IEnumerator checkNameParameterSet2()
    {
        while (!nameParameterSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
		notepad.blinkDuplicateObjective(objectives[9], 2);
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
        notepad.blinkObjective(objectives[10]);
        StartCoroutine("checkNameSet2");
    }

    IEnumerator checkNameSet2()
    {
        while (!nameSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[11]);
        StartCoroutine("checkAgeSet2");
    }

    IEnumerator checkAgeSet2()
    {
        while (!ageSet(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        notepad.blinkObjective(objectives[12]);
        instance2.transform.Find("Constructor/Door/DoorExt").GetComponent<Door>().enableDoor();
        StartCoroutine("checkReturn");
    }

    IEnumerator checkReturn()
    {
        while (!outsideOfMethod(2))
        {
            yield return new WaitForSeconds(0.1f);
        }
        address2.ToHands();

        ic2.SetInstanceCompletion(true);

        player.moveTo(mainMovePos);
        notepad.setActiveText(0);
        notepad.setTitle("MAIN");
        notepad.blinkObjective(objectives[13]);
        StartCoroutine("checkPlayerBackInMain");

    }
    IEnumerator checkPlayerBackInMain()
    {
        while (!player.checkPlayerPos(mainMovePos.transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        instanceBox2.transform.localScale = insConScale;
        StartCoroutine("checkInstance2Container");
    }

    IEnumerator checkInstance2Container()
    {
        while (!instanceBox2.isVarInBox())
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Activity Finished
        PlayerPrefs.SetInt("MultipleInstances", 1);
        PlayerPrefs.Save();
        notepad.endOfActivity();
    }


    bool outsideOfMethod(int num)
    {
        Transform roomMovePoint;
        if (num == 1)
        {
            roomMovePoint = instance1ConstructorRoom.transform.Find("MovePoint");
            return (player.checkPlayerPos(roomMovePoint.position));
        }
        else
        {
            roomMovePoint = instance2ConstructorRoom.transform.Find("MovePoint");
            return (player.checkPlayerPos(roomMovePoint.position));
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

    void setUpScales(Transform[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            initialScales.Add(values[i].localScale);
            values[i].localScale = new Vector3(0, 0, 0);
        }
    }

    void showInstanceValues(Transform[] instanceValues , bool key)
    {

        if (key)
        {
            for (int i = 0; i < instanceValues.Length; i++)
            {
                instanceValues[i].localScale = initialScales[i];
            }
        }
        else
        {
            for (int i = 0; i < instanceValues.Length; i++)
            {
                instanceValues[i].localScale = new Vector3(0, 0, 0);
            }
        }

    }
}
