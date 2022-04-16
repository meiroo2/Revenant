using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{
    EnemyState m_enemyState;

    public virtual void Idle() { }
    public virtual void Attack() { }
    protected void HumanInit()
    {
        if (transform.localScale.x < 0)
            setisRightHeaded(false);
        else
            setisRightHeaded(true);
        
        
    }
}
