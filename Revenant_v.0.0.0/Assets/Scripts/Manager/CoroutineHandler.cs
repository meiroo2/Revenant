using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CoroutineHandler : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_CoroutineElement;
    
    
    // Member Variables
    private List<CoroutineElement> m_CoroutineElementList = new List<CoroutineElement>();


    // Functions
    public CoroutineElement StartCoroutine_Handler(IEnumerator _enumerator)
    {
        var element = Instantiate(p_CoroutineElement, transform).GetComponent<CoroutineElement>();
        element.m_Handler = this;
        m_CoroutineElementList.Add(element);
        
        return element.StartCoroutine_Element(_enumerator);
    }

    public void DeleteCoroutineElement(CoroutineElement _target)
    {
        m_CoroutineElementList.Remove(_target);
    }

    public void RegisterCoroutineHandler()
    {
        m_CoroutineElementList.Clear();
    }
    public void UnregisterCoroutineHandler()
    {
        for (int i = 0; i < m_CoroutineElementList.Count; i++)
        {
            m_CoroutineElementList[i].StopCoroutine_Element();
        }
    }
}