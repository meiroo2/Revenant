using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{
    EnemyState m_enemyState;

    public virtual void Idle() { }

    protected void Init()
    {
        if (transform.localScale.x < 0)
            setisRightHeaded(false);
        else
            setisRightHeaded(true);
    }
}
