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

    public float m_GameTimer { get; private set; } = 0f;


    // Member Variables
    private InGame_UI m_IngameUI;
    private Player_UI m_PlayerUI;
    
    private static GameMgr m_Instance = null;
    public static GameMgr GetInstance() { return m_Instance; }
    private bool m_isStartTimer = false;
    private ObjectDefine[] m_Objects;

    private int m_DeathCount = 0;
    private bool m_SceneStartWhiteOut = false;
    private bool m_IsGamePaused = false;
    [HideInInspector] public bool m_CanInputAnyKey = false;
    


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

        if (!FindObjectOfType<DataHandleManager>())
        {
            Debug.Log("DataHandleManager 없음!!!! 배치 요망!!!");
        }
    }

    private void Start()
    {
        m_Objects = GameObject.FindObjectsOfType<ObjectDefine>();

        AssignCanvasObjs();
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
        Debug.Log("신 개시 로딩 첫번쨰");

        p_PlayerMgr.ResetPlayer();
        
        p_PlayerManipulator.SetPlayer();
        p_PlayerManipulator.SetNegotiator();
        
        p_CoroutineHandler.ResetCoroutineHandler();
        m_Objects = GameObject.FindObjectsOfType<ObjectDefine>();
        //m_SceneStartWhiteOut = false;
        AssignCanvasObjs();
        m_IngameUI.DoWhiteOut(true);
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CurSceneReload();
        }

        if (Input.anyKeyDown && m_CanInputAnyKey)
        {
            m_CanInputAnyKey = false;
            m_IngameUI.SetCallback(CurSceneReload);
            m_IngameUI.DoWhiteOut(false);
        }
    }

    // Functions
    public void PlayerDead()
    {
        m_DeathCount++;
        SetObjectState(ObjectState.Pause);
        TimelineMgr.GetInstance().StartDeathTimeline();

        
        AssignCanvasObjs();
        m_PlayerUI.SetPlayerUIVisible(false);
        
        m_SceneStartWhiteOut = true;
    }
    public void resetTimer() 
    {
        m_isStartTimer = false;
        m_GameTimer = 0f;
    }

    public void SetObjectState(ObjectState _state)
    {
        for (int i = 0; i < m_Objects.Length; i++)
        {
            m_Objects[i].setObjectState(_state);
        }
    }

    public void CurSceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AssignCanvasObjs()
    {
        var mainCanvas = InstanceMgr.GetInstance().m_MainCanvas;
        m_IngameUI = mainCanvas.GetComponentInChildren<InGame_UI>();
        m_PlayerUI = mainCanvas.GetComponentInChildren<Player_UI>();
    }
}