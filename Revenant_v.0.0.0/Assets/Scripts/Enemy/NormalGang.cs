using System;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;


public class NormalGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_MinFollowDistance { get; protected set; } = 0.2f;
    [field: SerializeField] public Transform p_GunPos { get; protected set; }

    // Member Variables
    private IDLE_NormalGang m_IDLE;
    private WALK_NormalGang m_WALK;
    private ATTACK_NormalGang m_ATTACK;

    public int m_AngleBetPlayer { get; protected set; } // 위에서부터 0, 1, 2
    private Vector2 m_DistBetPlayer;


    // Constructor
    private void Awake()
    {
        m_CurEnemyFSM = new IDLE_NormalGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();
        
        m_EnemyRigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_originPos = transform.position;
        m_PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player
            .p_Player_RealPos;
        m_IDLE = new IDLE_NormalGang(this);
        m_WALK = new WALK_NormalGang(this);
        m_ATTACK = new ATTACK_NormalGang(this);
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
    public void CalculateAngleBetPlayer()
    {
        m_AngleBetPlayer = StaticMethods.getAnglePhase(p_GunPos.position,
            m_PlayerTransform.position, 3, 20);
    }
    public Vector2 GetDistBetPlayer()
    {
        return new Vector2(transform.position.x - m_PlayerTransform.position.x,
            transform.position.y - m_PlayerTransform.position.y);
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
            
            case EnemyStateName.WALK:
                m_CurEnemyFSM = m_WALK;
                break;
            
            case EnemyStateName.ATTACK:
                m_CurEnemyFSM = m_ATTACK;
                break;
            
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
}