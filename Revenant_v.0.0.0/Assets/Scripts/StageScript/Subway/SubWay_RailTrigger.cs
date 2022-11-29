using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;


public class SubWay_RailTrigger : MonoBehaviour
{
    public UnityEvent m_TriggerAction;

    [ReadOnly] public bool m_IsUsed = false;
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_IsUsed || !col.CompareTag("@Player"))
            return;

        m_IsUsed = true;
        m_TriggerAction?.Invoke();
    }
}