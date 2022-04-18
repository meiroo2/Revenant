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
    public int m_autoCount { get; set; } = 0;
    public float m_autoTime { get; set; } = 1.0f;

    public float m_burstTime { get; set; } = 0.15f;

    // 점사
    public int m_burstCount { get; set; } = 0;


    [field: SerializeField]
    public int m_burstMaxCount { get; set; } = 3;

    protected void Init()
    {
        m_isPlayers = false;
    }

    public void BulletFire()
    {
        

        GameObject gameObject = Instantiate(m_bulletPrefab);
        Enemy_Bullet bullet = gameObject.GetComponent<Enemy_Bullet>();
        bullet.damage = m_bulletdamage;
        bullet.speed = m_bulletspeed;
        bullet.goVector = (enemy.m_isRightHeaded) ? Vector2.right : Vector2.left;
        gameObject.transform.position = transform.position;
    }
}
