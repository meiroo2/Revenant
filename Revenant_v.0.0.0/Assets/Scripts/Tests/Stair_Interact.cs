using System;
using UnityEngine;


    public class Stair_Interact : MonoBehaviour
    {
        private bool m_IsTriggered = false;
        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Animator.enabled = false;
            m_IsTriggered = false;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!m_IsTriggered)
            {
                m_IsTriggered = true;
                m_Animator.enabled = true;
            }
        }
    }