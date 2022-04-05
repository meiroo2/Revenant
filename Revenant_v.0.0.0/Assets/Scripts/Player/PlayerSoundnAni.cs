using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundnAni : MonoBehaviour
{
    // Visible Member Variables
    public Player m_Player;
    public bool m_isPlay = true;
    public GameObject[] m_AnimatorInterFaceObjs;
    public Animator m_PlayerAnimator;
    public GameObject m_Head;
    public GameObject[] m_Body;
    public GameObject m_Leg;
    public GameObject m_Arm;

    public SpriteRenderer[] m_ThrowArmSprites;
    public GameObject[] m_BasicArms;

    // Member Variables
    private IPlayerAnimator[] m_Animators;
    private int isRightHeaded;

    private Animator m_OutArmAnimator;

    private SpriteRenderer m_PlayerSprite;
    private SpriteRenderer[] m_HeadSprites;
    private SpriteRenderer[] m_BodySprites;
    private SpriteRenderer[] m_LegSprites;
    private SpriteRenderer[] m_ArmSprites;

    private SpriteRenderer[] m_BasicArmSprites;

    private bool[] m_curSpriteCheck = new bool[] { false, true, true, true, true };
    private bool[] m_beforeSpriteCheck = new bool[] { false, true, true, true, true };

    private bool m_isArmBasic = true;



    // Constructors
    private void Awake()
    {
        m_Animators = new IPlayerAnimator[m_AnimatorInterFaceObjs.Length];
        for(int i = 0; i < m_AnimatorInterFaceObjs.Length; i++)
        {
            m_Animators[i] = m_AnimatorInterFaceObjs[i].GetComponent<IPlayerAnimator>();
        }

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

        temp = null;
        temp = new List<SpriteRenderer>();
        for(int i = 0; i < m_BasicArms.Length; i++)
        {
            temp.AddRange(m_BasicArms[i].GetComponentsInChildren<SpriteRenderer>());
        }
        m_BasicArmSprites = temp.ToArray();

        for(int i = 0; i < m_ThrowArmSprites.Length; i++)
        {
            if (m_ThrowArmSprites[i].gameObject.GetComponent<Animator>())
                m_OutArmAnimator = m_ThrowArmSprites[i].gameObject.GetComponent<Animator>();
        }
    }

    // Updates

    // Functions
    public void playShotAni()
    {
        if (!m_isArmBasic)
        {
            m_OutArmAnimator.Play("ThrowAnim", -1, 0f);
        }
    }
    public void playplayerAnim()
    {
        if (m_isPlay)
        {
            switch (m_Player.m_curPlayerState)
            {
                case playerState.IDLE:
                    foreach (IPlayerAnimator element in m_Animators)
                    {
                        element.PlayPlayerPartAni(new PlayerPartAniParam(m_Player.m_curPlayerState));
                    }
                    m_PlayerAnimator.SetInteger("DoDash", 0);
                    break;

                case playerState.WALK:
                    isRightHeaded = m_Player.m_isRightHeaded ? 1 : -1;
                    if (isRightHeaded == (int)m_Player.m_playerMoveVec.x)
                    {
                        //Debug.Log("Forward");
                        foreach (IPlayerAnimator element in m_Animators)
                        {
                            element.PlayPlayerPartAni(new PlayerPartAniParam(m_Player.m_curPlayerState, true));
                        }
                    }
                    else
                    {
                        //Debug.Log("Backward");
                        foreach (IPlayerAnimator element in m_Animators)
                        {
                            element.PlayPlayerPartAni(new PlayerPartAniParam(m_Player.m_curPlayerState, false));
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

        if (m_isArmBasic)
        {
            if (_Arm)
            {
                for (int i = 0; i < m_ArmSprites.Length; i++) { m_ArmSprites[i].enabled = (_Arm == true) ? true : false; }
                for (int i = 0; i < m_ThrowArmSprites.Length; i++) { m_ThrowArmSprites[i].enabled = (_Arm != true) ? true : false; }
            }
            else
            {
                for (int i = 0; i < m_ArmSprites.Length; i++) { m_ArmSprites[i].enabled = (_Arm == true) ? true : false; }
            }
        }
        else
        {
            if (_Arm)
            {
                for (int i = 0; i < m_ArmSprites.Length; i++) { m_ArmSprites[i].enabled = (_Arm == true) ? true : false; }

                for (int i = 0; i < m_BasicArmSprites.Length; i++) { m_BasicArmSprites[i].enabled = (_Arm != true) ? true : false; }

                for (int i = 0; i < m_ThrowArmSprites.Length; i++) { m_ThrowArmSprites[i].enabled = (_Arm == true) ? true : false; }
            }
            else
            {
                for (int i = 0; i < m_ArmSprites.Length; i++) { m_ArmSprites[i].enabled = (_Arm == true) ? true : false; }
                for (int i = 0; i < m_ThrowArmSprites.Length; i++) { m_ThrowArmSprites[i].enabled = (_Arm == true) ? true : false; }
            }
        }

        for (int i = 0; i < m_BodySprites.Length; i++) { m_BodySprites[i].enabled = (_Body == true) ? true : false; }
        for (int i = 0; i < m_LegSprites.Length; i++) { m_LegSprites[i].enabled = (_Leg == true) ? true : false; }
        m_curSpriteCheck[0] = _Player;
        m_curSpriteCheck[1] = _Head;
        m_curSpriteCheck[2] = _Body;
        m_curSpriteCheck[3] = _Leg;

        m_curSpriteCheck[4] = _Arm;
    }
    public void changeArmMode(bool _isBasic)
    {
        Debug.Log("호출추리");
        if (_isBasic)
        {
            m_isArmBasic = true;
            for(int i = 0; i < m_BasicArmSprites.Length; i++)
            {
                m_BasicArmSprites[i].enabled = true;
            }
            for(int i = 0; i < m_ThrowArmSprites.Length; i++)
            {
                m_ThrowArmSprites[i].enabled = false;
            }
        }
        else
        {
            m_isArmBasic = false;
            for (int i = 0; i < m_BasicArmSprites.Length; i++)
            {
                m_BasicArmSprites[i].enabled = false;
            }
            for (int i = 0; i < m_ThrowArmSprites.Length; i++)
            {
                m_ThrowArmSprites[i].enabled = true;
            }
        }
    }
}