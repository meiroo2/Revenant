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

    // 자동 발사
    public int m_autoMaxCount { get; set; } = 1;
    int m_autoCount = 0;
    float m_autoTime = 1.0f;

    public float m_burstTime { get; set; } = 0.15f;

    // 점사
    public int m_burstCount { get; set; } = 0;


    [field: SerializeField]
    public int m_burstMaxCount { get; set; } = 3;

    protected void Init()
    {
        m_isPlayers = false;
    }

    public override int Fire()
    {
        if(m_autoMaxCount > 1)
        {
            AutoBullet();
            BulletBurst();
        }
        else
        {
            BulletBurst();
        }
        return 1;
    }

    void AutoBullet()
    {
        m_autoCount++;
        if (m_autoCount <= m_autoMaxCount)
        {
            Debug.Log("Auto: " + m_autoCount);
            Invoke(nameof(Fire), m_autoTime);
        }
        else
        {
            m_autoCount = 0;
        }
    }

    void BulletBurst()
    {
        m_burstCount++;

        if (m_burstCount <= m_burstMaxCount)
        {
            Debug.Log("Fire: " + m_burstCount);
            BulletFire();
            Invoke(nameof(Fire), m_burstTime);
        }
        else
        {
            m_burstCount = 0;
        }
    }

    void BulletFire()
    {
        GameObject gameObject = Instantiate(m_bulletPrefab);
        Enemy_Bullet bullet = gameObject.GetComponent<Enemy_Bullet>();
        bullet.damage = m_bulletdamage;
        bullet.speed = m_bulletspeed;
        bullet.goVector = (enemy.m_isRightHeaded) ? Vector2.right : Vector2.left;
        gameObject.transform.position = transform.position;
    }
}
