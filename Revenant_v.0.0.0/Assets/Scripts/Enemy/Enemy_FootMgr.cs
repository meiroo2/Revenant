using System;
using System.Collections;
using UnityEngine;


public class Enemy_FootMgr : MonoBehaviour
{
    // Member Variables
    private BasicEnemy m_Enemy;
    private Transform m_EnemyTransform;

    private Vector2 m_TempVecForRay;

    public Vector2 m_FootNormal { get; private set; }
    private Vector2 m_FootRayPos;
    private RaycastHit2D m_FootHit;
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

        m_FootHit = Physics2D.Raycast(m_FootRayPos, -transform.up, 0.3f, m_LayerMask);

        Debug.DrawRay(m_FootRayPos, -transform.up * 0.3f, Color.blue, 0.05f);
        m_FootNormal = m_FootHit.normal;
    }

    // Functions
    private void OnTriggerStay2D(Collider2D other)
    {
        var LowerStair = other.TryGetComponent(out StairPos UpPos);
        var UpperStair = other.TryGetComponent(out StairPos DownPos);

        var StairSensor = other.GetComponent<StairPos>();
        
        if (m_IsOnStair)
            return;
        
        if (!StairSensor)
            return;
        
        if (m_Enemy.bMoveToUsedStair && StairSensor && StairSensor.m_IsUpPos)
        {
            Debug.Log("Is AI hitting UpperStair?");
            m_Enemy.bMoveToUseStairDown = true;
            if (m_Enemy.bMoveToUseStairDown)
            {
                Debug.Log("UpperStair MoveToUseStairDown - " + m_Enemy.bMoveToUseStairDown);
                m_Enemy.bMoveToUsedStair = false;
                m_StairPos = UpPos;
                m_IsOnStair = true;
                m_Enemy.bIsOnStair = m_IsOnStair;
                m_StairPos.m_ParentStair.MoveOrder(16);
                m_Enemy.GoToStairLayer(true);

                if (!ReferenceEquals(m_StairPosCoroutine, null))
                    StopCoroutine(m_StairCoroutine);
                m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(true));
            }
        }
        else if (m_Enemy.bMoveToUsedStair && StairSensor && !StairSensor.m_IsUpPos)
        {
            Debug.Log("Is AI hitting LowerStair?");
            m_Enemy.bMoveToUseStairUp = true;
            if (m_Enemy.bMoveToUseStairUp)
            {
                m_Enemy.bMoveToUsedStair = false;
                m_StairPos = DownPos;
                m_IsOnStair = true;
                m_Enemy.bIsOnStair = m_IsOnStair;
                m_StairPos.m_ParentStair.MoveOrder(16);
                m_Enemy.GoToStairLayer(true);

                if (!ReferenceEquals(m_StairPosCoroutine, null))
                    StopCoroutine(m_StairCoroutine);
                m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(false));

                
            }
        }
    }


    // if (m_Enemy.bMoveToUsedStair && LowerStair)
    // {
    //     m_Enemy.bMoveToUseStairUp = true;
    //     
    //     if (m_Enemy.bMoveToUseStairUp)
    //     {
    //         if (!UpperStair) 
    //             return;
    //     
    //         if (DownPos.m_IsUpPos == true)
    //             return;
    //
    //         m_Enemy.bMoveToUsedStair = false;
    //         m_StairPos = DownPos;
    //         m_StairPos.m_ParentStair.MoveOrder(16);
    //         //Debug.Log("위로 올라가기 시작");
    //         m_Enemy.GoToStairLayer(true);
    //         m_IsOnStair = true;
    //
    //         if(!ReferenceEquals(m_StairPosCoroutine, null))
    //             StopCoroutine(m_StairCoroutine);
    //         m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(false));
    //     }
    // }
    // if (m_Enemy.bMoveToUsedStair && UpperStair)
    // {
    //     m_Enemy.bMoveToUseStairDown = true;
    //     
    //     if (m_Enemy.bMoveToUseStairDown)
    //     {
    //         if (!LowerStair) 
    //             return;
    //     
    //         if (UpPos.m_IsUpPos == false)
    //             return;
    //     
    //         m_Enemy.bMoveToUsedStair = false;
    //         m_StairPos = UpPos;
    //         m_StairPos.m_ParentStair.MoveOrder(16);
    //         m_Enemy.GoToStairLayer(true);
    //         m_IsOnStair = true;
    //     
    //         if(!ReferenceEquals(m_StairPosCoroutine, null))
    //             StopCoroutine(m_StairCoroutine);
    //         m_StairPosCoroutine = StartCoroutine(StairPosCoroutine(true));
    //     }
    // }

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
                        m_StairPos.m_ParentStair.MoveOrder(16);

                        Debug.Log("내려가려다가 왼쪽으로 빠짐");

                        m_Enemy.GoToStairLayer(false);
                        m_IsOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }
                else
                {
                    // 윗센서 X좌표보다 오른쪽 == 키다운후 오른쪽으로 빠짐
                    if (transform.position.x > stairPosX + m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(16);

                        Debug.Log("내려가려다가 오른쪽으로 빠짐");

                        m_Enemy.GoToStairLayer(false);
                        m_IsOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }

                if (!m_Enemy.bMoveToUseStairDown)
                {
                    m_StairPos.m_ParentStair.MoveOrder(16);

                    Debug.Log("내려가려다가 키 업");

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_StairPos = null;
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
                        m_StairPos.m_ParentStair.MoveOrder(10);

                        Debug.Log("올라가려다가 오른쪽으로 빠짐");

                        m_Enemy.GoToStairLayer(false);
                        m_IsOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }
                else
                {
                    if (transform.position.x < stairPosX - m_SensorXGap)
                    {
                        m_StairPos.m_ParentStair.MoveOrder(10);

                        Debug.Log("올라가려다가 왼쪽으로 빠짐");

                        m_Enemy.GoToStairLayer(false);
                        m_IsOnStair = false;
                        m_StairPos = null;
                        break;
                    }
                }


                if (!m_Enemy.bMoveToUseStairUp)
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);

                    Debug.Log("올라가려다가 키 업");

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_StairPos = null;
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
                    m_StairPos.m_ParentStair.MoveOrder(16);

                    Debug.Log("돌아왓당");
                    m_Enemy.bMoveToUseStairUp = false;
                    m_Enemy.bMoveToUseStairDown = false;

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_Enemy.bIsOnStair = m_IsOnStair;
                    m_StairPos = null;
                    break;
                }
                else if (transform.position.x >= DownPosX) // 아래 센서보다 오른쪽
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);

                    Debug.Log("돌아왓당");
                    m_Enemy.bMoveToUseStairUp = false;
                    m_Enemy.bMoveToUseStairDown = false;

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_Enemy.bIsOnStair = m_IsOnStair;
                    m_StairPos = null;
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
                    m_StairPos.m_ParentStair.MoveOrder(16);

                    Debug.Log("돌아왓당");

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_StairPos = null;
                    break;
                }
                else if (transform.position.x <= DownPosX)
                {
                    m_StairPos.m_ParentStair.MoveOrder(10);

                    Debug.Log("돌아왓당");

                    m_Enemy.GoToStairLayer(false);
                    m_IsOnStair = false;
                    m_StairPos = null;
                    break;
                }

                yield return null;
            }
        }

        yield break;
    }
}