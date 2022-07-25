using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class CoroutineHandler : MonoBehaviour
{
    // Member Variables
    public GameObject p_CoroutineElement;


    // Functions
    public CoroutineElement StartCoroutine_Handler(IEnumerator _enumerator)
    {
        var element = Instantiate(p_CoroutineElement, transform).GetComponent<CoroutineElement>();
        
        return element.StartCoroutine_Element(_enumerator);
    }
}