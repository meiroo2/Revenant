using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDoor_TRoom2 : MonoBehaviour
{
    private Animator m_Animator;
    private BoxCollider2D m_BoxCollider;
    private bool m_isOpen = false;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_BoxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if (!m_isOpen)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CenterDoor_ani"))
            {
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_isOpen = true;
                    m_BoxCollider.enabled = false;
                }
            }
        }
    }
}
