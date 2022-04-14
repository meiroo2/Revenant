using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour, IUI
{
    // Visible Member Variables


    // Member Variables
    private SpriteRenderer[] m_SpriteRenderers;
    private Animator[] m_Animators;

    public bool m_isActive { get; set; }
    private Vector2 m_PinVec;
    private bool m_isPinned = false;

    // Constructors
    private void Awake()
    {
        m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        m_Animators = GetComponentsInChildren<Animator>();
        for (int i = 0; i < m_SpriteRenderers.Length; i++)
            m_SpriteRenderers[i].enabled = false;
    }
    // Updates
    private void Update()
    {
        if (m_isPinned)
        {
            transform.position = m_PinVec;
        }
    }

    // Physics

    // Functions
    public int ActivateIUI(IUIParam _input)
    {
        if (_input.m_ToActive)
        {
            if (!m_isActive)
            {
                m_isActive = true;
                for (int i = 0; i < m_SpriteRenderers.Length; i++)
                    m_SpriteRenderers[i].enabled = true;
            }
        }
        else
        {
            if (m_isActive)
            {
                m_isActive = false;
                for (int i = 0; i < m_SpriteRenderers.Length; i++)
                    m_SpriteRenderers[i].enabled = false;
            }
        }
        return 0;
    }
    public int AniSetIUI(IUIParam _input)
    { 
        if(m_Animators.Length > 0)
        {
            for(int i = 0; i < m_Animators.Length; i++)
            {
                m_Animators[i].SetInteger(_input.m_AnimatorParam, _input.m_AnimatorParamValue);
            }
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public int PosSetIUI(IUIParam _input)
    {
        m_isPinned = true;
        m_PinVec = _input.m_Position.position;
        return 0;
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
