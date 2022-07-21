using System;
using System.Collections;
using UnityEngine;


public class HitSFX_Instance : MonoBehaviour
{
    // Visible Member Variables
    public GameObject[] p_ObjsWhenOffOnAnimationEnd;
    
    
    // Member Variables
    private Animator m_Animator;
    private Coroutine m_Coroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        m_Animator.Rebind();
        m_Coroutine = StartCoroutine(CheckAnimation());
    }

    private void OnDisable()
    {
        if(m_Coroutine != null)
            StopCoroutine(m_Coroutine);
        
        if (p_ObjsWhenOffOnAnimationEnd.Length > 0)
        {
            for (int i = 0; i < p_ObjsWhenOffOnAnimationEnd.Length; i++)
            {
                p_ObjsWhenOffOnAnimationEnd[i].SetActive(true);
            }
        }
    }


    // Functions
    private IEnumerator CheckAnimation()
    {
        
        while (true)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            
            yield return null;
        }

        // 애니메이션 끝남
        if (p_ObjsWhenOffOnAnimationEnd.Length > 0)
        {
            for (int i = 0; i < p_ObjsWhenOffOnAnimationEnd.Length; i++)
            {
                p_ObjsWhenOffOnAnimationEnd[i].SetActive(false);
            }
        }

        m_Coroutine = StartCoroutine(DisableObj());
    }

    private IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}