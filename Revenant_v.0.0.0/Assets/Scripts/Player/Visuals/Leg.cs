using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour, IPlayerVisualPart
{
    private Player m_Player;
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
    }
    /*
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
    */
    public void SetVisible(bool _isVisible) { }
    public void SetAnim(string _ParamName, int _value) { }
    public void SetSprite(int _inputIdx) { }
    public void SetAniVisible(bool _isVisible) { }
}
