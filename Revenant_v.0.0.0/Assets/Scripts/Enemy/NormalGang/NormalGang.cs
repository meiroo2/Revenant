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


public class NormalGang : BasicEnemy, ISpriteMatChange
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("NormalGang Values")]
    public float p_AlertSpeed = 1f;

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public float p_MeleeDistance = 0.1f;

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public float p_MeleePointAtkTime = 0.5f;
    
    [field: SerializeField, BoxGroup("NormalGang Values")]
    public bool p_IsLookAround = false;

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public float p_LookAroundDelay = 1f;

    [field: SerializeField, BoxGroup("NormalGang Values"), PropertyOrder(0)]
    public Transform[] p_PatrolPos { get; protected set; } = null;


    [field: SerializeField, BoxGroup("NormalGang Values"), Space(20f)]
    public RuntimeAnimatorController p_NormalAnimator { get; protected set; }

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public RuntimeAnimatorController p_AttackAnimator { get; protected set; }

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public BasicWeapon_Enemy p_MeleeWeapon { get; private set; }

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public Enemy_HotBox p_HeadBox;

    [field: SerializeField, BoxGroup("NormalGang Values")]
    public Enemy_HotBox p_BodyBox;

    // Member Variables
    public Enemy_Rotation m_EnemyRotation { get; private set; }
    public WeaponMgr m_WeaponMgr { get; private set; }
    private Animator m_Animator;

    private IDLE_NormalGang m_IDLE;
    private FOLLOW_NormalGang m_FOLLOW;
    private ATTACK_NormalGang m_ATTACK;
    private STUN_NormalGang m_Stun;
    private DEAD_NormalGang m_Dead;
    public int m_AngleBetPlayer { get; protected set; } // 위에서부터 0, 1, 2
    public Player m_Player { get; private set; }
    private Vector2 m_DistBetPlayer;
    private Enemy_HotBox[] m_HotBoxes;
    public Transform m_GunPos { get; protected set; } = null;
    
    

    // Constructor
    private void Awake()
    {
        InitHuman();
        InitEnemy();

        m_GunPos = GetComponentInChildren<WeaponMgr>().transform;
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

        m_Alert.SetAlertSpeed(p_AlertSpeed);

        m_IDLE = new IDLE_NormalGang(this);
        m_FOLLOW = new FOLLOW_NormalGang(this);
        m_ATTACK = new ATTACK_NormalGang(this);
        m_Stun = new STUN_NormalGang(this);
        m_Dead = new DEAD_NormalGang(this);

        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;

        InitISpriteMatChange();
        
        m_EnemyIdx = 0;
    }

    private void Start()
    {
        m_CurEnemyFSM.StartState();

        m_OriginPos = transform.position;

        var instance = InstanceMgr.GetInstance();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_PlayerTransform = m_Player.transform;
        m_PlayerLocationSensor = m_Player.m_PlayerLocationSensor;
    }

    private void OnEnable()
    {
        m_Alert.SetAlertSpeed(p_AlertSpeed);
    }

    public override void InitEnemy()
    {
        base.InitEnemy();
    }


    // Updates
    private void Update()
    {
        if (m_ObjectState == ObjectState.Pause)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public override void StartPlayerCognition(bool _instant = false)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD || m_PlayerCognition)
            return;

        Debug.Log(gameObject.name + "이 플레이어를 인지합니다.");
        ChangeEnemyFSM(EnemyStateName.FOLLOW);
    }

    public override void SoundCognition(SoundHotBoxParam _param)
    {
        if (m_PlayerCognition)
            return;

        Debug.Log(gameObject.name + " 사운드 인지");
        StartPlayerCognition();
    }

    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        var tripleshot = GetComponentInChildren<TripleShot_Enemy>();

        p_Hp = _mgr.N_HP;
        p_MoveSpeed = _mgr.N_Speed;
        p_StunHp = _mgr.N_StunThreshold;
        p_VisionDistance = _mgr.N_Vision_Distance;
        p_AtkDistance = _mgr.N_GunFire_Distance;
        p_MeleeDistance = _mgr.N_MeleeAttack_Distance;
        p_AlertSpeed = _mgr.N_AlertSpeedMulti;
        p_StunAlertSpeed = _mgr.N_StunAlertSpeedMulti;
        p_HeadBox.p_DamageMulti = _mgr.N_HeadDmgMulti;
        p_BodyBox.p_DamageMulti = _mgr.N_BodyDmgMulti;

        tripleshot.p_BulletDamage = _mgr.N_BulletDamage;
        tripleshot.p_FireDelay = _mgr.N_FireDelay;
        tripleshot.p_BulletSpeed = _mgr.N_BulletSpeed;
        tripleshot.p_BulletRandomRotation = _mgr.N_BulletRandomRotation;


#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(p_HeadBox);
        EditorUtility.SetDirty(p_BodyBox);
        EditorUtility.SetDirty(tripleshot);
#endif
    }

    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            // 불릿타임으로 막타 맞아서 사망 시
            if (_weaponType == WeaponType.BULLET_TIME)
            {
                m_DeadReasonForMat = 1;
                p_OriginalMat = p_DisappearMat;
                
                ChangeMat(SpriteMatType.DISAPPEAR);
            }
            
            if (_point == HitBoxPoint.HEAD)
                m_Animator.Play("Head");
            else if (_point == HitBoxPoint.BODY)
                m_Animator.Play("Body");

            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }


        if (m_CurStunValue >= p_StunHp)
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
        m_AngleBetPlayer = StaticMethods.getAnglePhase(m_GunPos.position,
            m_PlayerTransform.position, 3, p_AngleLimit);
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

    // For MatChanger
    public bool m_IgnoreMatChanger { get; set; }
    public SpriteType m_SpriteType { get; set; }
    public SpriteMatType m_CurSpriteMatType { get; set; }
    public Material p_OriginalMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_BnWMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_RedHoloMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_DisappearMat { get; set; }

    private Coroutine m_MatTimeCoroutine = null;
    private readonly int ManualTimer = Shader.PropertyToID("_ManualTimer");

    public void ChangeMat(SpriteMatType _matType)
    {
        if (!ReferenceEquals(m_MatTimeCoroutine, null))
        {
            StopCoroutine(m_MatTimeCoroutine);
            m_MatTimeCoroutine = null;
        }

        if (m_IgnoreMatChanger || !gameObject.activeSelf)
            return;

        m_CurSpriteMatType = _matType;
        switch (_matType)
        {
            case SpriteMatType.ORIGIN:
                m_CurSpriteMatType = SpriteMatType.ORIGIN;
                m_Renderer.material = p_OriginalMat;
                break;

            case SpriteMatType.BnW:
                m_CurSpriteMatType = SpriteMatType.BnW;
                m_Renderer.material = p_BnWMat;
                break;

            case SpriteMatType.REDHOLO:
                m_CurSpriteMatType = SpriteMatType.REDHOLO;
                m_MatTimeCoroutine = StartCoroutine(MatTimeInput());
                m_Renderer.material = p_RedHoloMat;
                break;

            case SpriteMatType.DISAPPEAR:
                m_CurSpriteMatType = SpriteMatType.DISAPPEAR;
                m_Renderer.material = p_DisappearMat;
                break;
        }
    }
    
    public void InitISpriteMatChange()
    {
        m_SpriteType = SpriteType.ENEMY;
        m_CurSpriteMatType = SpriteMatType.ORIGIN;
        p_OriginalMat = m_Renderer.material;
        
        if(!p_BnWMat)
            Debug.Log("Info : ISpriteMat BnWMat Null");
        if(!p_RedHoloMat)
            Debug.Log("Info : ISpriteMat RedHoloMat Null");
        if(!p_DisappearMat)
            Debug.Log("Info : ISpriteMat DisappearMat Null");
    }

    private IEnumerator MatTimeInput()
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            m_Renderer.material.SetFloat(ManualTimer, timer);
            yield return null;
        }
    }
}