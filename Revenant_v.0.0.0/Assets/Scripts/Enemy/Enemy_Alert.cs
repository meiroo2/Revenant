using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_Alert : MonoBehaviour
{
    // Member Variables
    private bool m_AlertFilling = false;

    [field : SerializeField] public Animator m_Animator { get; private set; }
    private SpriteRenderer m_Renderer;
    
    public delegate void AlertDelegate(); 
    private AlertDelegate m_Callback = null;
    private AnimatorStateInfo m_CurAniState;
    
    private Coroutine m_CurCoroutine;

    
    // Hashes
    private static readonly int IsActivate = Animator.StringToHash("IsActivate");
    private static readonly int IsHit = Animator.StringToHash("IsHit");
    private static readonly int Idle_White = Animator.StringToHash("Idle_White");
    private static readonly int DoAttack = Animator.StringToHash("DoAttack");
    private static readonly int AlertSpeed = Animator.StringToHash("AlertSpeed");
    private static readonly int DoStun = Animator.StringToHash("DoStun");


    // Constructors
    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }


    // Updates
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!m_AlertFilling)
            return;

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")&&
            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            m_Callback?.Invoke();
            m_AlertFilling = false;
        }
    }

    
    // Functions
    public void SetAlertSpeed(float _speed)
    {
        m_Animator.SetFloat(AlertSpeed, _speed);
    }
    
    public void SetAlertStun()
    {
        m_Animator.SetTrigger(DoStun);
        
        if (!ReferenceEquals(m_CurCoroutine, null))
            StopCoroutine(m_CurCoroutine);
        
        m_CurCoroutine = StartCoroutine(DoAlertStun());
    }

    private IEnumerator DoAlertStun()
    {
        while (true)
        {
            yield return null;

            m_CurAniState = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (m_CurAniState.normalizedTime >= 1f)
                break;
        }

        m_Callback?.Invoke();
        m_Animator.Play(Idle_White);
        
        yield break;
    }
    
    public void SetAlertActive(bool _isActive)
    {
        m_Animator.SetBool(IsActivate, true);
    }

    public void SetAlertFill(bool _doFill)
    {
        if (_doFill)
        {
            m_Animator.SetBool("IsCharge", _doFill);

            if (!ReferenceEquals(m_CurCoroutine, null))
                StopCoroutine(m_CurCoroutine);
            m_CurCoroutine = StartCoroutine(AlertFillCheck());
        }
        else
        {
            m_Animator.SetBool("IsCharge", false);
            m_Animator.SetTrigger("IsAttackEnd");
        }
    }

    private IEnumerator AlertFillCheck()
    {
        while (true)
        {
            m_CurAniState = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (m_CurAniState.IsName("GageUp") && m_CurAniState.normalizedTime >= 1f)
                break;
            
            yield return null;
        }
        m_Animator.SetTrigger(DoAttack);
        m_Callback?.Invoke();

        yield break;
    }
    
    public void SetCallback(AlertDelegate _input, bool _doReset = false)
    {
        if (_doReset)
            m_Callback = null;

        m_Callback += _input;
    }
}