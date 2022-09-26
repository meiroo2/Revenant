using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InstanceMgr : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_MainCanvas;

    [BoxGroup("In_Canvas")] public GameObject p_Canvas_RageGauge;

    [BoxGroup("In_World")] public GameObject p_AimCursor;
    [BoxGroup("In_World")] public GameObject p_BulletTimeMgr;
    [BoxGroup("In_World")] public GameObject p_MatChanger;
    
    public GameObject[] m_ShouldBeMadeInWorld;
    public GameObject[] m_ShouldBeMadeInCanvas;

    private static InstanceMgr Instance;
    public static InstanceMgr GetInstance() { return Instance; }

    private void Awake()
    {
        m_MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Instance = this;
        
        SpawnInWorld();

        for (int i = 0; i < m_ShouldBeMadeInCanvas.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInCanvas[i], m_MainCanvas.transform);
        }
        Instantiate(p_Canvas_RageGauge, m_MainCanvas.transform);
    }

    private void SpawnInWorld()
    {
        for (int i = 0; i < m_ShouldBeMadeInWorld.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInWorld[i], this.gameObject.transform);
        }
        Instantiate(p_AimCursor, this.gameObject.transform);
        Instantiate(p_BulletTimeMgr, this.gameObject.transform);
        Instantiate(p_MatChanger, gameObject.transform);
    }
}
