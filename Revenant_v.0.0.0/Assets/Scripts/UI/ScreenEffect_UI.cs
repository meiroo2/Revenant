using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class ScreenEffect_UI : MonoBehaviour
{
    // Visible Member Variables
    public float p_EdgeEffectFadeoutspeed = 5f;
    public float p_ColorDistortionPower = 1f;
    public float p_ColorDistortionRestoreSpeed = 5f;
    
    public float p_LensDistortPower = -0.5f;
    public float p_LensDistortRestoreSpeed = 2f;

    public float p_VignettePower = 0.5f;
    public float p_VignetteSpeed = 1f;

    public float p_AREffectSpeed = 1f;
    
    // Member Variables
    private Color m_Color;
    private Image m_Image;
    private Coroutine m_EdgeEffectCoroutine;

    private Camera m_MainCamera;
    private VolumeProfile m_CamVolumeProfile;
    private UnityEngine.Rendering.Universal.ChromaticAberration m_Chroma;
    private UnityEngine.Rendering.Universal.LensDistortion m_LensDistort;
    private UnityEngine.Rendering.Universal.Vignette m_Vignette;
    private Coroutine m_LensDistortCoroutine;
    private Coroutine m_ColorDistortionCoroutine;
    private Coroutine m_VignetteCoroutine;

    private BulletTime_AR m_BulletTime_AR;
    private RageGauge_UI m_RageGauge_UI;
    
    
    // Constructor
    private void Awake()
    {
        m_MainCamera = Camera.main;
        if (m_MainCamera != null) 
            m_CamVolumeProfile = m_MainCamera.GetComponent<Volume>().profile;

        if (m_CamVolumeProfile.TryGet(out m_Chroma))
        {
            m_Chroma.intensity.value = 0f;
        }
        else
        {
            Debug.Log("MainCamera의 현재 Profile에 ChromaticAberration 없음");
        }

        if (m_CamVolumeProfile.TryGet(out m_LensDistort))
        {
            m_LensDistort.intensity.value = 0f;
        }
        else
        {
            Debug.Log("MainCamera의 현재 Profile에 LensDistort 없음");
        }

        if (m_CamVolumeProfile.TryGet(out m_Vignette))
        {
            m_Vignette.intensity.value = 0f;
        }


        m_Image = GetComponentInChildren<Image>();
        m_Color = new Color(1, 1, 1, 0);
        m_Image.color = m_Color;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_BulletTime_AR = instance.m_BulletTime_AR;
        m_RageGauge_UI = instance.m_RageGauge.p_RageGaugeUI;
        //m_BulletTime_AR.p_FadeSpeed = p_AREffectSpeed;
    }


    // Updates


    // Functions

    public void ActivateAREffect(bool _isTrue)
    {
        m_BulletTime_AR.ActivateUsingFade(_isTrue);
        m_RageGauge_UI.MoveRageGaugeUI(_isTrue);
    }

    /// <summary>
    /// 카메라에 비네팅 효과를 부드럽게 넣거나 뻅니다.
    /// </summary>
    /// <param name="_isTrue">True면 넣음</param>
    public void ActivateVignetteEffect(bool _isTrue)
    {
        m_Vignette.intensity.value = _isTrue ? 0f : p_VignettePower;

        m_VignetteCoroutine = StartCoroutine(VignetteEffectCoroutine(_isTrue));
    }

    private IEnumerator VignetteEffectCoroutine(bool _isTrue)
    {
        while (true)
        {
            if (_isTrue)
            {
                m_Vignette.intensity.value += Time.unscaledDeltaTime * p_VignetteSpeed;
                if (m_Vignette.intensity.value >= p_VignettePower)
                {
                    m_Vignette.intensity.value = p_VignettePower;
                    break;
                }
            }
            else
            {
                m_Vignette.intensity.value -= Time.unscaledDeltaTime * p_VignetteSpeed;
                if (m_Vignette.intensity.value <= 0f)
                {
                    m_Vignette.intensity.value = 0f;
                    break;
                }
            }
            
            yield return null;
        }

        yield break;
    }
    
    
    
    /// <summary>
    /// 화면 가장자리에 유혈 효과를 생성합니다.
    /// </summary>
    public void ActivateScreenEdgeEffect()
    {
        // 우선 해당 화면 이펙트 알파값 최대
        m_Color = Color.white;
        m_Image.color = m_Color;
        
        if(m_EdgeEffectCoroutine != null)
            StopCoroutine(m_EdgeEffectCoroutine);
        
        m_EdgeEffectCoroutine = StartCoroutine(EdgeEffectStart());
    }

    // 내부 카메라 이펙트 코루틴
    private IEnumerator EdgeEffectStart()
    {
        while (true)
        {
            m_Image.color = m_Color;
            m_Color.a -= Time.unscaledDeltaTime * p_EdgeEffectFadeoutspeed;
            
            if (m_Color.a <= 0f)
                break;
            
            yield return null;
        }
    }

    /// <summary>
    /// 지정된 시간 동안 화면을 왜곡시킵니다.
    /// </summary>
    /// <param name="_time">지정 시간</param>
    public void ActivateLensDistortEffect(float _time)
    {
        m_LensDistort.intensity.value = p_LensDistortPower;
        
        if(!ReferenceEquals(m_LensDistortCoroutine, null))
            StopCoroutine(m_LensDistortCoroutine);

        m_LensDistortCoroutine = StartCoroutine(LensDistortStart(_time));
    }

    private IEnumerator LensDistortStart(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        while (true)
        {
            m_LensDistort.intensity.value += Time.unscaledDeltaTime * p_LensDistortRestoreSpeed;

            if (m_LensDistort.intensity.value >= 0f)
            {
                m_LensDistort.intensity.value = 0f;
                break;
            }
            
            yield return null;
        }
    }
    
    
    
    public void ActivateScreenColorDistortionEffect()
    {
        m_Chroma.intensity.value = p_ColorDistortionPower;
        
        if(m_ColorDistortionCoroutine != null)
            StopCoroutine(m_ColorDistortionCoroutine);

        m_ColorDistortionCoroutine = StartCoroutine(ColorDistortionStart());
    }

    private IEnumerator ColorDistortionStart()
    {
        while (true)
        {
            m_Chroma.intensity.value -= Time.unscaledDeltaTime * p_ColorDistortionRestoreSpeed;
            
            if (m_Chroma.intensity.value <= 0f)
                break;
            
            yield return null;
        }
    }
}