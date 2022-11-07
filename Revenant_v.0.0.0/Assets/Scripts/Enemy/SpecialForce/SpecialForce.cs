using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Update = UnityEngine.PlayerLoop.Update;


public class SpecialForce : BasicEnemy
{
    // Visible Member Variables
    
    // Basic
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_AttackDistance { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Bullet_Speed { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Bullet_Spread { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Fire_Delay { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Stun_Time { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_MeleeDistance { get; private set; } = 0.4f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_AlertSpeed { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_AlertFadeinSpeed { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_HeadDmgMulti { get; private set; } = 2;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_BodyDmgMulti { get; private set; } = 1;
    
    // Advanced
    [field: SerializeField, BoxGroup("SpecialForce"), PropertySpace(10f, 0f)] 
    public Vector2 p_Roll_Tick { get; private set; }

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Refresh { get; private set; } = 0.5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_Roll_Chance { get; private set; } = 50;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Cooldown { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Speed_Multi { get; private set; } = 3f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_OnAlert_MoveSpeedMulti = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_GapDistance = 1f;

    // Roll
    [field: SerializeField, BoxGroup("SpecialForce"), Title("For Hide")] public float p_Hide_Chance = 10f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Find_Distance = 1f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Time_Min = 1f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Time_Cancel = 1f;
    
    // Assign
    [field: SerializeField, Title("Assign")] public Animator p_Animator { get; private set; }
    [field: SerializeField] public Enemy_HotBox[] p_HitHotboxes { get; private set; }
    public TripleShot_Enemy p_TripleShotWeapon;
    public MeleeWeapon_Enemy p_MeleeWeapon;
    public Enemy_HotBox p_HeadHotBox;
    public Enemy_HotBox p_BodyHotBox;
    
    

    // Member Variables
    [HideInInspector] public bool m_UseHide = false;
    [HideInInspector] public bool m_IsMouseTouched = false;
    public bool m_GlobalRollCooldown { get; private set; } = false;
    private Coroutine m_RollCooldownCoroutine;
    
    private bool m_FSMLock = false;
    private SpecialForce_IDLE m_IDLE;
    private SpecialForce_FOLLOW m_FOLLOW;
    private SpecialForce_STUN m_STUN;
    private SpecialForce_DEAD m_DEAD;
    private SpecialForce_ROLL m_ROLL;
    private SpecialForce_ATTACK m_ATTACK;
    private SpecialForce_HIDDEN m_HIDDEN;

    public WeaponMgr m_WeaponMgr { get; private set; }

    public CoroutineHandler m_CoroutineHandler { get; private set; }

    public AlertSystem m_AlertSystem { get; private set; }
    public MeleeWeapon_Enemy m_MeleeWeapon { get; private set; }
    public SpecialForce_UseRange m_UseRange { get; private set; }
    public HideSlot m_CurHideSlot = null;
    public bool m_LastActionIsHide = false;


    // Constructors
    private void Awake()
    {
        m_AlertSystem = GetComponentInChildren<AlertSystem>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        m_MeleeWeapon = GetComponentInChildren<MeleeWeapon_Enemy>();
        m_UseRange = GetComponentInChildren<SpecialForce_UseRange>();
        m_EnemyHotBoxes = p_HitHotboxes;
        InitHuman();
        m_Animator = p_Animator;

        m_IDLE = new SpecialForce_IDLE(this);
        m_FOLLOW = new SpecialForce_FOLLOW(this);
        m_STUN = new SpecialForce_STUN(this);
        m_DEAD = new SpecialForce_DEAD(this);
        m_ROLL = new SpecialForce_ROLL(this);
        m_ATTACK = new SpecialForce_ATTACK(this);
        m_HIDDEN = new SpecialForce_HIDDEN(this);
        
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;
    }

    private void Start()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;

        var instance = InstanceMgr.GetInstance();
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
        
        m_AlertSystem.SetAlertSpeed(p_AlertSpeed);
        m_AlertSystem.SetStunAlertSpeed(1f);
        m_AlertSystem.SetFadeInSpeed(p_AlertFadeinSpeed);
        
        m_CurEnemyFSM.StartState();
    }

    private void OnEnable()
    {
        m_AlertSystem.SetAlertSpeed(p_AlertSpeed);
        m_AlertSystem.SetStunAlertSpeed(1f);
        m_AlertSystem.SetFadeInSpeed(p_AlertFadeinSpeed);
    }


    // Updates
    private void FixedUpdate()
    {
        if (m_FSMLock)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions

    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        p_Hp = _mgr.SF_Hp;
        p_TripleShotWeapon.p_BulletDamage = _mgr.SF_BulletDamage;
        p_MeleeWeapon.p_BulletDamage = _mgr.SF_BulletDamage;
        p_TripleShotWeapon.p_BulletRandomRotation = p_Bullet_Spread;
        p_Bullet_Speed = _mgr.SF_BulletSpeed;
        p_TripleShotWeapon.p_BulletSpeed = p_Bullet_Speed;
        p_Fire_Delay = _mgr.SF_FireDelay;
        p_MoveSpeed = _mgr.SF_MoveSpeed;
        p_OnAlert_MoveSpeedMulti = _mgr.SF_OnAlert_MoveSpeedMulti;
        p_Stun_Time = _mgr.SF_StunTime;
        p_StunHp = _mgr.SF_StunThreshold;
        p_VisionDistance = _mgr.SF_VisionDistance;
        p_AttackDistance = _mgr.SF_AttackDistance;
        p_MeleeDistance = _mgr.SF_MeleeDistance;
        p_GapDistance = _mgr.SF_GapDistance;
        p_AlertSpeed = _mgr.SF_AlertSpeed;
        p_AlertFadeinSpeed = _mgr.SF_AlertFadeInSpeed;
        
        p_HeadDmgMulti = _mgr.SF_HeadDmgMulti;
        p_HeadHotBox.p_DamageMulti = p_HeadDmgMulti;
        
        p_BodyDmgMulti = _mgr.SF_BodyDmgMulti;
        p_BodyHotBox.p_DamageMulti = p_BodyDmgMulti;
        
        
        p_Roll_Refresh = _mgr.SF_Roll_Refresh;
        p_Roll_Tick = _mgr.SF_Roll_Tick;
        p_Roll_Chance = _mgr.SF_Roll_Chance;
        p_Roll_Cooldown = _mgr.SF_Roll_Cooldown;
        p_Roll_Speed_Multi = _mgr.SF_Roll_Speed_Multi;

        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(p_HeadHotBox);
            EditorUtility.SetDirty(p_BodyHotBox);
            EditorUtility.SetDirty(p_TripleShotWeapon);
            EditorUtility.SetDirty(p_MeleeWeapon);
        #endif
    }

    public override void SetRigidToPoint(float _addSpeed = 1f)
    {
        SetRigidByDirection(m_MovePoint.x > transform.position.x);
    }
    
    public override void SetRigidByDirection(bool _isRight, float _addSpeed = 1f)
    {
        if (_isRight)
        {
            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
        else
        {
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
    }
    
    /// <summary>
    /// Roll의 쿨다운을 개시합니다.
    /// </summary>
    public void RollCooldown()
    {
        Debug.Log("Roll 쿨다운 시작");
        m_GlobalRollCooldown = true;
        
        if(!ReferenceEquals(m_RollCooldownCoroutine, null))
            StopCoroutine(m_RollCooldownCoroutine);
        
        m_RollCooldownCoroutine = StartCoroutine(CalRollCooldown());
    }
    private IEnumerator CalRollCooldown()
    {
        yield return new WaitForSeconds(p_Roll_Cooldown);
        m_GlobalRollCooldown = false;
        Debug.Log("Roll 쿨다운 끝");
    }
    
    public override void MouseTouched(bool _isTouch)
    {
        m_IsMouseTouched = _isTouch;
    }

    public override void StartPlayerCognition(bool _instant = false)
    {
        if (_instant)
        {
            
        }
        else
        {
            ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
    }
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }

        
        if (m_CurStunValue >= p_StunHp)
        {
            m_CurStunValue = 0;
            ChangeEnemyFSM(EnemyStateName.STUN);
            return;
        }


        if (!m_PlayerCognition)
        {
            StartPlayerCognition();
        }
    }
    
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log(_name);
        
        m_FSMLock = true;
        
        bool isFound = true;
        switch (_name)
        {
            case EnemyStateName.IDLE:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_IDLE;
                break;
            
            case EnemyStateName.FOLLOW:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_FOLLOW;
                break;
            
            case EnemyStateName.ROLL:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_ROLL;
                break;
            
            case EnemyStateName.ATTACK:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_ATTACK;
                break;
            
            case EnemyStateName.STUN:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_STUN;
                break;
            
            case EnemyStateName.HIDDEN:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_HIDDEN;
                break;
            
            case EnemyStateName.DEAD:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_DEAD;
                break;

            default:
                isFound = false;
                Debug.Log("ERR : SpecialForce에서 정의되지 않은 FSM 변경 시도");
                break;
        }

        
        if (isFound)
        {
            m_CurEnemyStateName = _name;
            m_CurEnemyFSM.StartState();
        }

        m_FSMLock = false;
    }
}





















