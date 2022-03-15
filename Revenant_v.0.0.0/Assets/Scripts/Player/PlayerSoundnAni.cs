using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundnAni : MonoBehaviour
{
    // Member Variables
    public bool m_isPlay = true;

    public Animator[] m_Animators;

    public Animator m_PlayerAnimator;
    public SpriteRenderer[] m_SpriteRenderers;

    private Player m_Player;
    private int isRightHeaded;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponent<Player>();
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
                        Debug.Log("Forward");
                        foreach (Animator element in m_Animators)
                        {
                            element.SetInteger("isWalk", 1);
                        }
                    }
                    else
                    {
                        Debug.Log("Backward");
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
    public void setPlayerSprites(bool _setTrue)
    {
        if (_setTrue)
            foreach (SpriteRenderer element in m_SpriteRenderers)
            {
                element.enabled = true;
            }
        else
            foreach (SpriteRenderer element in m_SpriteRenderers)
            {
                element.enabled = false;
            }
    }
}