using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;





public class AlertSystem : MonoBehaviour
{
    // Member Variables
    public Animator m_Animator { get; private set; }
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
    /// Alert�� Stun ���·� ������ �����մϴ�.
    /// </summary>
    public void DoStun()
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

        m_StunCoroutine = StartCoroutine(CalStun());
    }

    private IEnumerator CalStun()
    {
        while (true)
        {
            yield return null;
            
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }

        m_Animator.SetInteger(Stun, 0);
        m_IsGaugeUp = false;
        
        yield break;
    }

    /// <summary>
    /// Alert�� Gauge ����� �����մϴ�.
    /// </summary>
    /// <param name="_action">��� �� ȣ���� Delegate Action</param>
    public void AlertGaugeUp(Action _action = null)
    {
        m_Animator.SetInteger(GaugeUp, 0);
        m_Animator.SetInteger(Attack, 0);
        
        if(!ReferenceEquals(m_GaugeUpCoroutine, null))
            StopCoroutine(m_GaugeUpCoroutine);

        m_GaugeUpCoroutine = StartCoroutine(CalGaugeUp(_action));
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
    /// Alert Animator�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void ResetAlert()
    {
        m_Animator.enabled = false;
        m_Animator.enabled = true;
    }

    /// <summary>
    /// Alert�� �����ŵ�ϴ�.
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

        m_Animator.SetInteger(FadeIn, 2);
        _action?.Invoke();
        
        yield break;
    }
    
    /// <summary>
    /// Alert�� ��� �ӵ�(���� �ӵ�)�� �����մϴ�.
    /// </summary>
    /// <param name="_speed"></param>
    public void SetAlertSpeed(float _speed)
    {
        m_Animator.SetFloat(AlertSpeed, _speed);
    }

    /// <summary>
    /// Stun�� Alert�� ��� �ӵ��� �����մϴ�.
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