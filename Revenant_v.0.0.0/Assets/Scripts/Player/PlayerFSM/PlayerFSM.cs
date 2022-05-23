using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class PlayerFSM
{
    protected Player m_Player;
    protected Player_InputMgr m_InputMgr;
    protected Transform m_PlayerTransform;

    protected PlayerFSM(Player _player, Player_InputMgr _inputMgr)
    {
        m_Player = _player;
        m_InputMgr = _inputMgr;
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

public class PlayerIDLE : PlayerFSM
{
    private float m_KeyInput = 0f;

    public PlayerIDLE(Player _player, Player_InputMgr _inputMgr) : base(_player, _inputMgr)
    {
        
    }

    public override void StartState()
    {
        //m_Player = _player;
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_KeyInput = Input.GetAxisRaw("Horizontal");
        
        if(m_KeyInput != 0f)
            m_Player.ChangePlayerFSM(PlayerStateName.WALK);
        else if(Input.GetKeyDown(KeyCode.Space) && m_Player.m_LeftRollCount > 0)
            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
    }

    public override void ExitState()
    {
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class PlayerWALK : PlayerFSM
{
    private float m_KeyInput = 0f;
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;

    public PlayerWALK(Player _player, Player_InputMgr _inputMgr) : base(_player, _inputMgr)
    {

    }
    
    public override void StartState()
    {
        m_StairMgr = m_Player.m_PlayerStairMgr;
        m_Rigid = m_Player.m_PlayerRigid;
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_KeyInput = Input.GetAxisRaw("Horizontal");

        if (m_KeyInput == 0f)
        {
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
            return;
        }

        if (!m_Player.m_CanMove)
            return;


        if ((m_Player.m_isRightHeaded ? 1 : -1) == (int)m_KeyInput) // 키 인풋과 바라보는 방향이 같을 때
        {
            if (m_Player.m_isRightHeaded)
                m_Rigid.velocity =
                    -new Vector2(-m_StairMgr.m_PlayerNormal.y, m_StairMgr.m_PlayerNormal.x) * m_Player.m_Speed;
            else
                m_Rigid.velocity =
                    new Vector2(-m_StairMgr.m_PlayerNormal.y, m_StairMgr.m_PlayerNormal.x) * m_Player.m_Speed;
        }
        else // 키 인풋과 바라보는 방향이 다를 때(BackWalk)
        {
            if (m_Player.m_isRightHeaded) // 오른쪽보고 뒤로 걷기
                m_Rigid.velocity = StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * (m_Player.m_Speed * m_Player.p_BackWalkSpeedRatio);
            else
                m_Rigid.velocity = -StaticMethods.getLPerpVec(m_StairMgr.m_PlayerNormal) * (m_Player.m_Speed * m_Player.p_BackWalkSpeedRatio);
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_Player.m_LeftRollCount > 0)
        {
            if(m_KeyInput > 0)
                m_Player.setisRightHeaded(true);
            else
                m_Player.setisRightHeaded(false);

            m_Player.ChangePlayerFSM(PlayerStateName.ROLL);
        }
    }

    public override void ExitState()
    {
        m_Rigid.velocity = Vector2.zero;
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class PlayerROLL : PlayerFSM
{
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;
    private Vector2 m_PlayerNormalVec;
    private Animator m_PlayerAnimator;

    public PlayerROLL(Player _player, Player_InputMgr _inputMgr) : base(_player, _inputMgr)
    {
        
    }
    
    public override void StartState()
    {
        m_StairMgr = m_Player.m_PlayerStairMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        m_PlayerAnimator = m_Player.m_PlayerAnimator;

        m_Player.m_CanMove = false;
        m_Player.m_CanShot = false;
        m_Player.m_PlayerHotBox.setPlayerHotBoxCol(false);
        m_Player.m_LeftRollCount -= 1;
        m_Player.m_PlayerUIMgr.UpdateRollCount(m_Player.m_LeftRollCount);

        if (!m_Player.m_isRecoveringRollCount)
            m_Player.StartCoroutine(m_Player.RecoverRollCount());

        m_Player.m_playerRotation.m_doRotate = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_PlayerNormalVec = m_Player.m_PlayerStairMgr.m_PlayerNormal;

        if (m_Player.m_isRightHeaded)   // 우측 구르기
            m_Rigid.velocity = -(StaticMethods.getLPerpVec(m_PlayerNormalVec) * m_Player.m_Speed) *
                               m_Player.p_RollSpeedRatio;
        else
            m_Rigid.velocity = StaticMethods.getLPerpVec(m_PlayerNormalVec) * (m_Player.m_Speed * m_Player.p_RollSpeedRatio);
        
        if(m_PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
    }

    public override void ExitState()
    {
        m_Player.m_CanMove = true;
        m_Player.m_CanShot = true;
        m_Rigid.velocity = Vector2.zero;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        m_Player.m_playerRotation.m_doRotate = true;
        
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class PlayerHIDDEN : PlayerFSM
{
    private float m_KeyInput = 0f;
    private Player_StairMgr m_StairMgr;
    private Rigidbody2D m_Rigid;

    public PlayerHIDDEN(Player _player, Player_InputMgr _inputMgr) : base(_player, _inputMgr)
    {
        
    }

    public override void StartState()
    {
        m_StairMgr = m_Player.m_PlayerStairMgr;
        m_Rigid = m_Player.m_PlayerRigid;
        
        m_Player.m_CanMove = false;
        m_Player.m_CanShot = false;
        m_Player.m_PlayerAniMgr.setSprites(true, false, false, false, false);
    }

    public override void UpdateState()
    {
        CheckNull();
        
        m_KeyInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && m_Player.m_LeftRollCount > 0)
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
        m_Player.m_CanShot = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}

public class PlayerDEAD : PlayerFSM
{
    private Rigidbody2D m_Rigid;


    public PlayerDEAD(Player _player, Player_InputMgr _inputMgr) : base(_player, _inputMgr)
    {
        
    }
    
    public override void StartState()
    {
        m_Rigid = m_Player.m_PlayerRigid;

        m_Rigid.velocity = Vector2.zero;
        m_Player.m_CanMove = false;
        m_Player.m_CanShot = false;
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
        m_Player.m_CanShot = true;
        m_Player.m_playerRotation.m_doRotate = true;
        m_Player.m_PlayerAniMgr.setSprites(false, true, true, true, true);
        
        ExitFinalProcess();
    }

    public override void NextPhase()
    {

    }
}