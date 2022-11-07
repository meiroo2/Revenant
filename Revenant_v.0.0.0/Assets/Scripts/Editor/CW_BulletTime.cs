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
    [MenuItem(("에디터/불릿타임"))]
    private static void OpenWindow()
    {
        GetWindow<CW_BulletTime>().Show();
        LoadBulletTimeData();
    }

    
    #region BulletTimeMgr Variables

    [TabGroup("BulletTime"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float B_BulletTimeLimit;

    [TabGroup("BulletTime"), ShowInInspector, TableList, LabelWidth(m_LabelWidth)]
    private static float B_ShotDelayTime;

    #endregion

    
    // Functions
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("BulletTime")]
    private static void BulletTime_적용하기()
    {
        var bulletTimeMgr = Resources.Load<BulletTimeMgr>("Logic/BulletTimeMgr");

        bulletTimeMgr.p_BulletTimeLimit = B_BulletTimeLimit;
        bulletTimeMgr.p_ShotDelayTime = B_ShotDelayTime;
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(bulletTimeMgr);
        #endif
    }
    

    private static void LoadBulletTimeData()
    {
        var bulletTimeMgr = Resources.Load<BulletTimeMgr>("Logic/BulletTimeMgr");

        B_BulletTimeLimit = bulletTimeMgr.p_BulletTimeLimit;
        B_ShotDelayTime = bulletTimeMgr.p_ShotDelayTime;
    }
}
