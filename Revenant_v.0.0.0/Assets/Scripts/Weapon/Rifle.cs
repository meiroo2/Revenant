using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : BASEWEAPON
{
    // Visible Member Variables


    // Member Variables
    private float m_FireDelay = 0.1f;
    private float m_TrueDelay = 0.5f;
    private int m_FireNum = 3;
    private bool m_Resting = false;

    private bool m_Shooting = false;

    // Constructors


    // Updates
    private void Update()
    {
        if (m_Shooting && !m_Resting && m_FireDelay == 0.1f)
        {
            m_FireNum--;

            GameObject InstancedBullet = Instantiate(m_BulletPrefab);
            Player_Bullet InstancedBullet_Script = InstancedBullet.GetComponent<Player_Bullet>();

            if (m_Player.m_isRightHeaded)
                InstancedBullet_Script.InitBullet(m_BulletSpeed, m_BulletDamage);
            else
                InstancedBullet_Script.InitBullet(-m_BulletSpeed, m_BulletDamage);

            InstancedBullet.transform.SetPositionAndRotation(m_Player_Arm.position, m_Player_Arm.rotation);
            InstancedBullet_Script.m_aimedObjId = m_aimCursor.AimedObjid;

            if (m_FireNum == 0)
                m_Resting = true;
        }
        else if (m_Shooting && m_Resting)
        {
            m_TrueDelay -= Time.deltaTime;
            if (m_TrueDelay <= 0f)
            {
                m_FireDelay = 0.1f;
                m_TrueDelay = 0.5f;
                m_FireNum = 3;
                m_Shooting = false;
                m_Resting = false;
            }
        }

        if (m_Shooting)
        {
            m_FireDelay -= Time.deltaTime;
            if (m_FireDelay <= 0f)
            {
                m_FireDelay = 0.1f;
            }
        }
    }


    // Physics


    // Functions
    public override void Fire()
    {
        if (m_PlayerGun.m_canShot)
        {
            if (!m_Shooting)
                m_Shooting = true;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
