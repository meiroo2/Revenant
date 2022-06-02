using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_ValueManipulator))]

public class CS_PlayerValueManipulator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player_ValueManipulator manipulator = (Player_ValueManipulator)target;
        if (GUILayout.Button("설정값 적용하기"))
        {
            manipulator.SetPlayerValues();
        }
    }
}