using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class HitSFX_Instance : MonoBehaviour
{
    // Visible Member Variables
    public Light2D[] p_LightsArr;
    
    
    // Member Variables
    private float[] m_LightIntensityArr;

    private Animator m_Animator;
    private Coroutine m_Coroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();

        if (p_LightsArr.Length <= 0)
            return;
        
        m_LightIntensityArr = new float[p_LightsArr.Length];
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            m_LightIntensityArr[i] = p_LightsArr[i].intensity;
        }
    }
    private void OnEnable()
    {
        m_Animator.Rebind();
        m_Coroutine = StartCoroutine(CheckAnimation());
    }

    private void OnDisable()
    {
        if(m_Coroutine != null)
            StopCoroutine(m_Coroutine);
        
        RestoreLightIntensity();
    }


    // Functions
    private IEnumerator CheckAnimation()
    {
        float animNormalizedTime;
        while (true)
        {
            animNormalizedTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            DecreaseLightIntensity(animNormalizedTime);
            
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            
            yield return null;
        }

        m_Coroutine = StartCoroutine(DisableObj());
    }

    private IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void DecreaseLightIntensity(float _input)
    {
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            p_LightsArr[i].intensity = m_LightIntensityArr[i] - (_input * m_LightIntensityArr[i]);
        }
    }
    private void RestoreLightIntensity()
    {
        for (int i = 0; i < p_LightsArr.Length; i++)
        {
            p_LightsArr[i].intensity = m_LightIntensityArr[i];
        }
    }
}