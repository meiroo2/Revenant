using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DefenseEnemy_Status))]
public class DefenseEnemy_Editor : Editor
{
    //[Header("             [ 체력 ]")]
    //[SerializeField]
    //int m_HP = 10;

    //[Header("             [ 색상 ]")]
    //[ColorUsage(true)]
    //[SerializeField]
    //Color m_colorPalete;


    //public DefenseEnemy selected;

    //private void OnEnable()
    //{
    //    if(AssetDatabase.Contains(target))
    //    {
    //        selected = null;
    //    }
    //    else
    //    {
    //        selected = (DefenseEnemy_Status)target;
    //    }
    //}

    //public override void OnInspectorGUI()
    //{
    //    if (selected == null)
    //        return;

    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();
    //    EditorGUILayout.LabelField("**************************");
    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();

    //    Color tempColor = Color.cyan;

    //    GUI.color = tempColor;
    //    selected.m_preFightTime = EditorGUILayout.FloatField("전투 딜레이", selected.m_Hp);




    //    base.OnInspectorGUI();
    //}






}
