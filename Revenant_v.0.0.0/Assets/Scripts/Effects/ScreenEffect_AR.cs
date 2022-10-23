using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ScreenEffect_AR : MonoBehaviour
{
    // Visible Member Variables
    public float p_FadeSpeed = 1f;
    
    
    // Member Variables
    private float m_Timer = 0f;
    
    public SpriteRenderer[] m_RendererArr;
    public SpriteRenderer[] m_TimeInputRenArr;
    
    private Coroutine m_Coroutine;
    private Camera m_MainCam;
    private Color m_InitColor = Color.white;
    private readonly int TimeInput = Shader.PropertyToID("_TimeInput");
    
    private bool m_SetProperty = false;
    private float unscaledDeltatime = 0f;
    
    // Constructors
    private void Awake()
    {
        m_InitColor = m_RendererArr[0].color;
        for (int i = 0; i < m_RendererArr.Length; i++)
        {
            m_RendererArr[i].color = new Color(m_InitColor.r, m_InitColor.g, m_InitColor.b, 0f);
        }
        m_MainCam = Camera.main;
    }
    
    
    // Functions
    private void Update()
    {
        if (!m_SetProperty)
            return;

        unscaledDeltatime += Time.unscaledDeltaTime;
        for (int i = 0; i < m_TimeInputRenArr.Length; i++)
        {
            m_TimeInputRenArr[i].material.SetFloat(TimeInput, unscaledDeltatime);
        }
        transform.localScale = new Vector3(m_MainCam.orthographicSize / 10.8f,
            m_MainCam.orthographicSize / 10.8f, 1f);
    }

    /// <summary>
    /// 해당 이펙트를 Fade in/out으로 부드럽게 나타내거나 없앱니다.
    /// </summary>
    /// <param name="_isTrue">true면 나타나기</param>
    public void ActivateUsingFade(bool _isTrue)
    {
        m_SetProperty = _isTrue;
        
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
            unscaledDeltatime += Time.unscaledDeltaTime;
            
            if (_isTrue)
            {
                color.a +=  unscaledDeltatime * p_FadeSpeed;
                
                if (color.a >= 1f)
                {
                    color.a = 1f;
                    for (int i = 0; i < m_RendererArr.Length; i++)
                    {
                        m_RendererArr[i].color = color;
                    }

                    break;
                }

                for (int i = 0; i < m_RendererArr.Length; i++)
                {
                    m_RendererArr[i].color = color;
                }
            }
            else
            {
                color.a -=  unscaledDeltatime * p_FadeSpeed;
                
                if (color.a <= 0f)
                {
                    color.a = 0f;
                    for (int i = 0; i < m_RendererArr.Length; i++)
                    {
                        m_RendererArr[i].color = color;
                    }
                    break;
                }

                for (int i = 0; i < m_RendererArr.Length; i++)
                {
                    m_RendererArr[i].color = color;
                }
            }

            yield return null;
        }
        
        yield break;
    }
}




























