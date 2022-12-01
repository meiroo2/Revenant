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

    // Assign
    [Space(20f)] 
    public Player_AniMgr m_PlayerAniMgr;
    
    // Member Variables
    public bool m_IsReloading { get; private set; } = false;
    private WeaponMgr m_WeaponMgr;
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private Player_InputMgr m_InputMgr;
    private Player_UI m_PlayerUI;
    private bool m_BanVisualRotation = false;

    private Coroutine m_BackToAttackCoroutine;
    
    private bool m_isRecoilLerping = false;
    private float m_RecoilTimer = 1f;

    private Vector2[] m_InitRecoilTransformPosArr;

    private Coroutine m_RecoilCoroutine;
    private Coroutine m_ReloadCoroutine;

    private readonly int Reload = Animator.StringToHash("Reload");
    private readonly int SReload = Animator.StringToHash("SReload");
    private readonly int WReload = Animator.StringToHash("WReload");
    

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
        m_PlayerUI = _instance.m_Player_UI;
        
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_WeaponMgr = m_Player.m_WeaponMgr;
        m_InputMgr = GameMgr.GetInstance().p_PlayerInputMgr;
    }
    
    
    // Updates
    private void Update()
    {
        /*
        if (m_InputMgr.m_IsPushAttackKey && m_Player.m_CanAttack && !m_IsReloading)
            DoAttack();
        
        if (m_InputMgr.m_IsPushReloadKey && m_WeaponMgr.m_CurWeapon.GetCanReload() == 1 &&
            !m_IsReloading)
            DoReload();

        if (!m_PlayerAniMgr.m_IsFightMode || m_BanVisualRotation)
            return;
        */
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
    
    
    /// <summary>
    /// 플레이어의 공격을 실행합니다.
    /// </summary>
    /// <returns>0 = 실패, 1 = 성공, 2 = 재장전 시도</returns>
    public int DoAttack()
    {
        // 각도
        m_PlayerRotation.DoRotate();
        
        if (m_IsReloading)
            return 0;
        
        if(m_WeaponMgr.m_CurWeapon.m_LeftRounds <= 0)
            DoReload();
        
        
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("BulletFull",
            (float)m_WeaponMgr.m_CurWeapon.m_LeftRounds / 8f);
        
        switch (m_WeaponMgr.m_CurWeapon.Fire())
        {
            case 0:
                // 발사 실패
                Debug.Log("플레이어 발사 실패");
                
                return 0;
                break;

            case 1:
                // 발사 성공
                m_RecoilTimer = 1f;
                
                // 반동
                DoRecoil();

                // 전투모드 바꾸고 각도 초기화
                if (!m_Player.m_BulletTimeMgr.m_IsBulletTimeActivating)
                {
                    m_PlayerAniMgr.ChangeArmAniToAngleChange(true);
                    ChangeRotatingVisualPart(m_PlayerRotation.m_curAnglePhase);
                }

                // Timer 확인용 코루 - 마우스 클릭하고 시간 지나면 FightMode 끔
                if(!ReferenceEquals(m_BackToAttackCoroutine, null))
                    StopCoroutine(m_BackToAttackCoroutine);
            
                m_BackToAttackCoroutine = StartCoroutine(BackToAttackAnimTimer());

                return 1;
                break;

            case 2:
                // 총알 없음 (자동 재장전)
                m_Player.m_SoundPlayer.PlayPlayerSoundOnce(2);
                if (!m_IsReloading)
                    DoReload();
                
                return 2;
                break;
        }

        return -1;
    }

    public void ForceStopBackToAttackAnim()
    {
        if(!ReferenceEquals(m_BackToAttackCoroutine, null))
            StopCoroutine(m_BackToAttackCoroutine);
        
        m_PlayerAniMgr.ChangeArmAniToAngleChange(false);
    }
    
    private IEnumerator BackToAttackAnimTimer()
    {
        yield return new WaitForSeconds(p_InitArmLerpDelayTime);

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

        m_PlayerAniMgr.ChangeArmAniToAngleChange(false);
    }

    
    
    
    /// <summary>
    /// 현재 들고 있는 무기를 재장전합니다.
    /// </summary>
    /// <param name="_force">true일 경우 탄창을 소모하지 않고, 애니 스킵후 강제 재장전</param>
    public void DoReload(bool _force = false)
    {
        // 중복 재장전 금지, 재장전 불가능시 return
        if (m_IsReloading || !m_WeaponMgr.m_CurWeapon.GetCanReload())
            return;
        
        if (_force)
        {
            m_WeaponMgr.m_CurWeapon.Reload();
            return;
        }
   
        // 사격 후딜, 재장전 코루틴 멈춤
        if(!ReferenceEquals(m_BackToAttackCoroutine, null))
            StopCoroutine(m_BackToAttackCoroutine);
        
        if(!ReferenceEquals(m_ReloadCoroutine, null))
            StopCoroutine(m_ReloadCoroutine);

        // 매직코딩이니까 꼭 고쳐라
        if (m_Player.m_CurPlayerFSMName != PlayerStateName.HIDDEN)
            m_PlayerAniMgr.SetVisualParts(false, true, true, false);
        
        m_PlayerAniMgr.ChangeArmAniToAngleChange(false);


        // 사운드 재생
        m_IsReloading = true;
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(0);
        
        m_PlayerUI.ActivateReloadMode(true);
        
        m_ReloadCoroutine = StartCoroutine(ReloadCoroutine());
    }

    
    /// <summary>
    /// Reload를 멈춥니다.(즉시 발동)
    /// </summary>
    public void StopReload()
    {
        if(!ReferenceEquals(m_BackToAttackCoroutine, null))
            StopCoroutine(m_BackToAttackCoroutine);
        
        if (!ReferenceEquals(m_ReloadCoroutine, null))
            StopCoroutine(m_ReloadCoroutine);
        
        m_IsReloading = false;
        m_PlayerUI.ActivateReloadMode(false);
        
        m_PlayerAniMgr.p_UpperBody.SetAnim_Int(Reload, 0);
        m_PlayerAniMgr.ChangeArmAniToAngleChange(false);
    }

    
    private IEnumerator ReloadCoroutine()
    {
        float normTime = 0f;
        Animator upAnimator = m_Player.m_PlayerAniMgr.p_UpperBody.m_Animator;
        VisualPart upVisPart = m_Player.m_PlayerAniMgr.p_UpperBody;
        
        upAnimator.SetInteger(Reload, 1);
        upAnimator.Play(m_Player.m_CurPlayerFSMName == PlayerStateName.WALK ? WReload : SReload,
            -1, 0f);
        
        int paramNum = m_Player.m_CurPlayerFSMName == PlayerStateName.WALK ? 2 : 1;

        while (true)
        {
            yield return null;
            normTime = upAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            m_PlayerUI.p_ReloadCircle.fillAmount = normTime;

            switch (m_Player.m_CurPlayerFSMName)
            {
                case PlayerStateName.IDLE:
                    if (paramNum == 1)
                        break;
                    
                    paramNum = 1;
                    upAnimator.Play(SReload, -1, normTime);
                    break;
                
                case PlayerStateName.WALK:
                    if (paramNum == 2)
                        break;
                    
                    paramNum = 2;
                    upAnimator.Play(WReload, -1, normTime);
                    break;
                
                default:
                    if (paramNum == 1)
                        break;
                    
                    paramNum = 1;
                    upAnimator.Play(SReload, -1, normTime);
                    break;
            }

            if (normTime >= 1f)
                break;
        }

        upAnimator.SetInteger(Reload, 0);
        
        m_WeaponMgr.m_CurWeapon.Reload();
        m_PlayerUI.ActivateReloadMode(false);
        m_IsReloading = false;
        
        yield break;
    }
    

    // 기타 분류하고 싶은 것이 있을 경우
}