using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BASEWEAPON
{
    // Visible Member Variables
    [Space(30f)]
    [Header("For Effect")]
    public Animator m_GunFireAnimator;
    public Animator m_ReflectionAnimator;

    public GameObject m_Shell;
    public GameObject m_ShellPos;

    // Member Variables


    // Constructors
    private void Awake()
    {
        m_LeftBullet = m_BulletPerMag;
        m_LeftMag = m_Magcount;
    }
    private void OnEnable()
    {
        m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }

    // Updates


    // Physics


    // Functions
    public override int Fire()
    {
        if (m_isDelayEnd)
        {
            if(m_LeftBullet > 0)
            {
                m_LeftBullet--;
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

    // 기타 분류하고 싶은 것이 있을 경우
    private void Internal_Fire()
    {
        m_isDelayEnd = false;

        Invoke(nameof(setisDelayEndToTrue), m_ShotDelay);

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

        GameObject InstancedShell = Instantiate(m_Shell);
        InstancedShell.transform.position = m_ShellPos.transform.position;

        if (m_Player.m_isRightHeaded)
            InstancedShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f, 1f), ForceMode2D.Impulse);
        else
            InstancedShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 1f), ForceMode2D.Impulse);

        m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }
}
