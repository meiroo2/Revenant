using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_AniMgr : MonoBehaviour
{
    // Visible Member Variables
    public Sprite[] Load_HeadSprites { get; private set; }
    public Player_VisualPartMgr[] p_Head;
    public Player_VisualPartMgr[] p_Body;
    public Player_VisualPartMgr[] p_Leg;
    public Player_VisualPartMgr[] p_OJacket;
    public Player_VisualPartMgr[] p_IJacket;
    public Player_VisualPartMgr[] p_Gun;
    public Player_VisualPartMgr[] p_OArm;
    public Player_VisualPartMgr[] p_IArm;

    // Member Variables
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private WeaponMgr m_WeaponMgr;
    private Animator m_PlayerAnimator;
    private SpriteRenderer m_PlayerSpriteRenderer;
    private int m_curAnglePhase;

    private Player_VisualPartMgr m_cur_Head;
    private Player_VisualPartMgr m_cur_Body;
    private Player_VisualPartMgr m_cur_Leg;
    private Player_VisualPartMgr m_cur_OJacket;
    private Player_VisualPartMgr m_cur_IJacket;
    private Player_VisualPartMgr m_cur_Gun;
    private Player_VisualPartMgr m_cur_OArm;
    private Player_VisualPartMgr m_cur_IArm;

    private Player_ArmMgr m_PlayerArmMgr;
    public int m_curArmIdx { get; private set; } = 0;
    private bool[] m_curActivePartState = { false, true, true, true, true };


    // Constructors
    private void Awake()
    {
        Load_HeadSprites = Resources.LoadAll<Sprite>("PC/Head");

        m_cur_Head = p_Head[0];
        m_cur_Body = p_Body[0];
        m_cur_Leg = p_Leg[0];
        m_cur_OJacket = p_OJacket[0];
        m_cur_IJacket = p_IJacket[0];
        m_cur_Gun = p_Gun[0];
        m_cur_OArm = p_OArm[0];
        m_cur_IArm = p_IArm[0];
    }
    private void Start()
    {
        m_PlayerRotation = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerRotation;
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerAnimator = m_Player.GetComponent<Animator>();
        m_PlayerSpriteRenderer = m_Player.GetComponent<SpriteRenderer>();
        m_WeaponMgr = m_Player.m_WeaponMgr;
        m_PlayerArmMgr = m_PlayerRotation.gameObject.GetComponent<Player_ArmMgr>();
    }

    // Updates
    private void Update()
    {
        if(m_curAnglePhase != m_PlayerRotation.m_curAnglePhase)
        {
            m_curAnglePhase = m_PlayerRotation.m_curAnglePhase;
            m_cur_Head.m_setPartSprite(m_curAnglePhase);
            m_cur_IJacket.m_setPartSprite(m_curAnglePhase);
            m_cur_OJacket.m_setPartSprite(m_curAnglePhase);
        }
    }

    // Functions
    public void playplayerAnim()
    {
        switch (m_Player.m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                break;

            case PlayerStateName.WALK:
                m_cur_IJacket.m_setPartAniVisible(true);
                m_cur_IJacket.m_setPartAni("isWalk", 1);
                m_cur_OJacket.m_setPartAniVisible(true);
                m_cur_OJacket.m_setPartAni("isWalk", 1);

                if (m_Player.m_isRightHeaded)
                {
                    if(m_Player.getIsPlayerWalkStraight() == true)
                        m_cur_Leg.m_setPartAni("isWalk", 1);
                    else
                        m_cur_Leg.m_setPartAni("isWalk", -1);
                }
                else
                {
                    if (m_Player.getIsPlayerWalkStraight() == true)
                        m_cur_Leg.m_setPartAni("isWalk", 1);
                    else
                        m_cur_Leg.m_setPartAni("isWalk", -1);
                }
                break;

            case PlayerStateName.ROLL:
                setSprites(true, false, false, false, false);
                m_PlayerAnimator.SetInteger("DoDash", 1);
                break;

            case PlayerStateName.HIDDEN:
                setSprites(true, false, false, false, false);
                m_PlayerAnimator.SetInteger("DoHide", 1);
                break;
        }
    }
    public void exitplayerAnim()
    {
        switch (m_Player.m_CurPlayerFSMName)
        {
            case PlayerStateName.IDLE:
                break;

            case PlayerStateName.WALK:
                m_cur_IJacket.m_setPartAniVisible(false);
                m_cur_OJacket.m_setPartAniVisible(false);
                m_cur_Leg.m_setPartAni("isWalk", 0);
                break;

            case PlayerStateName.ROLL:
                setSprites(false, true, true, true, true);
                m_PlayerAnimator.SetInteger("DoDash", 0);
                break;

            case PlayerStateName.HIDDEN:
                setSprites(false, true, true, true, true);
                m_PlayerAnimator.SetInteger("DoHide", 0);
                break;
        }
    }
    public void setSprites(bool _Player, bool _Head, bool _Body, bool _Leg, bool _Arm)
    {
        m_curActivePartState[0] = _Player;
        m_curActivePartState[1] = _Head;
        m_curActivePartState[2] = _Body;
        m_curActivePartState[3] = _Leg;
        m_curActivePartState[4] = _Arm;

        if (_Player) m_PlayerSpriteRenderer.enabled = true;
        else m_PlayerSpriteRenderer.enabled = false;

        if (_Head) m_cur_Head.m_setPartVisible(true);
        else m_cur_Head.m_setPartVisible(false);

        if (_Body)
        {
            m_cur_IJacket.m_setPartVisible(true);
            m_cur_OJacket.m_setPartVisible(true);
            m_cur_Body.m_setPartVisible(true);
        }
        else
        {
            m_cur_IJacket.m_setPartVisible(false);
            m_cur_OJacket.m_setPartVisible(false);
            m_cur_Body.m_setPartVisible(false);
        }

        if (_Leg) m_cur_Leg.m_setPartVisible(true);
        else m_cur_Leg.m_setPartVisible(false);

        if (_Arm)
        {
            m_cur_Gun.m_setPartVisible(m_WeaponMgr.m_CurWeapon.p_WeaponType, true);
            m_cur_OArm.m_setFullVisible(true);
            m_cur_IArm.m_setFullVisible(true);
        }
        else
        {
            m_cur_Gun.m_setPartVisible(m_WeaponMgr.m_CurWeapon.p_WeaponType, false);
            m_cur_OArm.m_setFullVisible(false);
            m_cur_IArm.m_setFullVisible(false);
        }
    }
    public void changeArm(int _idx)
    {
        m_cur_IArm.gameObject.SetActive(false);
        m_cur_OArm.gameObject.SetActive(false);

        m_curArmIdx = _idx;

        m_cur_IArm = p_IArm[m_curArmIdx];
        m_cur_OArm = p_OArm[m_curArmIdx];

        m_cur_IArm.gameObject.SetActive(true);
        m_cur_OArm.gameObject.SetActive(true);

        if (m_curActivePartState[4] == true)
        {
            m_cur_IArm.m_setFullVisible(true);
            m_cur_OArm.m_setFullVisible(true);
        }
        else
        {
            m_cur_IArm.m_setFullVisible(false);
            m_cur_OArm.m_setFullVisible(false);
        }


        m_PlayerArmMgr.changeArmPartPos(m_curArmIdx);
    }
}