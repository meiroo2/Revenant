using System;
using FMOD;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;
using FixedUpdate = Unity.VisualScripting.FixedUpdate;


public class Drone : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_RushSpeedRatio { get; protected set; } = 2f;
    [field: SerializeField] public float p_ToRushDistance { get; protected set; } = 2f;
    [field: SerializeField] public float p_WiggleSpeed { get; protected set; } = 1f;
    [field: SerializeField] public float p_WigglePower { get; protected set; } = 1f;
    [field: SerializeField] public int p_BombHp { get; protected set; } = 20;
    [field: SerializeField] public float p_BombRange { get; protected set; } = 2f;
    
    
    // Member Variables
    private FOLLOW_Drone m_FOLLOW;
    private RUSH_Drone m_RUSH;
    
    private float m_SinYValue = 0;
    private float m_Timer = 0;
    
    
    
    // Constructors
    public void Awake()
    {
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        
        m_FOLLOW = new FOLLOW_Drone(this);
        m_RUSH = new RUSH_Drone(this);

        m_CurEnemyStateName = EnemyStateName.FOLLOW;
        m_CurEnemyFSM = m_FOLLOW;
        m_CurEnemyFSM.StartState();
    }

    public void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_PlayerTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.p_Player_RealPos;
    }
    
    // Updates
    public void Update()
    {
        transform.position = StaticMethods.getPixelPerfectPos(transform.position);
    }

    public void FixedUpdate()
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
            case EnemyStateName.FOLLOW:
                m_CurEnemyFSM = m_FOLLOW;
                break;
            
            case EnemyStateName.RUSH:
                m_CurEnemyFSM = m_RUSH;
                break;

            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
    
    public override void MoveByDirection_FUpdate(bool _isRight)
    {
        m_Timer += Time.deltaTime;
        m_SinYValue = Mathf.Sin(m_Timer * p_WiggleSpeed) * p_WigglePower;
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = new Vector2(1, m_SinYValue) * (p_Speed * Time.deltaTime);
            
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = new Vector2(-1, m_SinYValue) * (p_Speed * Time.deltaTime);
        }
    }
    public override void ResetMovePoint(Vector2 _destinationPos)
    {
        m_Timer = 0;
        m_MovePoint = (_destinationPos - (Vector2)transform.position).normalized;
        m_MovePoint = StaticMethods.getLPerpVec(m_MovePoint);
        m_EnemyRigid.velocity = m_MovePoint * 1.5f;
    }
    public override void MoveToPoint_FUpdate()
    {
        //m_EnemyRigid.velocity = m_MovePoint * (p_Speed * p_RushSpeedRatio * Time.deltaTime);
    }
}