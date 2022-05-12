using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightMgr))]

public class CS_LightMgr : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LightMgr manipulator = (LightMgr)target;
        if (GUILayout.Button("설정값 적용하기"))
        {
            manipulator.setLightValues();
            manipulator.setParallaxValues();
        }

    }
}
