using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class InspectorEventTriggerer : MonoBehaviour
{
    public UnityEvent events;
}

[CustomEditor(typeof(InspectorEventTriggerer))]
public class InspectorEventTriggererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InspectorEventTriggerer myScript = (InspectorEventTriggerer)target;
        if (GUILayout.Button("Trigger Event"))
        {
            myScript.events.Invoke();
        }
    }
}
