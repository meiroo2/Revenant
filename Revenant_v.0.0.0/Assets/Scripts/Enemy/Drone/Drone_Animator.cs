using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Animator : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HitBody()
    {
        animator.SetTrigger("hitBody");
    }
    public void HitTarget()
    {
        animator.SetTrigger("hitTarget");
    }
}
