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
    private float m_CurGaugeValue = 0f;
    private bool m_SafetyLock = false;
    public bool m_IsHitMaxOnce = false;

    
    // 0-1의 Fill Amount 비례수
    private float m_Multiply = 0.1f;


    private Coroutine m_RefillCoroutine;
    private BulletTimeMgr m_BulletTimeMgr;

    public RageGauge_UI p_RageGaugeUI;

	// Constructors
	private void Awake()
    {
        m_CurGaugeValue = 0f;
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
    /// RageGauge를 사용해 스킬을 발동합니다.
    /// 성공했을 경우 게이지를 깎고 True를 반환합니다.
    /// </summary>
    /// <param name="_idx"></param>
    /// <returns></returns>
    public bool UseSkillByConsumeRageGauge(int _idx)
    {
        switch (_idx)
        {
            case 0:
                // Roll
                if (m_CurGaugeValue - p_Gauge_Consume_Roll >= 0f)
                {
                    p_RageGaugeUI.ForceStopShake(0);
                    ChangeGaugeValue(m_CurGaugeValue - p_Gauge_Consume_Roll);
                    return true;
                }
                else
                {
                    p_RageGaugeUI.ShakeImg(0);
                    return false;
                }
                break;
            
            case 1:
                // Melee
                if (m_CurGaugeValue - p_Gauge_Consume_Melee >= 0f)
                {
                    p_RageGaugeUI.ForceStopShake(1);
                    ChangeGaugeValue(m_CurGaugeValue - p_Gauge_Consume_Melee);
                    return true;
                }
                else
                {
                    p_RageGaugeUI.ShakeImg(1);
                    return false;
                }

                break;
            
            case 2:
                // Ult
                if (m_IsHitMaxOnce)
                {
                    p_RageGaugeUI.ForceStopShake(2);
                    m_IsHitMaxOnce = false;
                    return true;
                }
                else
                {
                    p_RageGaugeUI.ShakeImg(2);
                    return false;
                }
                break;
        }

        return false;
    }

    

    /// <summary>
    /// 매 1초마다 지정된 값으로 광분 게이지를 자연회복합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RefillGauge()
    {
        //Debug.Log("");
        // Timescale에 영향이 가지 않도록 Realtime 기준으로 작동합니다.
        while (true)
        {
            if (!m_SafetyLock && !m_TempStop)
            {
                if (m_CurGaugeValue < p_Gauge_Refill_Limit)
                {
                    ChangeGaugeValue(m_CurGaugeValue + (Time.unscaledDeltaTime * p_Gauge_Refill_Nature));
                    
                    if (m_CurGaugeValue > p_Gauge_Refill_Limit)
                        ChangeGaugeValue(p_Gauge_Refill_Limit);
                }
                else if(m_CurGaugeValue > p_Gauge_Refill_Limit)
                {
                    ChangeGaugeValue(m_CurGaugeValue - (Time.unscaledDeltaTime * p_Gauge_Consume_Nature));

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
    /// 광분 게이지에 값을 추가합니다.
    /// </summary>
    /// <param name="_input"></param>
    public void AddGaugeValue(float _input)
    {
        if (_input <= 0f)
            return;
        
        ChangeGaugeValue(m_CurGaugeValue + _input);
    }

    private float[] m_FloatArr = new float[4];
    /// <summary>
    /// 현재 광분 게이지 값을 변경합니다.
    /// </summary>
    /// <param name="_input">원하는 값</param>
    private void ChangeGaugeValue(float _input)
    {
        if (_input < 0f)
            return;
        
        m_SafetyLock = true;
        m_CurGaugeValue = _input;
        
        // Max치보다 높게 들어올 경우 풀로 채워줘야 함
        if (m_CurGaugeValue > p_Gauge_Max)
            m_CurGaugeValue = p_Gauge_Max;
        
        m_FloatArr[0] = m_CurGaugeValue / p_Gauge_Max;
        m_FloatArr[1] = m_CurGaugeValue / p_Gauge_Consume_Roll;
        m_FloatArr[2] = m_CurGaugeValue / p_Gauge_Consume_Melee;

        if (!m_IsHitMaxOnce)
        {
            m_FloatArr[3] = m_CurGaugeValue / p_Gauge_Max;
            if (m_FloatArr[3] >= 1f)
            {
                m_IsHitMaxOnce = true;
            }
        }

        for (int i = 0; i < m_FloatArr.Length; i++)
        {
            p_RageGaugeUI.UpdateRageGaugeUI(i, m_FloatArr[i]);
        }

        m_SafetyLock = false;
    }
}