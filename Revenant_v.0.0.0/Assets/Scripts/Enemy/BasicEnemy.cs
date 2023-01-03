using System.Collections;
using System;
using System.Collections.Generic;
using FMOD;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum EnemyStateName
{
    IDLE,
    PATROL,
    FOLLOW,
    RUSH,
    ATTACK,
    STUN,
    ROTATION,
    CHANGE,
    BREAK,
    ROLL,
    HIT,
    HIDDEN,
    DEAD
}

public class BasicEnemy : Human
{
    // Visible Member Variables
    //[PropertySpace(SpaceBefore = 20, SpaceAfter = 20), PropertyOrder(3)]
    [field: SerializeField, BoxGroup("BasicEnemy Values")] protected bool p_OverrideEnemyMgr = false;
    [field: SerializeField, BoxGroup("BasicEnemy Values")] public int p_AngleLimit { get; protected set; } = 20;
    
    [field: SerializeField, BoxGroup("BasicEnemy Values")] public float p_VisionDistance { get; protected set; }

    [field: SerializeField, BoxGroup("BasicEnemy Values")] public float p_AtkDistance;

    [ShowInInspector, ReadOnly, BoxGroup("BasicEnemy Values"), PropertySpace(SpaceBefore = 0, SpaceAfter = 20)]
    public EnemyStateName m_CurEnemyStateName { get; protected set; }
    

    // Member Variables
    public bool m_IsDead = false;
    
    private RaycastHit2D m_NullHit;
    public SpriteRenderer m_Renderer { get; protected set; }
    private List<EnemySpawner> m_EnemySpawnerList = new List<EnemySpawner>();
    protected Enemy_HotBox[] m_EnemyHotBoxes;
    protected Enemy_UseRange m_EnemyUseRange;
    protected LocationSensor m_EnemyLocationSensor;
    protected LocationSensor m_PlayerLocationSensor;
    public Enemy_FootMgr m_Foot { get; protected set; }
    public Animator m_Animator { get; protected set; }
    public Enemy_Alert m_Alert { get; protected set; }
    public Transform m_PlayerTransform { get; protected set; }
    public Rigidbody2D m_EnemyRigid { get; protected set; }
    public RaycastHit2D m_VisionHit { get; protected set; }
    public Enemy_FSM m_CurEnemyFSM { get; protected set; }

    protected Vector2 m_MovePoint;
    private Coroutine m_MatCoroutine;

    /// <summary>
    /// 사망 사유를 기재합니다. 머터리얼 교체용
    /// 0기본, 1불릿타임
    /// </summary>
    public int m_DeadReasonForMat { get; protected set; } = 0;

    public bool m_PlayerCognition { get; set; } = false;
    
    public bool bMoveToUsedDoor { get; set; } = false;
    public bool bIsOnStair { get; set; } = false;
    public bool bMoveToUsedStair { get; set; } = false;
    public bool bMoveToUseStairUp { get; set; } = false;
    public bool bMoveToUseStairDown { get; set; } = false;
    public int EnemyStairNum { get; set; } = 0;
    public int EnemyMapSectionNum { get; set; } = 0;
    
    [HideInInspector] public List<Vector2> WayPointsVectorList;
    [HideInInspector] public int WayPointsIndex = 0;

    public SoundPlayer m_SoundPlayer { get; protected set; }
    protected int m_EnemyIdx = 0;
    private Coroutine m_WalkSoundCoroutine = null;
    private bool m_IsWalking = false;

    // Functions
    
    public virtual void StartWalkSound(bool _start, float _time = 1f)
    {
        if (_start && m_IsWalking)
            return;
        
        if (!ReferenceEquals(m_WalkSoundCoroutine, null))
        {
            StopCoroutine(m_WalkSoundCoroutine);
            m_WalkSoundCoroutine = null;
        }

        if (!_start)
        {
            m_IsWalking = false;
            return;
        }

        m_IsWalking = true;
        m_WalkSoundCoroutine = StartCoroutine(WalkSoundEnumerator(_time));
    }

    protected virtual IEnumerator WalkSoundEnumerator(float _time)
    {
        while (true)
        {
            yield return new WaitForSeconds(_time);
            m_SoundPlayer.PlayEnemySound(m_EnemyIdx, 0, transform.position, m_Foot.m_CurMatType);
        }
        
        yield break;
    }
    
    /// <summary>
    /// 적이 플레이어를 바라보고 있다면 True를 반환합니다.
    /// </summary>
    /// <returns>플레이어를 바라보고 있는지 여부</returns>
    public bool IsFacePlayer()
    {
        if (m_IsRightHeaded)
        {
            // 오른쪽을 보고 있을 때
            return transform.position.x < m_PlayerTransform.position.x;
        }
        else
        {
            return transform.position.x > m_PlayerTransform.position.x;
        }
    }
    
    /// <summary>
    /// 해당 적이 플레이어 인지를 시작합니다.
    /// </summary>
    /// <param name="_instant">즉시 시작 여부</param>
    public virtual void StartPlayerCognition(bool _instant = false)
    {
    }

    public virtual void SoundCognition(SoundHotBoxParam _param)
    {
    }

    public virtual void InitEnemy()
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

    public void GoToStairLayer(bool _input)
    {
        gameObject.layer = _input ? 9 : 11;
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
    public virtual float GetDistanceBetPlayer()
    {
        return Vector2.Distance(m_PlayerTransform.position, transform.position);
    }

    /// <summary>적이 더 왼쪽에 있을 경우 true를 반환합니다.</summary>
    public bool GetIsLeftThenPlayer()
    {
        return transform.position.x < m_PlayerTransform.position.x ? true : false;
    }

    public virtual void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            // MatType Check
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }

        if (m_CurStunValue >= p_StunHp)
        {
            m_CurStunValue = 0;
            ChangeEnemyFSM(EnemyStateName.STUN);
        }
    }
    public virtual void RaycastVisionCheck()
    {
        Vector2 position = transform.position;
        position.y -= 0.36f;
        
        int layerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Player"));
        
        if (m_IsRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(position, Vector2.right, p_VisionDistance, layerMask);
            Debug.DrawRay(position, Vector2.right * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast( position, -Vector2.right, p_VisionDistance, layerMask);
            Debug.DrawRay(position, -Vector2.right * p_VisionDistance, Color.red);
        }

        //Debug.Log(m_VisionHit.collider.name);
        
        if (!ReferenceEquals(m_VisionHit.collider, null) && !m_VisionHit.collider.CompareTag("Player"))
            m_VisionHit = m_NullHit;
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
    
    /// <summary>
    /// 대상의 위치가 적이 바라보는 방향에 있나 판단합니다.
    /// </summary>
    /// <param name="_objective">대상 위치</param>
    /// <returns></returns>
    public virtual bool IsExistInEnemyView(Vector2 _objective)
    {
        bool isRight = !(transform.position.x - _objective.x > 0);
        
        return m_IsRightHeaded ? isRight : !isRight;
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
    
    /// <summary>
    /// MovePoint 방향으로 Rigid의 Velocity를 변경합니다.
    /// </summary>
    public virtual void SetRigidToPoint(float _addSpeed = 1f)
    {
        SetRigidByDirection(m_MovePoint.x > transform.position.x);
    }

    public void ResetRigid()
    {
        m_EnemyRigid.velocity = Vector2.zero;
    }


    /// <summary>
    /// 파라미터에 따라 발 밑 Normal벡터에 직교하는 방향대로 이동합니다.
    /// </summary>
    /// <param name="_isRight">True시 오른쪽으로 이동</param>
    public virtual void SetRigidByDirection(bool _isRight, float _addSpeed = 1f)
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

    public virtual void GoToPlayerRoom()
    {
        if (StaticMethods.IsRoomEqual(m_EnemyLocationSensor.GetLocation(), m_PlayerLocationSensor.GetLocation()))
        {
            // 적과 플레이어의 방이 같을 경우
            
            // 적과 플레이어의 좌우판별
            SetRigidByDirection(!(transform.position.x > m_PlayerTransform.position.x));
        }
        else
        {
            // 적과 플레이어의 방이 다를 경우
            SetDestinationToPlayer();
            SetRigidToPoint();
        }
    }

    
    public void SetDestinationToPlayer()
    {
        Debug.Log(m_EnemyLocationSensor.GetLocation() + "Sans" + GetBodyCenterPos() + "dasln + "
        + m_PlayerLocationSensor.GetLocation());
        ResetMovePoint(m_EnemyLocationSensor.GetLocation().
            GetRoomDestPos(GetBodyCenterPos(), m_PlayerLocationSensor.GetLocation()));
    }


    /// <summary>
    /// Enemy의 Hotbox의 Active 상태를 조절합니다.
    /// </summary>
    /// <param name="_isOn"></param>
    public virtual void SetHotBoxesActive(bool _isOn)
    {
        for (int i = 0; i < m_EnemyHotBoxes.Length; i++)
        {
            m_EnemyHotBoxes[i].gameObject.SetActive(_isOn);
        }
    }


    /// <summary>
    /// 마우스가 Cognition 핫박스에 닿으면 해당 함수를 호출합니다.
    /// </summary>
    /// <param name="_isTouch">터치 / 터치취소</param>
    public virtual void MouseTouched(bool _isTouch)
    {
        
    }
    
    public void MoveNextPoint()
    {
        if (WayPointsIndex == WayPointsVectorList.Count - 1) { }
        else { WayPointsIndex++; }
    }
    
    /** 리스트를 비우고 인덱스를 초기화 함으로써 플레이어 방향으로 이동한다 */
    public void MoveToPlayer()
    {
        WayPointsIndex = 0;
        WayPointsVectorList.Clear();
    }

    /** 플레이어가 AI보다 위에 있는지 판단하는 함수 */
    public bool IsPlayerUpper()
    {
        if (transform.position.y < m_PlayerTransform.position.y)
        {
            return true;
        }
        return false;
    }

    /** 플레이어랑 같은 바닥에 있는지 판단하는 함수 */
    public bool IsSameFloorWithPlayer(bool EnemyMoveToUsedDoor, bool EnemyIsOnStair, bool PlayerIsOnStair)
    {
        float HeightBetweenPlayerAndEnemy = Mathf.Abs(transform.position.y - m_PlayerTransform.position.y);
        if (HeightBetweenPlayerAndEnemy <= 0.1f)
        {
            if (EnemyMoveToUsedDoor && !EnemyIsOnStair)
            {
                bMoveToUsedDoor = false;
                return true;
            }
            if (!EnemyIsOnStair && !PlayerIsOnStair)
            {
                return true;
            }
        }
        return false;
    }

    /** 플레이어랑 같은 계단에 있는지 판단하는 함수 */
    public bool IsSameStairWithPlayer(bool EnemyIsOnStair, bool PlayerIsOnStair, int EnemyStairNum, int PlayerStairNum)
    {
        if (EnemyIsOnStair && PlayerIsOnStair && EnemyStairNum == PlayerStairNum)
            return true;
        
        return false;
    }
}