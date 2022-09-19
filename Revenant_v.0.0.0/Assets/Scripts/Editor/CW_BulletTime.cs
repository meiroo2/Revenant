using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class CW_BulletTime : OdinEditorWindow
{
    // Const Variables
    private const float m_LabelWidth = 170f;
    
    // Constructors
    [MenuItem(("������/�Ҹ�Ÿ��"))]
    private static void OpenWindow()
    {
        GetWindow<CW_BulletTime>().Show();
        LoadBulletTimeData();
    }

    [TabGroup("BulletTime"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float B_BulletTimeLimit;

    [TabGroup("BulletTime"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float B_ShotDelayTime;
    
    // Functions
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("BulletTime")]
    private static void BulletTime_�����ϱ�()
    {
        var bulletTimeMgr = GameObject.FindGameObjectWithTag("InstanceMgr").GetComponent<InstanceMgr>().
            p_BulletTimeMgr.GetComponent<BulletTimeMgr>();

        bulletTimeMgr.p_BulletTimeLimit = B_BulletTimeLimit;
        bulletTimeMgr.p_ShotDelayTime = B_ShotDelayTime;
    }
    

    private static void LoadBulletTimeData()
    {
        var bulletTimeMgr = GameObject.FindGameObjectWithTag("InstanceMgr").GetComponent<InstanceMgr>().
            p_BulletTimeMgr.GetComponent<BulletTimeMgr>();

        B_BulletTimeLimit = bulletTimeMgr.p_BulletTimeLimit;
        B_ShotDelayTime = bulletTimeMgr.p_ShotDelayTime;
    }
}
