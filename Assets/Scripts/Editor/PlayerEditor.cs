using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player player = (Player)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Gain Purple Encryption Key"))
        {
            player.getPurpleEncryptionKey();
        }

        if (GUILayout.Button("Gain Green Encryption Key"))
        {
            player.getGreenEncryptionKey();
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Gain Double Jump"))
        {
            player.getDoubleJump();
        }
    }
}
