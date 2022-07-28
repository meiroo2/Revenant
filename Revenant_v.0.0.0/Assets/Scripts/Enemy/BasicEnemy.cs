using System.Collections;
using System;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Debug = UnityEngine.Debug;

public enum EnemyStateName
{
    IDLE,  
    WALK,
    FOLLOW,
    RUSH,
    ATTACK,
    STUN,
    ROTATION,
    CHANGE,
    DEAD
}

public class BasicEnemy : Human
{
    // Visible Member Variables
    [field: SerializeField] protected bool p_OverrideEnemyMgr = false;
    [field: SerializeField] public float p_VisionDistance { get; protected set; }
    [field: SerializeField] public float p_HearColSize { get; protected set; }
    [field: SerializeField] public int p_AngleLimit { get; protected set; } = 20;
    [field: SerializeField] public float p_AttackModeDelay { get; protected set; } = 0f;


    // Member Variables
    protected SpriteRenderer m_Renderer;
    private List<EnemySpawner> m_EnemySpawnerList = new List<EnemySpawner>();
    protected Enemy_HotBox[] m_EnemyHotBoxes;
    protected Enemy_UseRange m_EnemyUseRange;
    protected LocationSensor m_EnemyLocationSensor;
    protected LocationSensor m_PlayerLocationSensor;
    protected Enemy_FootMgr m_Foot;
    public Animator m_Animator { get; protected set; }
    public Enemy_Alert m_Alert { get; protected set; }
    public Transform m_PlayerTransform { get; protected set; }
    public Rigidbody2D m_EnemyRigid { get; protected set; }
    public RaycastHit2D m_VisionHit { get; protected set; }
    protected Enemy_FSM m_CurEnemyFSM;
    public EnemyStateName m_CurEnemyStateName { get; protected set; }

    protected Vector2 m_MovePoint;

    private Coroutine m_MatCoroutine;
    
    // Functions
    protected void InitEnemy()
    {
        m_EnemyHotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_Animator = GetComponentInChildren<Animator>();
    }
    public virtual void SetEnemyValues(EnemyMgr _mgr) { }

    public Vector2 GetMovePoint() { return m_MovePoint; }
    public void SendDeathAlarmToSpawner()
    {
        foreach (var ele in m_EnemySpawnerList)
        {
            ele.AchieveEnemyDeath(this.gameObject);
        }
    }

    public void AddEnemySpawner(EnemySpawner _spawner)
    {
        m_EnemySpawnerList.Add(_spawner);
    }
    
    /// <summary>현재 위치에서 플레이어의 위치를 뺀 Vector2를 반환합니다.</summary>
    public virtual Vector2 GetPositionDifferenceBetPlayer()
    {
        var position = transform.position;
        var playerPos = m_PlayerTransform.position;
        return new Vector2(position.x - playerPos.x, position.y - playerPos.y);
    }

    /// <summary>현재 위치에서 플레이어의 위치까지 Vector2.Distance를 반환합니다.</summary>
    public float GetDistanceBetPlayer()
    {
        return Vector2.Distance(m_PlayerTransform.position, transform.position);
    }

    /// <summary>적이 더 왼쪽에 있을 경우 true를 반환합니다.</summary>
    public bool GetIsLeftThenPlayer()
    {
        return transform.position.x < m_PlayerTransform.position.x ? true : false;
    }

    public virtual void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;
        
        p_Hp -= _damage * (_point == HitBoxPoint.HEAD ? 2 : 1);
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }

        if (m_CurStunValue >= p_stunThreshold)
        {
            m_CurStunValue = 0;
            ChangeEnemyFSM(EnemyStateName.STUN);
        }
    }
    public virtual void RaycastVisionCheck()
    {
        Vector2 position = transform.position;
        position.y -= 0.36f;
        
        if (m_IsRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(position, Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, Vector2.right * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast( position, -Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, -Vector2.right * p_VisionDistance, Color.red);
        }
    }
    public virtual void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log("상태 전이" + _name);
        m_CurEnemyStateName = _name;
        
        m_CurEnemyFSM.ExitState();
        switch (m_CurEnemyStateName)
        {
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }

    public virtual void SetViewDirectionToPlayer()
    {
        if (transform.position.x > m_PlayerTransform.position.x && m_IsRightHeaded)
            setisRightHeaded(false);
        else if(transform.position.x < m_PlayerTransform.position.x && !m_IsRightHeaded)
            setisRightHeaded(true);
    }

    public virtual void ResetMovePoint(Vector2 _destinationPos)
    {
        m_MovePoint = _destinationPos;
    }
    
    public virtual void MoveToPoint_FUpdate()
    {
        MoveByDirection(m_MovePoint.x > transform.position.x);
    }

    /// <summary>파라미터에 따라 발 밑 Normal벡터에 직교하는 방향대로 이동합니다.</summary>
    /// /// <param name="_isRight">True시 오른쪽으로 이동</param>
    public virtual void MoveByDirection(bool _isRight)
    {
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_Speed);
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_Speed);
        }
    }

    public virtual void GoToPlayerRoom()
    {
        if (StaticMethods.IsRoomEqual(m_EnemyLocationSensor.GetLocation(), m_PlayerLocationSensor.GetLocation()))
        {
            // 적과 플레이어의 방이 같을 경우
            
            // 적과 플레이어의 좌우판별
            MoveByDirection(!(transform.position.x > m_PlayerTransform.position.x));
        }
        else
        {
            // 적과 플레이어의 방이 다를 경우
            SetDestinationToPlayer();
            MoveToPoint_FUpdate();
        }
    }

    
    public void SetDestinationToPlayer()
    {
        Debug.Log(m_EnemyLocationSensor.GetLocation() + "Sans" + GetBodyCenterPos() + "dasln + "
        + m_PlayerLocationSensor.GetLocation());
        ResetMovePoint(m_EnemyLocationSensor.GetLocation().
            GetRoomDestPos(GetBodyCenterPos(), m_PlayerLocationSensor.GetLocation()));
    }


    public void SetHotBoxesActive(bool _isOn)
    {
        for (int i = 0; i < m_EnemyHotBoxes.Length; i++)
        {
            m_EnemyHotBoxes[i].gameObject.SetActive(_isOn);
        }
    }
}