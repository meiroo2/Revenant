using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StairMgr : MonoBehaviour
{
    public bool isStairUpKey { get; private set; } = false;
    public bool isStairDownKey { get; private set; } = false;

    private Player m_Player;
    private StairPos m_stairPos;

    private bool m_isOnStair = false;
    private Vector2 m_jumpPos;

    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isStairUpKey && !m_isOnStair)
        {
            m_stairPos = null;
            isStairUpKey = false;
            m_stairPos = collision.GetComponent<StairPos>();

            int temp = m_stairPos.StairDetectorDetected(gameObject.GetInstanceID());
            if(temp == 1)
            {
                m_isOnStair = true;
                m_jumpPos = collision.transform.position;
                m_jumpPos.y += 0.1f;
                m_Player.GoToStairLayer(true, m_jumpPos);
            }
        }
        else if (isStairDownKey && !m_isOnStair)
        {
            isStairDownKey = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isOnStair)
        {
            m_stairPos = null;
            m_stairPos = collision.GetComponent<StairPos>();
            switch (m_stairPos.StairDetectorDetected(gameObject.GetInstanceID()))
            {
                case 0:
                    m_isOnStair = false;
                    m_Player.GoToStairLayer(false, Vector2.zero);
                    break;
            }
        }
    }
}