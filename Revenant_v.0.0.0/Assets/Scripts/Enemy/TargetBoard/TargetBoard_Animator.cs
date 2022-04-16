using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard_Animator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HitBodyAni()
    {
        animator.SetBool("isAlive", false);
        animator.SetTrigger("isBody");

    }
    public void HitHeadAni()
    {
        animator.SetBool("isAlive", false);
        animator.SetTrigger("isHead");
        
    }
}
