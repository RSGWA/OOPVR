using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveLine : MonoBehaviour
{

    Transform spline, addressBox, variableBox;
    Transform point1, point2, point3;
    Transform insBox, curve, addressValue;
    VariableBoxController instanceContainer;

    public float curveHeight = 1;
    private bool ready = false;

    void Awake()
    {

        spline = transform.parent.Find("BezierSpline");
        addressBox = transform;




    }
    // Update is called once per frame
    void Update()
    {
        if( curve != null && addressValue != null)
        {
            UpdatePoints();

        }
        else
        {

        }
    }


    public void CreateCurveTo(Transform toTransform)
    {
        curve = Instantiate(spline, transform.position, transform.rotation, transform);
        addressValue = toTransform;
        UpdatePoints();
        HideCurve();

    }

    void UpdatePoints()
    {
        point1 = curve.Find("Point1");
        point2 = curve.Find("Point2");
        point3 = curve.Find("Point3");


        point1.position = addressBox.position;
        point3.position = addressValue.position;

        Vector3 pos = Vector3.Lerp(point1.position, point3.position, 0.5f);
        point2.position = new Vector3(pos.x, pos.y + curveHeight, pos.z);
        curve.GetComponent<BezierSpline>().ConstructLinearPath();
        curve.GetComponent<BezierSpline>().AutoConstructSpline();
    }


    public void HideCurve()
    {
        curve.localScale = new Vector3(0, 0, 0);
    }

    public void ShowCurve()
    {
        curve.localScale = new Vector3(1, 1, 1);
        //curve.GetComponent<BezierSpline>().ConstructLinearPath();
        //curve.GetComponent<BezierSpline>().AutoConstructSpline2();
    }
}
