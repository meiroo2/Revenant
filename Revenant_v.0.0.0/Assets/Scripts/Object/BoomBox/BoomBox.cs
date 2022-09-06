using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class BoomBox : MonoBehaviour
{
    // Visible Member Variables
    public int p_Hp = 30;
    public int p_Damage = 10;
    public int p_StunValue = 10;


    // Member Variables
    private BoomBox_HotBox m_HotBox;
    private BoomBox_Bomb m_Bomb;
    private Animator m_Animator;
    private Coroutine m_Coroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_HotBox = GetComponentInChildren<BoomBox_HotBox>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Bomb = GetComponentInChildren<BoomBox_Bomb>();
        
        m_HotBox.m_BoomBox = this;
    }

    private void OnDisable()
    {
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }
    }


    // Functions
    public void GetHit(int _damage)
    {
        if (p_Hp <= 0)
            return;
        
        p_Hp -= _damage;
        
        if (p_Hp <= 0)
        {
            m_Bomb.Explode(p_Damage, p_StunValue);
            m_Animator.SetTrigger("Explode");
            m_HotBox.gameObject.SetActive(false);
            m_Coroutine = StartCoroutine(CheckExplodeEnd());
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