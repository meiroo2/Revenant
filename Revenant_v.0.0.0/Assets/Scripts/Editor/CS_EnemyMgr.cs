using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


#if UNITY_EDITOR
using System.Xml;
using UnityEditor;      
[CustomEditor(typeof(EnemyMgr))]
public class CS_EnemyMgr : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyMgr manipulator = (EnemyMgr)target;
        if (GUILayout.Button("설정값 일괄 적용하기"))
        {
            Warning.Open();
        }
    }
}

public class Warning : EditorWindow
{
    [MenuItem("Window/Example")]
    public static void Open()
    {
        if (!Warning.HasOpenInstances<Warning>())
        {
            var exampleWindow = CreateInstance<Warning> ();
            exampleWindow.Show();
        }
    }
    void OnGUI()
    {
        Rect r = (Rect)EditorGUILayout.BeginVertical("Button");
        if (GUI.Button(r, GUIContent.none))
        {
            FindObjectOfType<EnemyMgr>().SetAllEnemys();
            Close();
            Debug.Log("EnemyMgr 설정값 적용됨");   
        }
        GUILayout.Label("주의해서 누르세요");
        GUILayout.Label("실수하면 바보병신ㅋㅋ");
        EditorGUILayout.EndVertical();
    }

}
#endif