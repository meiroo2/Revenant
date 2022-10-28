using System;
using System.Collections;
using UnityEngine;


public class UnityCoroutineTest : MonoBehaviour
{
    private CoroutineElement m_Element;
    private Coroutine m_Coroutine;

    private void Start()
    {
        m_Coroutine = StartCoroutine(sansBi());
        m_Element = GameMgr.GetInstance().p_CoroutineHandler.StartCoroutine_Handler(sansBi());
    }

    private void Update()
    {
        Debug.LogAssertion((!ReferenceEquals(m_Coroutine, null)).ToString() + ", " + m_Coroutine);
        if (Input.GetKeyDown(KeyCode.N))
        {
            m_Element.StopCoroutine_Element();
        }
    }

    private IEnumerator sansBi()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.M))
                break;
            
            
            yield return null;
        }

        Debug.Log("SansED");
        yield break;
    }
}