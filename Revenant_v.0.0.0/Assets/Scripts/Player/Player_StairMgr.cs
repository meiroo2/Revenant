using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StairMgr : MonoBehaviour
{
    public bool isStairUpKey { get; private set; } = false;
    public bool isStairDownKey { get; private set; } = false;

    private Player m_Player;
    private StairPos m_stairPos;
    private StairPos m_NullStairPos;

    private bool m_isOnStair = false;
    private Vector2 m_jumpPos;

    public Vector2 m_PlayerNormal { get; private set; }

    private void Start()
    {
        m_NullStairPos = null;

        m_PlayerNormal = Vector2.up;
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isStairUpKey = true;
        }    
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isStairUpKey = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isStairDownKey = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isStairDownKey = false;
        }
    }

    public void ChangePlayerNormal(Vector2 _normal)
    {
        m_PlayerNormal = _normal;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_isOnStair && (isStairUpKey || isStairDownKey))
        {
            m_stairPos = null;
            m_stairPos = collision.GetComponent<StairPos>();

            if (m_stairPos.m_ParentStair.m_isRightUp)   // 우상향 계단
            {
                // 우상향 계단에서 올라갈 때
                if (isStairUpKey && m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                    (transform.position.x < m_stairPos.transform.position.x) &&
                    (m_Player.m_HumanMoveVec.x >= 0) &&
                    m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
                // 우상향 계단에서 내려갈 때
                else if (isStairDownKey && !m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                   (transform.position.x > m_stairPos.transform.position.x) &&
                   (m_Player.m_HumanMoveVec.x <= 0) &&
                   m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
            }
            else if (!m_stairPos.m_ParentStair.m_isRightUp)  // 좌상향 계단
            {
                // 좌상향 계단에서 올라갈 때
                if (isStairUpKey && m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                    (transform.position.x > m_stairPos.transform.position.x) && 
                    (m_Player.m_HumanMoveVec.x <= 0) &&
                    m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
                    m_Player.GoToStairLayer(true, m_jumpPos, m_stairPos.m_ParentStair.m_StairNormalVec);
                }
                // 좌상향 계단에서 내려갈 때
                else if (isStairDownKey && !m_stairPos.m_isGoingUp && m_stairPos.m_isInitDetector &&
                    (transform.position.x < m_stairPos.transform.position.x) &&
                    (m_Player.m_HumanMoveVec.x >= 0) &&
                    m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()) == 1)
                {
                    m_isOnStair = true;
                    m_jumpPos = collision.transform.position;
                    m_jumpPos.y += 0.1f;
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
                    case 0:
                        m_isOnStair = false;
                        m_Player.GoToStairLayer(false, Vector2.zero, Vector2.zero);
                        break;
                }
        }
    }
}