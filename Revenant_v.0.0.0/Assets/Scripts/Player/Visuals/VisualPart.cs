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
            Debug.Log("ERR : VisualPart에 SpriteRenderer 없음");
        }
        
        if (TryGetComponent(out Animator Ani))
        {
            m_Animator = Ani;
            m_Animator.enabled = true;
        }
        else
        {
            Debug.Log("ERR : VisualPart에 Animator 없음");
        }
    }

    private void Start()
    {
        if (m_SpriteRenderer && p_InitSpriteOn == false)
            m_SpriteRenderer.enabled = false;
    }

    /// <summary>
    /// SpriteRenderer의 enabled 여부를 설정합니다.
    /// </summary>
    /// <param name="_isVisible">SpriteRenderer enabled?</param>
    public void SetVisible(bool _isVisible)
    {
        if (_isVisible == m_isVisible)
            return;
        
        m_isVisible = _isVisible;
        m_SpriteRenderer.enabled = m_isVisible;
    }
    
    /// <summary>
    /// Animator의 enabled 여부를 결정합니다.
    /// </summary>
    /// <param name="_isVisible">Animator enabled?</param>
    public void SetAniVisible(bool _isVisible)
    {
        if (_isVisible == m_Animator.enabled)
            return;
            
        if (m_Animator)
            m_Animator.enabled = _isVisible;
    }
    
    /// <summary>
    /// Animator의 변수를 조절합니다. (Int형)
    /// </summary>
    /// <param name="_ParamName">파라미터 이름</param>
    /// <param name="_value">바꿀 값</param>
    public void SetAnim_Int(string _ParamName, int _value)
    {
        if (m_Animator)
            m_Animator.SetInteger(_ParamName, _value);
        else
            Debug.Log("ERR : VisualPart_Animator_Null");
    }
    
    /// <summary>
    /// Animator의 변수를 조절합니다. (Trigger형)
    /// </summary>
    /// <param name="_ParamName">바꿀 값</param>
    public void SetAnim_Trigger(string _ParamName)
    {
        if (m_Animator)
            m_Animator.SetTrigger(_ParamName);
        else
            Debug.Log("ERR : VisualPart_Animator_Null");
    }
    
    public void SetSprite(int _inputIdx)
    {
        if (m_Animator.enabled == true)
            Debug.Log("ERR : VisualPart에서 Animator가 켜져있는데 SetSprite를 시도합니다.");
        
        if (p_Sprites.Length <= 1 || _inputIdx < 0 || _inputIdx >= p_Sprites.Length)
        {
            Debug.Log("ERR : VisualPart_Sprite Idx Out Of Range");
        }

        m_CurSpriteIdx = _inputIdx;
        m_SpriteRenderer.sprite = p_Sprites[m_CurSpriteIdx];
    }
}