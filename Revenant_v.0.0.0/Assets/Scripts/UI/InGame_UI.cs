using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    // Visible Member Variables
    public float p_WhiteOutSpeed = 1f;
    
    [Space(20f)]
    [Header("Plz Assign")]
    public Image m_WhiteOutImg;
    public ScreenEffect_UI m_ScreenEffectUI;
    
    
    // Member Variables

    private Color m_WhiteOutColor = new Color(1, 1, 1, 0);

    public delegate void WhiteOutDelegate();
    private WhiteOutDelegate m_Callback;
    
    
    // Constructors
    private void Awake()
    {
        m_WhiteOutImg.color = m_WhiteOutColor;
    }

    
    // Functions
    public void DoWhiteOut(bool _IsFadeOut)
    {
        m_WhiteOutColor = Color.white;
        
        if (_IsFadeOut)
        {
            m_WhiteOutImg.color = m_WhiteOutColor;
            StartCoroutine(WhiteFadeOut());
        }
        else
        {
            m_WhiteOutColor.a = 0;
            m_WhiteOutImg.color = m_WhiteOutColor;
            StartCoroutine(WhiteFadeIn());
        }
    }
    public void SetCallback(WhiteOutDelegate _func)
    {
        m_Callback = _func;
    }
    private IEnumerator WhiteFadeOut()
    {
        while (true)
        {
            m_WhiteOutColor.a -= Time.deltaTime * p_WhiteOutSpeed;
            m_WhiteOutImg.color = m_WhiteOutColor;

            if (m_WhiteOutColor.a <= 0f)
            {
                m_WhiteOutImg.color = Color.clear;
                m_WhiteOutColor = Color.white;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator WhiteFadeIn()
    {
        while (true)
        {
            m_WhiteOutColor.a += Time.deltaTime * p_WhiteOutSpeed;
            m_WhiteOutImg.color = m_WhiteOutColor;
            
            if (m_WhiteOutColor.a >= 1f)
            {
                m_WhiteOutColor = Color.white;
                m_WhiteOutImg.color = m_WhiteOutColor;

                if (!ReferenceEquals(m_Callback, null))
                {
                    m_Callback();
                }
                
                yield break;
            }
            
            yield return null;
        }
    }
}