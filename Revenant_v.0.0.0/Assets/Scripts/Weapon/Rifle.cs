using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : BASEWEAPON
{
    // Visible Member Variables


    // Member Variables


    // Constructors


    // Updates
    private void Update()
    {

    }


    // Physics


    // Functions
    public override void Fire()
    {
        if (m_PlayerGun.m_canShot)
        {
            GameObject InstancedBullet = Instantiate(m_BulletPrefab);
            Player_Bullet InstancedBullet_Script = InstancedBullet.GetComponent<Player_Bullet>();

            if (m_Player.m_isRightHeaded)
                InstancedBullet_Script.InitBullet(m_BulletSpeed, m_BulletDamage);
            else
                InstancedBullet_Script.InitBullet(-m_BulletSpeed, m_BulletDamage);

            InstancedBullet.transform.SetPositionAndRotation(m_Player_Arm.position, m_Player_Arm.rotation);
            InstancedBullet_Script.m_aimedObjId = m_aimCursor.AimedObjid;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
