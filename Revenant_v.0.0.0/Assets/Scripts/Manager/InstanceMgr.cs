using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InstanceMgr : MonoBehaviour
{
    // Visible Member Variables
    [HideInInspector] public GameObject m_MainCanvas;
    [HideInInspector] public GameObject m_MainCam;
    
    [BoxGroup("In_World")] public GameObject p_AimCursor;
    [BoxGroup("In_World")] public GameObject p_BulletTimeMgr;
    [BoxGroup("In_World")] public GameObject p_MatChanger;
    
    [BoxGroup("In_Canvas")] public GameObject p_Canvas_RageGauge;

    [field: SerializeField, BoxGroup("In_Cam")] private GameObject p_ScreenEffect_AR;
    
    public GameObject[] m_ShouldBeMadeInWorld;
    public GameObject[] m_ShouldBeMadeInCanvas;
    
    // Member Variables
    public ScreenEffect_AR m_ScreenEffect_AR { get; private set; }

    private static InstanceMgr Instance;
    public static InstanceMgr GetInstance() { return Instance; }

    private void Awake()
    {
        m_MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        m_MainCam = Camera.main.gameObject;
        Instance = this;
        
        SpawnInWorld();
        SpawnInCam();

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

    private void SpawnInCam()
    {
        m_ScreenEffect_AR = 
            Instantiate(p_ScreenEffect_AR, m_MainCam.transform).GetComponent<ScreenEffect_AR>();

    }
}
