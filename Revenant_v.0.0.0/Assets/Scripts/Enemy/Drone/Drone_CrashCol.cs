using System;
using UnityEngine;


public class Drone_CrashCol : MonoBehaviour
{
    // Member Variables
    public delegate void DroneCrashColDelegate();
    private DroneCrashColDelegate m_Callback = null;
    
    private CircleCollider2D m_CrashCol;
    
    // Constructors
    private void Awake()
    {
        m_CrashCol = GetComponent<CircleCollider2D>();
        m_CrashCol.enabled = false;
    }
    
    // Functions
    public void SetCrashCol(bool _on)
    {
        m_CrashCol.enabled = _on;
    }
    public void SetCallback(DroneCrashColDelegate _input, bool _doReset = false)
    {
        if (_doReset)
            m_Callback = null;

        m_Callback += _input;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_Callback is null)
            return;

        m_Callback();
        m_Callback = null;
    }
}