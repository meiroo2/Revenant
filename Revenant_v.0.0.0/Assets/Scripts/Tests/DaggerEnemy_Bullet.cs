using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEnemy_Bullet : Bullet
{
    private int m_Damage = 10;

    private DaggerEnemy m_DaggerEnemy;
    private IHotBox m_WillHitHotBox;

    private bool m_AttackSuccess = false;

    private void Awake()
    {
        m_DaggerEnemy = GetComponentInParent<DaggerEnemy>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        m_AttackSuccess = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_AttackSuccess)
        {
            m_WillHitHotBox = collision.GetComponent<IHotBox>();
            if (collision.CompareTag("Player"))
            {
                m_WillHitHotBox.HitHotBox(new IHotBoxParam(m_Damage, 0, transform.position, WeaponType.BULLET));
                m_AttackSuccess = true;
            }
        }
    }
}