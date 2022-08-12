using Sirenix.OdinInspector;
using UnityEngine;


public class ShieldGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public int p_Shield_Hp = 10;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_AttackDistance = 0.4f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_AttackSpeed = 1f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public int p_AttackDamage = 10;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_GapDistance = 1f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_BackMoveSpeedRatio = 0.8f;
    
    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_BrokenStateSpeedRatio = 1.5f;
    
    
    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }

    private IDLE_ShieldGang m_IDLE;
    private FOLLOW_ShieldGang m_FOLLOW;
    private ATTACK_ShieldGang m_ATTACK;
    private DEAD_ShieldGang m_DEAD;
    private CHANGE_ShieldGang m_CHANGE;
    private ROTATION_ShieldGang m_ROTATION;
    private STUN_ShieldGang m_STUN;
    private BREAK_ShieldGang m_BREAK;
    
    public Shield m_Shield { get; private set; }
    public bool m_IsFoundPlayer = false;
    public bool m_IsShieldBroken { get; private set; } = false;


    // Constructor
    private void Awake()
    {
        InitHuman();
        m_Shield = GetComponentInChildren<Shield>();
        m_Shield.p_Shield_Hp = p_Shield_Hp;
        
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();

        m_CurEnemyFSM = new IDLE_ShieldGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();

        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
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
        m_BREAK = new BREAK_ShieldGang(this);

        // Weapon Value Initiate
        m_WeaponMgr.m_CurWeapon.p_BulletDamage = p_AttackDamage;
    }


    // Updates
    private void Update()
    {
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public void ShieldBroken()
    {
        m_IsShieldBroken = true;
        ChangeEnemyFSM(EnemyStateName.BREAK);
    }
    public override void MoveByDirection(bool _isRight)
    {
        if (!m_IsShieldBroken)
        {
            float moveSpeed = p_MoveSpeed;

            if (m_IsRightHeaded)
            {
                if (GetIsLeftThenPlayer() == !_isRight)
                    moveSpeed *= p_BackMoveSpeedRatio;
            }
            else
            {
                if (GetIsLeftThenPlayer() == _isRight)
                    moveSpeed *= p_BackMoveSpeedRatio;
            }

            if (_isRight)
            {
                m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (moveSpeed);
            }
            else
            {
                m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (moveSpeed);
            }
        }
        else
        {
            if (_isRight)
            {
                if(!m_IsRightHeaded)
                    setisRightHeaded(true);

                m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed * p_BrokenStateSpeedRatio);
            }
            else
            {
                if(m_IsRightHeaded)
                    setisRightHeaded(false);
            
                m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed * p_BrokenStateSpeedRatio);
            }
        }
    }
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log(gameObject.name + "의 상태 전이" + _name);
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
            
            case EnemyStateName.BREAK:
                m_CurEnemyFSM = m_BREAK;
                break;

            default:
                Debug.Log("ERR : 존재하지 않는 상태 전이 요청");
                break;
        }

        m_CurEnemyFSM.StartState();
    }
    public override void StartPlayerCognition()
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD && m_PlayerCognition) 
            return;
        
        Debug.Log(gameObject.name + "이 플레이어를 인지합니다.");
        ChangeEnemyFSM(EnemyStateName.FOLLOW);
    }
}
