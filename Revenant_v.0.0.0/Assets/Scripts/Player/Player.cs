using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public enum PlayerStateName
{
    IDLE,
    WALK,
    ROLL,
    HIDDEN,
    MELEE,
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
    public float p_RollDecelerationSpeedMulti { get; private set; } = 1f;

    [field: SerializeField, MinMaxSlider(0f, 1f), Title("Evade Values"), BoxGroup("Player Values")]
    public Vector2 p_JustEvadeNormalizeTime { get; private set; } = Vector2.zero;
    [BoxGroup("Player Values")]
    public float p_JustEvadeStopTime = 0.1f;
    
    [field: SerializeField] 
    public Transform p_CenterTransform { get; private set; }


    // Member Variables
    [field : SerializeField] public PlayerRotation m_playerRotation { get; private set; }
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
    public Player_InputMgr m_InputMgr { get; private set; }
    public Player_HitscanRay m_PlayerHitscanRay { get; private set; }
    public Player_ObjInteracter m_ObjInteractor { get; private set; }
    [field : SerializeField]public Player_MeleeAttack m_MeleeAttack { get; private set; }
    public Player_ArmMgr m_ArmMgr { get; private set; }
    public RageGauge_UI m_RageGauge { get; private set; }
    public BulletTimeMgr m_BulletTimeMgr { get; private set; }

    public ScreenEffect_UI m_ScreenEffectUI { get; private set; }

    public ParticleMgr m_ParticleMgr { get; private set; }
    public Negotiator_Player m_Negotiator { get; private set; }


    private bool m_isRecoveringRollCount = false;
    public float m_LeftRollCount { get; set; }
    public bool m_canRoll { get; private set; } = true;
    public bool m_canChangeWeapon { get; private set; } = false;
    public bool m_CanHide { get; set; } = true;


    private Player_IDLE m_IDLE;
    private Player_WALK m_WALK;
    private Player_ROLL m_ROLL;
    private Player_HIDDEN m_HIDDEN;
    private Player_MELEE m_MELEE;
    private Player_DEAD m_DEAD;

    private HideSlot m_CurHideSlot;

    private RageGauge_UI m_RageUI;
    private Player_MatMgr _mPlayerMatMgr;
    public SoundPlayer m_SFXMgr { get; private set; }
    public HitSFXMaker m_HitSFXMaker { get; private set; }
    private PlayerFSM m_CurPlayerFSM;
    private SoundMgr m_SoundMgr;
    private IEnumerator m_FootStep;

    private Vector2 m_PlayerPosVec;

    private Coroutine m_WalkSoundCoroutine;



    // For Player_Managers



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
        m_ObjInteractor = GetComponentInChildren<Player_ObjInteracter>();
        m_MeleeAttack = GetComponentInChildren<Player_MeleeAttack>();
        m_ArmMgr = GetComponentInChildren<Player_ArmMgr>();
        m_Negotiator = GetComponentInChildren<Negotiator_Player>();

        m_IDLE = new Player_IDLE(this);
        m_WALK = new Player_WALK(this);
        m_ROLL = new Player_ROLL(this);
        m_HIDDEN = new Player_HIDDEN(this);
        m_MELEE = new Player_MELEE(this);
        m_DEAD = new Player_DEAD(this);


        m_ObjectType = ObjectType.Human;
        m_ObjectState = ObjectState.Active;
        m_LeftRollCount = p_MaxRollCount;
        m_CanAttacked = true;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_PlayerUIMgr = instance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_RageUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
        m_InputMgr = instance.GetComponentInChildren<Player_InputMgr>();
        m_SoundMgr = instance.GetComponentInChildren<SoundMgr>();
        m_SFXMgr = instance.GetComponentInChildren<SoundPlayer>();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();
        m_ParticleMgr = instance.GetComponentInChildren<ParticleMgr>();
        
        m_RageGauge = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
        m_ScreenEffectUI = instance.m_MainCanvas.GetComponentInChildren<InGame_UI>()
            .GetComponentInChildren<ScreenEffect_UI>();
        

        
        m_CurPlayerFSM = m_IDLE;
        m_CurPlayerFSM.StartState();
    }

    public void SetPlayer(PlayerManipulator _input)
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
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(m_MeleeAttack);
        #endif
    }

    public void SetNegotiator(PlayerManipulator _input)
    {
        var nego = GetComponentInChildren<Negotiator_Player>();
        
        nego.p_BulletDamage = _input.N_Damage;
        nego.p_StunValue = _input.N_StunValue;
        nego.p_MinFireDelay = _input.N_MinFireDelay;
        nego.p_MaxRound = _input.N_MaxBullet;
        nego.p_MaxMag = _input.N_MaxMag;
        nego.p_ReloadTime = _input.N_ReloadSpeed;
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(nego);
        #endif
    }



    // Update
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause) // Pause면 중지
            return;

        m_CurPlayerFSM.UpdateState();
    }
    


    // Player FSM Functions
    // ReSharper disable Unity.PerformanceAnalysis
    public void ChangePlayerFSM(PlayerStateName _name)
    {
        m_PlayerAniMgr.ExitPlayerAnim();
        
        //Debug.Log("상태 전이" + _name);
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

            default:
                Debug.Log("Player->ChangePlayerFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }

        m_CurPlayerFSM.StartState();
        m_PlayerAniMgr.PlayPlayerAnim();
    }



    // Functions
    public Vector2 GetPlayerCenterPos()
    {
        return p_CenterTransform.position;
    }
    public void SetWalkSoundCoroutine(bool _isOn)
    {
        if (_isOn)
        {
            m_WalkSoundCoroutine = StartCoroutine(PlayWalkSound());
        }
        else
        {
            StopCoroutine(m_WalkSoundCoroutine);
        }

    }

    private IEnumerator PlayWalkSound()
    {
        while (true)
        {
            m_SoundMgr.MakeSound(GetPlayerCenterPos(), true, SOUNDTYPE.PLAYERNOISE);
            m_SFXMgr.playPlayerSFXSound(4);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public bool IsPlayerOnStair()
    {
        /*
        if (gameObject.layer == 10)
        {
            m_PlayerLocationSensor
            return true;
        }

        return false;
        */
        return false;
    }

    public void GoToStairLayer(bool _input, Vector2 _movePos, Vector2 _normal)
    {
        if (_input)
        {
            gameObject.layer = 10;
        }
        else
        {
            gameObject.layer = 12;
        }
    }

    public void UseRollCount()
    {
        m_LeftRollCount -= 1f;
        m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);

        if (!m_isRecoveringRollCount)
            StartCoroutine(RecoverRollCount());
    }

    public IEnumerator RecoverRollCount()
    {
        m_isRecoveringRollCount = true;
        while (m_LeftRollCount < p_MaxRollCount)
        {
            m_LeftRollCount += Time.deltaTime * p_RollCountRecoverSpeed;
            m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);
            yield return null;
        }

        m_LeftRollCount = 3f;
        m_PlayerUIMgr.SetRollGauge(m_LeftRollCount);
        m_isRecoveringRollCount = false;
    }
    
    public override void setisRightHeaded(bool _isRightHeaded)
    {
        m_IsRightHeaded = _isRightHeaded;
        var TempLocalScale = transform.localScale;

        if ((m_IsRightHeaded && TempLocalScale.x < 0) || (!m_IsRightHeaded && TempLocalScale.x > 0))
            transform.localScale = new Vector3(-TempLocalScale.x, TempLocalScale.y, 1);

        _mPlayerMatMgr.FlipAllNormalsToRight(m_IsRightHeaded);

        m_PlayerAniMgr.PlayPlayerAnim();
    }

    public void setPlayerHp(int _value)
    {
        p_Hp = _value;
        m_PlayerUIMgr.SetHp(p_Hp);
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
        switch (_direction)
        {
            case 0:
                m_PlayerRigid.velocity = Vector2.zero;
                break;

            case 1:
                if (!m_IsRightHeaded)
                    setisRightHeaded(true);

                m_PlayerRigid.velocity =
                    -StaticMethods.getLPerpVec(m_PlayerFootMgr.m_PlayerNormal).normalized * (p_MoveSpeed * _multi);
                break;

            case -1:
                if (m_IsRightHeaded)
                    setisRightHeaded(false);

                m_PlayerRigid.velocity =
                    StaticMethods.getLPerpVec(m_PlayerFootMgr.m_PlayerNormal).normalized * (p_MoveSpeed * _multi);
                break;
        }
    }
}