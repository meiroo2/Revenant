using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class ScreenEffect_AR : MonoBehaviour
{
    // Visible Member Variables
    public float p_FadeSpeed = 1f;
    
    
    // Member Variables
    private SpriteRenderer m_Renderer;
    private Animator m_Animator;
    private Coroutine m_Coroutine;

    private Color m_InitColor = Color.white;
    
    
    // Constructors
    private void Awake()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            m_Renderer = renderer;
            m_InitColor = m_Renderer.color;
        }

        if (TryGetComponent<Animator>(out Animator animator))
        {
            m_Animator = animator;
        }

        m_Renderer.color = new Color(m_InitColor.r, m_InitColor.g, m_InitColor.b, 0f);
    }
    
    
    // Functions
    
    /// <summary>
    /// 해당 이펙트를 Fade in/out으로 부드럽게 나타내거나 없앱니다.
    /// </summary>
    /// <param name="_isTrue">true면 나타나기</param>
    public void ActivateUsingFade(bool _isTrue)
    {
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }

        m_Coroutine = StartCoroutine(FadeCoroutine(_isTrue));
    }


    private IEnumerator FadeCoroutine(bool _isTrue)
    {
        Color color = Color.white;
        if (_isTrue)
            color.a = 0f;

        while (true)
        {
            if (_isTrue)
            {
                color.a += Time.unscaledDeltaTime * p_FadeSpeed;
                
                if (color.a >= 1f)
                {
                    color.a = 1f;
                    m_Renderer.color = color;
                    break;
                }

                m_Renderer.color = color;
            }
            else
            {
                color.a -= Time.unscaledDeltaTime * p_FadeSpeed;
                
                if (color.a <= 0f)
                {
                    color.a = 0f;
                    m_Renderer.color = color;
                    break;
                }

                m_Renderer.color = color;
            }
            
            yield return null;
        }
        
        yield break;
    }
}




























