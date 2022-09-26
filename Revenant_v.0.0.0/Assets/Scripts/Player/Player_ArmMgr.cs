using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_ArmMgr : MonoBehaviour
{
    // Visible Member Variables
    public VisualPart[] p_RotatingVisualPartArr;

    public float p_BackToAnimTime = 1f;
    public float RecoilDistance = 0.05f;
    public float RecoilSpeed = 20f;
    
    
    // Member Variables
    [HideInInspector] public bool m_IsReloading = false;
    private Player_AniMgr m_PlayerAniMgr;
    private WeaponMgr m_WeaponMgr;
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private Player_InputMgr m_InputMgr;

    private Coroutine m_BackToAttackCoroutine;

    private bool m_isRecoilLerping = false;
    private float m_RecoilTimer = 1f;
    



    // Constructors
    private void Awake()
    {
        m_PlayerRotation = GetComponent<PlayerRotation>();
        //m_PlayerRotation.m_AnglePhaseAction += ReceiveAnglePhase;
    }
    private void Start()
    {
        InstanceMgr _instance = InstanceMgr.GetInstance();
        m_Player = _instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_WeaponMgr = m_Player.m_WeaponMgr;
        m_PlayerAniMgr = m_Player.m_PlayerAniMgr;
        m_InputMgr = m_Player.m_InputMgr;
    }
    
    
    // Updates
    private void Update()
    {
        if (m_InputMgr.m_IsPushAttackKey && m_Player.m_CanAttack && !m_IsReloading)
            DoAttack();
        
        if (m_InputMgr.m_IsPushReloadKey && m_WeaponMgr.m_CurWeapon.GetCanReload() == 1 &&
            !m_IsReloading)
            DoReload();

        if (!m_PlayerAniMgr.m_IsFightMode)
            return;
        
        ChangeRotatingVisualPart(m_PlayerRotation.m_curAnglePhase);
    }

    // Physics


    // Functions

    /// <summary>
    /// Action을 이용해 PlayerRotation에서 값을 받습니다.
    /// </summary>
    /// <param name="_Phase"></param>
    private void ReceiveAnglePhase(int _Phase)
    {
        for (int i = 0; i < p_RotatingVisualPartArr.Length; i++)
        {
            p_RotatingVisualPartArr[i].SetSprite(_Phase);
        }
    }

    private void ChangeRotatingVisualPart(int _phase)
    {
        for (int i = 0; i < p_RotatingVisualPartArr.Length; i++)
        {
            p_RotatingVisualPartArr[i].SetSprite(_phase);
        }
    }
    
    
    private void DoAttack()
    {
        switch (m_WeaponMgr.m_CurWeapon.Fire())
        {
            case 0:
                // 발사 실패
                Debug.Log("플레이어 발사 실패");
                break;

            case 1:
                // 발사 성공
                m_RecoilTimer = 1f;
                break;

            case 2:
                // 총알 없음 (자동 재장전)
                m_Player.m_SFXMgr.playPlayerSFXSound(1);
                if (!m_IsReloading)
                    DoReload();
                break;
        }

        m_PlayerAniMgr.ChangeAniModeToFight(true);
        
        // Timer 확인용 코루 - 마우스 클릭하고 시간 지나면 FightMode 끔
        if(!ReferenceEquals(m_BackToAttackCoroutine, null))
            StopCoroutine(m_BackToAttackCoroutine);
            
        m_BackToAttackCoroutine = StartCoroutine(BackToAttackAnimTimer());
    }

    private IEnumerator BackToAttackAnimTimer()
    {
        yield return new WaitForSeconds(p_BackToAnimTime);
        m_PlayerAniMgr.ChangeAniModeToFight(false);
    }

    
    
    
    
    private void DoReload()
    {
        m_PlayerAniMgr.ChangeAniModeToFight(false);
        
        m_Player.m_SFXMgr.playPlayerSFXSound(2);
        m_Player.m_PlayerUIMgr.ResetCallback();
        m_Player.m_PlayerUIMgr.AddCallback(m_WeaponMgr.m_CurWeapon.Reload);
        m_Player.m_PlayerUIMgr.AddCallback(() => m_IsReloading = false);
        m_Player.m_PlayerUIMgr.AddCallback(() => m_Player.m_PlayerAniMgr.PlayReloadAnim(false));
        m_Player.m_PlayerUIMgr.StartReload();
        m_Player.m_PlayerAniMgr.PlayReloadAnim(true);
    }

    public void StopReload()
    {
        if (!m_IsReloading)
            return;
        
        m_Player.m_PlayerUIMgr.ForceStopReload();
    }
    

    // 기타 분류하고 싶은 것이 있을 경우
}