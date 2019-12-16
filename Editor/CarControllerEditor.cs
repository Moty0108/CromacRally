using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CarControllerEditor : Editor
{
    CarController carcontorller;

    private void OnEnable()
    {
        carcontorller = (CarController)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
    }
}
