using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour, IPlayerAnimator
{
    private Player m_Player;
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }

    public void PlayPlayerPartAni(PlayerPartAniParam _inputParam)
    {
        switch (_inputParam.m_PlayerState)
        {
            case playerState.IDLE:
                m_Animator.SetInteger("isWalk", 0);
                break;
            case playerState.WALK:
                if (_inputParam.m_isPlayerWalkFoward)
                    m_Animator.SetInteger("isWalk", 1);
                else
                    m_Animator.SetInteger("isWalk", -1);
                break;
        }
    }
}
