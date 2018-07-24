using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanceController : MonoBehaviour
{

    private PlayerController player;

    private GameObject[] movePoints;
    private Animator anim;

    bool instanceCreated = false;
    bool instanceLowered = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        movePoints = GameObject.FindGameObjectsWithTag("Move"); //this also need changes for multiple instances
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

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
}