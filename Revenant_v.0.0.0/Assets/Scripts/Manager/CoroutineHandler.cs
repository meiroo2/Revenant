using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Mono가 아닌 객체에서 Coroutine을 사용하기 위한 클래스입니다.
/// </summary>
public class CoroutineHandler : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_CoroutineElement;
    
    
    // Member Variables
    private List<CoroutineElement> m_CoroutineElementList = new List<CoroutineElement>();


    // Functions
    
    /// <summary>
    /// Handler가 인자로 받은 IEnumerator를 인스턴스화한 객체에 넣고 실행합니다.
    /// </summary>
    /// <param name="_enumerator"> 실행할 IEnumerator </param>
    /// <returns> 이 CoroutineElement를 받아서 수동 정지해야 함. </returns>
    public CoroutineElement StartCoroutine_Handler(IEnumerator _enumerator)
    {
        var element = Instantiate(p_CoroutineElement, transform).GetComponent<CoroutineElement>();
        element.m_Handler = this;
        m_CoroutineElementList.Add(element);
        
        return element.StartCoroutine_Element(_enumerator);
    }

    /// <summary>
    /// 인스턴스화 되어진 해당 CoroutineElement를 삭제합니다.
    /// </summary>
    /// <param name="_target"> 삭제할 대상 </param>
    public void DeleteCoroutineElement(CoroutineElement _target)
    {
        if (!m_CoroutineElementList.Contains(_target)) 
            return;
        
        m_CoroutineElementList.Remove(_target);
        Destroy(_target.gameObject);
        _target = null;
    }

    /// <summary>
    /// 신 리로드시 생성된 모든 CoroutineElement를 제거시킵니다.
    /// </summary>
    public void ResetCoroutineHandler()
    {
        var arr = GameObject.FindObjectsOfType<CoroutineElement>();
        for (int i = 0; i < arr.Length; i++)
        {
            Destroy(arr[i].gameObject);
        }

        m_CoroutineElementList.Clear();
        m_CoroutineElementList.TrimExcess();
    }
}