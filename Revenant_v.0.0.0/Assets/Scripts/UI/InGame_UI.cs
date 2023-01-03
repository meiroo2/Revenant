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
    public Image m_ScreenCoverImg;
    public ScreenEffect_UI m_ScreenEffectUI;
    
    
    // Member Variables
    private Color m_WhiteClearColor = new Color(1, 1, 1, 0);
    private Coroutine m_BlackFadeCoroutine;


    // Constructors
    private void Awake()
    {
        m_ScreenCoverImg.color = m_WhiteClearColor;
    }
    

    // Functions
    public void DoBlackFade(bool _isIn, float _speed, Action _action)
    {
        if (!ReferenceEquals(m_BlackFadeCoroutine, null))
        {
            StopCoroutine(m_BlackFadeCoroutine);
            m_BlackFadeCoroutine = null;
        }
        
        m_BlackFadeCoroutine = StartCoroutine(BlackFadeIn(_isIn, _speed, _action));
    }
    
    
    private IEnumerator BlackFadeIn(bool _isIn, float _speed, Action _action)
    {
        Color startColor = Color.black;
        Color endColor = Color.black;
        if (_isIn)
            startColor.a = 0f;
        else
            endColor.a = 0f;

        float lerpVal = 0f;
        while (true)
        {
            m_ScreenCoverImg.color = Color.Lerp(startColor, endColor, lerpVal);
            lerpVal += Time.deltaTime * _speed;

            if (lerpVal >= 1f)
            {
                lerpVal = 1f;
                m_ScreenCoverImg.color = Color.Lerp(startColor, endColor, lerpVal);
                break;
            }
            
            yield return null;
        }
        
        _action?.Invoke();
    }
}