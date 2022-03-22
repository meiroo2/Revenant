using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundnAni : MonoBehaviour
{
    // Visible Member Variables
    public bool m_isPlay = true;
    public Animator[] m_Animators;
    public Animator m_PlayerAnimator;
    public GameObject m_Head;
    public GameObject[] m_Body;
    public GameObject m_Leg;
    public GameObject m_Arm;

    // Member Variables
    private Player m_Player;
    private int isRightHeaded;

    private SpriteRenderer m_PlayerSprite;
    private SpriteRenderer[] m_HeadSprites;
    private SpriteRenderer[] m_BodySprites;
    private SpriteRenderer[] m_LegSprites;
    private SpriteRenderer[] m_ArmSprites;

    private bool[] m_curSpriteCheck = new bool[] { false, true, true, true, true };
    private bool[] m_beforeSpriteCheck = new bool[] { false, true, true, true, true };



    // Constructors
    private void Awake()
    {
        m_Player = GetComponent<Player>();

        m_PlayerSprite = m_Player.GetComponent<SpriteRenderer>();
        m_HeadSprites = m_Head.GetComponentsInChildren<SpriteRenderer>();

        List<SpriteRenderer> temp = new List<SpriteRenderer>();
        for(int i =0; i < m_Body.Length; i++)
        {
            temp.AddRange(m_Body[i].GetComponentsInChildren<SpriteRenderer>());
        }

        m_BodySprites = temp.ToArray();

        m_LegSprites = m_Leg.GetComponentsInChildren<SpriteRenderer>();
        m_ArmSprites = m_Arm.GetComponentsInChildren<SpriteRenderer>();
    }

    // Updates

    // Functions
    public void playplayerAnim()
    {
        if (m_isPlay)
        {
            switch (m_Player.m_curPlayerState)
            {
                case playerState.IDLE:
                    foreach (Animator element in m_Animators)
                    {
                        element.SetInteger("isWalk", 0);
                    }
                    m_PlayerAnimator.SetInteger("DoDash", 0);
                    break;

                case playerState.WALK:
                    isRightHeaded = m_Player.m_isRightHeaded ? 1 : -1;
                    if (isRightHeaded == (int)m_Player.m_playerMoveVec.x)
                    {
                        //Debug.Log("Forward");
                        foreach (Animator element in m_Animators)
                        {
                            element.SetInteger("isWalk", 1);
                        }
                    }
                    else
                    {
                        //Debug.Log("Backward");
                        foreach (Animator element in m_Animators)
                        {
                            element.SetInteger("isWalk", -1);
                        }
                    }
                    break;

                case playerState.RUN:
                    break;

                case playerState.ROLL:
                    m_PlayerAnimator.SetInteger("DoDash", 1);
                    break;

                case playerState.HIDDEN:
                    break;

                case playerState.HIDDEN_STAND:
                    break;

                case playerState.INPORTAL:
                    break;

                case playerState.DEAD:
                    foreach (Animator element in m_Animators)
                    {
                        element.SetInteger("isWalk", 0);
                    }
                    break;
            }
        }
    }
    public void exitplayerAnim()
    {
        if (m_isPlay)
        {
            switch (m_Player.m_curPlayerState)
            {
                case playerState.IDLE:
                    break;

                case playerState.WALK:
                    break;

                case playerState.RUN:
                    break;

                case playerState.ROLL:
                    m_PlayerAnimator.SetInteger("DoDash", 0);
                    break;

                case playerState.HIDDEN:
                    break;

                case playerState.HIDDEN_STAND:
                    break;

                case playerState.INPORTAL:
                    break;

                case playerState.DEAD:
                    break;
            }
        }
    }
    public void setSprites(bool _Player, bool _Head, bool _Body, bool _Leg, bool _Arm)
    {
        m_beforeSpriteCheck = m_curSpriteCheck;
        m_PlayerSprite.enabled = (_Player == true) ? true : false;
        for (int i = 0; i < m_HeadSprites.Length; i++) { m_HeadSprites[i].enabled = (_Head == true) ? true : false; }
        for (int i = 0; i < m_ArmSprites.Length; i++) { m_ArmSprites[i].enabled = (_Arm == true) ? true : false; }
        for (int i = 0; i < m_BodySprites.Length; i++) { m_BodySprites[i].enabled = (_Body == true) ? true : false; }
        for (int i = 0; i < m_LegSprites.Length; i++) { m_LegSprites[i].enabled = (_Leg == true) ? true : false; }
        m_curSpriteCheck[0] = _Player;
        m_curSpriteCheck[1] = _Head;
        m_curSpriteCheck[2] = _Body;
        m_curSpriteCheck[3] = _Leg;
        m_curSpriteCheck[4] = _Arm;
    }
}