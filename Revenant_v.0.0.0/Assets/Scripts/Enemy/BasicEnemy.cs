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
    public Material p_WhiteMat;
    
    [field: SerializeField] protected bool p_OverrideEnemyMgr = false;
    [field: SerializeField] public float p_VisionDistance { get; protected set; }
    [field: SerializeField] public float p_HearColSize { get; protected set; }
    [field: SerializeField] public int p_AngleLimit { get; protected set; } = 20;
    [field: SerializeField] public float p_AttackModeDelay { get; protected set; } = 0f;


    // Member Variables
    protected SpriteRenderer m_Renderer;
    protected Material m_DefaultMat;
    private List<EnemySpawner> m_EnemySpawnerList = new List<EnemySpawner>();
    public Enemy_HotBox[] m_EnemyHotBoxes { get; set; }
    protected Enemy_UseRange m_EnemyUseRange;
    protected LocationSensor m_EnemyLocationSensor;
    protected LocationSensor m_PlayerLocationSensor;
    protected Enemy_FootMgr m_Foot;
    public Enemy_Alert m_Alert { get; protected set; }
    public Transform m_PlayerTransform { get; protected set; }
    public Rigidbody2D m_EnemyRigid { get; protected set; }
    public RaycastHit2D m_VisionHit { get; protected set; }
    protected Enemy_FSM m_CurEnemyFSM;
    public EnemyStateName m_CurEnemyStateName { get; protected set; }

    protected Vector2 m_MovePoint;

    private Coroutine m_MatCoroutine;
    
    // Functions
    protected void ChangeWhiteMat(float _time)
    {
        if(m_MatCoroutine != null)
            StopCoroutine(m_MatCoroutine);

        m_Renderer.material = m_DefaultMat;
        m_MatCoroutine = StartCoroutine(ChangeMatCoroutine(_time));
    }

    private IEnumerator ChangeMatCoroutine(float _time)
    {
        m_Renderer.material = p_WhiteMat;
        yield return new WaitForSeconds(_time);
        m_Renderer.material = m_DefaultMat;
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
    
    public virtual Vector2 GetDistBetPlayer()
    {
        var position = transform.position;
        var position1 = m_PlayerTransform.position;
        return new Vector2(position.x - position1.x, position.y - position1.y);
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
        if (m_MovePoint.x > transform.position.x)
        {
            MoveByDirection_FUpdate(true);
        }
        else
        {
            MoveByDirection_FUpdate(false);
        }
    }

    public virtual void MoveByDirection_FUpdate(bool _isRight)
    {
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal) * (p_Speed * Time.deltaTime);
            //Debug.Log(m_EnemyRigid.velocity);
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal) * (p_Speed * Time.deltaTime);
        }
    }

    public virtual void GoToPlayerRoom()
    {
        if (StaticMethods.IsRoomEqual(m_EnemyLocationSensor.GetLocation(), m_PlayerLocationSensor.GetLocation()))
        {
            // 적과 플레이어의 방이 같을 경우
            
            // 적과 플레이어의 좌우판별
            MoveByDirection_FUpdate(!(transform.position.x > m_PlayerTransform.position.x));
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
    
}