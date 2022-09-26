using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RageGauge_UI : MonoBehaviour
{
    // Visible Member Variables
    [BoxGroup("게이지 비주얼")] public Image p_BackImg;
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
    private bool m_TempStop = false;
    private DynamicUIMgr m_DynaUIMgr;
    public float m_CurGaugeValue { get; private set; } = 0;
    private bool m_SafetyLock = false;
    private Vector2 m_InitPos;

    private Vector2 m_InitBackImgScale;
    private Vector2 m_InitGaugeImgScale;

    private Color m_InitGaugeColor;
    
    private Vector2 m_RectInitPos;

    // 0-1의 Fill Amount 비례수
    private float m_Multiply = 0.1f;

    private RectTransform m_RectTransform;
    private Coroutine m_Coroutine;
    private Coroutine m_ShakeCoroutine;
    private BulletTimeMgr m_BulletTimeMgr;
    
    // Constructors
    private void Awake()
    {
        m_InitGaugeColor = p_GaugeImg.color;
        m_RectTransform = GetComponent<RectTransform>();
        m_RectInitPos = m_RectTransform.anchoredPosition;
        
        if (ReferenceEquals(p_GaugeImg, null))
        {
            Debug.Log("ERR : RageGauge_UI에서 이미지 할당되지 않음.");
        }


        m_InitBackImgScale = p_BackImg.rectTransform.localScale;
        m_InitGaugeImgScale = p_GaugeImg.rectTransform.localScale;
        m_Multiply = 1f / p_Gauge_Max;
        p_BulletTimeIndicator.enabled = false;
    }

    private void Start()
    {
        m_DynaUIMgr = GameMgr.GetInstance().GetComponent<DynamicUIMgr>();
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
       
    }

    // Functions

    /// <summary>
    /// RageGauge를 일시적으로 Timer로 사용하기 위해 일반화된 시간값을 받습니다. (즉시 적용)
    /// </summary>
    /// <param name="_normalTime"></param>
    public void GetTimePassed(float _normalTime)
    {
        //Debug.Log(p_GaugeImg.fillAmount);
        p_GaugeImg.fillAmount = _normalTime;
    }
    
    /// <summary>
    /// 일시적으로 RageGauge의 Update를 중단/재개합니다.
    /// </summary>
    public void TempStopRageGauge(bool _isStop)
    {
        m_TempStop = _isStop;
    }
    
    /// <summary>
    /// BulletTime 작동시 Gauge의 Dynamic 애니메이션을 적용합니다.
    /// </summary>
    /// <param name="_isTrue"></param>
    public void GaugeToBulletTime(bool _isTrue)
    {
        if (_isTrue)
        {
            RectTransform backForm = p_BackImg.rectTransform;
            m_DynaUIMgr.ExpandUI(backForm, m_InitBackImgScale,
                new Vector2(backForm.localScale.x + 0.07f, backForm.localScale.y),
                5f);

            RectTransform gaugeForm = p_GaugeImg.rectTransform;
            m_DynaUIMgr.ExpandUI(gaugeForm, m_InitGaugeImgScale,
                new Vector2(gaugeForm.localScale.x + 0.07f, gaugeForm.localScale.y),
                5f);
            
            m_DynaUIMgr.ChangeColor(p_GaugeImg, m_InitGaugeColor, Color.white, 3f);
        }
        else
        {
            RectTransform backForm = p_BackImg.rectTransform;
            m_DynaUIMgr.ExpandUI(backForm, backForm.localScale,
                m_InitBackImgScale,
                5f);

            RectTransform gaugeForm = p_GaugeImg.rectTransform;
            m_DynaUIMgr.ExpandUI(gaugeForm, gaugeForm.localScale,
                m_InitGaugeImgScale,
                5f);
            
            m_DynaUIMgr.ChangeColor(p_GaugeImg, Color.white, m_InitGaugeColor, 3f);
        }
    }
    
    
    /// <summary>
    /// 원하는 양만큼 게이지 양이 충분한지 알려줍니다.
    /// </summary>
    /// <param name="_value">원하는 소모량</param>
    /// <returns>소모 가능 여부</returns>
    public bool CanConsume(float _value)
    {
        bool returnVal = m_CurGaugeValue - _value >= 0;
        
        if(!returnVal)
            m_DynaUIMgr.Shake(m_RectTransform, m_InitPos, 30f, 1500f, 10f, 30f);
        
        return returnVal;
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
            if (!m_SafetyLock && !m_TempStop)
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
            
            //m_BulletTimeMgr.AddFinaleAction(() => p_BulletTimeIndicator.enabled = false);
        }
        
        m_SafetyLock = false;
    }
}