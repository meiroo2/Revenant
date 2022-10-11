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
        p_WeaponType = 0;
        m_LeftRounds = p_MaxRound;
        p_MaxRound = 7;
        
        m_isShotDelayEnd = true;
        ReloadWeaponData();
    }
    private new void Start()
    {
        base.Start();
        
        var tempIns = InstanceMgr.GetInstance();

        m_AimCursorTransform = tempIns.GetComponentInChildren<AimCursor>().transform;
        m_SoundMgr = tempIns.GetComponentInChildren<SoundMgr>();
        m_SoundPlayer = tempIns.GetComponentInChildren<SoundPlayer>();
        m_Player = tempIns.GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerHitscanRay = m_Player.m_PlayerHitscanRay;
        m_Player_Arm = m_Player.m_playerRotation.gameObject.transform;
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_BulletLaserMgr = tempIns.GetComponentInChildren<BulletLaserMgr>();
        m_BulletTimeMgr = tempIns.GetComponentInChildren<BulletTimeMgr>();
        m_ParticleMgr = tempIns.GetComponentInChildren<ParticleMgr>();

        m_RageGauge = tempIns.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();

        m_PlayerUI = m_Player.m_PlayerUIMgr;
        
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, p_MaxRound);
    }
    
    
    // Functions
    
    public override void SetLeftRounds(int _leftRounds)
    {
        m_LeftRounds = _leftRounds;
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, p_MaxRound);
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
        switch (m_LeftRounds)
        {
            case 0:
                m_LeftRounds = p_MaxRound;
                break;

            case > 0:
                m_LeftRounds = p_MaxRound + 1;
                break;

            default:
                break;
        }
        
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, p_MaxRound);
    }

    public override bool GetCanReload()
    {
        return m_LeftRounds <= p_MaxRound;
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

        IHotBox hotBox;
        IHotBoxParam hotBoxParam;
        
        switch (result.m_ResultCheckNum)
        {
            case 0:
                // 빈 곳(Ray 발사해도 아무것도 감지 X)
                if (m_BulletTimeMgr.m_IsBulletTimeActivating)
                {
                    m_BulletTimeMgr.BookFire(m_AimCursorTransform.position, null);
                }
                else
                {
                    m_SoundPlayer.playGunFireSound(0, gameObject);
                    SpawnSFX(result.m_RayDestinationPos);
                }
                break;
            
            case 1:
                // 조준 실패로 근처 벽
                hotBox = result.m_RayHitPoint.collider.GetComponent<IHotBox>();
                hotBoxParam = new IHotBoxParam(p_BulletDamage, p_StunValue,
                    result.m_RayDestinationPos, WeaponType.BULLET);

                if (m_BulletTimeMgr.m_IsBulletTimeActivating)
                {
                    m_BulletTimeMgr.BookFire(new BulletTimeParam(hotBox, hotBoxParam, m_AimCursorTransform.position,
                        null));
                }
                else
                {
                    m_SoundPlayer.playGunFireSound(0, gameObject);
                    SpawnSFX(result.m_RayDestinationPos);
                    hotBox.HitHotBox(hotBoxParam);
                }

                m_SoundMgr.MakeSound(result.m_RayDestinationPos, true, SOUNDTYPE.BULLET);
                m_SoundMgr.MakeSound(m_AimCursorTransform.position, true, SOUNDTYPE.BULLET);
                break;
            
            case 2:
                // 조준 성공
                hotBox = result.m_RayHitPoint.collider.GetComponent<IHotBox>();
              
                
                // 현재 불릿타임 여부에 따라 발사할지 예약할지 선택
                if (m_BulletTimeMgr.m_IsBulletTimeActivating)
                {
                    hotBoxParam = new IHotBoxParam(p_BulletDamage, p_StunValue, m_AimCursorTransform.position, WeaponType.BULLET_TIME, false);
                    
                    m_BulletTimeMgr.BookFire(new BulletTimeParam(hotBox, hotBoxParam, 
                        m_AimCursorTransform.position, MakeShell));
                }
                else
                {
                    hotBoxParam = new IHotBoxParam(p_BulletDamage, p_StunValue, m_AimCursorTransform.position, WeaponType.BULLET);
                    
                    m_SoundPlayer.playGunFireSound(0, gameObject);
                    SpawnSFX(result.m_RayDestinationPos);
                    hotBox.HitHotBox(hotBoxParam);
                }
                
                
                m_SoundMgr.MakeSound(result.m_RayDestinationPos, true, SOUNDTYPE.BULLET);
                m_SoundMgr.MakeSound(m_AimCursorTransform.position, true, SOUNDTYPE.BULLET);
                break;
        }

        // UI 업데이트 필요
        m_PlayerUI.SetLeftRoundsNMag(m_LeftRounds, p_MaxRound);
    }

    private void MakeShell()
    {
        if(m_Player.m_IsRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-2f, 2f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-2f, 2f), Random.Range(0.8f, 1.5f)));
    }
    
    private void SpawnSFX(Vector2 _rayDestPos)
    {
        MakeShell();
        p_MuzFlashPuller.EnableNewObj();
        m_BulletLaserMgr.PoolingBulletLaser(p_LaserStartPos.position, _rayDestPos);
    }
}