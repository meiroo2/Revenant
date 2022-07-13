using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Negotiator_Player : BasicWeapon_Player
{
    // Visible Member Variables
    public ObjectPuller p_MuzFlashPuller;
    public Transform m_ShellPos;
    public Sprite p_BulletSprite;
    
    
    // Member Variables
    private BulletPuller m_Puller;
    private BulletParam m_BulletParam;
    private BulletLaserMgr m_BulletLaserMgr;

    private void Awake()
    {
        m_isShotDelayEnd = true;
    }
    private void Start()
    {
        var tempIns = InstanceMgr.GetInstance();
        
        m_SoundMgrSFX = tempIns.GetComponentInChildren<SoundMgr_SFX>();
        m_Player = tempIns.GetComponentInChildren<Player_Manager>().m_Player;
        m_Player_Arm = m_Player.m_playerRotation.gameObject.transform;
        m_AimCursor = tempIns.GetComponentInChildren<AimCursor>();
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_Puller = tempIns.GetComponentInChildren<BulletPuller>();
        m_BulletLaserMgr = tempIns.GetComponentInChildren<BulletLaserMgr>();
        m_PlayerUI = m_Player.m_PlayerUIMgr;
        
        m_BulletParam = new BulletParam(true, p_BulletSprite, true, m_Player_Arm.position,
            m_Player_Arm.rotation, p_BulletDamage, p_BulletSpeed, p_StunValue, m_AimCursor.AimedObjid);
    }

    public override int Fire()
    {
        if (!m_isShotDelayEnd || m_isReloading)
            return 0;

        if (m_LeftRounds > 0)
        {
            Internal_Fire();
            return 1;
        }
        else
        {
            return 2;
        }

        return 0;
    }

    public override void Reload()
    {
        Internal_Reload();
    }

    public override int GetCanReload()
    {
        if (m_LeftMags <= 0 || m_LeftRounds > p_MaxBullet)
            return 0;
        else
        {
            return 1;
        }
    }

    protected override void Internal_Reload()
    {
        switch (m_LeftRounds)
        {
            case 0:
                m_LeftMags--;
                m_LeftRounds = p_MaxBullet;
                break;

            case > 0:
                m_LeftMags--;
                m_LeftRounds = p_MaxBullet + 1;
                break;

            default:
                break;
        }
        
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, m_LeftMags);
    }

    public override void InitWeapon()
    {
        
    }

    public override void ExitWeapon()
    {
        
    }

    private void Internal_Fire()
    {
        m_isShotDelayEnd = false;
        StartCoroutine(SetShotDelay());
        m_LeftRounds--;
        
        m_SoundMgrSFX.playGunFireSound(0, gameObject);

        m_BulletParam.m_IsRightHeaded = m_Player.m_IsRightHeaded;
        m_BulletParam.m_Position = m_Player_Arm.position;
        m_BulletParam.m_Rotation = m_Player_Arm.rotation;
        m_BulletParam.m_AimedObjID = m_AimCursor.AimedObjid;
        

        HitscanTargetInfo targetInfo = m_AimCursor.GetHitscanTargetInfo();
        
        // 빗나감!
        if(targetInfo.m_HotBox == null)
            m_BulletLaserMgr.PoolingBulletLaser(transform.position, targetInfo.m_AimedPos);
        else
        {
            targetInfo.m_HotBox.HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue, targetInfo.m_AimedPos,
                WeaponType.BULLET));
            m_BulletLaserMgr.PoolingBulletLaser(transform.position, targetInfo.m_AimedPos);
        }

       p_MuzFlashPuller.EnableNewObj();
        
        if(m_Player.m_IsRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));
        
        
        // UI 업데이트 필요
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, m_LeftMags);
    }
}