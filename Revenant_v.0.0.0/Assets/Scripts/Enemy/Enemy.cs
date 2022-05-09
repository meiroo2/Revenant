using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{

    public Transform m_playerTransform { get; set; } // �÷��̾� �ǽð� ��ġ

    public EnemyState curEnemyState { get; set; }

    //EnemyState m_enemyState;

    // ���� �������� ( Ư�� ��ġ�� �����ؾ� ���� ���� )
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
        //���ƴ�

        
    }
}
