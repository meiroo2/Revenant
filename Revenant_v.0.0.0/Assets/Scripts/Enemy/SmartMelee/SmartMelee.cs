using System.Collections;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;


public enum TraceType
{
    Fastest = 0,
    BackDoor
}
public class SmartMelee : BasicEnemy
{
    // Visible Member Variables
    [BoxGroup("SmartTrace")] public TraceType p_TraceType;
     
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_MeleeDistance { get; protected set; } = 0.2f;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public bool p_IsLookAround = false;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_LookAroundDelay = 1f;
    [field: SerializeField, BoxGroup("MeleeGang Values"),Range(0.0f, 1.0f)] public float p_AttackTiming;
    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_FollowSpeedMulti;

    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_DelayAfterAttack = 0.1f;

    [field: SerializeField, BoxGroup("MeleeGang Values")] public float p_StunWaitTime { get; private set; } = 0.5f;
    
    [field: SerializeField] public RuntimeAnimatorController p_NormalAniCont;
    [field: SerializeField] public RuntimeAnimatorController p_FightAniCont;
    [field: SerializeField, Space(10f)] public Enemy_HotBox p_HeadBox;
    [field: SerializeField] public Enemy_HotBox p_BodyBox;

    public SectionManager p_SectionManager;
    public SectionSensor p_SectionSensor;
    public SmartMelee_UseRange p_SmartUseRange;
    
    
    
    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }
    private IHotBox[] m_HotBoxes;

    private IDLE_SmartMelee m_IDLE;
    private FOLLOW_SmartMelee m_FOLLOW;


    private Vector2 m_DistBetPlayer;
    
    public Player m_Player { get; private set; }

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

        m_IDLE = new IDLE_SmartMelee(this);
        m_FOLLOW = new FOLLOW_SmartMelee(this);
        
        m_CurEnemyFSM = m_FOLLOW;
        m_CurEnemyStateName = EnemyStateName.FOLLOW;

        m_Animator.runtimeAnimatorController = p_NormalAniCont;
    }

    private void Start()
    {
        m_OriginPos = transform.position;

        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_PlayerTransform = m_Player.transform;
        m_PlayerLocationSensor = m_Player.m_PlayerLocationSensor;
        
        m_CurEnemyFSM.StartState();
    }


    // Updates
    private void Update()
    {
        m_CurEnemyFSM.UpdateState();
    }


    // Functions

    [Button]
    public void StartTrace()
    {
        
    }

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

    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;
        
        
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

    public void ResetStunValue()
    {
        m_CurStunValue = 0;
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
            
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
}