using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class CW_Useful : OdinEditorWindow
{
    private const float m_LabelWidth = 150f;
    
    [TabGroup("Analysis"), ShowInInspector, DisplayAsString, LabelWidth(m_LabelWidth)]
    public static int Num_of_NormalGang = 0;
    [TabGroup("Analysis"), ShowInInspector, DisplayAsString, LabelWidth(m_LabelWidth)]
    public static int Num_of_MeleeGang = 0;
    [TabGroup("Analysis"), ShowInInspector, DisplayAsString, LabelWidth(m_LabelWidth)]
    public static int Num_of_DroneGang = 0;
    [TabGroup("Analysis"), ShowInInspector, DisplayAsString, LabelWidth(m_LabelWidth)]
    public static int Num_of_ShieldGang = 0;
    
    [MenuItem(("에디터/편의기능"))]
    private static void OpenWindow()
    {
        GetWindow<CW_Useful>().Show();
        Analyze();
    }
    
    [PropertySpace(20f), Button(ButtonSizes.Large), TabGroup("Analysis")]
    private static void 새로고침()
    {
        Analyze();
    }
    
    [PropertySpace(20f), Button(ButtonSizes.Large)]
    private static void 플레이어_땅에_붙이기()
    {
        int m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
        Transform playerTransform = GameObject.FindGameObjectWithTag("@Player").transform;
        
        RaycastHit2D m_FootHit = Physics2D.Raycast(playerTransform.position, -playerTransform.up, 
            1f, m_LayerMask);

        float posY = MathF.Round(m_FootHit.point.y + 0.64f, 2);
        playerTransform.position = new Vector2(m_FootHit.point.x, posY);
    }
    
    [PropertySpace(20f), Button(ButtonSizes.Large)]
    private static void 보스_땅에_붙이기()
    {
        int m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
        Transform playerTransform = GameObject.FindObjectOfType<BossGang>().transform;
        
        RaycastHit2D m_FootHit = Physics2D.Raycast(playerTransform.position, -playerTransform.up, 
            1f, m_LayerMask);

        float posY = MathF.Round(m_FootHit.point.y, 2);
        playerTransform.position = new Vector2(m_FootHit.point.x, posY);
    }

    private static void Analyze()
    {
        Num_of_NormalGang = GameObject.FindObjectsOfType<NormalGang>().Length;
        Num_of_MeleeGang = GameObject.FindObjectsOfType<MeleeGang>().Length;
        Num_of_DroneGang = GameObject.FindObjectsOfType<Drone>().Length;
        Num_of_ShieldGang = GameObject.FindObjectsOfType<ShieldGang>().Length;
    }
}
