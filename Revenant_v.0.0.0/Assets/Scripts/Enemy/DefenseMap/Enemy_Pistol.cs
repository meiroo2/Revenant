using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pistol : Enemy_Gun
{

    bool m_canFire = true;
    
    float m_fireDelay = 1.0f;
    public override int Fire()
    {
        FireBullet();
        return 1;
    }

    public void FireBullet()
    {
        if(m_canFire)
        {
            m_canFire = false;
            Invoke(nameof(FireDelay), m_fireDelay);
            BulletBurst();
        }
            
    }
    void FireDelay()
    {
        m_canFire = true;
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

        }

    }


    public override int Reload()
    {
        return 0;
    }
}
