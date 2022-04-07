using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    public void OnSceneGUI()
    {
        FieldOfView FOV = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(FOV.transform.position, Vector3.forward, Vector3.up, 360, FOV.GetViewRadius());
        Vector3 viewAngleA = FOV.DirFromAngle(-FOV.GetViewAngle() / 2, false);
        Vector3 viewAngleB = FOV.DirFromAngle(FOV.GetViewAngle() / 2, false);

        Handles.DrawLine(FOV.transform.position, FOV.transform.position + (viewAngleA * FOV.GetViewRadius()) * FOV.GetDirection());
        Handles.DrawLine(FOV.transform.position, FOV.transform.position + (viewAngleB * FOV.GetViewRadius()) * FOV.GetDirection());
    }
}
