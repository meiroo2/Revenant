﻿using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class MeleeGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_MeleeDistance { get; protected set; } = 0.2f;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public bool p_IsLookAround = false;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_LookAroundDelay = 1f;
    [field: SerializeField, BoxGroup("MeleeGang Values"),Range(0.0f, 1.0f)] public float p_AttackTiming;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_FollowSpeedMulti;

    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_DelayAfterAttack = 0.1f;

    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_StunWaitTime { get; private set; } = 0.5f;

    [field: SerializeField, BoxGroup("MeleeGang Values")] public Transform[] p_PatrolPosArr { get; private set; } = null;
    [field: SerializeField] public RuntimeAnimatorController p_NormalAniCont;
    [field: SerializeField] public RuntimeAnimatorController p_FightAniCont;
    [field: SerializeField, Space(10f)] public Enemy_HotBox p_HeadBox;
    [field: SerializeField] public Enemy_HotBox p_BodyBox;


    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }
    public MatChanger m_MatChanger { get; private set; }
    private IHotBox[] m_HotBoxes;

    private IDLE_MeleeGang m_IDLE;
    private FOLLOW_MeleeGang m_FOLLOW;
    private ATTACK_MeleeGang m_ATTACK;
    private DEAD_MeleeGang m_DEAD;
    private CHANGE_MeleeGang m_CHANGE;
    private STUN_MeleeGang m_STUN;

    [HideInInspector] public bool m_IsTurning = false;
    public bool m_IsPatrol { get; private set; } = false;
    private Vector2 m_DistBetPlayer;

    /// <summary>
    /// 0 == 에러, 1 == 머리, 2 == 몸통
    /// </summary>
    public int m_DeathReason { get; private set; } = 0;


    // Constructor
    private void Awake()
    {
        InitHuman();
        InitEnemy();
        
        m_Renderer = GetComponentInChildren<SpriteRenderer>();

        m_Animator = GetComponentInChildren<Animator>();
        m_HotBoxes = GetComponentsInChildren<IHotBox>();
        m_EnemyUseRange = GetComponentInChildren<Enemy_UseRange>();
        m_EnemyLocationSensor = GetComponentInChildren<LocationSensor>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();

        m_CurEnemyFSM = new IDLE_MeleeGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();
        
        m_IDLE = new IDLE_MeleeGang(this);
        m_FOLLOW = new FOLLOW_MeleeGang(this);
        m_ATTACK = new ATTACK_MeleeGang(this);
        m_DEAD = new DEAD_MeleeGang(this);
        m_CHANGE = new CHANGE_MeleeGang(this);
        m_STUN = new STUN_MeleeGang(this);

        if (p_PatrolPosArr.Length > 0)
            m_IsPatrol = true;

        m_Animator.runtimeAnimatorController = p_NormalAniCont;
    }

    private void Start()
    {
        m_OriginPos = transform.position;

        Player tempPlayer = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_PlayerTransform = tempPlayer.transform;
        m_PlayerLocationSensor = tempPlayer.m_PlayerLocationSensor;
        m_MatChanger = InstanceMgr.GetInstance().GetComponentInChildren<MatChanger>();
    }


    // Updates
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public override void SetRigidByDirection(bool _isRight, float _addSpeed = 1f)
    {
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
        
        /*
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            if (!m_PlayerCognition)
                m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed);
            else
                m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * p_FollowSpeedMulti);
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);

            if (!m_PlayerCognition)
                m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed);
            else
                m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * p_FollowSpeedMulti);
        }
        */
    }

    public override void StartPlayerCognition(bool _instant = false)
    {
        if (_instant)
        {
            m_Animator.runtimeAnimatorController = p_FightAniCont;
            m_PlayerCognition = true;
            ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
        else
        {
            m_PlayerCognition = true;
            ChangeEnemyFSM(EnemyStateName.CHANGE);
        }
    }
    
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr) 
            return;

        var meleeWeapon = GetComponentInChildren<MeleeWeapon_Enemy>();
        
        p_Hp = _mgr.M_HP;
        p_StunHp = _mgr.M_StunThreshold;
        meleeWeapon.p_BulletDamage = _mgr.M_MeleeDamage;
        p_MoveSpeed = _mgr.M_Speed;
        p_VisionDistance = _mgr.M_Vision_Distance;
        p_MeleeDistance = _mgr.M_MeleeAttack_Distance;
        p_AttackTiming = _mgr.M_PointAttackTime;
        p_HeadBox.p_DamageMulti = _mgr.M_HeadDmgMulti;
        p_BodyBox.p_DamageMulti = _mgr.M_BodyDmgMulti;
        p_FollowSpeedMulti = _mgr.M_FollowSpeedMulti;
        p_DelayAfterAttack = _mgr.M_DelayAfterAttack;
        p_StunWaitTime = _mgr.M_StunWaitTime;

        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(p_HeadBox);
            EditorUtility.SetDirty(p_BodyBox);
            EditorUtility.SetDirty(meleeWeapon);
        #endif
    }
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;

        // Turn 도중 피격 - NormalMode시
        if (m_IsTurning)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                setisRightHeaded(!m_IsRightHeaded);       
            }
        }
        
        if (p_Hp <= 0)
        {
            if (_point == HitBoxPoint.HEAD)
                m_DeathReason = 1;
            else if (_point == HitBoxPoint.BODY)
                m_DeathReason = 2;

            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }
        
        if (m_PlayerCognition == false && m_CurEnemyStateName != EnemyStateName.CHANGE)
        {
            StartPlayerCognition();
            return;
        }


        if (!m_PlayerCognition)
            return;
        
        m_CurStunValue += _stunValue;
        ChangeEnemyFSM(EnemyStateName.STUN);
    }

    public void ResetStunThreshold()
    {
        p_StunHp = 0;
    }

    public void SetEnemyHotBox(bool _isOn)
    {
        p_HeadBox.gameObject.SetActive(_isOn);
        p_BodyBox.gameObject.SetActive(_isOn);
    }
    
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log("상태 전이" + _name);
        m_CurEnemyStateName = _name;
        
        m_CurEnemyFSM.ExitState();
        
        switch (m_CurEnemyStateName)
        {
            case EnemyStateName.IDLE:
                m_CurEnemyFSM = m_IDLE;
                break;
            
            case EnemyStateName.FOLLOW:
                m_CurEnemyFSM = m_FOLLOW;
                break;
            
            case EnemyStateName.ATTACK:
                m_CurEnemyFSM = m_ATTACK;
                break;

            case EnemyStateName.DEAD:
                m_CurEnemyFSM = m_DEAD;
                break; 
            
            case EnemyStateName.CHANGE:
                m_CurEnemyFSM = m_CHANGE;
                break;
            
            case EnemyStateName.STUN:
                m_CurEnemyFSM = m_STUN;
                break;

            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
}