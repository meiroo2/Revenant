using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPart : MonoBehaviour, IPlayerVisualPart
{
    public Sprite[] m_Sprites;
    public bool m_InitSpriteOn = false;
    public bool m_InitAnimatorOn = false;

    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;

    public bool m_isVisible { get; private set; } = false;
    public bool m_isAniVisible { get; private set; } = false;
    private int m_CurSpriteIdx = 0;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();

        m_SpriteRenderer.enabled = m_InitSpriteOn;
        if (m_Animator)
            m_Animator.enabled = m_InitAnimatorOn;
    }

    public void SetVisible(bool _isVisible) 
    {
        m_isVisible = _isVisible;
        m_SpriteRenderer.enabled = m_isVisible;
    }
    public void SetAniVisible(bool _isVisible)
    {
        if (m_Animator)
            m_Animator.enabled = _isVisible;
    }
    public void SetAnim(string _ParamName, int _value)
    {
        if (m_Animator)
            m_Animator.SetInteger(_ParamName, _value);
        else
            Debug.Log("VisualPart_Animator_Null");
    }
    public void SetSprite(int _inputIdx)
    {
        if (_inputIdx >= 0 && _inputIdx < m_Sprites.Length)
        {
            if (m_CurSpriteIdx != _inputIdx)
            {
                m_CurSpriteIdx = _inputIdx;
                m_SpriteRenderer.sprite = m_Sprites[m_CurSpriteIdx];
            }
        }
        else
            Debug.Log("VisualPart_SetSprite_IdxOutOfRange");
    }
}