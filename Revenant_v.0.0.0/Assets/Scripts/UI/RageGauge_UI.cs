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
    public Image p_BulletTimeIndicator;

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
    public int p_Gauge_Refill_JustEvade { get; private set; } = 5;
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

    private BulletTimeMgr m_BulletTimeMgr;
    
    // Constructors
    private void Awake()
    {
        if (ReferenceEquals(p_NaturalImg, null) || ReferenceEquals(p_RageImg, null))
        {
            Debug.Log("ERR : RageGauge_UI에서 이미지 할당되지 않음.");
        }

        m_Multiply = 2f / p_Gauge_Max;
        p_BulletTimeIndicator.enabled = false;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();
        
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
        // Timescale에 영향이 가지 않도록 Realtime 기준으로 작동합니다.
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
    /// 현재 광분 게이지 값을 변경합니다.
    /// </summary>
    /// <param name="_input">원하는 값</param>
    public void ChangeGaugeValue(int _input)
    {
        //Debug.Log(_input -m_CurGaugeValue  + " 게이지 변화");
        
        if (_input < 0)
            return;
        
        // Max치보다 높게 들어올 경우 풀로 채워줘야 함
        if (_input >= p_Gauge_Max)
        {
            _input = p_Gauge_Max;
        }
        
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

        // 만약 Max치를 찍었을 경우, 불릿타임 사용 가능 인디케이터를 띄웁니다.
        if (m_CurGaugeValue >= p_Gauge_Max && !m_BulletTimeMgr.m_CanUseBulletTime)
        {
            m_BulletTimeMgr.SetCanUseBulletTime();
            p_BulletTimeIndicator.enabled = true;
        }
        
        m_SafetyLock = false;
    }
}