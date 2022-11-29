using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_BoomBox : Breakable
{
    public int Damage = 10;
    public int StunValue = 10;
    
    private BoomBox_Bomb m_Bomb;
    
    private Coroutine m_Coroutine;
    
    private void Start()
    {
        m_Bomb = GetComponentInChildren<BoomBox_Bomb>();
    }

    public override void GetHit(int _damage)
    {
        if (Health <= 0)
            return;
        
        Health -= _damage;
        
        if (Health <= 0)
        {
            m_Bomb.Explode(Damage, StunValue);
            m_Animator.SetTrigger("Explode");
            m_HotBox.gameObject.SetActive(false);
            m_Coroutine = StartCoroutine(CheckExplodeEnd());
        }
    }
    
    private void OnDisable()
    {
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }
    }
    
    private IEnumerator CheckExplodeEnd()
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
