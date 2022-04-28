using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gun : WEAPON
{
    [SerializeField]
    Enemy enemy;

    

    [SerializeField]
    float m_bulletspeed = 0.2f;

    [SerializeField]
    int m_bulletdamage = 10;

    TutoRoom03EnemyMgr m_enemyMgr;

    // 자동 발사
    public int m_autoMaxCount { get; set; } = 1;
    public int m_autoCount { get; set; } = 0;
    public float m_autoTime { get; set; } = 1.0f;

    public float m_burstTime { get; set; } = 0.15f;

    // 점사
    public int m_burstCount { get; set; } = 0;


    [field: SerializeField]
    public int m_burstMaxCount { get; set; } = 1;

    protected void Init()
    {
        m_isPlayers = false;
        m_enemyMgr = GameObject.Find("TutoRoom3EnemyMgr").GetComponent<TutoRoom03EnemyMgr>();
    }

    public void BulletFire()
    {
        GameObject gameObject = Instantiate(m_bulletPrefab);
        Enemy_Bullet bullet = gameObject.GetComponent<Enemy_Bullet>();
        bullet.m_damage = m_bulletdamage;
        bullet.m_speed = m_bulletspeed;
        bullet.goVector = (enemy.m_isRightHeaded) ? Vector2.right : Vector2.left;
        gameObject.transform.position = transform.position;
        if(m_burstCount == 1&&m_enemyMgr)
            m_enemyMgr.m_first_bullet = gameObject.transform;
    }
}
