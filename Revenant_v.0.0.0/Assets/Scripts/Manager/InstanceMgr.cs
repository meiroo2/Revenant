using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMgr : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_MainCanvas;

    public GameObject p_Canvas_RageGauge;
    public GameObject[] m_ShouldBeMadeInWorld;
    public GameObject[] m_ShouldBeMadeInCanvas;

    private static InstanceMgr Instance;
    public static InstanceMgr GetInstance() { return Instance; }

    private void Awake()
    {
        m_MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        
        Instance = this;
        for (int i = 0; i < m_ShouldBeMadeInWorld.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInWorld[i], this.gameObject.transform);
        }

        Instantiate(p_Canvas_RageGauge, m_MainCanvas.transform);

        for (int i = 0; i < m_ShouldBeMadeInCanvas.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInCanvas[i], m_MainCanvas.transform);
        }
    }
}
