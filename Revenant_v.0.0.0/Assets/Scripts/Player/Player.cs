using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum PlayerStateName
{
    IDLE,
    WALK,
    ROLL,
    HIDDEN,
    MELEE,
    BULLET_TIME,
    DEAD
}

public class Player : Human
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("Player Values")]
    public float p_RollCountRecoverSpeed { get; private set; } = 1f;

    [field: SerializeField, BoxGroup("Player Values")]
    public float p_RollSpeedMulti { get; private set; } = 1.3f;

    [field: SerializeField, BoxGroup("Player Values")]
    public float p_BackSpeedMulti { get; private set; } = 0.7f;

    [field: SerializeField, BoxGroup("Player Values")]
    public float p_RunSpeedRatio { get; private set; } = 1.5f;

    [field: SerializeField, BoxGroup("Player Values")]
    public int p_MaxRollCount { get; private set; } = 3;

    [field: SerializeField, BoxGroup("Player Values")]
    public float p_MeleeSpeedMulti { get; private set; } = 2f;

    [field: SerializeField, BoxGroup("Player Values")]
    public float p_RollDecelerationSpeed { get; private set; } = 1f;

    [BoxGroup("Player Values")] public float p_ReloadSpeed = 1f;


    [field: SerializeField, MinMaxSlider(0f, 1f), Title("Evade Values"), BoxGroup("Player Values")]
    public Vector2 p_JustEvadeNormalizeTime { get; private set; } = Vector2.zero;

    [BoxGroup("Player Values")] public float p_JustEvadeStopTime = 0.1f;

    // Breath
    [Title("For Breath Effect"), BoxGroup("Player Values")]
    public Vector2 m_BreathPivotPos;
    public float m_BreathDelay = 0.5f;
    public int[] p_BreathEffectSceneIdxArr;
    

    public Player_DeadProcess p_DeadProcess;
    [field: SerializeField] public Transform p_CenterTransform { get; private set; }
    [field: SerializeField] public LeftBullet_WUI p_LeftBullet_WUI { get; private set; }


    // Member Variables
    [ReadOnly] public bool bIsOnStair = false;
    public int PlayerStairNum { get; set; } = 0;
    public int PlayerMapSectionNum { get; set; } = 0;

    [field: SerializeField] public PlayerRotation m_playerRotation { get; private set; }
    public Player_WeaponMgr m_WeaponMgr { get; private set; }
    public Player_UseRange m_useRange { get; private set; }
    public Player_AniMgr m_PlayerAniMgr { get; private set; }
    public PlayerStateName m_CurPlayerFSMName { get; private set; }
    public Rigidbody2D m_PlayerRigid { get; private set; }
    public Animator m_PlayerAnimator { get; private set; }
    public Player_FootMgr m_PlayerFootMgr { get; private set; }
    public Player_HotBox m_PlayerHotBox { get; private set; }
    public Player_UI m_PlayerUIMgr { get; private set; }
    public LocationSensor m_PlayerLocationSensor { get; private set; }
    public Player_HitscanRay m_PlayerHitscanRay { get; private set; }
    [field: SerializeField] public Player_MeleeAttack m_MeleeAttack { get; private set; }
    public Player_ArmMgr m_ArmMgr { get; private set; }
    public RageGauge m_RageGauge { get; private set; }
    public BulletTimeMgr m_BulletTimeMgr { get; private set; }

    public ScreenEffect_UI m_ScreenEffectUI { get; private set; }
    public SimpleEffectPuller m_SimpleEffectPuller { get; private set; }

    public BallLauncher m_BallLauncher { get; private set; }
    public ParticleMgr m_ParticleMgr { get; private set; }
    public Negotiator_Player m_Negotiator { get; private set; }
    public Player_WorldUI m_WorldUI { get; private set; }
    public Player_InputMgr m_InputMgr { get; private set; }
    public BulletTime_AR m_BulletTimeAR { get; private set; }
    public RageGauge_UI m_RageUI { get; private set; }

    private bool m_isRecoveringRollCount = false;
    public float m_LeftRollCount { get; set; }
    public bool m_canRoll { get; private set; } = true;
    public bool m_canChangeWeapon { get; private set; } = false;
    public bool m_CanHide { get; set; } = true;
    public int m_MoveDirection { get; private set; } = 0;
    [HideInInspector] public bool m_CancelMeleeStartAnim = false;
    

    private Player_IDLE m_IDLE;
    private Player_WALK m_WALK;
    private Player_ROLL m_ROLL;
    private Player_HIDDEN m_HIDDEN;
    private Player_MELEE m_MELEE;
    private Player_DEAD m_DEAD;
    private Player_BULLET_TIME m_BULLETTIME;

    private HideSlot m_CurHideSlot;
    
    private Player_MatMgr _mPlayerMatMgr;
    public SoundPlayer m_SoundPlayer { get; private set; }
    public HitSFXMaker m_HitSFXMaker { get; private set; }
    private PlayerFSM m_CurPlayerFSM;
    private SoundMgr m_SoundMgr;
    private IEnumerator m_FootStep;

    private Vector2 m_PlayerPosVec;

    private Coroutine m_WalkSoundCoroutine;

    private bool m_SafeFSMLock = false;

    // For Breath Effect
    private Coroutine m_BreathCoroutine = null;
    

    // Constructor
    private void Awake()
    {
        _mPlayerMatMgr = GetComponent<Player_MatMgr>();
        m_PlayerAnimator = GetComponent<Animator>();
        m_PlayerRigid = GetComponent<Rigidbody2D>();
        m_PlayerLocationSensor = GetComponentInChildren<LocationSensor>();
        m_PlayerHitscanRay = GetComponentInChildren<Player_HitscanRay>();
        m_PlayerAniMgr = GetComponentInChildren<Player_AniMgr>();
        m_PlayerHotBox = GetComponentInChildren<Player_HotBox>();
        m_PlayerFootMgr = GetComponentInChildren<Player_FootMgr>();
        m_WeaponMgr = GetComponentInChildren<Player_WeaponMgr>();
        m_useRange = GetComponentInChildren<Player_UseRange>();
        m_MeleeAttack = GetComponentInChildren<Player_MeleeAttack>();
        m_ArmMgr = GetComponentInChildren<Player_ArmMgr>();
        m_Negotiator = GetComponentInChildren<Negotiator_Player>();
        m_WorldUI = GetComponentInChildren<Player_WorldUI>();
        m_BallLauncher = GetComponentInChildren<BallLauncher>();

        m_ObjectType = ObjectType.Player;
        m_ObjectState = ObjectState.Active;
        m_LeftRollCount = p_MaxRollCount;
        m_CanAttacked = true;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_PlayerUIMgr = instance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_SoundMgr = instance.GetComponentInChildren<SoundMgr>();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();
        m_ParticleMgr = instance.GetComponentInChildren<ParticleMgr>();
        m_SimpleEffectPuller = instance.GetComponentInChildren<SimpleEffectPuller>();
        
        m_RageGauge = instance.m_RageGauge;
        m_RageUI = m_RageGauge.p_RageGaugeUI;
        m_ScreenEffectUI = instance.m_MainCanvas.GetComponentInChildren<InGame_UI>()
            .GetComponentInChildren<ScreenEffect_UI>();

        m_BulletTimeAR = instance.m_BulletTime_AR;
        
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;

        m_IDLE = new Player_IDLE(this);
        m_WALK = new Player_WALK(this);
        m_ROLL = new Player_ROLL(this);
        m_HIDDEN = new Player_HIDDEN(this);
        m_MELEE = new Player_MELEE(this);
        m_DEAD = new Player_DEAD(this);
        m_BULLETTIME = new Player_BULLET_TIME(this);

        m_CurPlayerFSM = m_IDLE;
        m_CurPlayerFSM.StartState();
    }

    public void SetPlayer(PlayerManipulator _input, bool _isEditor)
    {
        p_Hp = _input.P_HP;
        p_StunAlertSpeed = _input.P_StunInvincibleTime;
        p_MoveSpeed = _input.P_Speed;
        p_BackSpeedMulti = _input.P_BackSpeedMulti;
        p_RollSpeedMulti = _input.P_RollSpeedMulti;
        p_MeleeSpeedMulti = _input.P_MeleeSpeedMulti;
        m_MeleeAttack.m_Damage = _input.P_MeleeDamage;
        m_MeleeAttack.m_StunValue = _input.P_MeleeStunValue;
        p_JustEvadeNormalizeTime = new Vector2(_input.P_JustEvadeStartTime, _input.P_JustEvadeEndTime);
        p_RollDecelerationSpeed = _input.P_RollDecelerationSpeed;
        p_ReloadSpeed = _input.P_ReloadSpeed;


        if (!_isEditor)
            return;
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(m_MeleeAttack);
        #endif
    }

    public void SetNegotiator(PlayerManipulator _input, bool _isEditor)
    {
        var nego = GetComponentInChildren<Negotiator_Player>();

        nego.p_BulletDamage = _input.N_Damage;
        nego.p_StunValue = _input.N_StunValue;
        nego.p_MinFireDelay = _input.N_MinFireDelay;

        if (!_isEditor)
            return;
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(nego);
        #endif
    }



    // Update
    private void Update()
    {
        if (m_SafeFSMLock || m_ObjectState == ObjectState.Pause) // Pause면 중지
            return;

        m_CurPlayerFSM.UpdateState();
    }


    // Player FSM Functions
    // ReSharper disable Unity.PerformanceAnalysis
    public void ChangePlayerFSM(PlayerStateName _name)
    {
        m_SafeFSMLock = true;

//        Debug.Log("상태 전이" + _name);
        m_CurPlayerFSMName = _name;

        m_CurPlayerFSM.ExitState();
        switch (m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                m_CurPlayerFSM = m_IDLE;
                break;

            case PlayerStateName.WALK:
                m_CurPlayerFSM = m_WALK;
                break;

            case PlayerStateName.ROLL:
                m_CurPlayerFSM = m_ROLL;
                break;

            case PlayerStateName.HIDDEN:
                m_CurPlayerFSM = m_HIDDEN;
                break;

            case PlayerStateName.MELEE:
                m_CurPlayerFSM = m_MELEE;
                break;

            case PlayerStateName.DEAD:
                m_CurPlayerFSM = m_DEAD;
                break;

            case PlayerStateName.BULLET_TIME:
                m_CurPlayerFSM = m_BULLETTIME;
                break;

            default:
                Debug.Log("Player->ChangePlayerFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }

        m_CurPlayerFSM.StartState();
        m_SafeFSMLock = false;
    }



    // Functions

    /// <summary>
    /// 플레이어 좌우에 Ray를 싸 빈 공간 여부를 확인합니다.
    /// 0이면 없음, 1이면 왼쪽, 2면 오른쪽, 3이면 전부 빔
    /// </summary>
    /// <returns>빈 공간 여부</returns>
    public int GetIsEmptyNearPlayer(float _wantMovePoint)
    {
        //Debug.Log("Sans");
        
        RaycastHit2D leftHitPoint;
        RaycastHit2D rightHitPoint;
        Vector2 rayStartPos = p_CenterTransform.position;

        float rayLength = _wantMovePoint * 1.875f;
        int layerMask = LayerMask.GetMask("Floor");

        leftHitPoint = Physics2D.Raycast(rayStartPos, Vector2.left, rayLength, layerMask);
        rightHitPoint = Physics2D.Raycast(rayStartPos, -Vector2.left, rayLength, layerMask);
        
        Debug.DrawRay(rayStartPos, Vector2.left * rayLength, Color.magenta, 1f);
        Debug.DrawRay(rayStartPos, -Vector2.left * rayLength, Color.magenta, 1f);

        bool isLeftEmpty = ReferenceEquals(leftHitPoint.collider, null);
        bool isRightEmpty = ReferenceEquals(rightHitPoint.collider, null);
        
        if (isLeftEmpty && !isRightEmpty)
            return 1;
        else if (!isLeftEmpty && isRightEmpty)
            return 2;
        else if(isLeftEmpty && isRightEmpty)
            return 3;

        return 0;
    }

    /// <summary>
    /// 플레이어의 FSM 상태 진입 / 탈출 시 특정 Action을 재생하도록 붙입니다.
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_action"></param>
    /// <param name="_isInit"></param>
    public void AttachActionOnFSM(PlayerStateName _name, Action _action, bool _isInit)
    {
        switch (_name)
        {
            case PlayerStateName.IDLE:
                if (_isInit)
                {
                    m_IDLE.m_InitAction = _action;
                }
                else
                {
                    m_IDLE.m_ExitAction = _action;
                }
                break;

            case PlayerStateName.WALK:
                if (_isInit)
                {
                    m_WALK.m_InitAction = _action;
                }
                else
                {
                    m_WALK.m_ExitAction = _action;
                }
                break;

            case PlayerStateName.ROLL:
                if (_isInit)
                {
                    Debug.Log("Roll에 Action 붙임");
                    m_ROLL.m_InitAction = _action;
                }
                else
                {
                    m_ROLL.m_ExitAction = _action;
                }
                break;

            case PlayerStateName.HIDDEN:
                if (_isInit)
                {
                    m_HIDDEN.m_InitAction = _action;
                }
                else
                {
                    m_HIDDEN.m_ExitAction = _action;
                }
                break;

            case PlayerStateName.MELEE:
                if (_isInit)
                {
                    m_MELEE.m_InitAction = _action;
                }
                else
                {
                    m_MELEE.m_ExitAction = _action;
                }
                break;

            case PlayerStateName.DEAD:
                if (_isInit)
                {
                    m_DEAD.m_InitAction = _action;
                }
                else
                {
                    m_DEAD.m_ExitAction = _action;
                }
                break;
            
            case PlayerStateName.BULLET_TIME:
                if (_isInit)
                {
                    m_BULLETTIME.m_InitAction = _action;
                }
                else
                {
                    m_BULLETTIME.m_ExitAction = _action;
                }
                break;

            default:
                Debug.Log("ERR : AttachActionOnFSM OOR");
                break;
        }
    }

    /// <summary>
    /// FSM에 붙인 Acion을 null로 초기화합니다.
    /// </summary>
    /// <param name="_name"></param>
    public void RemoveActionOnFSM(PlayerStateName _name)
    {
        switch (_name)
        {
            case PlayerStateName.IDLE:
                m_IDLE.m_InitAction = null;
                m_IDLE.m_ExitAction = null;
                break;

            case PlayerStateName.WALK:
                m_WALK.m_InitAction = null;
                m_WALK.m_ExitAction = null;
                break;

            case PlayerStateName.ROLL:
                m_ROLL.m_InitAction = null;
                m_ROLL.m_ExitAction = null;
                break;

            case PlayerStateName.HIDDEN:
                m_HIDDEN.m_InitAction = null;
                m_HIDDEN.m_ExitAction = null;
                break;

            case PlayerStateName.MELEE:
                m_MELEE.m_InitAction = null;
                m_MELEE.m_ExitAction = null;
                break;

            case PlayerStateName.DEAD:
                m_DEAD.m_InitAction = null;
                m_DEAD.m_ExitAction = null;
                break;
            
            case PlayerStateName.BULLET_TIME:
                m_BULLETTIME.m_InitAction = null;
                m_BULLETTIME.m_ExitAction = null;
                break;

            default:
                Debug.Log("ERR : AttachActionOnFSM OOR");
                break;
        }
    }
    
    public Vector2 GetPlayerCenterPos()
    {
        return p_CenterTransform.position;
    }

    public void GoToStairLayer(bool _input)
    {
        gameObject.layer = _input ? 10 : 12;
    }

    public override void setisRightHeaded(bool _isRightHeaded)
    {
        m_IsRightHeaded = _isRightHeaded;
        var TempLocalScale = transform.localScale;

        if ((m_IsRightHeaded && TempLocalScale.x < 0) || (!m_IsRightHeaded && TempLocalScale.x > 0))
            transform.localScale = new Vector3(-TempLocalScale.x, TempLocalScale.y, 1);

        _mPlayerMatMgr.FlipAllNormalsToRight(m_IsRightHeaded);


        if (p_LeftBullet_WUI.gameObject.activeSelf)
            p_LeftBullet_WUI.MovetoLeftSide(m_IsRightHeaded);
        //m_PlayerAniMgr.PlayPlayerAnim();
    }

    public void setPlayerHp(int _value)
    {
        p_Hp = _value;
    }

    public bool GetIsPlayerWalkStraight()
    {
        return (m_IsRightHeaded && m_InputMgr.m_IsPushRightKey) || (!m_IsRightHeaded && m_InputMgr.m_IsPushLeftKey);
    }

    public Vector2 GetPlayerFootPos()
    {
        RaycastHit2D hit = m_PlayerFootMgr.GetFootRayHit();

        if (!ReferenceEquals(hit.collider, null))
        {
            return hit.point;
        }
        else
        {
            return m_PlayerPosVec;
        }
    }

    /// <summary>파라미터에 따라 발 밑 Normal벡터에 직교하는 방향대로 이동합니다.</summary>
    /// <param name="_direction">-1 : 왼, 0 : 정지, 1 : 오른</param>
    /// <param name="_multi"> 이동속도 추가 배율 </param>>
    public void MoveByDirection(int _direction, float _multi = 1f)
    {
        m_MoveDirection = _direction;
        switch (_direction)
        {
            case 0:
                m_PlayerRigid.velocity = Vector2.zero;
                break;

            case 1:
                m_PlayerRigid.velocity =
                    -StaticMethods.getLPerpVec(m_PlayerFootMgr.m_PlayerNormal) * (p_MoveSpeed * _multi);
                break;

            case -1:
                m_PlayerRigid.velocity =
                    StaticMethods.getLPerpVec(m_PlayerFootMgr.m_PlayerNormal) * (p_MoveSpeed * _multi);
                break;
        }
    }

    /// <summary>
    /// Player의 숨 이펙트를 켜거나 끕니다.
    /// </summary>
    /// <param name="_isActive"></param>
    public void ActiveBreathCoroutine(bool _isActive)
    {
        if (!ReferenceEquals(m_BreathCoroutine, null))
        {
            StopCoroutine(m_BreathCoroutine);
            m_BreathCoroutine = null;
        }

        int curSceneIdx = GameMgr.GetInstance().m_CurSceneIdx;
        bool isSceneCorrect = false;
        for (int i = 0; i < p_BreathEffectSceneIdxArr.Length; i++)
        {
            if (curSceneIdx == p_BreathEffectSceneIdxArr[i])
            {
                isSceneCorrect = true;
                break;
            }
        }

        if (!isSceneCorrect)
            return;
        
        if (_isActive)
            m_BreathCoroutine = StartCoroutine(BreathEnumerator());
    }

    private IEnumerator BreathEnumerator()
    {
        Vector2 spawnPos;
        while (true)
        {
            spawnPos = transform.position;
            spawnPos.x += m_IsRightHeaded ? m_BreathPivotPos.x : -m_BreathPivotPos.x;
            spawnPos.y += m_BreathPivotPos.y;

            m_SimpleEffectPuller.SpawnSimpleEffect(11, spawnPos, !m_IsRightHeaded);
            
            yield return new WaitForSeconds(m_BreathDelay);
        }

        yield break;
    }
}