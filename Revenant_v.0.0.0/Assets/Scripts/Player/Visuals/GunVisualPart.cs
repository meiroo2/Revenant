using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisualPart : MonoBehaviour, IPlayerVisualPart
{
    public Sprite[] m_GunSprites = new Sprite[5];
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;

    private bool m_isVisible = false;
    private int m_CurSpriteIdx = 0;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();

        m_SpriteRenderer.enabled = true;
        if (m_Animator)
            m_Animator.enabled = true;
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
        if (m_Animator.enabled == false)
            m_Animator.enabled = true;

        if (m_Animator)
            m_Animator.SetInteger(_ParamName, _value);
        else
            Debug.Log("VisualPart_Animator_Null");
    }
    public void SetSprite(int _inputIdx)
    {
        if (m_Animator.enabled == true)
            m_Animator.enabled = false;

        if (_inputIdx >= 0 && _inputIdx < m_GunSprites.Length)
        {
            if (m_CurSpriteIdx != _inputIdx)
            {
                m_CurSpriteIdx = _inputIdx;
                m_SpriteRenderer.sprite = m_GunSprites[m_CurSpriteIdx];
            }
        }
        else
            Debug.Log("VisualPart_SetSprite_IdxOutOfRange");
    }
}