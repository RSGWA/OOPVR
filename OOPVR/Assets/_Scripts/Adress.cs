using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSolution
{


    public class Adress : MonoBehaviour
    {
        public Transform[] instanceBoxes;

        Transform spline, addressBox, variableBox;
        Transform point1, point2, point3;
        Transform insBox, line;
        BezierSpline bs;
        List<Transform> bsList = new List<Transform>();
        VariableBoxController instanceContainer;

        public float curveHeight = 1;
        private bool ready = false;

        void Awake()
        {

            spline = transform.Find("BezierSpline");
            addressBox = transform.Find("AddressBox");
            bs = spline.GetComponent<BezierSpline>();

            SetInstanceBoxes();
            
        }

        // Update is called once per frame
        void Update()
        {
            Check();
            
        }

        void Check()
        {
            if(instanceBoxes.Length == 1)
            {
                instanceContainer = instanceBoxes[0].GetComponent<VariableBoxController>();
                if (!instanceContainer.isSelected() & instanceContainer.isVarInBox())
                {
                    CreateCurve(instanceContainer.transform, bsList[0]);
                    ShowSpline(bsList[0]);
                }
                else
                {
                    HideSpline(bsList[0]);
                }
            }
            else
            {
                for (int i = 0; i < instanceBoxes.Length; i++)
                {
                    instanceContainer = instanceBoxes[i].GetComponent<VariableBoxController>();
                    if (!instanceContainer.isSelected() & instanceContainer.isVarInBox())
                    {
                        CreateCurve(instanceContainer.transform, bsList[i]);
                        ShowSpline(bsList[i]);
                    }
                    else
                    {
                        HideSpline(bsList[i]);
                    }
                    
                }
            }

        }
        void SetInstanceBoxes()
        {
            for(int i=0; i< instanceBoxes.Length; i++)
            {
                insBox = instanceBoxes[i];
                SetupPoints(insBox);

            }
        }

        void SetupPoints(Transform instanceBox)
        {
            line = Instantiate(spline, transform.position, transform.rotation, transform);
            bsList.Add(line);

            point1 = line.Find("Point1");
            point2 = line.Find("Point2");
            point3 = line.Find("Point3");
           

            point1.position = addressBox.position;
            point3.position = instanceBox.position;

            Vector3 pos = Vector3.Lerp(point1.position, point3.position, 0.5f);
            point2.position = new Vector3(pos.x, pos.y + curveHeight, pos.z);
        }

        void CreateCurve(Transform instanceBox, Transform line)
        {
            point1 = line.Find("Point1");
            point2 = line.Find("Point2");
            point3 = line.Find("Point3");


            point1.position = addressBox.position;
            point3.position = instanceBox.position;

            Vector3 pos = Vector3.Lerp(point1.position, point3.position, 0.5f);
            point2.position = new Vector3(pos.x, pos.y + curveHeight, pos.z);

        }

        void HideSpline(Transform curve)
        {
            curve.localScale = new Vector3(0, 0, 0);
        }

        void ShowSpline(Transform curve)
        {
            curve.localScale = new Vector3(1, 1, 1);
            curve.GetComponent<BezierSpline>().ConstructLinearPath();
            curve.GetComponent<BezierSpline>().AutoConstructSpline();
        }
    }
}