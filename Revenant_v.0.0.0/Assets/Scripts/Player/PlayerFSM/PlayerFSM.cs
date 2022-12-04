using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class PlayerFSM
{
    protected Player m_Player;
    protected Player_InputMgr m_InputMgr;
    protected Transform m_PlayerTransform;
    
    // For Action
    public Action m_InitAction = null;
    public Action m_ExitAction = null;

    protected PlayerFSM(Player _player)
    {
        m_Player = _player;
    }

    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();

    protected void InitFunc()
    {
        m_InputMgr = m_Player.m_InputMgr;
        m_PlayerTransform = m_Player.transform;
    }
    
    protected void CheckNull()
    {
        if (ReferenceEquals(m_Player, null))
            return;
    }

    protected void ExitFinalProcess()
    {
        //m_Player.m_Player_AniMgr.exitplayerAnim();
    }

    protected void CheckMeleeAttack()
    {
        //m_InputMgr.
    }
}

public class Player_IDLE : PlayerFSM
{
    private RageGauge m_RageGauge;
    private Player_UseRange m_UseRange;
    
    public Player_IDLE(Player _player) : base(_player)
    {
        
    }

    public override void StartState()
    {
        InitFunc();
        m_UseRange = m_Player.m_useRange;
        m_RageGauge = m_Player.m_RageGauge;
        m_Player.m_CanHide = true;
        
        m_Player.m_ArmMgr.ForceStopBackToAttackAnim();
        m_Player.m_PlayerAniMgr.SetVisualParts(false, true, true, false);
        
        m_Player.m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        
        m_Player.ActiveBreathCoroutine(true);
    }

    public override void UpdateState()
    {
        if (m_InputMgr.m_IsPushAttackKey)
        {
            m_Player.m_ArmMgr.DoAttack();
        }
        else if (m_InputMgr.m_IsPushReloadKey)
        {
            m_Player.m_ArmMgr.DoReload();
        }
        
        if (m_InputMgr.m_IsPushInteractKey)
        {
            m_UseRange.UseNearestObj();
        }
        
        
        if(m_InputMgr.GetDirectionalKeyInput() != 0)
            m_Player.ChangePlayerFSM(PlayerStateName.WALK);
        else if(m_InputMgr.m_IsPushRollKey && m_RageGauge.UseSkillByConsumeRageGauge(0))
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        else if(m_InputMgr.m_IsPushSideAttackKey && m_RageGauge.UseSkillByConsumeRageGauge(1))
            m_Player.ChangePlayerFSM(PlayerStateName.MELEE);
        else if (m_InputMgr.m_IsPushBulletTimeKey && m_RageGauge.UseSkillByConsumeRageGauge(2))
        {
            m_Player.ChangePlayerFSM(PlayerStateName.BULLET_TIME);
        }
        else if (m_InputMgr.m_IsPushHideKey)
        {
            if(m_UseRange.DoHide(true) == 1)
                m_Player.ChangePlayerFSM(PlayerStateName.HIDDEN);
        }
    }

    public override void ExitState()
    {
        m_Player.ActiveBreathCoroutine(false);
        m_Player.m_CanHide = false;
        m_Player.m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        ExitFinalProcess();
    }
}

public class Player_WALK : PlayerFSM
{
    private RageGauge m_RageGauge;
    
    private int m_CurInput = 0;
    
    private VisualPart m_UpperBody;
    private VisualPart m_LowerBody;
    private Player_UseRange m_UseRange;
    private readonly int Walk = Animator.StringToHash("Walk");

    private float m_WalkSoundDelay = 0.5f;
    private CoroutineElement m_WalkSoundCoroutineElement;
    private MatType m_MatType;


    public Player_WALK(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_PlayerTransform = m_Player.transform;
        m_UseRange = m_Player.m_useRange;
        m_RageGauge = m_Player.m_RageGauge;
        m_UpperBody = m_Player.m_PlayerAniMgr.p_UpperBody;
        m_LowerBody = m_Player.m_PlayerAniMgr.p_LowerBody;
        
        m_Player.m_CanHide = true;
        m_InputMgr = m_Player.m_InputMgr;
        m_CurInput = 0;

        m_WalkSoundCoroutineElement = GameMgr.GetInstance().p_CoroutineHandler.StartCoroutine_Handler(CheckMatType());
    }

    public override void UpdateState()
    {
        if (m_InputMgr.m_IsPushAttackKey)
        {
            m_Player.m_ArmMgr.DoAttack();
        }
        else if (m_InputMgr.m_IsPushReloadKey)
        {
            m_Player.m_ArmMgr.DoReload();
        }
        
        if (m_InputMgr.m_IsPushInteractKey)
        {
            m_UseRange.UseNearestObj();
        }
        
        
        m_CurInput = m_InputMgr.GetDirectionalKeyInput();
        
        if (m_Player.m_CanMove)
        {
            switch (m_CurInput)
            {
                case -1:
                    if (m_Player.GetIsPlayerWalkStraight())
                    {
                        m_UpperBody.SetAnim_Int(Walk, 1);
                        m_LowerBody.SetAnim_Int(Walk, 1);
                        m_Player.MoveByDirection(-1);
                    }
                    else
                    {
                        m_UpperBody.SetAnim_Int(Walk, -1);
                        m_LowerBody.SetAnim_Int(Walk, -1);
                        m_Player.MoveByDirection(-1, m_Player.p_BackSpeedMulti);
                    }

                    break;

                case 0:
                    m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
                    break;

                case 1:
                    if (m_Player.GetIsPlayerWalkStraight())
                    {
                        m_UpperBody.SetAnim_Int(Walk, 1);
                        m_LowerBody.SetAnim_Int(Walk, 1);
                        m_Player.MoveByDirection(1);
                    }
                    else
                    {
                        m_UpperBody.SetAnim_Int(Walk, -1);
                        m_LowerBody.SetAnim_Int(Walk, -1);
                        m_Player.MoveByDirection(1, m_Player.p_BackSpeedMulti);
                    }

                    break;
            }
        }


        if (m_InputMgr.m_IsPushRollKey && m_RageGauge.UseSkillByConsumeRageGauge(0))
        {
            m_Player.setisRightHeaded(m_CurInput > 0);
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        else if (m_InputMgr.m_IsPushSideAttackKey && m_RageGauge.UseSkillByConsumeRageGauge(1))
        {
            m_Player.ChangePlayerFSM(PlayerStateName.MELEE);
        }
        else if (m_InputMgr.m_IsPushBulletTimeKey && m_RageGauge.UseSkillByConsumeRageGauge(2))
        {
            m_Player.ChangePlayerFSM(PlayerStateName.BULLET_TIME);
        }
        else if (m_InputMgr.m_IsPushHideKey)
        {
            if(m_UseRange.DoHide(true) == 1)
                m_Player.ChangePlayerFSM(PlayerStateName.HIDDEN);
        }
    }

    public override void ExitState()
    {
        m_UpperBody.SetAnim_Int(Walk, 0);
        m_LowerBody.SetAnim_Int(Walk, 0);

        m_Player.m_CanHide = false;
        m_Player.MoveByDirection(0);

        if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
        {
            m_WalkSoundCoroutineElement.StopCoroutine_Element();
            m_WalkSoundCoroutineElement = null;
        }

        ExitFinalProcess();
    }

    private IEnumerator CheckMatType()
    {
        SoundPlayer player = GameMgr.GetInstance().p_SoundPlayer;
        Collider2D collider;
        
        while (true)
        {
            collider = m_Player.m_PlayerFootMgr.GetFootRayHit().collider;
            if (ReferenceEquals(collider, null))
            {
                Debug.LogWarning("ERR : There is no Collider On CheckMatType()");
            }

            if (collider.TryGetComponent(out IMatType matType))
            {
                player.PlayCommonSoundByMatType(0, matType.m_matType, m_PlayerTransform.position);
            }
            else
            {
                Debug.LogWarning("ERR : There is no Collider On CheckMatType()");
            }
            yield return new WaitForSeconds(m_WalkSoundDelay);
        }

        yield break;
    }
}

public class Player_ROLL : PlayerFSM
{
    private Vector2 m_PlayerNormalVec;
    private Animator m_FullBodyAnimator;
    private RageGauge m_RageGauge;
    private Player_InputMgr m_InputMgr;
    private Player_UseRange m_UseRange;
    
    private float m_DecelerationSpeed = 0f;
    private float m_Timer = 0f;
    
    private CoroutineElement m_JustEvadeCoroutineElement;
    private bool m_CoroutineExitCheck = false;
    
    private readonly int Roll = Animator.StringToHash("Roll");

    public Player_ROLL(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_JustEvadeCoroutineElement = null;
        m_CoroutineExitCheck = false;

        m_FullBodyAnimator = m_Player.m_PlayerAniMgr.p_FullBody.m_Animator;
        m_RageGauge = m_Player.m_RageGauge;
        m_InputMgr = m_Player.m_InputMgr;
        m_UseRange = m_Player.m_useRange;
        
        m_InitAction?.Invoke();
        m_Player.m_playerRotation.m_doRotate = false;

        m_Player.m_PlayerHotBox.m_hotBoxType = 2;
        m_DecelerationSpeed = 0f;
        m_Timer = 0f;
        
        m_Player.m_PlayerAniMgr.SetVisualParts(true, false, false, false);
        m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Roll, 1);
        
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(4);
        //m_RageGauge.ChangeGaugeValue(m_RageGauge.m_CurGaugeValue - m_RageGauge.p_Gauge_Consume_Roll);

        // 코루틴 시작
        m_JustEvadeCoroutineElement = 
            GameMgr.GetInstance().p_CoroutineHandler.StartCoroutine_Handler(CheckEvade());
        
        m_CoroutineExitCheck = true;
    }

    public override void UpdateState()
    {
        m_Timer = Time.deltaTime;
        
        m_Player.MoveByDirection(m_Player.m_IsRightHeaded ? 1 : -1, (m_Player.p_RollSpeedMulti - m_DecelerationSpeed));
        if (m_Player.p_RollSpeedMulti - m_DecelerationSpeed > 0f)
            m_DecelerationSpeed += m_Timer * m_Player.p_RollDecelerationSpeed;
        
        
        if (m_InputMgr.m_IsPushSideAttackKey && m_RageGauge.UseSkillByConsumeRageGauge(1))
        {
            m_Player.m_CancelMeleeStartAnim = true;
            m_Player.ChangePlayerFSM(PlayerStateName.MELEE);
        }
        else if (m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
        }
        else if (m_InputMgr.m_IsPushHideKey && m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            if (m_UseRange.DoHide(true) == 1)
                m_Player.ChangePlayerFSM(PlayerStateName.HIDDEN);
        }
    }

    public override void ExitState()
    {
        m_ExitAction?.Invoke();
        m_Player.m_CanHide = false;
        m_Player.m_CanAttack = true;

        m_Player.MoveByDirection(0);
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerHotBox.m_hotBoxType = 0;
        m_Player.m_PlayerHotBox.m_HitCount = 0;
        
        m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Roll, 0);
        m_Player.m_PlayerAniMgr.SetVisualParts(false, true, true, false);
        
        // 코루틴 끝
        SafetyOut();

        ExitFinalProcess();
    }

    private IEnumerator CheckEvade()
    {
        float normalTime;

        // 좋은 생각이 이거말고 떠오르지 않음
        while (true)
        {
            normalTime = m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (m_Player.m_PlayerHotBox.m_HitCount > 0)
            {
                if (normalTime >= m_Player.p_JustEvadeNormalizeTime.x &&
                    normalTime <= m_Player.p_JustEvadeNormalizeTime.y)
                {
                    // 저스트 회피
                    m_Player.m_ScreenEffectUI.ActivateScreenColorDistortionEffect();
                    m_Player.m_BulletTimeMgr.LerpingTimeScale(0.1f);
                    m_Player.m_ScreenEffectUI.ActivateLensDistortEffect(0.2f);
                    
                    m_Player.m_ParticleMgr.MakeParticle(m_Player.GetPlayerCenterPos(),
                        m_Player.p_CenterTransform,  JustEvadeGaugeUp);
                    
                    Debug.Log("저스트 회피!!");
                    break;
                }
                else
                {
                    // 그냥 회피
                    m_Player.m_ParticleMgr.MakeParticle(m_Player.GetPlayerCenterPos(),
                        m_Player.p_CenterTransform,  EvadeGaugeUp);
                    
                    Debug.Log("그냥 회피");
                    break;
                }
            }
            
            yield return null;
        }
        
        SafetyOut();
        yield break;
    }

    private void JustEvadeGaugeUp()
    {
        var rageGauge = m_Player.m_RageGauge;
        rageGauge.AddGaugeValue(rageGauge.p_Gauge_Refill_JustEvade);
    }

    private void EvadeGaugeUp()
    {
        var rageGauge = m_Player.m_RageGauge;
        rageGauge.AddGaugeValue(rageGauge.p_Gauge_Refill_Evade);
    }

    private void SafetyOut()
    {
        if (!m_CoroutineExitCheck)
            return;
        
        m_CoroutineExitCheck = false;
        //Debug.Log("SafetyOut");

        if (!ReferenceEquals(m_JustEvadeCoroutineElement, null))
        {
            m_JustEvadeCoroutineElement.StopCoroutine_Element();
            m_JustEvadeCoroutineElement = null;
        }
    }
}

public class Player_HIDDEN : PlayerFSM
{
    private int m_KeyInput = 0;
    private Player_FootMgr _mFootMgr;
    private Rigidbody2D m_Rigid;
    private SoundPlayer m_SoundPlayer;
    private RageGauge m_RageGauge;
    private Player_UseRange m_UseRange;
    private readonly int Hide = Animator.StringToHash("Hide"); 

    public Player_HIDDEN(Player _player) : base(_player)
    {
       
    }

    public override void StartState()
    {
        m_RageGauge = m_Player.m_RageGauge;
		m_InitAction?.Invoke();

		m_Player.m_CanAttack = false;
        m_InputMgr = m_Player.m_InputMgr;
        m_UseRange = m_Player.m_useRange;
        
        m_Player.m_PlayerAniMgr.SetVisualParts(true, false, false, false);
        m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Hide, 1);
        
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(5);
        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        
        // RageGauge 숨기시 일시정지
        m_RageGauge.TempStopRageGauge(true);
    }

    public override void UpdateState()
    {
        m_KeyInput = m_InputMgr.GetDirectionalKeyInput();

        if (m_InputMgr.m_IsPushRollKey && m_RageGauge.UseSkillByConsumeRageGauge(0))
        {
            if(m_KeyInput > 0)
                m_Player.setisRightHeaded(true);
            else if(m_KeyInput < 0)
                m_Player.setisRightHeaded(false);
            else if(m_Player.m_playerRotation.GetIsMouseRight())
                m_Player.setisRightHeaded(true);
            else if(!m_Player.m_playerRotation.GetIsMouseRight())
                m_Player.setisRightHeaded(false);
            
            m_Player.m_useRange.ForceExitFromHideSlot();
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        else if (!m_InputMgr.m_IsPushHideKey)
        {
            if (m_UseRange.DoHide(false) == 1)
            {
                m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
            }
        }
        else if (m_InputMgr.m_IsPushReloadKey)
        {
            m_Player.m_ArmMgr.DoReload();
        }
    }

    public override void ExitState()
    {
        m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Hide, 0);
        m_Player.m_PlayerAniMgr.SetVisualParts(false, true, true, false);

        // RageGauge 업데이트 복구
        m_RageGauge.TempStopRageGauge(false);
        
        m_Player.m_CanMove = true;
        m_Player.m_CanAttack = true;
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(6);
        ExitFinalProcess();
    }
}

public class Player_MELEE : PlayerFSM
{
    private Animator m_FullBodyAnimator;
    private float m_CurAniTime;
    private Player_InputMgr m_InputMgr;
    private int m_Phase = 0;
    
    private readonly int Melee = Animator.StringToHash("Melee");

    public Player_MELEE(Player _player) : base(_player)
    {
        InitFunc();
    }

    public override void StartState()
    {
        m_InputMgr = m_Player.m_InputMgr;
        m_Player.m_ArmMgr.StopReload();
		m_InitAction?.Invoke();
		m_Player.m_CanAttack = false;
        m_Player.m_playerRotation.m_doRotate = false;
        
        m_FullBodyAnimator = m_Player.m_PlayerAniMgr.p_FullBody.m_Animator;
        m_Player.m_PlayerAniMgr.SetVisualParts(true, false, false, false);
        
        m_Phase = 0;
        m_CurAniTime = 0f;
        
        // ROll에서 넘어왔는지 체크
        if (m_Player.m_CancelMeleeStartAnim)
        {
            m_Phase = 1;
            m_Player.m_CancelMeleeStartAnim = false;
            m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Melee, 2);
        }
        else
        {
            m_Phase = 0;
            m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Melee, 1);
        }

        m_Player.m_MeleeAttack.StartMelee();
        m_Player.m_SoundPlayer.PlayPlayerSoundOnce(3);

       m_Player.m_SimpleEffectPuller.SpawnSimpleEffect(6, m_Player.m_PlayerFootMgr.GetFootRayHit().point,
           !m_Player.m_IsRightHeaded);

       m_Player.m_PlayerHotBox.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        if (m_Player.m_InputMgr.m_IsPushRollKey && 
            m_Player.m_RageGauge.UseSkillByConsumeRageGauge(0))
        {
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        
        switch (m_Phase)
        {
            case 0:
                // Melee_Start Ani
                m_CurAniTime = m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_CurAniTime >= 1f)
                {
                    m_CurAniTime = 0f;
                    m_FullBodyAnimator.SetInteger(Melee, 2);
                    m_Phase = 1;
                }
                break;
            
            case 1:
                // Melee_Mid Ani
                m_Player.MoveByDirection(m_Player.m_IsRightHeaded ? 1 : -1, m_Player.p_MeleeSpeedMulti);
                
                m_CurAniTime = m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_CurAniTime >= 1f)
                {
                    m_CurAniTime = 0f;
                    m_FullBodyAnimator.SetInteger(Melee, 3);
                    m_Player.MoveByDirection(0);
                    m_Phase = 2;
                }
                break;
            
            case 2:
                m_CurAniTime = m_FullBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_CurAniTime >= 1f)
                {
                    m_CurAniTime = 0f;
                    m_FullBodyAnimator.SetInteger(Melee, 0);
                    m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Player.m_PlayerHotBox.gameObject.SetActive(true);
        m_Player.m_CanAttack = true;
        m_Player.m_CanMove = true;
        m_Player.m_playerRotation.m_doRotate = true;
        
        m_Player.m_PlayerAniMgr.p_FullBody.SetAnim_Int(Melee, 0);
        m_Player.m_PlayerAniMgr.SetVisualParts(false, true, true, false);
        
        m_Player.m_MeleeAttack.StopMelee();
        m_Player.MoveByDirection(0);
    }
}

public class Player_DEAD : PlayerFSM
{
    private Rigidbody2D m_Rigid;
    
    private CoroutineHandler m_CoroutineHandler;
    private CoroutineElement m_CoroutineElement;

    public Player_DEAD(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        
        m_Rigid = m_Player.m_PlayerRigid;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(false);
        //m_Player.m_PlayerAnimator.Play("Dead");
        
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        m_Player.m_playerRotation.m_doRotate = false;

        m_Player.m_PlayerAniMgr.SetVisualParts(false, false, false, false);

        SceneChangeMgr SCMgr = GameMgr.GetInstance().p_SceneChangeMgr;
        int curSceneIdx = GameMgr.GetInstance().m_CurSceneIdx;

        m_Player.p_DeadProcess.StartDeadAni(() => SCMgr.InitSceneEndWithSmooth(curSceneIdx, 20f));
        /*
        // Respawn in 5 seconds
        m_CoroutineElement =
            m_CoroutineHandler.StartCoroutine_Handler(DelayGetActiveCheckPointPosition(5.0f));
            */
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        Debug.Log("Player Dead ExitState");
        m_Player.m_CanMove = true;
        m_Player.m_CanAttack = true;
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(true);
        ExitFinalProcess();
    }

    /** Delay Respawn at CheckPoints */
    IEnumerator DelayGetActiveCheckPointPosition(float DeltaSeconds)
    {
        yield return new WaitForSeconds(DeltaSeconds);

        GameMgr.GetInstance().CurSceneReload();
        
        // // Change the Player transform to Activated CheckPoint
        // m_Player.transform.position = CheckPoint.GetActiveCheckPointPosition();
        //
        // ExitState();
        // m_Player.setPlayerHp(500);
        
        m_CoroutineElement.StopCoroutine_Element();
    }
}

public class Player_BULLET_TIME : PlayerFSM
{
    private Animator m_PlayerAnimator;
    private Player_AniMgr m_AniMgr;
    private BulletTimeMgr m_BulletTimeMgr;
    private BulletTime_AR m_BulletTimeAR;
    private readonly int h_BulletTime = Animator.StringToHash("BulletTime");
    private int m_Phase = 0;
    private float m_BulletTimeLimit = 0f;

    private float m_Timer = 0f;
    private EventInstance m_SoundInstance;

    public Player_BULLET_TIME(Player _player) : base(_player)
    {
        InitFunc();
    }

    public override void StartState()
    {
        m_BulletTimeAR = m_Player.m_BulletTimeAR;
        m_BulletTimeMgr = m_Player.m_BulletTimeMgr;
        m_AniMgr = m_Player.m_PlayerAniMgr;
        m_PlayerAnimator = m_AniMgr.p_FullBody.m_Animator;
        m_InputMgr = m_Player.m_InputMgr;
		m_InitAction?.Invoke();
		m_Phase = 0;
        m_Timer = 0f;
        
        m_Player.m_PlayerRigid.velocity = Vector2.zero;
        
        // 핫박스 무적
        m_Player.m_PlayerHotBox.gameObject.SetActive(false);

        // 재장전 캔슬
        m_Player.m_ArmMgr.StopReload();
        
        // 플립제거
        m_Player.m_playerRotation.m_BanFlip = true;

        // AR 온 & 게이지 내리기
        m_BulletTimeAR.ActivateUsingFade(true);
        m_Player.m_RageUI.MoveRageGaugeUI(true);

        // 강제 재장전
        m_Player.m_WeaponMgr.m_CurWeapon.SetLeftRounds(m_Player.m_WeaponMgr.m_CurWeapon.p_MaxRound);

        m_AniMgr.ChangeArmAniToAngleChange(false);
        m_AniMgr.SetVisualParts(true,false,false,false);
        m_AniMgr.p_FullBody.m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        m_AniMgr.p_FullBody.SetAnim_Int("BulletTime", 1);

        m_BulletTimeMgr.ActivateBulletTime(true);
        m_BulletTimeLimit = m_BulletTimeMgr.p_BulletTimeLimit;
        
        // 사운드 테스트
        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/P_Bullettime_Start");
        m_SoundInstance = m_Player.m_SoundPlayer.GetPlayerSoundInstance(0);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_SoundInstance, m_PlayerTransform);
        m_SoundInstance.start();

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("TimeScale", 1f);
    }

    public override void UpdateState()
    {
        if (m_InputMgr.m_IsPushAttackKey && m_Player.m_Negotiator.m_LeftRounds > 0 && m_Phase == 0)
        {
            m_Player.m_ArmMgr.DoAttack();
        }
        
        switch (m_Phase)
        {
            case 0:
                m_Timer += Time.unscaledDeltaTime;
                
                m_BulletTimeAR.ChangeGaugeFill(1f - (m_Timer / m_BulletTimeLimit));
                
                if (m_Player.m_WeaponMgr.m_CurWeapon.m_LeftRounds <= 0 ||  m_Timer >= m_BulletTimeLimit)
                {
                    m_AniMgr.p_FullBody.SetAnim_Int("BulletTime", 2);
                    m_Phase = 1;
                    
                    // 사운드 테스트
                    m_SoundInstance.keyOff();
                    m_SoundInstance.release();
                    m_Player.m_SoundPlayer.PlayPlayerSoundOnce(8);
                }
                break;
            
            case 1:
                //  불릿타임 시간초과 체크
                if (m_AniMgr.p_FullBody.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_BulletTimeMgr.ActivateBulletTime(false);
                    m_AniMgr.p_FullBody.SetAnim_Int("BulletTime", 3);
                    m_BulletTimeMgr.FireAll();

                    
                    m_Phase = 2;
                }
                break;
            
            case 2:
                if (m_AniMgr.p_FullBody.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
                }
                break;
        }
    }

    public override void ExitState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("TimeScale", 0f);

        m_Player.m_PlayerHotBox.gameObject.SetActive(true);
        
        m_Player.m_playerRotation.m_BanFlip = false;
        m_AniMgr.p_FullBody.SetAnim_Int("BulletTime", 0);
        m_AniMgr.p_FullBody.m_Animator.updateMode = AnimatorUpdateMode.Normal;
        
        m_BulletTimeAR.ActivateUsingFade(false);
        m_Player.m_RageUI.MoveRageGaugeUI(false);
    }
}