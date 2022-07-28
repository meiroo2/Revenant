using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_MeleeAttackDistance { get; protected set; } = 0.2f;
    [field: SerializeField] public bool p_IsLookAround = false;
    [field: SerializeField] public float p_LookAroundDelay = 1f;
    [Range(0.0f, 1.0f)] public float p_AttackTiming;

    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }
    private IHotBox[] m_HotBoxes;

    private IdleMeleeGang m_IDLE;
    private FollowMeleeGang m_FOLLOW;
    private AttackMeleeGang m_ATTACK;
    private DeadMeleeGang m_DEAD;

    public bool m_IsFoundPlayer = false;
    private Vector2 m_DistBetPlayer;

    private bool m_IsChangeing = false;


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
        
        m_CurEnemyFSM = new IdleMeleeGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();
        
        m_IDLE = new IdleMeleeGang(this);
        m_FOLLOW = new FollowMeleeGang(this);
        m_ATTACK = new AttackMeleeGang(this);
        m_DEAD = new DeadMeleeGang(this);
    }

    private void Start()
    {
        m_OriginPos = transform.position;

        Player tempPlayer = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerTransform = tempPlayer.p_Player_RealPos;
        m_PlayerLocationSensor = tempPlayer.m_PlayerLocationSensor;
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
        
        p_Hp = _mgr.M_HP;
        p_Speed = _mgr.M_Speed;
        p_StunSpeed = _mgr.M_StunTime;
        p_stunThreshold = _mgr.M_StunThreshold;
        p_VisionDistance = _mgr.M_Vision_Distance;
        p_MeleeAttackDistance = _mgr.M_MeleeAttack_Distance;
        p_AttackTiming = _mgr.M_PointAttackTime;
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

            foreach (Enemy_HotBox ele in m_HotBoxes)
            {
                ele.gameObject.SetActive(false);
            }
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }

        if (m_CurStunValue >= p_stunThreshold)
        {
            m_CurStunValue = 0;
            ChangeEnemyFSM(EnemyStateName.STUN);
        }
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
            
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
}