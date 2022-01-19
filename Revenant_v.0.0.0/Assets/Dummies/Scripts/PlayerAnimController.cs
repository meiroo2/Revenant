using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public enum PlayerAnimState
    {
        IDLE,
        WALK
    }

    public Animator[] p_animators;

    public void setPlayerAnimState(PlayerAnimState playerAnimState)
    {
        switch (playerAnimState)
        {
            case PlayerAnimState.IDLE:
                foreach(Animator element in p_animators)
                {
                    element.SetBool("isWalk", false);
                }
                break;

            case PlayerAnimState.WALK:
                foreach (Animator element in p_animators)
                {
                    element.SetBool("isWalk", true);
                }
                break;
        }
    }
}
