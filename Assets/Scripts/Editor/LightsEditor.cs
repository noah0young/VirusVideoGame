using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightPlatform))]
public class LightsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LightPlatform light = (LightPlatform)target;

        if (GUILayout.Button("Toggle Light"))
        {
            light.toggleLight();
        }
    }
}
