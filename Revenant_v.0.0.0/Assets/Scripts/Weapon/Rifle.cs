using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : BASEWEAPON
{
    // Visible Member Variables
    public int FireCount = 3;
    public float ContinuousFireDelay = 0.1f;

    // Member Variables
    private int m_FireCount;

    // Constructors
    private void Awake()
    {
        m_FireCount = FireCount;
        m_LeftBullet = m_BulletPerMag;
        m_LeftMag = m_Magcount;
        m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }
    private void OnEnable()
    {
        m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }

    // Updates
    private void Update()
    {
        
    }


    // Physics


    // Functions
    public override int Fire()
    {
        if (m_isDelayEnd)
        {
            if (m_LeftBullet > 0)
            {
                Internal_Fire();
                return 1;
            }
            else
            {
                return 2;
            }
        }
        return 0;
    }
    public override bool Reload()
    {
        if (m_LeftMag > 0)
        {
            if (m_LeftBullet <= m_BulletPerMag && m_LeftBullet > 0)
            {
                m_LeftMag--;
                m_LeftBullet = m_BulletPerMag + 1;
                m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
                return true;
            }
            else if (m_LeftBullet == 0)
            {
                m_LeftMag--;
                m_LeftBullet = m_BulletPerMag;
                m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
    private void Internal_Fire()
    {
        if(m_FireCount > 0 && m_LeftBullet > 0)
        {
            m_LeftBullet--;
            m_isDelayEnd = false;
            m_FireCount--;

            GameObject InstancedBullet = Instantiate(m_BulletPrefab);
            Player_Bullet InstancedBullet_Script = InstancedBullet.GetComponent<Player_Bullet>();

            InstancedBullet_Script.m_SoundMgrSFX = m_SoundMgrSFX;

            if (m_Player.m_isRightHeaded)
                InstancedBullet_Script.InitBullet(m_BulletSpeed, m_BulletDamage);
            else
                InstancedBullet_Script.InitBullet(-m_BulletSpeed, m_BulletDamage);

            InstancedBullet.transform.SetPositionAndRotation(m_Player_Arm.position, m_Player_Arm.rotation);
            InstancedBullet_Script.m_aimedObjId = m_aimCursor.AimedObjid;
            m_SoundMgrSFX.playGunFireSound(0, m_Player.gameObject);

            Invoke(nameof(Internal_Fire), ContinuousFireDelay);

            m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
        }
        else
        {
            Invoke(nameof(setisDelayEndToTrue), m_ShotDelay);
            m_FireCount = FireCount;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
