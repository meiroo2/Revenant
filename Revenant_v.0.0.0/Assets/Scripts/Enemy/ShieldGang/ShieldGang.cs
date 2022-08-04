using UnityEngine;


public class ShieldGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_MinFollowDistance { get; protected set; } = 0.2f;


    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }

    private IDLE_ShieldGang m_IDLE;
    private FOLLOW_ShieldGang m_FOLLOW;
    private ATTACK_ShieldGang m_ATTACK;
    private DEAD_ShieldGang m_DEAD;
    private CHANGE_ShieldGang m_CHANGE;
    private ROTATION_ShieldGang m_ROTATION;
    private STUN_ShieldGang m_STUN;

    private Shield m_Shield;


    public bool m_IsFoundPlayer = false;
    public bool m_IsShield = true;
    private Vector2 m_DistBetPlayer;


    // Constructor
    private void Awake()
    {
        m_Shield = GetComponentInChildren<Shield>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();

        m_CurEnemyFSM = new IDLE_ShieldGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();

        m_EnemyRigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_OriginPos = transform.position;
        m_PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player
            .transform;

        m_IDLE = new IDLE_ShieldGang(this);
        m_FOLLOW = new FOLLOW_ShieldGang(this);
        m_ATTACK = new ATTACK_ShieldGang(this);
        m_CHANGE = new CHANGE_ShieldGang(this);
        m_ROTATION = new ROTATION_ShieldGang(this);
        m_DEAD = new DEAD_ShieldGang(this);
        m_STUN = new STUN_ShieldGang(this);
    }


    // Updates
    private void Update()
    {
        transform.position = StaticMethods.getPixelPerfectPos(transform.position);
    }

    private void FixedUpdate()
    {
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
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
            
            case EnemyStateName.ROTATION:
                m_CurEnemyFSM = m_ROTATION;
                break;
            
            case EnemyStateName.CHANGE:
                m_CurEnemyFSM = m_CHANGE;
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
