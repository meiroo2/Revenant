using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
    // Visible Member Variables
    [BoxGroup("게이지 비주얼")] public Image p_GaugeImg;
    [BoxGroup("게이지 비주얼")] public Image p_BulletTimeIndicator;

    
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Max = 100;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Nature = 1;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Attack_Multi = 1;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Evade = 1;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_JustEvade = 5;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Limit = 50;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Nature = 1;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Roll = 1;
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Melee = 1;
    
    
    // Member Variables
    public float m_CurGaugeValue { get; private set; } = 0;
    private bool m_SafetyLock = false;
    private Vector2 m_InitPos;
    

    // 0-1의 Fill Amount 비례수
    private float m_Multiply = 0.1f;

    private RectTransform m_RectTransform;
    private Coroutine m_Coroutine;
    private Coroutine m_ShakeCoroutine;
    private BulletTimeMgr m_BulletTimeMgr;
    
    // Constructors
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        
        if (ReferenceEquals(p_GaugeImg, null))
        {
            Debug.Log("ERR : RageGauge_UI에서 이미지 할당되지 않음.");
        }

        
        
        m_Multiply = 1f / p_Gauge_Max;
        p_BulletTimeIndicator.enabled = false;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();

        m_InitPos = m_RectTransform.anchoredPosition;
        
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShakeGauge();
        }
    }

    // Functions

    /// <summary>
    /// RageGauge를 잠깐 흔듭니다. 부정적 의미로 사용
    /// </summary>
    public void ShakeGauge()
    {
        if(!ReferenceEquals(m_ShakeCoroutine, null))
            StopCoroutine(m_ShakeCoroutine);

        m_RectTransform.anchoredPosition = m_InitPos;
        m_ShakeCoroutine = StartCoroutine(ShakeCoroutine());
    }
    private IEnumerator ShakeCoroutine()
    {
        Vector2 pos = m_InitPos;
        float right = pos.x + 30f;
        float left = pos.x - 30f;

        bool toRight = true;

        float shakeSpeed = 2000f;
        
        while (true)
        {
            if (left >= m_InitPos.x - 15f || right <= m_InitPos.x + 15f)
            {
                pos = Vector2.Lerp(pos, m_InitPos, shakeSpeed);
                if (Vector2.Distance(pos, m_InitPos) < 1f)
                {
                    m_RectTransform.anchoredPosition = m_InitPos;
                    break;
                }
            }
            
            if (toRight)
            {
                pos.x += Time.deltaTime * shakeSpeed;
                if (pos.x > right)
                {
                    toRight = false;
                    right -= 5f;
                    shakeSpeed -= 300f;
                }
            }
            else
            {
                pos.x -= Time.deltaTime * shakeSpeed;
                if (pos.x < left)
                {
                    toRight = true;
                    left += 5f;
                    shakeSpeed -= 300f;
                }
            }


            m_RectTransform.anchoredPosition = pos;
            yield return null;
        }
        Debug.Log("Out");
        yield break;
    }
    
    
    /// <summary>
    /// 원하는 양만큼 게이지 양이 충분한지 알려줍니다.
    /// </summary>
    /// <param name="_value">원하는 소모량</param>
    /// <returns>소모 가능 여부</returns>
    public bool CanConsume(float _value)
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


                    ChangeGaugeValue(m_CurGaugeValue + (Time.deltaTime * p_Gauge_Refill_Nature));
                    
                    if (m_CurGaugeValue > p_Gauge_Refill_Limit)
                        ChangeGaugeValue(p_Gauge_Refill_Limit);
                }
                else if(m_CurGaugeValue > p_Gauge_Refill_Limit)
                {
                    ChangeGaugeValue(m_CurGaugeValue - (Time.deltaTime * p_Gauge_Consume_Nature));

                    if (m_CurGaugeValue < p_Gauge_Refill_Limit)
                        ChangeGaugeValue(p_Gauge_Refill_Limit);
                }
                else
                {

                }
                
                
            }

            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// 현재 광분 게이지 값을 변경합니다.
    /// </summary>
    /// <param name="_input">원하는 값</param>
    public void ChangeGaugeValue(float _input)
    {
        if (_input < 0)
            return;
        
        m_SafetyLock = true;

        // Max치보다 높게 들어올 경우 풀로 채워줘야 함
        if (_input >= p_Gauge_Max)
        {
            _input = p_Gauge_Max;
        }
        
        m_CurGaugeValue = _input;
        p_GaugeImg.fillAmount = m_CurGaugeValue * m_Multiply;

        // 만약 Max치를 찍었을 경우, 불릿타임 사용 가능 인디케이터를 띄웁니다.
        if (m_CurGaugeValue >= p_Gauge_Max && !m_BulletTimeMgr.m_CanUseBulletTime)
        {
            m_BulletTimeMgr.SetCanUseBulletTime();
            p_BulletTimeIndicator.enabled = true;
            
            m_BulletTimeMgr.AddFinaleAction(() => p_BulletTimeIndicator.enabled = false);
        }
        
        m_SafetyLock = false;
    }
}