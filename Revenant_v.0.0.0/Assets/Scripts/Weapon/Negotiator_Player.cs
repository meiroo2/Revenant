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
    private SoundMgr m_SoundMgr;
    private Transform m_AimCursorTransform;
    private BulletTimeMgr m_BulletTimeMgr;
    private ParticleMgr m_ParticleMgr;

    private RageGauge_UI m_RageGauge;
    

    private void Awake()
    {
        m_isShotDelayEnd = true;
    }
    private new void Start()
    {
        base.Start();
        
        var tempIns = InstanceMgr.GetInstance();

        m_AimCursorTransform = tempIns.GetComponentInChildren<AimCursor>().transform;
        m_SoundMgr = tempIns.GetComponentInChildren<SoundMgr>();
        MSoundPlayer = tempIns.GetComponentInChildren<SoundPlayer>();
        m_Player = tempIns.GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerHitscanRay = m_Player.m_PlayerHitscanRay;
        m_Player_Arm = m_Player.m_playerRotation.gameObject.transform;
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_BulletLaserMgr = tempIns.GetComponentInChildren<BulletLaserMgr>();
        m_BulletTimeMgr = tempIns.GetComponentInChildren<BulletTimeMgr>();
        m_ParticleMgr = tempIns.GetComponentInChildren<ParticleMgr>();

        m_RageGauge = tempIns.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();

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
        

        HitscanResult result = m_PlayerHitscanRay.GetHitscanResult();
        switch (result.m_ResultCheckNum)
        {
            case 0:
                // 빈 곳(Ray 발사해도 아무것도 감지 X)
                MSoundPlayer.playGunFireSound(0, gameObject);
                SpawnSFX(result.m_RayDestinationPos);
                break;
            
            case 1:
                // 조준 실패로 근처 벽
                MSoundPlayer.playGunFireSound(0, gameObject);
                
                result.m_RayHitPoint.collider.GetComponent<IHotBox>().HitHotBox(
                    new IHotBoxParam(p_BulletDamage, p_StunValue, result.m_RayDestinationPos, WeaponType.BULLET));
                
                SpawnSFX(result.m_RayDestinationPos);
                
                m_SoundMgr.MakeSound(result.m_RayDestinationPos, true, SOUNDTYPE.BULLET);
                m_SoundMgr.MakeSound(m_AimCursorTransform.position, true, SOUNDTYPE.BULLET);
                break;
            
            case 2:
                // 조준 성공
                IHotBox hotBox = result.m_RayHitPoint.collider.GetComponent<IHotBox>();
                IHotBoxParam hotBoxParam = new IHotBoxParam(p_BulletDamage, p_StunValue,
                    m_AimCursorTransform.position, WeaponType.BULLET);

                // 현재 불릿타임 여부에 따라 발사할지 예약할지 선택
                if (m_BulletTimeMgr.m_BulletTimeActivating)
                {
                    m_BulletTimeMgr.AddHotBoxAction(() => SpawnSFX(result.m_RayDestinationPos));
                    m_BulletTimeMgr.BookFire(new BulletTimeParam(hotBox, hotBoxParam));
                }
                else
                {
                    MSoundPlayer.playGunFireSound(0, gameObject);
                    SpawnSFX(result.m_RayDestinationPos);
                    hotBox.HitHotBox(hotBoxParam);
                }
                
                
                m_SoundMgr.MakeSound(result.m_RayDestinationPos, true, SOUNDTYPE.BULLET);
                m_SoundMgr.MakeSound(m_AimCursorTransform.position, true, SOUNDTYPE.BULLET);
                break;
        }

        // UI 업데이트 필요
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, m_LeftMags);
    }

    private void MakeShell()
    {
        if(m_Player.m_IsRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));
    }
    
    private void SpawnSFX(Vector2 _rayDestPos)
    {
        MakeShell();
        p_MuzFlashPuller.EnableNewObj();
        m_BulletLaserMgr.PoolingBulletLaser(p_LaserStartPos.position, _rayDestPos);
    }
}