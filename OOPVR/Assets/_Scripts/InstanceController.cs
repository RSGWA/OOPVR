using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class defines the animations and set up when an instance is instatiated
 * 
 **/

public class InstanceController : MonoBehaviour
{

    private PlayerController player;

    private List<Transform> movePoints;
    private List<Transform> methods;
    private Animator anim;
    private bool completedInstantiation;

    bool instanceCreated = false;
    bool instanceLowered = false;

    static string INSTANCE_METHODS = "instance_methods";
    static string CONSTRUCTOR_METHODS = "constructors";

    private void Awake()
    {
        movePoints = new List<Transform>();
        SetMovePoints();
        
    }
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        
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
            CreateInstanceByDefault();
        }
    }

    public void createInstance()
    {
        this.gameObject.SetActive(true);
        EnableMovePositions(false);
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
        EnableMovePositions(true);
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

    void SetMovePoints()
    {
        int children = transform.childCount;
        string child;
        for (int i = 0; i < children; i++)
        {
            child = transform.GetChild(i).name;

            if (child == "DefaultConstructor" || child == "Constructor" || child == "SetName" || child == "GetName" || child == "IncrementAge")
            {
                child = child + "/MovePoint";
                movePoints.Add(transform.Find(child));
            }
        }
    }

    public void EnableMovePositions(bool key)
    {

        foreach (Transform mPoint in movePoints)
        {
            mPoint.GetComponent<TeleportMovePoint>().ShowMovePoint(key);
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

    void CreateInstanceByDefault()
    {
       
        string activityName = SceneManager.GetActiveScene().name;

        if (activityName == "SetNameActivity" || activityName == "GetNameActivity" || activityName == "IncrementAgeActivity" || activityName == "MultiInstancesMethodCallsActivity")
        {
            if (activityName != "SetNameActivity" && activityName != "MultiInstancesMethodCallsActivity")
            {
                transform.Find("SetName/ParametersPlatform/NameParameter/NameParameterBox").GetComponent<InteractiveItemGaze>().enabled = false;
            }
           
        }
       
    }




}