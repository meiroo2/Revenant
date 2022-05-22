﻿using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Negotiator_Player : BasicWeapon_Player
{
    // Visible Member Variables
    public ObjectPuller p_MuzFlashPuller;
    public Transform m_ShellPos;
    
    
    // Member Variables
    private Player_BulletPuller m_Puller;

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
        m_aimCursor = tempIns.GetComponentInChildren<AimCursor>();
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_Puller = tempIns.GetComponentInChildren<Player_BulletPuller>();
    }

    public override int Fire()
    {
        if (!m_isShotDelayEnd || m_isReloading)
            return 0;

        if (m_LeftBullet > 0)
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

    public override int Reload()
    {
        if (m_LeftMag <= 0 || m_LeftBullet > p_MaxBullet)
            return 0;
        else
        {
            StartCoroutine(SetReload());
            return 1;
        }
    }

    protected override void Internal_Reload()
    {
        switch (m_LeftBullet)
        {
            case 0:
                m_LeftMag--;
                m_LeftBullet = p_MaxBullet;
                break;

            case > 0:
                m_LeftMag--;
                m_LeftBullet = p_MaxBullet + 1;
                break;

            default:
                break;
        }
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
        m_LeftBullet--;
        
        m_SoundMgrSFX.playGunFireSound(0, gameObject);
        m_Puller.MakeBullet(m_Player.m_isRightHeaded, m_Player_Arm.position,
            m_Player_Arm.rotation, p_BulletSpeed, p_BulletDamage);
        
        p_MuzFlashPuller.EnableNewObj();
        
        if(m_Player.m_isRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));
        
        
        // UI 업데이트 필요
    }
}