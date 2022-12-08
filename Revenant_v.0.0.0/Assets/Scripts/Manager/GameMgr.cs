using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMgr : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public CoroutineHandler p_CoroutineHandler { get; private set; }
    [field: SerializeField] public Player_Manager p_PlayerMgr { get; private set; }
    [field: SerializeField] public Player_InputMgr p_PlayerInputMgr { get; private set; }
    [field: SerializeField] public PlayerManipulator p_PlayerManipulator { get; private set; }
    [field: SerializeField] public SoundPlayer p_SoundPlayer { get; private set; }
    [field: SerializeField] public DataHandleManager p_DataHandleMgr { get; private set; }
    [field: SerializeField] public SceneChangeMgr p_SceneChangeMgr { get; private set; }
    [field: SerializeField] public MatChanger p_MatChanger { get; private set; }
    [field: SerializeField] public EnemyMgr p_EnemyMgr { get; private set; }

    public float m_GameTimer { get; private set; } = 0f;


    // Member Variables

    private static GameMgr m_Instance = null;
    public static GameMgr GetInstance() { return m_Instance; }
    private bool m_isStartTimer = false;

    private int m_DeathCount = 0;
    private bool m_SceneStartWhiteOut = false;
    private bool m_IsGamePaused = false;
    [HideInInspector] public bool m_CanInputAnyKey = false;

    public int m_CurSceneIdx { get; private set; } = 0;



    // Constructor
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(this != m_Instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        m_CurSceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log("신 개시 로딩 첫번째");
        Time.timeScale = 1f;
        
        m_CurSceneIdx = SceneManager.GetActiveScene().buildIndex;
        p_SoundPlayer.BGMusicCalculate(m_CurSceneIdx);
        
        p_PlayerInputMgr.p_FireLock = false;
        p_SceneChangeMgr.CheckSmoothSceneChange();
        p_CoroutineHandler.ResetCoroutineHandler();
        p_DataHandleMgr.SetDataHandleMgr();
        p_MatChanger.InitMatChanger();
        
        p_PlayerMgr.ResetPlayer();
        p_PlayerManipulator.SetPlayer(false);
        p_PlayerManipulator.SetNegotiator(false);
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //CurSceneReload();
            p_SceneChangeMgr.InitSceneEndWithSmooth(m_CurSceneIdx, 20f);
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            p_PlayerInputMgr.SetAllInputLock(false);
            
            int sceneIdx = m_CurSceneIdx + 1;
            if (sceneIdx >= SceneManager.sceneCountInBuildSettings)
                return;
            
            RequestLoadScene(m_CurSceneIdx + 1, true);
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            p_PlayerInputMgr.SetAllInputLock(false);
            
            int sceneIdx = m_CurSceneIdx - 1;
            if (sceneIdx < 0)
                return;
            
            RequestLoadScene(m_CurSceneIdx - 1, true);
        }
    }

    // Functions
    public void RequestLoadScene(int _idx, bool _smooth = true)
    {
        if (_idx < 0 || _idx >= SceneManager.sceneCountInBuildSettings)
            return;

        if (_smooth)
        {
            p_SceneChangeMgr.InitSceneEndWithSmooth(_idx, 20f);
        }
        else
        {
            SceneManager.LoadScene(_idx);
        }
    }
    
    public void resetTimer() 
    {
        m_isStartTimer = false;
        m_GameTimer = 0f;
    }

    public void CurSceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}