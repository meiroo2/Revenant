using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Negotiator_Player : BasicWeapon_Player
{
    // Visible Member Variables
    public Transform p_LaserStartPos;
    public ObjectPuller p_MuzFlashPuller;
    public Transform m_ShellPos;
    public Sprite p_BulletSprite;
    
    
    // Member Variables
    private BulletLaserMgr m_BulletLaserMgr;
    private Player_HitscanRay m_PlayerHitscanRay;

    private void Awake()
    {
        m_isShotDelayEnd = true;
    }
    private new void Start()
    {
        base.Start();
        
        var tempIns = InstanceMgr.GetInstance();
        
        m_SoundMgrSFX = tempIns.GetComponentInChildren<SoundMgr_SFX>();
        m_Player = tempIns.GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerHitscanRay = m_Player.m_PlayerHitscanRay;
        m_Player_Arm = m_Player.m_playerRotation.gameObject.transform;
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_BulletLaserMgr = tempIns.GetComponentInChildren<BulletLaserMgr>();
        m_PlayerUI = m_Player.m_PlayerUIMgr;
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

        HitscanResult result = m_PlayerHitscanRay.GetHitscanResult();
        switch (result.m_ResultCheckNum)
        {
            case 0:
                // 빈 곳(Ray 발사해도 아무것도 감지 X)
                break;
            
            case 1:
                // 조준 실패로 근처 벽
                result.m_RayHitPoint.collider.GetComponent<IHotBox>().HitHotBox(
                    new IHotBoxParam(p_BulletDamage, p_StunValue, result.m_RayDestinationPos, WeaponType.BULLET));
                m_HitSFXMaker.EnableNewObj(0, result.m_RayDestinationPos);
                break;
            
            case 2:
                // 조준 성공
                IHotBox hotBox = result.m_RayHitPoint.collider.GetComponent<IHotBox>();
                hotBox.HitHotBox(new IHotBoxParam(p_BulletDamage, p_StunValue, result.m_RayDestinationPos,
                    WeaponType.BULLET));
                
                switch (hotBox.m_HitBoxInfo)
                {
                    case HitBoxPoint.HEAD:
                        m_HitSFXMaker.EnableNewObj(1, result.m_RayDestinationPos);
                        break;
                    
                    case HitBoxPoint.BODY:
                        m_HitSFXMaker.EnableNewObj(0, result.m_RayDestinationPos);
                        break;
                }
                break;
        }
        // Laser 소환
        m_BulletLaserMgr.PoolingBulletLaser(p_LaserStartPos.position, result.m_RayDestinationPos);

        
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