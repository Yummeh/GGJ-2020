using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Boundary), true)]
public class BoundaryEditor : Editor
{
    SerializedProperty showGraphicProp;
    SerializedProperty lineCheckBetweenPoints;

    protected virtual void InitFields()
    {
        showGraphicProp = serializedObject.FindProperty("showGraphic");

        lineCheckBetweenPoints = serializedObject.FindProperty("checkIfBetweenPoints");
    }

    public void OnEnable()
    {
        InitFields();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawFields();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawFields()
    {
        EditorGUI.BeginChangeCheck();

        showGraphicProp.boolValue = EditorGUILayout.Toggle("Show Graphics", showGraphicProp.boolValue);

        DrawPoints();

        if (EditorGUI.EndChangeCheck())
        {

        }
    }

    protected virtual void DrawPoints()
    {
        for (int i = 0; i < lineCheckBetweenPoints.arraySize; i++)
        {   
            SerializedProperty point = lineCheckBetweenPoints.GetArrayElementAtIndex(i);
            if(point != null)
                point.boolValue = EditorGUILayout.Toggle("point:" + (i + 1), point.boolValue);
        }
    }
}

