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
    
    
    // Member Variables
    private Color m_Color;
    private Image m_Image;
    private Coroutine m_EdgeEffectCoroutine;

    private Camera m_MainCamera;
    private VolumeProfile m_CamVolumeProfile;
    private UnityEngine.Rendering.Universal.ChromaticAberration m_Chroma;
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


        m_Image = GetComponentInChildren<Image>();
        m_Color = new Color(1, 1, 1, 0);
        m_Image.color = m_Color;
    }
    
    
    // Functions
    
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
            m_Color.a -= Time.deltaTime * p_EdgeEffectFadeoutspeed;
            
            if (m_Color.a <= 0f)
                break;
            
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
            m_Chroma.intensity.value -= Time.deltaTime * p_ColorDistortionRestoreSpeed;
            
            if (m_Chroma.intensity.value <= 0f)
                break;
            
            yield return null;
        }
    }
}