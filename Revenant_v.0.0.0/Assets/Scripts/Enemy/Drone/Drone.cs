using System;
using System.Collections;
using FMOD;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using FixedUpdate = Unity.VisualScripting.FixedUpdate;


public class Drone : BasicEnemy
{
    // Visible Member Variables
    [field: SerializeField] public float p_AlertSpeedRatio = 1f;
    [field: SerializeField] public float p_RushSpeedRatio { get; protected set; } = 2f;
    [field: SerializeField] public float p_ToRushXDistance { get; protected set; } = 2f;
    [field: SerializeField] public float p_WiggleSpeed { get; protected set; } = 1f;
    [field: SerializeField] public float p_WigglePower { get; protected set; } = 1f;
    [field: SerializeField] public int p_BombHp { get; protected set; } = 20;
    [field: SerializeField] public float p_BombRange { get; protected set; } = 2f;
    public float p_BreakPower = 1f;

    [Space(20f)] [Header("확인용 변수 모음")] 
    public int m_DeadReason = -1;
    public BombWeapon_Enemy m_BombWeapon;
    
    
    // FSMs
    private IDLE_Drone m_IDLE;
    private FOLLOW_Drone m_FOLLOW;
    private RUSH_Drone m_RUSH;
    private DEAD_Drone m_DEAD;
    
    
    // Member Variables
    public delegate void CoroutineDelegate();
    private CoroutineDelegate m_CoroutineCallback = null;
    
    public Player m_Player { get; private set; }
    private Enemy_HotBox[] m_HotBoxes;
    public Enemy_WeaponMgr m_WeponMgr;
    public Drone_CrashCol m_CrashCol { get; private set; }


    private Coroutine m_Coroutine;
    private Vector2 m_RushMoveVec;
    
    private float m_SinYValue = 0;
    private float m_Timer = 0;
    
    public delegate void UpdateDelegate();
    private UpdateDelegate m_Callback = null;
    


    // Constructors
    public void Awake()
    {
        InitHuman();
        InitEnemy();
        
        
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        
        m_DeadReason = -1;
        
        m_HotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_WeponMgr = GetComponentInChildren<Enemy_WeaponMgr>();
        m_CrashCol = GetComponentInChildren<Drone_CrashCol>();
        

        m_RushMoveVec = Vector2.zero;
        m_EnemyHotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();

        m_IDLE = new IDLE_Drone(this);
        m_FOLLOW = new FOLLOW_Drone(this);
        m_RUSH = new RUSH_Drone(this);
        m_DEAD = new DEAD_Drone(this);
        
        m_CurEnemyStateName = EnemyStateName.FOLLOW;
        m_CurEnemyFSM = m_FOLLOW;
    }

    public void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerTransform = m_Player.p_Player_RealPos;
        
        m_CurEnemyFSM.StartState();
    }
    
    // Updates
    public void Update()
    {
        if (m_ObjectState == ObjectState.Pause)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public void SetCoroutineCallback(CoroutineDelegate _input)
    {
        m_CoroutineCallback = _input;
    }
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        p_Hp = _mgr.D_HP;
        p_Speed = _mgr.D_Speed;
        p_StunSpeed = _mgr.D_StunTime;
        p_stunThreshold = _mgr.D_StunThreshold;
        p_AlertSpeedRatio = _mgr.D_AlertSpeedRatio;
        p_RushSpeedRatio = _mgr.D_RushSpeedRatio;
        p_ToRushXDistance = _mgr.D_ToRush_Distance;
        p_WiggleSpeed = _mgr.D_WiggleSpeed;
        p_WigglePower = _mgr.D_WigglePower;
        m_BombWeapon.p_BombRadius = _mgr.D_BombRadius;

        #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
        #endif
    }

    public void SetUpdateCallback(UpdateDelegate _input)
    {
        m_Callback = _input;
    }

    public void ResetCallback()
    {
        m_Callback = null;
    }
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;
        
        p_Hp -= _damage * (_point == HitBoxPoint.HEAD ? 2 : 1);
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            if (_point == HitBoxPoint.HEAD)
            {
                m_DeadReason = 0;
            }
            else if (_point == HitBoxPoint.BODY)
            {
                m_DeadReason = 1;
            }

            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }
    }
    public void SetHitBox(bool _on)
    {
        foreach (Enemy_HotBox ele in m_HotBoxes)
        {
            ele.gameObject.SetActive(_on);
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
            
            case EnemyStateName.RUSH:
                m_CurEnemyFSM = m_RUSH;
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
    
    /// <summary>파라미터에 기반하여 왼쪽 혹은 오른쪽 방향대로 velocity를 수정합니다.</summary>
    public override void MoveByDirection(bool _isRight)
    {
        Debug.Log("디렉션 호출");
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);
            
            m_EnemyRigid.velocity = Vector2.right * p_Speed;
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = Vector2.left * p_Speed;
        }
    }
    
    public override void ResetMovePoint(Vector2 _destinationPos)
    {
        m_MovePoint = (_destinationPos - (Vector2)transform.position).normalized;
    }
    
    public override void MoveToPoint_FUpdate()
    {
        m_EnemyRigid.velocity = m_MovePoint * (p_Speed * p_RushSpeedRatio);
    }

    public void VelocityBreak()
    {
        StartCoroutine(VelocitySlowDown());
    }

    private IEnumerator VelocitySlowDown()
    {
        Vector2 velocity = m_EnemyRigid.velocity;
        float power = 1f;
        
        while (true)
        {
            m_EnemyRigid.velocity = velocity * power;
            power -= Time.deltaTime * p_BreakPower;
            
            if (power <= 0f)
            {
                m_EnemyRigid.velocity = Vector2.zero;
                if (!ReferenceEquals(m_CoroutineCallback, null))
                {
                    m_CoroutineCallback();
                    m_CoroutineCallback = null;
                }

                yield break;
            }
            yield return null;
        }
    }
    
    
    /*
     public override void MoveByDirection_FUpdate(bool _isRight)
    {
        m_Timer += Time.deltaTime;
        m_SinYValue = Mathf.Sin(m_Timer * p_WiggleSpeed) * p_WigglePower;
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);
            
            //m_EnemyRigid.velocity = new Vector2(1, m_SinYValue).normalized * (p_Speed);
            m_EnemyRigid.velocity = Vector2.right * p_Speed;
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            //m_EnemyRigid.velocity = new Vector2(-1, m_SinYValue).normalized * (p_Speed);
            m_EnemyRigid.velocity = Vector2.left * p_Speed;
        }
    }
     */
}