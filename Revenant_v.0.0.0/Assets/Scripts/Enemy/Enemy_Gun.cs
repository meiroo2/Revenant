using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gun : WEAPON
{
    [SerializeField]
    Enemy enemy;

    [SerializeField]
    float m_bulletspeed = 1.0f;

    [SerializeField]
    int m_bulletdamage = 50;

    public float m_burstTime { get; set; } = 0.15f;

    public int m_burstCount { get; set; } = 0;

    [field: SerializeField]
    public int m_burstMaxCount { get; set; } = 3;

    protected void Init()
    {
        m_isPlayers = false;
    }

    protected void BulletCreate()
    {
        m_burstCount++;

        //Debug.Log("Fire");
        // 총알에 스테이터스 전달
        GameObject gameObject = Instantiate(m_bulletPrefab);
        Enemy_Bullet bullet = gameObject.GetComponent<Enemy_Bullet>();
        bullet.damage = m_bulletdamage;
        bullet.speed = m_bulletspeed;
        bullet.goVector = (enemy.m_isRightHeaded) ? Vector2.right : Vector2.left;
        gameObject.transform.position = transform.position;

        if(m_burstCount < m_burstMaxCount)
        {
            //Debug.Log(m_burstCount);
            Invoke(nameof(BulletCreate), m_burstTime);

        }
        else
        {
            m_burstCount = 0;
        }
    }
}
