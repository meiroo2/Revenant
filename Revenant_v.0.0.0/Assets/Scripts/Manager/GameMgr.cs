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

    public float m_GameTimer { get; private set; } = 0f;


    // Member Variables
    private InGame_UI m_IngameUI;
    private Player_UI m_PlayerUI;
    
    private static GameMgr m_Instance = null;
    public static GameMgr GetInstance() { return m_Instance; }
    private bool m_isStartTimer = false;

    private int m_DeathCount = 0;
    private bool m_SceneStartWhiteOut = false;
    private bool m_IsGamePaused = false;
    [HideInInspector] public bool m_CanInputAnyKey = false;

    private int m_CurSceneIdx = 0;
    


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
        m_CurSceneIdx = SceneManager.GetActiveScene().buildIndex;

        p_PlayerInputMgr.p_FireLock = false;
        p_SceneChangeMgr.CheckSmoothSceneChange();
        p_CoroutineHandler.ResetCoroutineHandler();
        p_DataHandleMgr.SetDataHandleMgr();
        
        p_PlayerMgr.ResetPlayer();

        p_PlayerManipulator.SetPlayer();
        p_PlayerManipulator.SetNegotiator();
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CurSceneReload();
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            int sceneIdx = m_CurSceneIdx + 1;
            if (sceneIdx >= SceneManager.sceneCountInBuildSettings)
                return;
            
            SceneManager.LoadScene(m_CurSceneIdx + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            int sceneIdx = m_CurSceneIdx - 1;
            if (sceneIdx < 0)
                return;
            
            SceneManager.LoadScene(m_CurSceneIdx - 1);
        }
    }

    // Functions
    public void RequestLoadScene(int _idx, bool _smooth = true)
    {
        if (_idx < 0 || _idx >= SceneManager.sceneCountInBuildSettings)
            return;

        if (_smooth)
        {
            p_SceneChangeMgr.InitSceneEndWithSmooth(_idx, 3f);
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