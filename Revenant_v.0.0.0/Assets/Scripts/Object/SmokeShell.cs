using System;
using System.Collections.Generic;
using UnityEngine;


public class SmokeShell : MonoBehaviour
{
    private Dictionary<BasicEnemy, float> m_EnemyDic = new Dictionary<BasicEnemy, float>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out BasicEnemy enemy))
        {
            m_EnemyDic.Add(enemy, enemy.p_AtkDistance);
            enemy.p_AtkDistance = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out BasicEnemy enemy))
        {
            enemy.p_AtkDistance = m_EnemyDic[enemy];
            m_EnemyDic.Remove(enemy);
        }
    }
}