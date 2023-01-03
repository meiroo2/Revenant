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
    public float p_ScreenBloodEftSpeed = 5f;
    public float p_ColorDistortionPower = 1f;
    public float p_ColorDistortionRestoreSpeed = 5f;
    
    public float p_LensDistortPower = -0.5f;
    public float p_LensDistortRestoreSpeed = 2f;

    [Space(20f), Header("Assign")] 
    public Image p_ScreenBloodImg;
    

    // Member Variables
    private Coroutine m_ScreenBloodEftCoroutine = null;

    private Camera m_MainCamera;
    private VolumeProfile m_CamVolumeProfile;
    private UnityEngine.Rendering.Universal.ChromaticAberration m_Chroma;
    private UnityEngine.Rendering.Universal.LensDistortion m_LensDistort;
    private UnityEngine.Rendering.Universal.Vignette m_Vignette;
    private Coroutine m_LensDistortCoroutine;
    private Coroutine m_ColorDistortionCoroutine;


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


        p_ScreenBloodImg.color = Color.clear;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
    }


    // Updates

    // Functions

    /// <summary>
    /// 화면 가장자리에 유혈 효과를 생성합니다.
    /// </summary>
    public void ActivateScreenBloodEft()
    {
        if (!ReferenceEquals(m_ScreenBloodEftCoroutine, null))
        {
            StopCoroutine(m_ScreenBloodEftCoroutine);
            m_ScreenBloodEftCoroutine = null;
        }
        
        m_ScreenBloodEftCoroutine = StartCoroutine(ScreenBloodEftIEnumerator());
    }

    // 내부 카메라 이펙트 Enumerator
    private IEnumerator ScreenBloodEftIEnumerator()
    {
        // 우선 해당 화면 이펙트 알파값 최대
        Color imgColor = Color.white;
        p_ScreenBloodImg.color = imgColor;

        float timer = 0f;
        
        while (true)
        {
            timer += Time.unscaledDeltaTime * p_ScreenBloodEftSpeed;
            imgColor.a = Mathf.Cos(timer);

            if (imgColor.a <= 0f)
            {
                imgColor.a = 0f;
                p_ScreenBloodImg.color = imgColor;
                break;
            }

            p_ScreenBloodImg.color = imgColor;
            yield return null;
        }

        yield break;
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