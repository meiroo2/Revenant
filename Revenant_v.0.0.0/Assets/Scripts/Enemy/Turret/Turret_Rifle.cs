using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rifle : Enemy_Gun
{
    TutoRoom03EnemyMgr m_tutoEnemyMgr;

    [SerializeField]
    Turret_Controller turret_controller;

    private void Awake()
    {
        m_tutoEnemyMgr = GameObject.Find("TutoRoom3EnemyMgr").GetComponent<TutoRoom03EnemyMgr>();
        Init();
    }
    public override int Fire()
    {
        Debug.Log("turret fire");
        FireBullet();
        return 1;
    }

    public void FireBullet()
    {
        BulletBurst();
    }

    void AutoBullet()
    {
        m_autoCount++;
        if (m_autoCount < m_autoMaxCount)
        {
            Debug.Log("Auto: " + m_autoCount);
            Invoke(nameof(FireBullet), m_autoTime);
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
            
            //Debug.Log("Fire: " + m_burstCount);
            
            BulletFire();
            Invoke(nameof(FireBullet), m_burstTime);
        }
        else
        {
            m_burstCount = 0;
            if (m_autoMaxCount > 1)
            {
                AutoBullet();
            }
            if(m_autoCount == 0) // 3점사 3회 완료
            {
                m_tutoEnemyMgr.SendToProgress();
            }
        }

    }


    public override int Reload()
    {
        return 0;
    }
}
