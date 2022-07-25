using System;
using System.Collections;
using UnityEngine;


public class CoroutineElement : MonoBehaviour
{
    // Member Variables
    private IEnumerator m_Enumerator = null;
    private Coroutine m_Coroutine;
    
    
    // Updates
    private void FixedUpdate()
    {
        if (ReferenceEquals(m_Coroutine, null))
        {
           // Debug.Log("Element End");
            Destroy(gameObject);
        }
    }


    // Functions
    public void StopCoroutine_Element()
    {
        //Debug.Log("Element Stop_Force");
        
        if(!ReferenceEquals(m_Coroutine, null))
            StopCoroutine(m_Coroutine);
        
        Destroy(gameObject);
    }

    public CoroutineElement StartCoroutine_Element(IEnumerator _enumerator)
    {
        //Debug.Log("Element Start");
        m_Enumerator = _enumerator;
        m_Coroutine = StartCoroutine(m_Enumerator);
        return this;
    }
}