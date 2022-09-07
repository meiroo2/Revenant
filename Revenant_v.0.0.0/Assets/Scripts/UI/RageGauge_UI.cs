using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
    // Visible Member Variables
    [Tooltip("자연회복 게이지 이미지")] public Image p_NaturalImg;
    [Tooltip("광분 게이지 이미지")] public Image p_RageImg;

    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public float p_Gauge_TickTime { get; private set; } = 1f;

    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Max { get; private set; } = 100;

    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Refill_Nature { get; private set; } = 1;
    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Refill_Attack { get; private set; } = 1;
    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Refill_Evade { get; private set; } = 1;
    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Refill_Limit { get; private set; } = 50;

    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Consume_Nature { get; private set; } = 1;
    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Consume_Roll { get; private set; } = 1;
    [field: SerializeField, BoxGroup("게이지 구성요소")]
    public int p_Gauge_Consume_Melee { get; private set; } = 1;
    
    
    // Member Variables
    public int m_CurGaugeValue { get; private set; } = 0;
    private bool m_SafetyLock = false;
    private float m_RageValue = 0f;
    private float m_NaturalValue = 0f;
    private float m_Multiply = 0.1f;

    private Coroutine m_Coroutine;
    
    // Constructors
    private void Awake()
    {
        if (ReferenceEquals(p_NaturalImg, null) || ReferenceEquals(p_RageImg, null))
        {
            Debug.Log("ERR : RageGauge_UI에서 이미지 할당되지 않음.");
        }

        m_Multiply = 2f / p_Gauge_Max;
    }

    private void Start()
    {
        ChangeGaugeValue(p_Gauge_Refill_Limit);
    }

    private void OnEnable()
    {
        m_Coroutine = StartCoroutine(RefillGauge());
    }

    private void OnDisable()
    {
        StopCoroutine(m_Coroutine);
    }


    // Updates


    // Functions

    /// <summary>
    /// 원하는 양만큼 게이지 양이 충분한지 알려줍니다.
    /// </summary>
    /// <param name="_value">원하는 소모량</param>
    /// <returns>소모 가능 여부</returns>
    public bool CanConsume(int _value)
    {
        return m_CurGaugeValue - _value >= 0;
    }

    /// <summary>
    /// 매 1초마다 지정된 값으로 광분 게이지를 자연회복합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RefillGauge()
    {
        while (true)
        {
            if (!m_SafetyLock)
            {
                if (m_CurGaugeValue < p_Gauge_Refill_Limit)
                {
                    ChangeGaugeValue(m_CurGaugeValue + p_Gauge_Refill_Nature);
                }
                else
                {
                    ChangeGaugeValue(m_CurGaugeValue - p_Gauge_Consume_Nature);
                }
            }
            yield return new WaitForSecondsRealtime(p_Gauge_TickTime);
        }

        yield break;
    }

    /// <summary>
    /// 광분 게이지를 원하는 값으로 변경 및 초기화합니다.
    /// </summary>
    /// <param name="_input">원하는 값</param>
    public void ChangeGaugeValue(int _input)
    {
        if (_input > p_Gauge_Max || _input < 0)
            return;
        
        m_SafetyLock = true;

        m_CurGaugeValue = _input;
        float value = _input * m_Multiply;

        if (value > 1f)
        {
            m_NaturalValue = 1f;
            value -= 1f;
            m_RageValue = value;
        }
        else
        {
            m_NaturalValue = value;
            m_RageValue = 0f;
        }
        
        p_NaturalImg.fillAmount = m_NaturalValue;
        p_RageImg.fillAmount = m_RageValue;
        
        m_SafetyLock = false;
    }
}