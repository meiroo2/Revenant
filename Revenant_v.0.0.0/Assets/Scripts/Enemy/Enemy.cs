using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{

    public Transform m_playerTransform { get; set; } // 플레이어 실시간 위치

    public EnemyState curEnemyState { get; set; }

    //EnemyState m_enemyState;

    // 감지 가능한지 ( 특정 위치에 도달해야 감지 가능 )
    public bool m_canSensor { get; set; } = false;

    public virtual void Damaged(float stun, float damage) { }

    public virtual void Idle() { }
    public virtual void Attack() { }
    protected void HumanInit()
    {
        if (transform.localScale.x < 0)
            setisRightHeaded(false);
        else
            setisRightHeaded(true);
        
        
    }

    public void OverLap()
    {
        //겹쳤다

        
    }
}
