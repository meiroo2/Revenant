using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
    // Visible Member Variables
    [Tooltip("자연회복 게이지 이미지")] public Image p_NaturalImg;
    [Tooltip("광분 게이지 이미지")] public Image p_RageImg;
    public float p_GaugeSpeed = 0.5f;
    
    // Member Variables
    private bool m_SafetyLock = false;
    private float m_NaturalValue = 1f;
    private float m_RageValue = 0f;
    
    
    // Constructors
    private void Awake()
    {
        if (ReferenceEquals(p_NaturalImg, null) || ReferenceEquals(p_RageImg, null))
        {
            Debug.Log("ERR : RageGauge_UI에서 이미지 할당되지 않음.");
        }
    }
    
    
    // Updates
    private void Update()
    {
        if (m_SafetyLock)
            return;

        if (m_NaturalValue < 1f)
        {
            m_NaturalValue += Time.deltaTime * p_GaugeSpeed;
            
            if (m_NaturalValue >= 1f)
                m_NaturalValue = 1f;
        }
        else if (m_NaturalValue >= 1f && m_RageValue > 0f)
        {
            m_RageValue -= Time.deltaTime * p_GaugeSpeed;
        }

        p_NaturalImg.fillAmount = m_NaturalValue;
        p_RageImg.fillAmount = m_RageValue;
    }
    
    
    // Functions
    /// <summary>
    /// RageGauge에서 값을 뻅니다. 성공 시 True를 반환합니다.
    /// </summary>
    /// <param name="_input"> 뺄 양 </param>
    /// <returns> 성공 여부 </returns>
    public bool MinusValueToRageGauge(float _input)
    {
        m_SafetyLock = true;

        if (m_NaturalValue + m_RageValue < _input)
        {
            m_SafetyLock = false;
            return false;
        }

        // Rage가 조금이라도 차 있는 경우
        if (m_RageValue > 0f)
        {
            m_RageValue -= _input;

            if (m_RageValue < 0f)
            {
                m_NaturalValue += m_RageValue;
                m_RageValue = 0f;
            }
        }
        else
        {
            m_NaturalValue -= _input;
        }
        
        p_NaturalImg.fillAmount = m_NaturalValue;
        p_RageImg.fillAmount = m_RageValue;

        m_SafetyLock = false;
        return true;
    }
    
    
    /// <summary>
    /// RageGauge에 값을 더합니다.
    /// </summary>
    /// <param name="_input"> 더할 양 </param>
    public void AddValueToRageGauge(float _input)
    {
        m_SafetyLock = true;


        // 아직 자연회복 중
        if (m_NaturalValue < 1f)
        {
            m_NaturalValue += _input;

            if (m_NaturalValue > 1f)
            {
                float remain = m_NaturalValue - 1f;
                m_NaturalValue = 1f;
                m_RageValue = remain;
            }
        }
        else if(m_NaturalValue >= 1f)
        {
            m_RageValue += _input;
            if (m_RageValue >= 1f)
            {
                m_RageValue = 1f;
            }
        }
        

        p_NaturalImg.fillAmount = m_NaturalValue;
        p_RageImg.fillAmount = m_RageValue;
        
        m_SafetyLock = false;
    }
}