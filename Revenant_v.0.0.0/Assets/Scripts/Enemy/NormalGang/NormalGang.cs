using System;
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;


public class NormalGang : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_AttackDelay = 0.5f;
    [field: SerializeField] public float p_CloseAttackDistance = 0.1f;
    [field: SerializeField] public Transform p_GunPos { get; protected set; }
    [field: SerializeField] public float p_MinFollowDistance { get; protected set; } = 0.2f;
    [field: SerializeField] public Transform[] p_PatrolPos { get; protected set; }

    [field: SerializeField] public bool p_IsLookAround = false;
    [field: SerializeField] public float p_LookAroundDelay = 1f;



    // Member Variables
    public Enemy_Rotation m_EnemyRotation { get; private set; }
    public WeaponMgr m_WeaponMgr { get; private set; }

    private IDLE_NormalGang m_IDLE;
    private WALK_NormalGang m_WALK;
    private ATTACK_NormalGang m_ATTACK;
    private STUN_NormalGang m_Stun;
    private DEAD_NormalGang m_Dead;

    public bool m_IsFoundPlayer = false;
    public int m_AngleBetPlayer { get; protected set; } // 위에서부터 0, 1, 2
    private Vector2 m_DistBetPlayer;


    // Constructor
    private void Awake()
    {
        m_EnemyUseRange = GetComponentInChildren<Enemy_UseRange>();
        m_EnemyLocationSensor = GetComponentInChildren<LocationSensor>();
        m_EnemyLocationInfo = GetComponentInChildren<LocationInfo>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_EnemyRotation = GetComponentInChildren<Enemy_Rotation>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        
        m_CurEnemyFSM = new IDLE_NormalGang(this);
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();
        m_EnemyRigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_OriginPos = transform.position;

        Player tempPlayer = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerTransform = tempPlayer.p_Player_RealPos;
        
        m_PlayerLocationInfo = tempPlayer.m_PlayerLocationInfo;
        m_PlayerLocationSensor = tempPlayer.m_PlayerLocationSensor;
        
        m_IDLE = new IDLE_NormalGang(this);
        m_WALK = new WALK_NormalGang(this);
        m_ATTACK = new ATTACK_NormalGang(this);
        m_Stun = new STUN_NormalGang(this);
        m_Dead = new DEAD_NormalGang(this);
        
        
    }

    public IEnumerator Update2()
    {
        while (true)
        {
            
            yield return null;
        }
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
    public override void ResetMovePoint(Vector2 _destinationPos)
    {
        m_MovePoint = _destinationPos;
    }
    public override void MoveToPoint_FUpdate()
    {
        if (m_MovePoint.x > transform.position.x)
        {
            MoveByDirection_FUpdate(true);
        }
        else
        {
            MoveByDirection_FUpdate(false);
        }
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