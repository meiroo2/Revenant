using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jacket : MonoBehaviour, IPlayerVisualPart
{
    private Player m_Player;
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }
    private void Start()
    {
        m_Animator.enabled = false;
    }

    /*
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
    */
    public void SetVisible(bool _isVisible) { }
    public void SetAnim(string _ParamName, int _value) { }
    public void SetSprite(int _inputIdx) { }
    public void SetAniVisible(bool _isVisible) { }
}
