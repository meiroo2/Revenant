using System;
using UnityEngine;
using UnityEngine.Events;


public class ActionInputPlayerTrigger : MonoBehaviour
{
    public UnityEvent m_Action;

    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Action?.Invoke();
    }
}