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
    [BoxGroup("In_World")] public GameObject p_ScreenCaptureMgr;
    
    [BoxGroup("In_Canvas")] public GameObject p_Canvas_RageGauge;
    [field: SerializeField, BoxGroup("In_Canvas")] private GameObject p_Player_UI;
    [field: SerializeField, BoxGroup("In_Canvas")] private GameObject p_BulletTime_AR;
    
    [field: SerializeField, BoxGroup("In_Cam")] private GameObject p_ScreenCaptureCanvas;
    
    public GameObject[] m_ShouldBeMadeInWorld;
    public GameObject[] m_ShouldBeMadeInCanvas;
    
    
    // Member Variables
    public BulletTime_AR m_BulletTime_AR { get; private set; }
    public Player_UI m_Player_UI { get; private set; }
    public LeftBullet_WUI m_LeftBullet_WUI { get; private set; }
    public ScreenCaptureEffectMgr m_ScreenCaptureMgr { get; private set; }
    public AR_ScreenCapture m_ScreenCaptureCanvas { get; private set; }
    public RageGauge m_RageGauge { get; private set; }


    // Instance
    private static InstanceMgr Instance;
    public static InstanceMgr GetInstance() { return Instance; }

    
    // Constructors
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

        m_RageGauge = Instantiate(p_Canvas_RageGauge, m_MainCanvas.transform).GetComponent<RageGauge>();
        m_Player_UI = Instantiate(p_Player_UI, m_MainCanvas.transform).GetComponent<Player_UI>();
        
        
        m_BulletTime_AR = 
            Instantiate(p_BulletTime_AR, m_MainCanvas.transform).GetComponent<BulletTime_AR>();
        m_BulletTime_AR.transform.localPosition = Vector2.zero;
    }

    private void SpawnInWorld()
    {
        for (int i = 0; i < m_ShouldBeMadeInWorld.Length; i++)
        {
            Instantiate(m_ShouldBeMadeInWorld[i], this.gameObject.transform);
        }
        Instantiate(p_AimCursor, this.gameObject.transform);
        Instantiate(p_BulletTimeMgr, this.gameObject.transform);

        m_ScreenCaptureMgr =
            Instantiate(p_ScreenCaptureMgr, this.gameObject.transform).GetComponent<ScreenCaptureEffectMgr>();
    }

    private void SpawnInCam()
    {
        Transform camTransform = m_MainCam.transform;

        m_ScreenCaptureCanvas =
            Instantiate(p_ScreenCaptureCanvas, camTransform).GetComponent<AR_ScreenCapture>();
        m_ScreenCaptureCanvas.transform.localPosition = Vector2.zero;
    }
}
