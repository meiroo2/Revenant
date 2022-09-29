using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPart : MonoBehaviour
{
    // Visible Member Variables
    public Sprite[] p_Sprites;
    public bool p_InitSpriteOn = true;
    public bool p_InitAnimatorOn = true;

    private SpriteRenderer m_SpriteRenderer;
    public Animator m_Animator { get; private set; }

    public bool m_isVisible { get; private set; } = false;
    public bool m_isAniVisible { get; private set; } = false;
    private int m_CurSpriteIdx = 0;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer Sren))
        {
            m_SpriteRenderer = Sren;
            m_SpriteRenderer.enabled = true;
        }
        else
        {
            Debug.Log("ERR : VisualPart�� SpriteRenderer ����");
        }
        
        if (TryGetComponent(out Animator Ani))
        {
            m_Animator = Ani;
            m_Animator.enabled = true;
        }
        else
        {
            Debug.Log("ERR : VisualPart�� Animator ����");
        }
    }

    private void Start()
    {
        if (m_SpriteRenderer && p_InitSpriteOn == false)
            m_SpriteRenderer.enabled = false;
    }

    /// <summary>
    /// SpriteRenderer�� enabled ���θ� �����մϴ�.
    /// </summary>
    /// <param name="_isVisible">SpriteRenderer enabled?</param>
    public void SetVisible(bool _isVisible)
    {
//        Debug.Log(_isVisible);
        m_isVisible = _isVisible;
        m_SpriteRenderer.enabled = m_isVisible;
    }
    
    /// <summary>
    /// Animator�� enabled ���θ� �����մϴ�.
    /// </summary>
    /// <param name="_isVisible">Animator enabled?</param>
    public void SetAniVisible(bool _isVisible)
    {
        if (m_Animator)
            m_Animator.enabled = _isVisible;
    }
    
    /// <summary>
    /// Animator�� ������ �����մϴ�. (Int��)
    /// </summary>
    /// <param name="_ParamName">�Ķ���� �̸�</param>
    /// <param name="_value">�ٲ� ��</param>
    public void SetAnim_Int(string _ParamName, int _value)
    {
        if (m_Animator)
            m_Animator.SetInteger(_ParamName, _value);
        else
            Debug.Log("ERR : VisualPart_Animator_Null");
    }
    
    /// <summary>
    /// Animator�� ������ �����մϴ�. (Trigger��)
    /// </summary>
    /// <param name="_ParamName">�ٲ� ��</param>
    public void SetAnim_Trigger(string _ParamName)
    {
        if (m_Animator)
            m_Animator.SetTrigger(_ParamName);
        else
            Debug.Log("ERR : VisualPart_Animator_Null");
    }
    
    public void SetSprite(int _inputIdx)
    {
        /*
        if (m_Animator.enabled == true)
            Debug.Log("ERR : VisualPart���� Animator�� �����ִµ� SetSprite�� �õ��մϴ�.");
        */
        
        if (p_Sprites.Length <= 1 || _inputIdx < 0 || _inputIdx >= p_Sprites.Length)
        {
            Debug.Log("ERR : VisualPart_Sprite Idx Out Of Range");
        }

        m_CurSpriteIdx = _inputIdx;
        m_SpriteRenderer.sprite = p_Sprites[m_CurSpriteIdx];
    }
}