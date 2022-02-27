using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BASEWEAPON
{
    // Visible Member Variables
    public Animator m_GunFireAnimator;
    public Animator m_ReflectionAnimator;
    public SoundMgr_SFX m_SoundMgrSFX;

    // Member Variables


    // Constructors
    private void Awake()
    {
        
    }
    private void Start()
    {

    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public override void Fire()
    {
        if (m_PlayerGun.m_canShot)
        {
            m_SoundMgrSFX.playGunFireSound(0, m_Player.gameObject);

            m_GunFireAnimator.Play("Rifle_Gunfire", -1, 0f);
            m_ReflectionAnimator.Play("Rifle_Reflection", -1, 0f);

            GameObject InstancedBullet = Instantiate(m_BulletPrefab);
            Player_Bullet InstancedBullet_Script = InstancedBullet.GetComponent<Player_Bullet>();

            InstancedBullet_Script.m_SoundMgrSFX = m_SoundMgrSFX;

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
