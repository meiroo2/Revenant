using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{
    [field: SerializeField]
    public EnemyState curEnemyState { get; set; }

    EnemyState m_enemyState;

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
        //°ãÃÆ´Ù

        
    }
}
