using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundnAni : MonoBehaviour
{
    // Member Variables
    public Animator[] m_Animators;

    private Player m_Player;
    private int isRightHeaded;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponent<Player>();
    }

    // Updates
    private void Update()
    {

    }

    // Functions
    public void playplayerAnim()
    {
        switch (m_Player.m_curPlayerState)
        {
            case playerState.IDLE:
                foreach (Animator element in m_Animators)
                {
                    element.SetInteger("isWalk", 0);
                }
                break;

            case playerState.WALK:
                foreach (Animator element in m_Animators)
                {
                    element.SetInteger("isWalk", 0);
                }

                isRightHeaded = m_Player.m_isRightHeaded ? 1 : -1;
                if (isRightHeaded == (int)m_Player.m_playerMoveVec.x)
                    foreach (Animator element in m_Animators)
                    {
                        element.SetInteger("isWalk", 1);
                    }
                else
                {
                    foreach (Animator element in m_Animators)
                    {
                        element.SetInteger("isWalk", -1);
                    }
                }
                break;

            case playerState.RUN:
                break;

            case playerState.ROLL:
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
