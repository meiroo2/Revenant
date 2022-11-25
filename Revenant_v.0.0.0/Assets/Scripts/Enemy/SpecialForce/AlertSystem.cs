using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;





public class AlertSystem : MonoBehaviour
{
    // Member Variables
    public Animator m_Animator { get; private set; }

    public bool m_IsOnline { get; private set; } = false;

    private readonly int AlertSpeed = Animator.StringToHash("AlertSpeed");
    private readonly int StunAlertSpeed = Animator.StringToHash("StunAlertSpeed");
    private readonly int FadeIn = Animator.StringToHash("FadeIn");
    private readonly int GaugeUp = Animator.StringToHash("GaugeUp");
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int Stun = Animator.StringToHash("Stun");

    private Coroutine m_GaugeUpCoroutine;
    private Coroutine m_StunCoroutine;
    private Coroutine m_FadeInCoroutine;

    private bool m_IsGaugeUp = false;
    private static readonly int FadeInSpeed = Animator.StringToHash("FadeInSpeed");


    // Constructors
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }
    
    
    // Functions

    /// <summary>
    /// Alert를 Stun 상태로 가도록 지시합니다.
    /// </summary>
    public void DoStun(Action _action)
    {
        if(!ReferenceEquals(m_GaugeUpCoroutine, null))
            StopCoroutine(m_GaugeUpCoroutine);
        
        if(!ReferenceEquals(m_StunCoroutine, null))
            StopCoroutine(m_StunCoroutine);
        
        if(!ReferenceEquals(m_FadeInCoroutine, null))
            StopCoroutine(m_FadeInCoroutine);

        m_IsGaugeUp = true;
        m_Animator.SetInteger(FadeIn, 2);
        m_Animator.SetInteger(GaugeUp, 0);
        m_Animator.SetInteger(Attack, 0);
        
        m_Animator.SetInteger(Stun, 1);

        m_StunCoroutine = StartCoroutine(CalStun(_action));
    }

    private IEnumerator CalStun(Action _action)
    {
        while (true)
        {
            yield return null;
            
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }

        m_Animator.SetInteger(Stun, 0);
        m_IsGaugeUp = false;
        
        _action?.Invoke();
        
        yield break;
    }

    /// <summary>
    /// Alert의 Gauge 상승을 시작합니다.
    /// </summary>
    /// <param name="_action">상승 후 호출할 Delegate Action</param>
    public void AlertGaugeUp(Action _action = null)
    {
        m_Animator.SetInteger(GaugeUp, 0);
        m_Animator.SetInteger(Attack, 0);
        
        if(!ReferenceEquals(m_GaugeUpCoroutine, null))
            StopCoroutine(m_GaugeUpCoroutine);

        m_GaugeUpCoroutine = StartCoroutine(CalGaugeUp(_action));
    }

    /// <summary>
    /// GauguUp 함수를 취소합니다.
    /// </summary>
    public void CancelGaugeUp()
    {
        m_Animator.SetInteger(GaugeUp, 0);
        m_Animator.SetInteger(Attack, 0);
        
        if(!ReferenceEquals(m_GaugeUpCoroutine, null))
            StopCoroutine(m_GaugeUpCoroutine);
    }

    private IEnumerator CalGaugeUp(Action _action)
    {
        yield return null;
        m_Animator.SetInteger(GaugeUp, 1);
        
        while (true)
        {
            yield return null;

            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }

        _action?.Invoke();
        m_Animator.SetInteger(Attack, 1);
        
        while (true)
        {
            yield return null;

            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }
        
        m_Animator.SetInteger(GaugeUp, 0);
        m_Animator.SetInteger(Attack, 0);

        yield break;
    }
    
    
    /// <summary>
    /// Alert Animator를 초기화합니다.
    /// </summary>
    public void ResetAlert()
    {
        m_Animator.enabled = false;
        m_Animator.enabled = true;
    }

    /// <summary>
    /// Alert를 등장시킵니다.
    /// </summary>
    public void DoFadeIn(Action _action)
    {
        m_Animator.SetInteger(FadeIn, 1);
        
        if(!ReferenceEquals(m_FadeInCoroutine, null))
            StopCoroutine(m_FadeInCoroutine);

        m_FadeInCoroutine = StartCoroutine(CalFadeIn(_action));
    }

    private IEnumerator CalFadeIn(Action _action)
    {
        while (true)
        {
            yield return null;

            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }

        m_IsOnline = true;
        m_Animator.SetInteger(FadeIn, 2);
        _action?.Invoke();
        
        yield break;
    }
    
    /// <summary>
    /// Alert의 재생 속도(차는 속도)를 결정합니다.
    /// </summary>
    /// <param name="_speed"></param>
    public void SetAlertSpeed(float _speed)
    {
        m_Animator.SetFloat(AlertSpeed, _speed);
    }

    /// <summary>
    /// Stun시 Alert의 재생 속도를 결정합니다.
    /// </summary>
    /// <param name="_speed"></param>
    public void SetStunAlertSpeed(float _speed)
    {
        m_Animator.SetFloat(StunAlertSpeed, _speed);
    }

    public void SetFadeInSpeed(float _speed)
    {
        m_Animator.SetFloat(FadeInSpeed, _speed);
    }
}