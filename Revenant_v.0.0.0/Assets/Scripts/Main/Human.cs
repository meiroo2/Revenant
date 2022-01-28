using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum humanState
{
    Live,
    Dead,
    Stun,
    Pause
}

public class Human : ObjectDefine
{
    // Member Variables
    [field: SerializeField] public humanState m_curHumanState { get; private set; } = humanState.Live;
    [field: SerializeField] public float m_Hp { get; private set; } = 10;
    [field: SerializeField] public float m_Speed { get; private set; } = 1f;
    [field: SerializeField] public float m_stunTime { get; private set; } = 0f;
    [field: SerializeField] public bool m_hasStun { get; private set; } = false;
    [field: SerializeField] public bool m_isEnemy { get; private set; } = false;
    [field: SerializeField] public bool m_isRightHeaded { get; private set; } = true;
    [field: SerializeField] public int[] m_curPos { get; set; } = new int[2];

    // żŔşęÁ§ĆŽŔÇ żřˇĄ Ŕ§ÄĄ
    public Vector2 m_originVec { get; set; }

    // Constructor
    private void Awake()
    {
        InitObjectDefine(ObjectType.Human, false, false);
        m_curPos[0] = 0;
        m_curPos[1] = 0;
    }

    // Functions
    public void humanAttacked(float _inputDamage)
    {
        if(m_curHumanState == humanState.Live)
        {
            if(m_Hp - _inputDamage > 0)
            {
                m_Hp -= _inputDamage;
                if (m_hasStun)
                {
                    m_curHumanState = humanState.Stun;
                    Invoke(nameof(changeStuntoLive), m_stunTime);
                }
            }
            else
            {
                m_Hp = -1f;
                m_curHumanState = humanState.Dead;

                if(m_isEnemy == false)
                {
                    // Play Dead Ani
                }
            }
        }
    }
    public void setisRightHeaded(bool _isRightHeaded)
    {
        m_isRightHeaded = _isRightHeaded;
        if (m_isRightHeaded)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
    }
    private void changeStuntoLive()
    {
        if (m_curHumanState == humanState.Stun)
            m_curHumanState = humanState.Live;
    }
    public void changehumanState(humanState _inputhumanState)
    {
        switch (_inputhumanState)
        {
            case humanState.Live:
                break;
            case humanState.Dead:
                CancelInvoke(nameof(changeStuntoLive));
                m_curHumanState = humanState.Dead;
                break;
            case humanState.Stun:
                break;
            case humanState.Pause:
                CancelInvoke(nameof(changeStuntoLive));
                m_curHumanState = humanState.Pause;
                break;
        }
    }
    
    // żŔşęÁ§ĆŽ¸Ś żřˇĄ Ŕ§ÄĄˇÎ şŻČŻ(y: -100)
    public void respawn()
    {
        // žĆˇĄˇÎ ¸šŔĚ śłžîÁłŔť ś§
        if(transform.position.y < -10.0f)
        {
            // żřˇĄ Ŕ§ÄĄˇÎ ľšžĆżÂ´Ů
            transform.position = m_originVec;
        }
    }

    public void buffHp()
    {
        m_Hp = 10000.0f;
    }
}