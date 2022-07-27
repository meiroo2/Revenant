using System;
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Update = UnityEngine.PlayerLoop.Update;


public class NormalGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_AlertSpeedRatio = 1f;
    [field: SerializeField] public float p_CloseAttackDistance = 0.1f;
    [field: SerializeField] public Transform p_GunPos { get; protected set; }
    [field: SerializeField] public float p_MinFollowDistance { get; protected set; } = 0.2f;
    [field: SerializeField] public Transform[] p_PatrolPos { get; protected set; }

    [field: SerializeField] public bool p_IsLookAround = false;
    [field: SerializeField] public float p_LookAroundDelay = 1f;

    [field: SerializeField] public RuntimeAnimatorController p_NormalAnimator { get; protected set; }
    [field: SerializeField] public RuntimeAnimatorController p_AttackAnimator { get; protected set; }



    // Member Variables
    public bool m_FoundPlayer { get; set; } = false;
    public Enemy_Rotation m_EnemyRotation { get; private set; }
    public WeaponMgr m_WeaponMgr { get; private set; }
    private Animator m_Animator;

    private IDLE_NormalGang m_IDLE;
    private FOLLOW_NormalGang m_FOLLOW;
    private ATTACK_NormalGang m_ATTACK;
    private STUN_NormalGang m_Stun;
    private DEAD_NormalGang m_Dead;

    public bool m_IsFoundPlayer = false;
    public int m_AngleBetPlayer { get; protected set; } // 위에서부터 0, 1, 2
    public Player m_Player { get; private set; }
    private Vector2 m_DistBetPlayer;
    private Enemy_HotBox[] m_HotBoxes;


    // Constructor
    private void Awake()
    {
        InitHuman();
        InitEnemy();

        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_HotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Alert = GetComponentInChildren<Enemy_Alert>();
        m_EnemyUseRange = GetComponentInChildren<Enemy_UseRange>();
        m_EnemyLocationSensor = GetComponentInChildren<LocationSensor>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_EnemyRotation = GetComponentInChildren<Enemy_Rotation>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        
        m_IDLE = new IDLE_NormalGang(this);
        m_FOLLOW = new FOLLOW_NormalGang(this);
        m_ATTACK = new ATTACK_NormalGang(this);
        m_Stun = new STUN_NormalGang(this);
        m_Dead = new DEAD_NormalGang(this);

        m_Alert.GetComponent<Animator>().SetFloat("AlertSpeed", p_AlertSpeedRatio);
        
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;
    }

    private void Start()
    {
        m_CurEnemyFSM.StartState();

        
        m_OriginPos = transform.position;

        var instance = InstanceMgr.GetInstance();
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerTransform = m_Player.p_Player_RealPos;
        m_PlayerLocationSensor = m_Player.m_PlayerLocationSensor;
    }


    // Updates
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr) 
            return;
        
        p_Hp = _mgr.N_HP;
        p_Speed = _mgr.N_Speed;
        p_StunSpeed = _mgr.N_StunTime;
        p_stunThreshold = _mgr.N_StunThreshold;
        p_VisionDistance = _mgr.N_Vision_Distance;
        p_MinFollowDistance = _mgr.N_GunFire_Distance;
        p_CloseAttackDistance = _mgr.N_MeleeAttack_Distance;
        p_AlertSpeedRatio = _mgr.N_AlertSpeedRatio;
        #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
        #endif
    }
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;
        
        p_Hp -= _damage * (_point == HitBoxPoint.HEAD ? 2 : 1);
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            if(_point == HitBoxPoint.HEAD)
                m_Animator.Play("Head");
            else if(_point == HitBoxPoint.BODY)
                m_Animator.Play("Body");
            
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }
        
        
        if (m_CurStunValue >= p_stunThreshold)
        {
            m_CurStunValue = 0;
            ChangeEnemyFSM(EnemyStateName.STUN);
        }
    }
    public void ChangeAnimator(bool _isNormal)
    {
        m_Animator.runtimeAnimatorController = _isNormal ? p_NormalAnimator : p_AttackAnimator;
    }
    public void CalculateAngleBetPlayer()
    {
        m_AngleBetPlayer = StaticMethods.getAnglePhase(p_GunPos.position,
            m_PlayerTransform.position, 3, p_AngleLimit);
    }
    public Vector2 GetDistBetPlayer()
    {
        return new Vector2(transform.position.x - m_PlayerTransform.position.x,
            transform.position.y - m_PlayerTransform.position.y);
    }
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        //Debug.Log("상태 전이" + _name);
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
            
            case EnemyStateName.STUN:
                m_CurEnemyFSM = m_Stun;
                break;
            
            case EnemyStateName.DEAD:
                m_CurEnemyFSM = m_Dead;
                break; 
            
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
}