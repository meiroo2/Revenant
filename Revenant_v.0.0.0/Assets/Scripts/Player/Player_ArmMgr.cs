using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_ArmMgr : MonoBehaviour
{
    // Visible Member Variables
    public VisualPart[] p_RotatingVisualPartArr;

    public float p_InitArmLerpDelayTime = 1f;
    public float p_DelayBetArmAngle = 0.2f;
    
    public float p_RecoilDistance = 0.05f;
    public float p_RecoilSpeed = 20f;

    public Transform[] p_RecoilTransforms;
    
    
    // Member Variables
    [HideInInspector] public bool m_IsReloading = false;
    private Player_AniMgr m_PlayerAniMgr;
    private WeaponMgr m_WeaponMgr;
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private Player_InputMgr m_InputMgr;
    private bool m_BanVisualRotation = false;

    private Coroutine m_BackToAttackCoroutine;
    
    private bool m_isRecoilLerping = false;
    private float m_RecoilTimer = 1f;

    private Vector2[] m_InitRecoilTransformPosArr;

    private Coroutine m_RecoilCoroutine;
    



    // Constructors
    private void Awake()
    {
        m_InitRecoilTransformPosArr = new Vector2[p_RecoilTransforms.Length];
        for (int i = 0; i < m_InitRecoilTransformPosArr.Length; i++)
        {
            m_InitRecoilTransformPosArr[i] = p_RecoilTransforms[i].localPosition;
        }
        
        
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

        if (!m_PlayerAniMgr.m_IsFightMode || m_BanVisualRotation)
            return;
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

    public void DoRecoil()
    {
        if (!ReferenceEquals(m_RecoilCoroutine, null))
        {
            StopCoroutine(m_RecoilCoroutine);
        }
        
        for (int i = 0; i < p_RecoilTransforms.Length; i++)
        {
            p_RecoilTransforms[i].localPosition = m_InitRecoilTransformPosArr[i];
        }

        m_RecoilCoroutine = StartCoroutine(RecoilCoroutine());
    }

    private IEnumerator RecoilCoroutine() 
    {
        float timer = 0f;
        
        for (int i = 0; i < p_RecoilTransforms.Length; i++)
        {
            p_RecoilTransforms[i].localPosition = new Vector2(
                p_RecoilTransforms[i].localPosition.x - p_RecoilDistance, p_RecoilTransforms[i].localPosition.y
                );
        }

        while (true)
        {
            timer += Time.deltaTime;
            
            for (int i = 0; i < p_RecoilTransforms.Length; i++)
            {          
                p_RecoilTransforms[i].localPosition =
                    Vector2.Lerp(p_RecoilTransforms[i].localPosition, m_InitRecoilTransformPosArr[i],
                        Time.deltaTime * p_RecoilSpeed);
            }

            if (timer >= 5f)
            {
                break;
            }

            yield return null;
        }
        
        yield break;
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
                
                // 반동
                DoRecoil();
                
                // 전투모드 바꾸고 각도 초기화
                if (!m_Player.m_BulletTimeMgr.m_IsBulletTimeActivating)
                {
                    m_PlayerAniMgr.ChangeAniModeToFight(true);
                    ChangeRotatingVisualPart(m_PlayerRotation.m_curAnglePhase);
                }

                // Timer 확인용 코루 - 마우스 클릭하고 시간 지나면 FightMode 끔
                if(!ReferenceEquals(m_BackToAttackCoroutine, null))
                    StopCoroutine(m_BackToAttackCoroutine);
            
                m_BackToAttackCoroutine = StartCoroutine(BackToAttackAnimTimer());
                
                break;

            case 2:
                // 총알 없음 (자동 재장전)
                m_Player.m_SFXMgr.playPlayerSFXSound(1);
                if (!m_IsReloading)
                    DoReload();
                break;
        }
    }

    private IEnumerator BackToAttackAnimTimer()
    {
        float Timer = 0f;

        while (true)
        {
            Timer += Time.deltaTime;
            ChangeRotatingVisualPart(m_PlayerRotation.m_curAnglePhase);

            if (Timer >= p_InitArmLerpDelayTime)
            {
                break;
            }
            
            yield return null;
        }

        switch (m_PlayerRotation.m_curAnglePhase)
        {
            case < 4:
                for (int i = 4; i <= 8; i += 2)
                {
                    ChangeRotatingVisualPart(i);
                    yield return new WaitForSeconds(p_DelayBetArmAngle);
                }
                break;
            
            case < 6:
                for (int i = 6; i <= 8; i += 2)
                {
                    ChangeRotatingVisualPart(i);
                    yield return new WaitForSeconds(p_DelayBetArmAngle);
                }
                break;
            
            case < 8:
                ChangeRotatingVisualPart(8);
                yield return new WaitForSeconds(p_DelayBetArmAngle);
                break;
        }

        m_PlayerAniMgr.ChangeAniModeToFight(false);
    }

    
    
    
    /// <summary>
    /// 현재 들고 있는 무기를 재장전합니다.
    /// </summary>
    /// <param name="_force">true일 경우 탄창을 소모하지 않고, 애니 스킵후 강제 재장전</param>
    private void DoReload(bool _force = false)
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