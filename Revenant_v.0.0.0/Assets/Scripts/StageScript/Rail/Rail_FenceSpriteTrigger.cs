using System;
using UnityEngine;


public class Rail_FenceSpriteTrigger : MonoBehaviour
{
    public Animator p_FenceAnimator;
    private readonly int Open = Animator.StringToHash("Open");

    private void OnTriggerEnter2D(Collider2D col)
    {
        p_FenceAnimator.SetInteger(Open, 1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        p_FenceAnimator.SetInteger(Open, 0);
    }
}