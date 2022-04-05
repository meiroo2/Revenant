using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jacket : MonoBehaviour, IPlayerAnimator
{
    private Player m_Player;
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    public void PlayPlayerPartAni(PlayerPartAniParam _inputParam)
    {
        switch (_inputParam.m_PlayerState)
        {
            case playerState.IDLE:
                m_Animator.SetInteger("isWalk", 0);
                m_Animator.enabled = false;
                break;
            case playerState.WALK:
                m_Animator.enabled = true;
                m_Animator.SetInteger("isWalk", 1);
                break;
        }
    }
}