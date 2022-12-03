using System;
using System.Collections;
using UnityEngine;


public class Enemy_FootMgr : MonoBehaviour
{
    // Member Variables
    private BasicEnemy m_Enemy;
    private Player _player;
    private Transform m_EnemyTransform;

    private Vector2 m_TempVecForRay;

    public MatType m_CurMatType { get; private set; } = MatType.Dirt;

    public Vector2 m_FootNormal { get; private set; }
    private Vector2 m_FootRayPos;
    private RaycastHit2D m_FootHit;
    private RaycastHit2D m_InitFootHit;
    private int m_LayerMask;
    private bool m_IsOnStair = false;
    private StairPos m_StairPos = null;

    private Coroutine m_StairPosCoroutine;
    private Coroutine m_StairCoroutine;

    private const float m_SensorXGap = 0.05f;
    private const float m_SensorYGap = 0.01f;

    // 얘네만 필요할때 누르고 있으면 됩니다.
    public bool m_VirtualUpBtn = false;
    public bool m_VirtualDownBtn = false;

    // VirtualUpBtn - true, DwonBtn - true로 만들면 계단 탐

    // Constructor
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    private void Start()
    {
        _player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }

    // Updates
    private void Update()
    {
        m_FootRayPos = new Vector2(transform.position.x, transform.position.y + 0.05f);

        switch (m_Enemy.gameObject.layer)
        {
            case 9:
                m_LayerMask = LayerMask.GetMask("Stair");
                break;

            case 11:
                m_LayerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("EmptyFloor"));
                break;
        }

        m_InitFootHit = Physics2D.Raycast(m_FootRayPos, -transform.up, 0.3f, m_LayerMask);
        if(ReferenceEquals(m_InitFootHit.collider, null))
            return;
        
        if (!ReferenceEquals(m_FootHit.collider, null))
        {
            // 만약 방금 밟은 콜라이더랑 다를 경우
            if (m_InitFootHit.collider != m_FootHit.collider)
            {
                if (m_InitFootHit.collider.TryGetComponent(out IMatType matType))
                {
                    m_CurMatType = matType.m_matType;
                }
                m_FootHit = m_InitFootHit;
            }
        }
        else
        {
            m_FootHit = m_InitFootHit;
            if (m_FootHit.collider.TryGetComponent(out IMatType _matType))
            {
                m_CurMatType = _matType.m_matType;
            }
        }
        
        Debug.DrawRay(m_FootRayPos, -transform.up * 0.3f, Color.blue, 0.05f);
        m_FootNormal = m_FootHit.normal;
    }

    // Functions
    private void OnTriggerStay2D(Collider2D other)
    {
        if (m_IsOnStair)
            return;

        var StairSensor = other.TryGetComponent(out StairPos StairTrigger);
        
        if (!StairSensor)
            return;

        if (StairSensor && StairTrigger.m_IsUpPos && m_Enemy.bMoveToUsedStair && !m_Enemy.IsPlayerUpper())
        {
            m_Enemy.bMoveToUseStairUp = false;
            m_Enemy.bMoveToUseStairDown = true;
        }
        else if (StairSensor && !StairTrigger.m_IsUpPos && m_Enemy.bMoveToUsedStair && m_Enemy.IsPlayerUpper())
        {
            m_Enemy.bMoveToUseStairDown = false;
            m_Enemy.bMoveToUseStairUp = true;
        }
        
        if (m_Enemy.bMoveToUseStairDown)
        {
            if (!other.TryGetComponent(out StairPos UpPos))
                return;

            if (!UpPos.m_IsUpPos)
                return;

            StartUsingStair(UpPos, true);
            m_Enemy.MoveNextPoint();
        }
        else if (m_Enemy.bMoveToUseStairUp)
        {
            if(!other.TryGetComponent(out StairPos DownPos))
                return;

            if (DownPos.m_IsUpPos)
                return;
            StartUsingStair(DownPos, false);
            m_Enemy.MoveNextPoint();
        }
    }
    
    private IEnumerator StairPosCoroutine(bool _isUp)
    {
        float stairPosX = m_StairPos.transform.position.x;
        bool isLeftUp = m_StairPos.m_ParentStair.m_isLeftUp;

        if (_isUp)
        {
            while (true)
            {
                if (transform.position.y < m_StairPos.transform.position.y - m_SensorYGap)
                {
                    Debug.Log("윗센서 Y보다 내려옴");

                    if (!ReferenceEquals(m_StairCoroutine, null))
                        StopCoroutine(m_StairCoroutine);
                    m_StairCoroutine = StartCoroutine(StairCoroutine(true));
                    break;
                }

                if (isLeftUp)
                {
                    // 윗센서 X좌표보다 왼쪽 == 키다운후 왼쪽으로 빠짐
                    if (transform.position.x < stairPosX - m_SensorXGap)
                    {
                        ReturnToFloor(16);
                        Debug.Log("내려가려다가 왼쪽으로 빠짐");
                        break;
                    }
                }
                else
                {
                    // 윗센서 X좌표보다 오른쪽 == 키다운후 오른쪽으로 빠짐
                    if (transform.position.x > stairPosX + m_SensorXGap)
                    {
                        ReturnToFloor(16);
                        Debug.Log("내려가려다가 오른쪽으로 빠짐");
                        break;
                    }
                }

                if (!m_Enemy.bMoveToUseStairDown)
                {
                    ReturnToFloor(16);
                    Debug.Log("내려가려다가 키 업");
                    
                    break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                // 아래 -> 위 올라가는 중
                if (transform.position.y > m_StairPos.transform.position.y + m_SensorYGap)
                {
                    Debug.Log("아래센서 Y좌표보다 올라옴");
                    if (!ReferenceEquals(m_StairCoroutine, null))
                        StopCoroutine(m_StairCoroutine);
                    m_StairCoroutine = StartCoroutine(StairCoroutine(false));
                    break;
                }

                if (isLeftUp)
                {
                    if (transform.position.x > stairPosX + m_SensorXGap)
                    {
                        // 계단 앞으로 고정
                        ReturnToFloor(16);
                        Debug.Log("올라가려다가 오른쪽으로 빠짐");
                        break;
                    }
                }
                else
                {
                    if (transform.position.x < stairPosX - m_SensorXGap)
                    {
                        // 계단 앞으로 고정
                        ReturnToFloor(16);
                        Debug.Log("올라가려다가 왼쪽으로 빠짐");
                        break;
                    }
                }
                
                if (!m_Enemy.bMoveToUseStairUp)
                {
                    // 계단 앞으로 고정
                    ReturnToFloor(16);
                    Debug.Log("올라가려다가 키 업");
                    break;
                }

                yield return null;
            }
        }

        yield break;
    }

    private IEnumerator StairCoroutine(bool _startFromUp)
    {
        var UpPosX = m_StairPos.m_ParentStair.m_UpPos.transform.position.x;
        var DownPosX = m_StairPos.m_ParentStair.m_DownPos.transform.position.x;

        if (m_StairPos.m_ParentStair.m_isLeftUp)
        {
            while (true)
            {
                if (transform.position.x <= UpPosX) // 위 센서보다 왼쪽
                {
                    ReturnToFloor(16);
                    Debug.Log("돌아왓당");

                    m_Enemy.MoveNextPoint();
                    
                    if (Mathf.Abs(m_Enemy.transform.position.y - _player.transform.position.y) <= 0.1f && !_player.bIsOnStair)
                    {
                        m_Enemy.bMoveToUseStairUp = false;
                        m_Enemy.bMoveToUseStairDown = false;
                        m_Enemy.MoveToPlayer();
                    }
                    break;
                }
                else if (transform.position.x >= DownPosX) // 아래 센서보다 오른쪽
                {
                    // 계단 앞으로 고정
                    ReturnToFloor(16);
                    Debug.Log("돌아왓당");

                    m_Enemy.MoveNextPoint();
                    
                    if (Mathf.Abs(m_Enemy.transform.position.y - _player.transform.position.y) <= 0.1f && !_player.bIsOnStair)
                    {
                        m_Enemy.bMoveToUseStairUp = false;
                        m_Enemy.bMoveToUseStairDown = false;
                        m_Enemy.MoveToPlayer();
                    }
                    break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                if (transform.position.x >= UpPosX)
                {
                    ReturnToFloor(16);
                    Debug.Log("돌아왓당");
                    break;
                }
                else if (transform.position.x <= DownPosX)
                {
                    // 계단 앞으로 고정
                    ReturnToFloor(16);
                    Debug.Log("돌아왓당");
                    break;
                }

                yield return null;
            }
        }

        yield break;
    }
    
    void StartUsingStair(StairPos UpDownPos, bool IsUp)
    {
        m_StairPos = UpDownPos;
        m_Enemy.EnemyStairNum = m_StairPos.m_ParentStair.StairNum;
        m_IsOnStair = true;
        m_Enemy.bIsOnStair = m_IsOnStair;
        m_StairPos.m_ParentStair.MoveOrder(16);
        m_Enemy.GoToStairLayer(true);

        if (!ReferenceEquals(m_StairPosCoroutine, null))
            StopCoroutine(m_StairCoroutine);
        m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(IsUp));
    }

    void ReturnToFloor(int MoveOrderNumber)
    {
        m_StairPos.m_ParentStair.MoveOrder(MoveOrderNumber);
        m_Enemy.bMoveToUsedStair = false;
        m_Enemy.GoToStairLayer(false);
        m_IsOnStair = false;
        m_Enemy.bIsOnStair = m_IsOnStair;
        m_StairPos = null;
        m_Enemy.EnemyStairNum = 0;
    }
}