using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    protected int Health = 0;
    [SerializeField]
    protected int HitCount = 0;
    
    protected Breakable_Hotbox m_HotBox;
    protected Animator m_Animator;
    protected Coroutine m_Coroutine;

    private void Awake()
    {
        m_HotBox = GetComponentInChildren<Breakable_Hotbox>();
        m_Animator = GetComponentInChildren<Animator>();

        m_HotBox.Breakable = this;
        
        Health = HitCount * 10;
    }

    public virtual void GetHit(int _damage) { }
    
    private void OnDisable()
    {
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }
    }
    
    protected IEnumerator CheckExplodeEnd()
    {
        AnimatorStateInfo stateInfo;
        
        while (true)
        {
            yield return null;

            stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.normalizedTime >= 1f)
            {
                break;
            }
        }
        
        gameObject.SetActive(false);
        
        yield break;
    }
}
