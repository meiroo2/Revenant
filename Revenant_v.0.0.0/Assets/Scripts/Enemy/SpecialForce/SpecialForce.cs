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


public class SpecialForce : BasicEnemy, ISpriteMatChange
{
    // Visible Member Variables
    
    // Basic
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Bullet_Speed { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Bullet_Spread { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_FireAnimSpeed { get; private set; } = 3f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Fire_Delay { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_MeleeRollDistance { get; private set; } = 0.4f;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_HeadDmgMulti { get; private set; } = 2;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_BodyDmgMulti { get; private set; } = 1;
    
    // Advanced
    [field: SerializeField, BoxGroup("SpecialForce"), PropertySpace(10f, 0f)] 
    public Vector2 p_Roll_Tick { get; private set; }

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Refresh { get; private set; } = 0.5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public int p_Roll_Chance { get; private set; } = 50;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Cooldown { get; private set; } = 5f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Roll_Speed_Multi { get; private set; } = 3f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_RunSpeedMulti = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_GapDistance = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public bool p_IsLookAround = false;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_LookAroundDelay = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public Transform[] p_PatrolPos { get; protected set; } = null;
    
    
    // Roll
    [field: SerializeField, BoxGroup("SpecialForce"), Title("For Hide")] public float p_Hide_Chance = 10f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Find_Distance = 1f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Time_Min = 1f;

    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Hide_Time_Cancel = 1f;
    
    
    // AlertSystem
    [field: SerializeField, BoxGroup("SpecialForce"), Title("AlertSystem")] public float p_Alert_Fade_Speed = 1f;
    [field: SerializeField, BoxGroup("SpecialForce")] public float p_Alert_Speed = 1f;
    
    // Assign
    [field: SerializeField, Title("Assign")]
    public AlertSystem p_AlertSystem;
    
    public SpriteRenderer p_FullRenderer;
    public Animator p_FullAnimator;
    
    public SpriteRenderer p_BodyRenderer;
    public Animator p_BodyAnimator;

    public SpriteRenderer p_ArmRenderer;
    public Animator p_ArmAnimator;

    public SpriteRenderer p_LegRenderer;
    public Animator p_LegAnimator;

    public RuntimeAnimatorController p_NormalFullCont;
    public RuntimeAnimatorController p_FightFullCont;
    
    [field: SerializeField] public Enemy_HotBox[] p_HitHotboxes { get; private set; }
    public SingleRifle p_SingleRifleWeapon;
    public Enemy_HotBox p_HeadHotBox;
    public Enemy_HotBox p_BodyHotBox;
    
    

    // Member Variables
    [HideInInspector] public bool m_UseHide = false;
    [HideInInspector] public bool m_IsMouseTouched = false;
    public bool m_GlobalRollCooldown { get; private set; } = false;
    private Coroutine m_RollCooldownCoroutine;
    
    private bool m_FSMLock = false;
    private SpecialForce_IDLE m_IDLE;
    private SpecialForce_PATROL m_PATROL;
    private SpecialForce_FOLLOW m_FOLLOW;
    private SpecialForce_STUN m_STUN;
    private SpecialForce_DEAD m_DEAD;
    private SpecialForce_ROLL m_ROLL;
    private SpecialForce_ATTACK m_ATTACK;
    private SpecialForce_HIDDEN m_HIDDEN;

    private int m_SpriteMode = 0;
    
    public Player m_Player { get; private set; }
    public WeaponMgr m_WeaponMgr { get; private set; }

    public CoroutineHandler m_CoroutineHandler { get; private set; }
    
    public MeleeWeapon_Enemy m_MeleeWeapon { get; private set; }
    public SpecialForce_UseRange m_UseRange { get; private set; }
    public HideSlot m_CurHideSlot = null;
    public bool m_LastActionIsHide = false;

    
    // FSM Variables
    [HideInInspector] public int m_ATK_RollState = 0;
    

    // Constructors
    private void Awake()
    {
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        m_MeleeWeapon = GetComponentInChildren<MeleeWeapon_Enemy>();
        m_UseRange = GetComponentInChildren<SpecialForce_UseRange>();
        m_EnemyHotBoxes = p_HitHotboxes;
        InitHuman();
        m_Animator = p_FullAnimator;
        
        m_IDLE = new SpecialForce_IDLE(this);
        m_PATROL = new SpecialForce_PATROL(this);
        m_FOLLOW = new SpecialForce_FOLLOW(this);
        m_STUN = new SpecialForce_STUN(this);
        m_DEAD = new SpecialForce_DEAD(this);
        m_ROLL = new SpecialForce_ROLL(this);
        m_ATTACK = new SpecialForce_ATTACK(this);
        m_HIDDEN = new SpecialForce_HIDDEN(this);
        
        p_HeadHotBox.p_DamageMulti = p_HeadDmgMulti;
        p_BodyHotBox.p_DamageMulti = p_BodyDmgMulti;
        
        m_PlayerCognition = false;
        m_SpriteMode = 0;
        
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;

        m_EnemyIdx = 4;
        
        m_Renderer = p_FullRenderer;
        InitISpriteMatChange();
    }

    private void Start()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;

        var instance = InstanceMgr.GetInstance();
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();

        SetSpriteMode(0);
        
        m_CurEnemyFSM.StartState();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
    }


    // Updates
    private void FixedUpdate()
    {
        if (m_FSMLock)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    
    /// <summary>
    /// SpecialForce의 Sprite Mode를 변경합니다.
    /// 0 = 전신노말, 1 = 전신전투, 2 = 상하분리
    /// </summary>
    /// <param name="_num"></param>
    public void SetSpriteMode(int _num)
    {
        switch (_num)
        {
            case 0:
                m_SpriteMode = _num;
                p_FullRenderer.enabled = true;
                p_FullAnimator.runtimeAnimatorController = p_NormalFullCont;
                p_BodyRenderer.enabled = false;
                p_ArmRenderer.enabled = false;
                p_LegRenderer.enabled = false;
                break;
            
            case 1:
                m_SpriteMode = _num;
                p_FullRenderer.enabled = true;
                p_FullAnimator.runtimeAnimatorController = p_FightFullCont;
                p_BodyRenderer.enabled = false;
                p_ArmRenderer.enabled = false;
                p_LegRenderer.enabled = false;
                break;
            
            case 2:
                m_SpriteMode = _num;
                p_FullRenderer.enabled = false;
                p_BodyRenderer.enabled = true;
                p_ArmRenderer.enabled = true;
                p_LegRenderer.enabled = true;
                break;
        }
    }
    
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        p_Hp = _mgr.SF_Hp;
        p_SingleRifleWeapon.p_BulletDamage = _mgr.SF_BulletDamage;
        p_Bullet_Speed = _mgr.SF_BulletSpeed;
        p_SingleRifleWeapon.p_BulletSpeed = p_Bullet_Speed;
        p_SingleRifleWeapon.p_BulletSpread = _mgr.SF_BulletSpread;
        p_FireAnimSpeed = _mgr.SF_FireAnimSpeed;
        p_Fire_Delay = _mgr.SF_FireDelay;
        p_MoveSpeed = _mgr.SF_MoveSpeed;
        p_RunSpeedMulti = _mgr.SF_RunSpeedMulti;
        p_StunAlertSpeed = _mgr.SF_StunAlertSpeed;
        p_StunHp = _mgr.SF_StunThreshold;
        p_VisionDistance = _mgr.SF_VisionDistance;
        p_AtkDistance = _mgr.SF_AttackDistance;
        p_MeleeRollDistance = _mgr.SF_MeleeRollDistance;
        p_GapDistance = _mgr.SF_GapDistance;

        p_HeadDmgMulti = _mgr.SF_HeadDmgMulti;
        p_HeadHotBox.p_DamageMulti = p_HeadDmgMulti;
        
        p_BodyDmgMulti = _mgr.SF_BodyDmgMulti;
        p_BodyHotBox.p_DamageMulti = p_BodyDmgMulti;
        
        
        p_Roll_Refresh = _mgr.SF_Roll_Refresh;
        p_Roll_Tick = _mgr.SF_Roll_Tick;
        p_Roll_Chance = _mgr.SF_Roll_Chance;
        p_Roll_Cooldown = _mgr.SF_Roll_Cooldown;
        p_Roll_Speed_Multi = _mgr.SF_Roll_Speed_Multi;

        p_Alert_Speed = _mgr.SF_AlertSpeed;
        p_Alert_Fade_Speed = _mgr.SF_AlertFadeSpeed;

        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(p_HeadHotBox);
            EditorUtility.SetDirty(p_BodyHotBox);
            EditorUtility.SetDirty(p_SingleRifleWeapon);
        #endif
    }

    public override void SetRigidToPoint(float _addSpeed = 1f)
    {
        SetRigidByDirection(m_MovePoint.x > transform.position.x, _addSpeed);
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
        Debug.Log("Touch");
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
            
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }

        if (m_CurEnemyStateName != EnemyStateName.STUN)
        {
            if (m_CurStunValue >= p_StunHp)
            {
                m_CurStunValue = 0;
                ChangeEnemyFSM(EnemyStateName.STUN);
                return;
            }
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
            
            case EnemyStateName.PATROL:
                m_CurEnemyFSM.ExitState();
                m_CurEnemyFSM = m_PATROL;
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
                p_BodyRenderer.material = p_OriginalMat;
                p_ArmRenderer.material = p_OriginalMat;
                p_LegRenderer.material = p_OriginalMat;
                break;

            case SpriteMatType.BnW:
                m_CurSpriteMatType = SpriteMatType.BnW;
                m_Renderer.material = p_BnWMat;
                p_BodyRenderer.material = p_BnWMat;
                p_ArmRenderer.material = p_BnWMat;
                p_LegRenderer.material = p_BnWMat;
                break;

            case SpriteMatType.REDHOLO:
                m_CurSpriteMatType = SpriteMatType.REDHOLO;
                m_MatTimeCoroutine = StartCoroutine(MatTimeInput());
                m_Renderer.material = p_RedHoloMat;
                p_BodyRenderer.material = p_RedHoloMat;
                p_ArmRenderer.material = p_RedHoloMat;
                p_LegRenderer.material = p_RedHoloMat;
                break;

            case SpriteMatType.DISAPPEAR:
                m_CurSpriteMatType = SpriteMatType.DISAPPEAR;
                m_Renderer.material = p_DisappearMat;
                p_BodyRenderer.material = p_DisappearMat;
                p_ArmRenderer.material = p_DisappearMat;
                p_LegRenderer.material = p_DisappearMat;
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
            p_BodyRenderer.material.SetFloat(ManualTimer, timer);
            p_ArmRenderer.material.SetFloat(ManualTimer, timer);
            p_LegRenderer.material .SetFloat(ManualTimer, timer);
            yield return null;
        }
    }
}





















