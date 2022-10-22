using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class TriggerOpenDoor : MonoBehaviour
{
    // Visible Member Variables
    public BoxCollider2D p_DoorCollider;
    public Animator p_Animator;
    
    
    // Member Variables
    private bool isOpen = false;
    private Coroutine m_DoorCoroutine = null;
    private readonly int Open = Animator.StringToHash("Open");


    // Constructors
    
    
    // Functions
    public void SetDoorOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        if (!ReferenceEquals(m_DoorCoroutine, null))
        {
            StopCoroutine(m_DoorCoroutine);
            m_DoorCoroutine = null;
        }

        m_DoorCoroutine = StartCoroutine(DoorAnimCheck());
    }

    private IEnumerator DoorAnimCheck()
    {
        while (true)
        {
            yield return null;
            if (p_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                p_Animator.SetBool(Open, isOpen);
                break;
            }
        }

        p_DoorCollider.enabled = !isOpen;
        
        yield break;
    }
    
}