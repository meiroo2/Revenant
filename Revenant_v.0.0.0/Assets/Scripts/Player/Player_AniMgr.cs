using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_AniMgr : MonoBehaviour
{
    // Visible Member Variables
    public Sprite[] Load_HeadSprites { get; private set; }
    public Player_VisualPartMgr p_Head;
    public Player_VisualPartMgr p_Body;
    public Player_VisualPartMgr p_Leg;
    public Player_VisualPartMgr p_OJacket;
    public Player_VisualPartMgr p_IJacket;
    public Player_VisualPartMgr p_Gun;
    public Player_VisualPartMgr p_OArm;
    public Player_VisualPartMgr p_IArm;

    // Member Variables
    private PlayerRotation m_PlayerRotation;
    private Player m_Player;
    private Player_Gun m_PlayerGun;
    private Animator m_PlayerAnimator;
    private SpriteRenderer m_PlayerSpriteRenderer;
    private int m_curAnglePhase;


    // Constructors
    private void Awake()
    {
        Load_HeadSprites = Resources.LoadAll<Sprite>("PC/Head");
    }
    private void Start()
    {
        m_PlayerRotation = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerRotation;
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerAnimator = m_Player.GetComponent<Animator>();
        m_PlayerSpriteRenderer = m_Player.GetComponent<SpriteRenderer>();
        m_PlayerGun = m_Player.m_playerGun;

        // Animator Init
        p_Leg.m_setPartAniVisible(true);
        p_Leg.m_setPartAni("isWalk", 0);
    }

    // Updates
    private void Update()
    {
        if(m_curAnglePhase != m_PlayerRotation.m_curAnglePhase)
        {
            m_curAnglePhase = m_PlayerRotation.m_curAnglePhase;
            p_Head.m_setPartSprite(m_curAnglePhase);
            p_IJacket.m_setPartSprite(m_curAnglePhase);
            p_OJacket.m_setPartSprite(m_curAnglePhase);
        }

    }

    // Functions
    public void playplayerAnim()
    {
        switch (m_Player.m_curPlayerState)
        {
            case playerState.IDLE:
                break;

            case playerState.WALK:
                p_IJacket.m_setPartAniVisible(true);
                p_IJacket.m_setPartAni("isWalk", 1);
                p_OJacket.m_setPartAniVisible(true);
                p_OJacket.m_setPartAni("isWalk", 1);

                if (m_Player.m_isRightHeaded)
                {
                    if(m_Player.getIsPlayerWalkStraight() == true)
                        p_Leg.m_setPartAni("isWalk", 1);
                    else
                        p_Leg.m_setPartAni("isWalk", -1);
                }
                else
                {
                    if (m_Player.getIsPlayerWalkStraight() == true)
                        p_Leg.m_setPartAni("isWalk", 1);
                    else
                        p_Leg.m_setPartAni("isWalk", -1);
                }
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
                setSprites(true, false, false, false, false);
                m_PlayerAnimator.SetInteger("DoDash", 1);
                break;

            case playerState.HIDDEN:
                setSprites(true, false, false, false, false);
                m_PlayerAnimator.SetInteger("DoHide", 1);
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }
    public void exitplayerAnim()
    {
        switch (m_Player.m_curPlayerState)
        {
            case playerState.IDLE:
                break;

            case playerState.WALK:
                p_IJacket.m_setPartAniVisible(false);
                p_OJacket.m_setPartAniVisible(false);
                p_Leg.m_setPartAni("isWalk", 0);
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
                setSprites(false, true, true, true, true);
                m_PlayerAnimator.SetInteger("DoDash", 0);
                break;

            case playerState.HIDDEN:
                setSprites(false, true, true, true, true);
                m_PlayerAnimator.SetInteger("DoHide", 0);
                break;

            case playerState.HIDDEN_STAND:
                break;

            case playerState.INPORTAL:
                break;

            case playerState.DEAD:
                break;
        }
    }
    public void setSprites(bool _Player, bool _Head, bool _Body, bool _Leg, bool _Arm)
    {
        if (_Player) m_PlayerSpriteRenderer.enabled = true;
        else m_PlayerSpriteRenderer.enabled = false;

        if (_Head) p_Head.m_setPartVisible(true);
        else p_Head.m_setPartVisible(false);

        if (_Body)
        {
            p_IJacket.m_setPartVisible(true);
            p_OJacket.m_setPartVisible(true);
            p_Body.m_setPartVisible(true);
        }
        else
        {
            p_IJacket.m_setPartVisible(false);
            p_OJacket.m_setPartVisible(false);
            p_Body.m_setPartVisible(false);
        }

        if (_Leg) p_Leg.m_setPartVisible(true);
        else p_Leg.m_setPartVisible(false);

        if (_Arm)
        {
            p_Gun.m_setPartVisible(m_PlayerGun.m_ActiveWeaponType, true);
            p_OArm.m_setFullVisible(true);
            p_IArm.m_setFullVisible(true);
        }
        else
        {
            p_Gun.m_setPartVisible(m_PlayerGun.m_ActiveWeaponType, false);
            p_OArm.m_setFullVisible(false);
            p_IArm.m_setFullVisible(false);
        }
    }
}