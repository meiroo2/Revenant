using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class BulletTime_AR : MonoBehaviour
{
    // Visible Member Variables
    public float p_FadeSpeed = 1f;
    public List<Image> m_ImgList = new List<Image>();
    public List<Image> m_MatImgList = new List<Image>();
    public Image p_GaugeImg;
    
    // Member Variables
    private float m_Timer = 0f;
    private readonly int TimeInput = Shader.PropertyToID("_TimeInput");
    private Color m_ClearColor = new Color(1f, 1f, 1f, 0f);

    private Coroutine m_FadeCoroutine = null;
    private Coroutine m_MatImgCoroutine = null;
    
    // Constructors
    private void Awake()
    {
        foreach (var VARIABLE in m_ImgList)
        {
            VARIABLE.color = m_ClearColor;
        }
    }
    
    
    // Functions

    /// <summary>
    /// BulletTime�� ������ �������� ���� �����մϴ�.
    /// �Ķ���� ����ó�� �Ϸ�
    /// </summary>
    /// <param name="_fillAmount"></param>
    public void ChangeGaugeFill(float _fillAmount)
    {
        float _val = _fillAmount;
        
        if (_val < 0f)
            _val = 0f;
        else if (_val > 1f)
            _val = 1f;

        p_GaugeImg.fillAmount = _val;
    }

    /// <summary>
    /// �ش� ����Ʈ�� Fade in/out���� �ε巴�� ��Ÿ���ų� ���۴ϴ�.
    /// </summary>
    /// <param name="_toFadeIn">true�� ��Ÿ����</param>
    public void ActivateUsingFade(bool _toFadeIn)
    {
        if (!ReferenceEquals(m_FadeCoroutine, null))
        {
            StopCoroutine(m_FadeCoroutine);
            m_FadeCoroutine = null;
        }

        if (_toFadeIn)
        {
            m_FadeCoroutine = StartCoroutine(FadeCoroutine(_toFadeIn, m_ImgList));
            
            if (!ReferenceEquals(m_MatImgCoroutine, null))
            {
                StopCoroutine(m_MatImgCoroutine);
                m_MatImgCoroutine = null;
            }
            m_MatImgCoroutine = StartCoroutine(MatImgEnumerator(m_MatImgList));
        }
        else
        {
            m_FadeCoroutine = StartCoroutine(FadeCoroutine(_toFadeIn, m_ImgList, () =>
            {
                if (!ReferenceEquals(m_MatImgCoroutine, null))
                {
                    StopCoroutine(m_MatImgCoroutine);
                    m_MatImgCoroutine = null;
                }
            }));
        }
    }


    private IEnumerator FadeCoroutine(bool _toFadeIn, List<Image> _fadeImgList, Action _action = null)
    {
        Color startColor = Color.white;
        Color endColor = startColor;

        if (_toFadeIn)
            startColor.a = 0f;
        else
            endColor.a = 0f;

        float lerpVal = 0f;
        while (true)
        {
            
            foreach (var imageElement in _fadeImgList)
            {
                imageElement.color = Color.Lerp(startColor, endColor, lerpVal);
            }

            lerpVal += Time.unscaledDeltaTime * p_FadeSpeed;
            if (lerpVal >= 1f)
            {
                lerpVal = 1f;
                foreach (var imageElement in _fadeImgList)
                {
                    imageElement.color = Color.Lerp(startColor, endColor, lerpVal);
                }

                break;
            }
            
            yield return null;
        }
        
        _action?.Invoke();

        yield break;
    }

    private IEnumerator MatImgEnumerator(List<Image> _matImgList)
    {
        while (true)
        {
            m_Timer += Time.unscaledDeltaTime;
            
            foreach (var VARIABLE in _matImgList)
            {
                VARIABLE.material.SetFloat(TimeInput, m_Timer);
            }

            yield return null;
        }
        
        yield break;
    }
}




























