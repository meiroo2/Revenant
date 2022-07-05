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

    protected void CheckNull()
    {
        if (ReferenceEquals(m_Player, null))
            return;
    }

    protected void ExitFinalProcess()
    {
        //m_Player.m_Player_AniMgr.exitplayerAnim();
    }
}

public class Player_IDLE : PlayerFSM
{
    
    public Player_IDLE(Player _player) : base(_player)
    {

    }

    public override void StartState()
    {
        m_InputMgr = m_Player.m_InputMgr;
        m_Player.m_PlayerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public override void UpdateState()
    {
        CheckNull();
        
        if(m_InputMgr.GetDirectionalKeyInput() != 0)
            m_Player.ChangePlayerFSM(PlayerStateName.WALK);
        else if(m_InputMgr.m_IsPushRollKey && m_Player.m_LeftRollCount >= 1f)
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
    }

    public override void ExitState()
    {
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
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;

    private int m_PreInput = 0;
    private int m_CurInput = 0;

    public Player_WALK(Player _player) : base(_player)
    {

    }
    
    public override void StartState()
    {
        m_StairMgr = m_Player.m_PlayerStairMgr;
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
            if (m_Player.m_IsRightHeaded)   // 우측
                m_Rigid.velocity = -StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * 
                                   (m_Player.p_Speed);
            else                            // 좌측
                m_Rigid.velocity = StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * 
                                   (m_Player.p_Speed);
        }
        else
        {
            if (m_Player.m_IsRightHeaded)   // 오른쪽 보고 뒤로
                m_Rigid.velocity = StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * 
                                   (m_Player.p_Speed * m_Player.p_BackWalkSpeedRatio);
            else                           // 왼쪽 보고 뒤로
                m_Rigid.velocity = -StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * 
                                   (m_Player.p_Speed * m_Player.p_BackWalkSpeedRatio);
        }
        
        if(m_CurInput != m_PreInput)
            m_Player.m_PlayerAniMgr.playplayerAnim();
        
        if (m_InputMgr.m_IsPushRollKey && m_Player.m_LeftRollCount >= 1f)
        {
            m_Player.setisRightHeaded(m_CurInput > 0);
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
    }

    public override void ExitState()
    {
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
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;
    private Vector2 m_PlayerNormalVec;
    private Animator m_PlayerAnimator;

    public Player_ROLL(Player _player) : base(_player)
    {
        
    }
    
    public override void StartState()
    {
        m_Player.m_SFXMgr.playPlayerSFXSound(0);
        
        m_StairMgr = m_Player.m_PlayerStairMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_PlayerAnimator = m_Player.m_PlayerAnimator;

        m_Player.m_CanMove = false;
        m_Player.m_CanFire = false;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(false);
        
        m_Player.UseRollCount();

        m_Player.m_playerRotation.m_doRotate = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_PlayerNormalVec = m_Player.m_PlayerStairMgr.m_PlayerNormal;

        if (m_Player.m_IsRightHeaded)   // 우측 구르기
            m_Rigid.velocity = -StaticMethods.getLPerpVec(m_PlayerNormalVec) * (m_Player.p_Speed * m_Player.p_RollSpeedRatio);
        else
            m_Rigid.velocity = StaticMethods.getLPerpVec(m_PlayerNormalVec) * (m_Player.p_Speed * m_Player.p_RollSpeedRatio);
        
        if(m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
    }

    public override void ExitState()
    {
        m_Player.m_CanFire = true;
        m_Player.m_CanMove = true;
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(true);
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class Player_HIDDEN : PlayerFSM
{
    private int m_KeyInput = 0;
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;
    private SoundMgr_SFX m_SFXMgr;

    public Player_HIDDEN(Player _player) : base(_player)
    {
        
    }

    public override void StartState()
    {
        m_SFXMgr = m_Player.m_SFXMgr;
        m_Player.m_CanFire = false;
        m_StairMgr = m_Player.m_PlayerStairMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_InputMgr = m_Player.m_InputMgr;
        
        m_SFXMgr.playPlayerSFXSound(5);
        m_Player.m_CanMove = false;
        m_Player.m_CanFire = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        CheckNull();

        m_KeyInput = m_InputMgr.GetDirectionalKeyInput();

        if (m_InputMgr.m_IsPushRollKey && m_Player.m_LeftRollCount >= 1f)
        {
            if(m_KeyInput > 0)
                m_Player.setisRightHeaded(true);
            else if(m_KeyInput < 0)
                m_Player.setisRightHeaded(false);
            else if(m_Player.m_playerRotation.getIsMouseRight())
                m_Player.setisRightHeaded(true);
            else if(!m_Player.m_playerRotation.getIsMouseRight())
                m_Player.setisRightHeaded(false);
            
            m_Player.m_useRange.ForceExitFromHiddenSlot();
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
        
    }

    public override void ExitState()
    {
        m_Player.m_CanMove = true;
        m_Player.m_CanFire = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_SFXMgr.playPlayerSFXSound(6);
        ExitFinalProcess();
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

        m_Player.m_PlayerAnimator.Play("Dead");
        
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_CanMove = false;
        m_Player.m_CanFire = false;
        m_Player.m_playerRotation.m_doRotate = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        CheckNull();
    }

    public override void ExitState()
    {
        m_Player.m_CanMove = true;
        m_Player.m_CanFire = true;
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}