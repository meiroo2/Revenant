using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleEffect : MonoBehaviour
{
    // Visible Member Variables
    [ReadOnly] public Transform m_ParentTransform;
    
    
    // Member Variables
    public int m_EffectType { get; private set; } = 0;

    private SpriteRenderer m_SpriteRenderer = null;
    public Animator m_Animator { get; private set; } = null;
    private AnimatorStateInfo m_AnistateInfo;
    
    
    // Constructor
    private void Awake()
    {
        InitCheck();
    }
    
    private void OnEnable()
    {
        if (m_EffectType == 2)
        {
            m_Animator.enabled = false;
            m_Animator.enabled = true;
        }
    }

    // Updates
    private void Update()
    {
        if (m_EffectType == 2)
        {
            m_AnistateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            
            if (m_AnistateInfo.normalizedTime >= 1f)
            {
                transform.parent = m_ParentTransform;
                gameObject.SetActive(false);
            }
        }
    }

    // Functions
    private void InitCheck()
    {
        if (TryGetComponent<SpriteRenderer>(out var renderer))
        {
            m_SpriteRenderer = renderer;
        }

        if (TryGetComponent<Animator>(out var animator))
        {
            m_Animator = animator;
        }

        if (ReferenceEquals(m_SpriteRenderer, null) && ReferenceEquals(m_Animator, null))
            m_EffectType = 0;
        else if (!ReferenceEquals(m_SpriteRenderer, null) && ReferenceEquals(m_Animator, null))
            m_EffectType = 1;
        else if (!ReferenceEquals(m_SpriteRenderer, null) && !ReferenceEquals(m_Animator, null))
            m_EffectType = 2;
    }
}
