using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class PlayerFSM
{
    protected Player m_Player;
    protected Player_InputMgr m_InputMgr;
    protected Transform m_PlayerTransform;

    protected PlayerFSM(Player _player)
    {
        m_Player = _player;
    }

    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void NextPhase();

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
    private RageGauge_UI m_RageGauge;
    
    public Player_IDLE(Player _player) : base(_player)
    {
        
    }

    public override void StartState()
    {
        InitFunc();
        m_RageGauge = m_Player.m_RageGauge;
        m_Player.m_CanHide = true;
        m_Player.m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.W))
            m_Player.m_PlayerRigid.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
        
        CheckNull();
        
        if(m_InputMgr.GetDirectionalKeyInput() != 0)
            m_Player.ChangePlayerFSM(PlayerStateName.WALK);
        else if(m_InputMgr.m_IsPushRollKey && m_RageGauge.CanConsume(m_RageGauge.p_Gauge_Consume_Roll))
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        else if(m_InputMgr.m_IsPushSideAttackKey && m_RageGauge.CanConsume(m_RageGauge.p_Gauge_Consume_Melee))
            m_Player.ChangePlayerFSM(PlayerStateName.MELEE);
    }

    public override void ExitState()
    {
        m_Player.m_CanHide = false;
        m_Player.m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class Player_WALK : PlayerFSM
{
    private float m_KeyInput = 0f;
    private Player_FootMgr _mFootMgr;
    private Rigidbody2D m_Rigid;
    private RageGauge_UI m_RageGauge;

    private int m_PreInput = 0;
    private int m_CurInput = 0;

    private Vector2 JumpVec = Vector2.zero;

    public Player_WALK(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_RageGauge = m_Player.m_RageGauge;
        
        m_Player.m_CanHide = true;
        _mFootMgr = m_Player.m_PlayerFootMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_InputMgr = m_Player.m_InputMgr;
        m_Player.SetWalkSoundCoroutine(true);
    }

    public override void UpdateState()
    {
        CheckNull();
        m_PreInput = m_CurInput;
        m_CurInput = m_InputMgr.GetDirectionalKeyInput();
        
        if (m_CurInput == 0)
        {
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
            return;
        }

        if (!m_Player.m_CanMove) 
            return;

        
        if (m_Player.GetIsPlayerWalkStraight())
        {
            if (m_Player.m_IsRightHeaded) // 우측
                m_Rigid.velocity = -StaticMethods.getLPerpVec(_mFootMgr.m_PlayerNormal) *
                                   (m_Player.p_MoveSpeed);
            else // 좌측
                m_Rigid.velocity = StaticMethods.getLPerpVec(_mFootMgr.m_PlayerNormal) *
                                   (m_Player.p_MoveSpeed);
        }
        else
        {
            if (m_Player.m_IsRightHeaded)   // 오른쪽 보고 뒤로
                m_Rigid.velocity = StaticMethods.getLPerpVec(_mFootMgr.m_PlayerNormal) * 
                                   (m_Player.p_MoveSpeed * m_Player.p_BackSpeedMulti);
            else                           // 왼쪽 보고 뒤로
                m_Rigid.velocity = -StaticMethods.getLPerpVec(_mFootMgr.m_PlayerNormal) * 
                                   (m_Player.p_MoveSpeed * m_Player.p_BackSpeedMulti);
        }
        
        
        if(m_CurInput != m_PreInput)
            m_Player.m_PlayerAniMgr.playplayerAnim();
        
        if (m_InputMgr.m_IsPushRollKey && m_RageGauge.CanConsume(m_RageGauge.p_Gauge_Consume_Roll))
        {
            m_Player.setisRightHeaded(m_CurInput > 0);
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        else if(m_InputMgr.m_IsPushSideAttackKey && m_RageGauge.CanConsume(m_RageGauge.p_Gauge_Consume_Melee))
            m_Player.ChangePlayerFSM(PlayerStateName.MELEE);
    }

    public override void ExitState()
    {
        m_Player.m_CanHide = false;
        m_Player.SetWalkSoundCoroutine(false);
        m_Rigid.velocity = Vector2.zero;
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class Player_ROLL : PlayerFSM
{
    private Player_FootMgr _mFootMgr;
    private Rigidbody2D m_Rigid;
    private Vector2 m_PlayerNormalVec;
    private Animator m_PlayerAnimator;
    private CoroutineElement m_CoroutineElement;
    private BulletTimeMgr m_BulletTimeMgr;
    private RageGauge_UI m_RageGauge;

    public Player_ROLL(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_RageGauge = m_Player.m_RageGauge;
        m_RageGauge.ChangeGaugeValue(m_RageGauge.m_CurGaugeValue -
                                     m_RageGauge.p_Gauge_Consume_Roll);
        
        m_Player.m_ArmMgr.StopReload();
        
        m_Player.m_SFXMgr.playPlayerSFXSound(0);

        m_Player.m_CanHide = true;
        _mFootMgr = m_Player.m_PlayerFootMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_PlayerAnimator = m_Player.m_PlayerAnimator;

        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        
        // 투과 히트박스로 변경
        m_Player.m_PlayerHotBox.m_hotBoxType = 2;
        
        m_Player.UseRollCount();

        m_Player.m_playerRotation.m_doRotate = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
        
        // 코루틴 시작
        m_CoroutineElement = GameMgr.GetInstance().p_CoroutineHandler.StartCoroutine_Handler(CheckEvade());
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_PlayerNormalVec = m_Player.m_PlayerFootMgr.m_PlayerNormal;

        if (m_Player.m_IsRightHeaded)   // 우측 구르기
            m_Rigid.velocity = -StaticMethods.getLPerpVec(m_PlayerNormalVec) * (m_Player.p_MoveSpeed * m_Player.p_RollSpeedMulti);
        else
            m_Rigid.velocity = StaticMethods.getLPerpVec(m_PlayerNormalVec) * (m_Player.p_MoveSpeed * m_Player.p_RollSpeedMulti);
        
        if(m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
    }

    public override void ExitState()
    {
        m_Player.m_CanHide = false;
        m_Player.m_CanAttack = true;
        m_Player.m_CanMove = true;
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerHotBox.m_hotBoxType = 0;

        m_Player.m_PlayerHotBox.m_HitCount = 0;
        
        // 코루틴 끝
        m_CoroutineElement.StopCoroutine_Element();
        
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }

    private IEnumerator CheckEvade()
    {
        float normalTime;

        // 좋은 생각이 이거말고 떠오르지 않음
        Player_HotBox hotBox = m_Player.m_PlayerHotBox;
        while (true)
        {
            normalTime = m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (m_Player.m_PlayerHotBox.m_HitCount > 0)
            {
                if (normalTime >= m_Player.p_JustEvadeNormalizeTime.x &&
                    normalTime <= m_Player.p_JustEvadeNormalizeTime.y)
                {
                    // 저스트 회피
                    m_Player.m_ScreenEffectUI.ActivateScreenColorDistortionEffect();
                    m_Player.m_BulletTimeMgr.ModifyTimeScale(0.2f);
                    m_Player.m_ScreenEffectUI.ActivateLensDistortEffect(0.2f);
                    
                    m_Player.m_ParticleMgr.MakeParticle(m_Player.GetPlayerCenterPos(),
                        m_Player.p_CenterTransform, 8f, JustEvadeGaugeUp);
                    
                    Debug.Log("저스트 회피!!");
                    yield break;
                }
                else
                {
                    // 그냥 회피
                    m_Player.m_ParticleMgr.MakeParticle(m_Player.GetPlayerCenterPos(),
                        m_Player.p_CenterTransform, 8f, EvadeGaugeUp);
                    Debug.Log("그냥 회피");
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void JustEvadeGaugeUp()
    {
        var rageGauge = m_Player.m_RageGauge;
        rageGauge.ChangeGaugeValue(rageGauge.m_CurGaugeValue + rageGauge.p_Gauge_Refill_JustEvade);
    }

    private void EvadeGaugeUp()
    {
        var rageGauge = m_Player.m_RageGauge;
        rageGauge.ChangeGaugeValue(rageGauge.m_CurGaugeValue + rageGauge.p_Gauge_Refill_Evade);
    }
}

public class Player_HIDDEN : PlayerFSM
{
    private int m_KeyInput = 0;
    private Player_FootMgr _mFootMgr;
    private Rigidbody2D m_Rigid;
    private SoundPlayer m_SFXMgr;
    private RageGauge_UI m_RageGauge;

    public Player_HIDDEN(Player _player) : base(_player)
    {
       
    }

    public override void StartState()
    {
        m_RageGauge = m_Player.m_RageGauge;
        m_Player.m_ArmMgr.StopReload();
        m_SFXMgr = m_Player.m_SFXMgr;
        m_Player.m_CanAttack = false;
        _mFootMgr = m_Player.m_PlayerFootMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_InputMgr = m_Player.m_InputMgr;
        
        m_SFXMgr.playPlayerSFXSound(5);
        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        m_KeyInput = m_InputMgr.GetDirectionalKeyInput();

        if (m_InputMgr.m_IsPushRollKey && m_RageGauge.CanConsume(m_RageGauge.p_Gauge_Consume_Roll))
        {
            if(m_KeyInput > 0)
                m_Player.setisRightHeaded(true);
            else if(m_KeyInput < 0)
                m_Player.setisRightHeaded(false);
            else if(m_Player.m_playerRotation.getIsMouseRight())
                m_Player.setisRightHeaded(true);
            else if(!m_Player.m_playerRotation.getIsMouseRight())
                m_Player.setisRightHeaded(false);
            
            m_Player.m_ObjInteractor.ForceExitFromHideSlot();
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        
    }

    public override void ExitState()
    {
        m_Player.m_CanMove = true;
        m_Player.m_CanAttack = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_SFXMgr.playPlayerSFXSound(6);
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class Player_MELEE : PlayerFSM
{
    private Animator m_PlayerAnimator;
    private float m_CurAniTime;
    private bool m_IsAttackFinished = false;

    public Player_MELEE(Player _player) : base(_player)
    {
        InitFunc();
    }

    public override void StartState()
    {
        var gauge = m_Player.m_RageGauge;
        gauge.ChangeGaugeValue(gauge.m_CurGaugeValue - gauge.p_Gauge_Consume_Melee);
        
        m_Player.m_ArmMgr.StopReload();
        m_IsAttackFinished = false;
        m_PlayerAnimator = m_Player.m_PlayerAnimator;
        
        m_Player.m_SFXMgr.playPlayerSFXSound(0);
        
        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        m_Player.m_playerRotation.m_doRotate = false;
        
        m_Player.m_MeleeAttack.StartMelee();
        m_Player.MoveByDirection(m_Player.m_IsRightHeaded ? 1 : -1, m_Player.p_MeleeSpeedMulti);
    }

    public override void UpdateState()
    {
        m_CurAniTime = m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        switch (m_CurAniTime)
        {
            case >= 1f:
                m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
                break;
        }
    }

    public override void ExitState()
    {
        m_Player.m_CanAttack = true;
        m_Player.m_CanMove = true;
        m_Player.m_playerRotation.m_doRotate = true;
        
        m_Player.m_MeleeAttack.StopMelee();
        m_Player.MoveByDirection(0);
    }

    public override void NextPhase()
    {
        
    }
}

public class Player_DEAD : PlayerFSM
{
    private Rigidbody2D m_Rigid;


    public Player_DEAD(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_Rigid = m_Player.m_PlayerRigid;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(false);
        m_Player.m_PlayerAnimator.Play("Dead");
        
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_CanMove = false;
        m_Player.m_CanAttack = false;
        m_Player.m_playerRotation.m_doRotate = false;
        m_Player.m_PlayerAniMgr.setSprites(false, false, false, false, false);
        
        GameMgr.GetInstance().PlayerDead();
    }

    public override void UpdateState()
    {
        CheckNull();
    }

    public override void ExitState()
    {
        m_Player.m_CanMove = true;
        m_Player.m_CanAttack = true;
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(true);
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}