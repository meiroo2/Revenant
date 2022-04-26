using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum humanState
{
    Active,
    Pause
}

public class Human : ObjectDefine
{
    // Member Variables
    public humanState m_curHumanState { get; private set; } = humanState.Active;
    [field: SerializeField] public float m_Hp { get; protected set; } = 10;
    [field: SerializeField] public float m_Speed { get; protected set; } = 1f;
    [field: SerializeField] public float m_stunTime { get; protected set; } = 0f;
    public bool m_isRightHeaded { get; private set; } = true;
    public MapInfo m_curPos { get; set; } = new MapInfo(0, 0, 0, 0, 0);

    // 오브젝트의 원래 위치
    public Vector2 m_originVec { get; set; }

    // Constructor

    // Functions

    public void setisRightHeaded(bool _isRightHeaded)
    {
        m_isRightHeaded = _isRightHeaded;
        if (m_isRightHeaded)
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void changehumanState(humanState _inputhumanState)
    {
        switch (_inputhumanState)
        {
            case humanState.Active:
                m_curHumanState = humanState.Active;
                m_canAttacked = true;
                m_canUse = true;
                break;

            case humanState.Pause:
                m_curHumanState = humanState.Pause;
                m_canAttacked = false;
                m_canUse = false;
                break;
        }
    }

    // 오브젝트를 원래 위치로 변환(y: -100)
    public void respawn()
    {
        // 아래로 많이 떨어졌을 때
        if (transform.position.y < -10.0f)
        {
            // 원래 위치로 돌아온다
            transform.position = m_originVec;
        }
    }

    public void buffHp()
    {
        m_Hp = 10000.0f;
    }
}