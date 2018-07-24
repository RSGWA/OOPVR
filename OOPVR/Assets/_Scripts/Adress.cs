using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{


    public class Adress : MonoBehaviour
    {

        Transform spline, addressBox, variableBox;
        Transform point1, point2, point3;
        BezierSpline bs;

        public float curveHeight = 1;
        private bool ready = false;

        void Awake()
        {
            
            spline = transform.Find("BezierSpline");
            point1 = spline.Find("Point1");
            point2 = spline.Find("Point2");
            point3 = spline.Find("Point3");
            addressBox = transform.Find("AddressBox");
            bs = spline.GetComponent<BezierSpline>();

            variableBox = GameObject.Find("VariableBox").transform;
            SetupPoints();
            
        }
        // Use this for initialization
        //void Start () {
        //    SetupPoints();
        //    spline = transform.Find("BezierSpline");
        //    point1 = spline.Find("Point1");
        //    point2 = spline.Find("Point2");
        //    point3 = spline.Find("Point3");
        //    addressBox = transform.Find("AddressBox");

        //    variableBox = GameObject.Find("VariableBox").transform;
        //}

        // Update is called once per frame
        void Update()
        {
            //SetupPoints();
            if (ready)
            {
                ConstructCurve();
            }
            
        }

        void SetupPoints()
        {
           
            point1.position = addressBox.position;
            point3.position = variableBox.position;

            Vector3 pos = Vector3.Lerp(point1.position,point3.position,0.5f);
            point2.position = new Vector3(pos.x, pos.y + curveHeight, pos.z);


            ready = true;
        }

        void createSpline()
        {

        }

        void ConstructCurve()
        {
            spline.GetComponent<BezierSpline>().ConstructLinearPath();
            spline.GetComponent<BezierSpline>().AutoConstructSpline();
        }
    }
}