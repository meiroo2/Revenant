using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FootMgr : MonoBehaviour
{
    private Player m_Player;
    private StairPos m_stairPos;
    private StairPos m_NullStairPos;
    private Player_InputMgr m_InputMgr;

    private bool m_isOnStair = false;
    private Vector2 m_jumpPos;
    private RaycastHit2D m_FootRay;
    private int m_LayerMask;

    public Vector2 m_PlayerNormal { get; private set; }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_InputMgr = m_Player.m_InputMgr;

        m_LayerMask = LayerMask.GetMask("Floor");
        m_NullStairPos = null;
        m_PlayerNormal = Vector2.up;
    }

    private void Update()
    {
        m_FootRay = Physics2D.Raycast(transform.position,
            -transform.up, 1f, m_LayerMask);
    }

    public RaycastHit2D GetFootRayHit()
    {
        return m_FootRay;
    }

    public void ChangePlayerNormal(Vector2 _normal)
    {
        m_PlayerNormal = _normal;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_isOnStair && (m_InputMgr.m_IsPushStairUpKey || m_InputMgr.m_IsPushStairDownKey))
        {
            m_stairPos = null;
            m_stairPos = collision.GetComponent<StairPos>();

            if (m_stairPos.m_ParentStair.m_isRightUp)   // 우상향 계단
            {
                // 우상향 계단에서 올라갈 때
                if (m_InputMgr.m_IsPushStairUpKey && m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                    (transform.position.x < m_stairPos.transform.position.x) &&
                    (m_Player.m_HumanFootNormal.x >= 0) &&
                    m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    ChangePlayerNormal(m_stairPos.m_ParentStair.m_StairNormalVec);
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
                // 우상향 계단에서 내려갈 때
                else if (m_InputMgr.m_IsPushStairDownKey && !m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                         (transform.position.x > m_stairPos.transform.position.x) &&
                         (m_Player.m_HumanFootNormal.x <= 0) &&
                         m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    ChangePlayerNormal(m_stairPos.m_ParentStair.m_StairNormalVec);
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
            }
            else if (!m_stairPos.m_ParentStair.m_isRightUp)  // 좌상향 계단
            {
                // 좌상향 계단에서 올라갈 때
                if (m_InputMgr.m_IsPushStairUpKey && m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                    (transform.position.x > m_stairPos.transform.position.x) && 
                    (m_Player.m_HumanFootNormal.x <= 0) &&
                    m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    ChangePlayerNormal(m_stairPos.m_ParentStair.m_StairNormalVec);
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
                // 좌상향 계단에서 내려갈 때
                else if (m_InputMgr.m_IsPushStairDownKey && !m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                         (transform.position.x < m_stairPos.transform.position.x) &&
                         (m_Player.m_HumanFootNormal.x >= 0) &&
                         m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    ChangePlayerNormal(m_stairPos.m_ParentStair.m_StairNormalVec);
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isOnStair)
        {
            m_stairPos = m_NullStairPos;
            m_stairPos = collision.GetComponent<StairPos>();
            if(!m_stairPos.m_isInitDetector)
                switch (m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()))
                {
                    case 0: // 계단 떠남
                        m_isOnStair = false;
                        ChangePlayerNormal(Vector2.up);
                        m_Player.GoToStairLayer(false, Vector2.zero, Vector2.zero);
                        break;
                }
        }
    }
}