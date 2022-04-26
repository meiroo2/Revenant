using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet_Animator : MonoBehaviour
{

    Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void Fade()
    {
        m_animator.SetTrigger("Fade");
    }

}
