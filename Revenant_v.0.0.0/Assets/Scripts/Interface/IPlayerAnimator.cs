using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartAniParam
{
    public playerState m_PlayerState { get; private set; }
    public bool m_isPlayerWalkFoward { get; private set; }

    public PlayerPartAniParam(playerState _inputState, bool _playerwalkfoward = true)
    {
        m_PlayerState = _inputState;
        m_isPlayerWalkFoward = _playerwalkfoward;
    }
}

public interface IPlayerAnimator
{
    public void PlayPlayerPartAni(PlayerPartAniParam _inputParam);
}