using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurve : MonoBehaviour
{
    public GameObject Object;

    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;
    public Vector3 P4;
    [Range(0f, 1f)]
    public float Value;

    private void Update()
    {
        Object.transform.position = BezierCurveFunc(P1, P2, P3, P4, Value);
    }

    public Vector3 BezierCurveFunc(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 A = Vector3.Lerp(p1, p2, value);
        Vector3 B = Vector3.Lerp(p2, p3, value);
        Vector3 C = Vector3.Lerp(p3, p4, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);

        return F;
    }
}
//[CanEditMultipleObjects]
//[CustomEditor(typeof(BezierCurve))]
//public class BezierCurveEditor : Editor
//{
//    private void OnSceneGUI()
//    {
//        BezierCurve Generator = (BezierCurve)target;

//        Generator.P1 = Handles.PositionHandle(Generator.P1, Quaternion.identity);
//        Generator.P2 = Handles.PositionHandle(Generator.P2, Quaternion.identity);
//        Generator.P3 = Handles.PositionHandle(Generator.P3, Quaternion.identity);
//        Generator.P4 = Handles.PositionHandle(Generator.P4, Quaternion.identity);

//        Handles.DrawLine(Generator.P1, Generator.P2);
//        Handles.DrawLine(Generator.P3, Generator.P4);

//        int Count = 50;
//        for (float i = 0; i < Count; i++)
//        {
//            float value_Before = i / Count;
//            Vector3 Before = Generator.BezierCurveFunc(Generator.P1, Generator.P2, Generator.P3, Generator.P4, value_Before);

//            float value_After = (i + 1) / Count;
//            Vector3 After = Generator.BezierCurveFunc(Generator.P1, Generator.P2, Generator.P3, Generator.P4, value_After);

//            Handles.DrawLine(Before, After);
//        }
//    }
//}
