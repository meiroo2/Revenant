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

    protected void Init()
    {
        m_isPlayers = false;
    }

    protected void BulletCreate()
    {
        Debug.Log("Fire");
        GameObject gameObject = Instantiate(m_bulletPrefab);
        Enemy_Bullet bullet = gameObject.GetComponent<Enemy_Bullet>();
        bullet.damage = m_bulletdamage;
        bullet.speed = m_bulletspeed;
        bullet.goVector = (enemy.m_isRightHeaded) ? Vector2.right : Vector2.left;


        gameObject.transform.position = transform.position;

    }
}
