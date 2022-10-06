using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RageGauge : MonoBehaviour
{
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Max = 100;                  // 게이지의 최대 수치
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Nature = 1;          // 1초마다 자동으로 충전되는 게이지의 양
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Attack_Multi = 1;    // 피해량 비례 게이지 충전율
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Evade = 1;           // 회피시 게이지 충전량
	[BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_JustEvade = 5;       // 저스트 회피시 게이지 충전량
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Refill_Limit = 50;          // 자연회복 및 자연감소
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Nature = 1;         // 1초마다 자동으로 감소되는 게이지의 양
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Roll = 1;           // 구르기시 소모되는 게이지의 양
    [BoxGroup("게이지 구성요소")] public float p_Gauge_Consume_Melee = 1;          // 근접 공격시 소모되는 게이지의 양
    
    
    // Member Variables
    private bool m_TempStop = false;
    public float m_CurGaugeValue { get; private set; } = 0;
    private bool m_SafetyLock = false;


    // 0-1의 Fill Amount 비례수
    private float m_Multiply = 0.1f;


    private Coroutine m_RefillCoroutine;
    private BulletTimeMgr m_BulletTimeMgr;

    public RageGauge_UI p_RageGaugeUI;

	// Constructors
	private void Awake()
    {
        m_Multiply = 1f / p_Gauge_Max;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_BulletTimeMgr = instance.GetComponentInChildren<BulletTimeMgr>();

        ChangeGaugeValue(p_Gauge_Refill_Limit);
    }

    private void OnEnable()
    {
        m_RefillCoroutine = StartCoroutine(RefillGauge());
    }

    private void OnDisable()
    {
        StopCoroutine(m_RefillCoroutine);
    }


    // Functions
    
    /// <summary>
    /// 일시적으로 RageGauge의 Update를 중단/재개합니다.
    /// </summary>
    public void TempStopRageGauge(bool _isStop)
    {
        m_TempStop = _isStop;
    }
    
    /// <summary>
    /// 원하는 양만큼 게이지 양이 충분한지 알려줍니다.
    /// </summary>
    /// <param name="_value">원하는 소모량</param>
    /// <returns>소모 가능 여부</returns>
    public bool CanConsume(float _value)
    {
        bool returnVal = m_CurGaugeValue - _value >= 0;

        if (!returnVal)
            p_RageGaugeUI.ShakeAction?.Invoke();


		return returnVal;
    }

    

    /// <summary>
    /// 매 1초마다 지정된 값으로 광분 게이지를 자연회복합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RefillGauge()
    {
        Debug.Log("");
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
        // p_GaugeImg.fillAmount = m_CurGaugeValue * m_Multiply;
        p_RageGaugeUI.p_GaugeImg.fillAmount = m_CurGaugeValue * m_Multiply;
		// 만약 Max치를 찍었을 경우, 불릿타임 사용 가능 인디케이터를 띄웁니다.
		if (m_CurGaugeValue >= p_Gauge_Max && !m_BulletTimeMgr.m_IsGaugeFull)
        {
            m_BulletTimeMgr.SetCanUseBulletTime();
            //p_BulletTimeIndicator.enabled = true;
            
            //m_BulletTimeMgr.AddFinaleAction(() => p_BulletTimeIndicator.enabled = false);
        }
        
        m_SafetyLock = false;
    }
}