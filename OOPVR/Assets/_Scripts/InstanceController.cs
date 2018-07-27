using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanceController : MonoBehaviour
{

    private PlayerController player;

    private GameObject[] movePoints;
    private List<Transform> methods;
    private Animator anim;
    private bool completedInstantiation;

    bool instanceCreated = false;
    bool instanceLowered = false;

    static string INSTANCE_METHODS = "instance_methods";
    static string CONSTRUCTOR_METHODS = "constructors";

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        movePoints = GameObject.FindGameObjectsWithTag("Move"); //this also need changes for multiple instances
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        completedInstantiation = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(completedInstantiation)
        {
            enableMethods(CONSTRUCTOR_METHODS, false);
            enableMethods(INSTANCE_METHODS, true);
        }

        if(instanceCreatedByDefault())
        {
            enableMethods(CONSTRUCTOR_METHODS, false);
            enableMethods(INSTANCE_METHODS, true);
        }
    }

    public void createInstance()
    {
        this.gameObject.SetActive(true);
        movePointVisible(false);
        InstanceControl("Create");
        InstanceControl("Lower");
        StartCoroutine("checkInstanceCreated");
        StartCoroutine("returnBlueprint");
        StartCoroutine("checkInstanceLowered");
    }

    // Checks if instance has finished being created so blueprint can be returned
    // to its original position
    IEnumerator checkInstanceCreated()
    {
        instanceCreated = false;

        while (!instanceCreated)
        {
            yield return null;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("InstanceCreated"))
            {
                instanceCreated = true;
            }
        }
    }

    IEnumerator checkInstanceLowered()
    {
        instanceLowered = false;

        while (!instanceLowered)
        {
            yield return null;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("InstanceCreatedIdle"))
            {
                instanceLowered = true;
            }
        }

        // Instance animation completed
        movePointVisible(true);
        enableMethods(INSTANCE_METHODS, false);
    }

    IEnumerator returnBlueprint()
    {
        while (!instanceCreated)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Note: Need change, two or more blueprints can exist at one time
        GameObject.FindGameObjectWithTag("Blueprint").GetComponent<BlueprintController>().returnToOrigin();

    }

    void InstanceControl(string direction)
    {
        anim.SetTrigger(direction);
    }

    public bool hasInstanceBeenCreated()
    {
        return instanceLowered;
    }

    void movePointVisible(bool b)
    {
        //foreach (GameObject movePoint in movePoints)
        //{
        //    movePoint.GetComponent<TeleportMovePoint>().ShowMovePoint(b);
        //}
        if (!b)
        {
            player.setInRoom(true);
        }
        else
        {
            player.setInRoom(false);
        }
    }


    //Enable/Disable method given room transform
    public void enableMethodEntrance(Transform room, bool key)
    {
        room.Find("Planks").GetComponent<PlanksController>().EnablePlanks(!key);
    }

    //Enable/Disable method given "code" instructions
    public void enableMethods(string code, bool key)
    {
        if(code == "constructors")
        {
            transform.Find("DefaultConstructor/Planks").GetComponent<PlanksController>().EnablePlanks(!key);
            transform.Find("Constructor/Planks").GetComponent<PlanksController>().EnablePlanks(!key);

        }else if(code == "instance_methods")
        {
            transform.Find("SetName/Planks").GetComponent<PlanksController>().EnablePlanks(!key);
            transform.Find("GetName/Planks").GetComponent<PlanksController>().EnablePlanks(!key);
            transform.Find("IncrementAge/Planks").GetComponent<PlanksController>().EnablePlanks(!key);
        }
    }

    public void SetInstanceCompletion(bool key)
    {
        completedInstantiation = key;
    }

    bool instanceCreatedByDefault()
    {
        string activityName = SceneManager.GetActiveScene().name;

        if (activityName == "SetNameActivity" || activityName == "GetName" || activityName == "IncrementAgeActivity")
        {
            if(activityName != "SetNameActivity")
            {
                transform.Find("SetName/ParametersPlatform/NameParameter/NameParameterBox").GetComponent<InteractiveItemGaze>().enabled = false;
            }
            return true;
        }
        return false;
    }






}