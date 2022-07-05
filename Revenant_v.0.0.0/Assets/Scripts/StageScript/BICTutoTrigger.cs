using System;
using UnityEngine;


public class BICTutoTrigger : MonoBehaviour
{
    // Visible Member Variables
    public string p_DebugTriggerString;
    
    // Member Variables
    private BICTutoMgr m_BICTutoMgr;
    private bool m_Triggered = false;
    
    
    // Constructors
    private void Awake()
    {
        m_BICTutoMgr = FindObjectOfType<BICTutoMgr>();
    }
    
    
    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_Triggered)
            return;

        if (col.CompareTag("Player"))
        {
            if (p_DebugTriggerString.Length > 0)
                Debug.Log(p_DebugTriggerString);

            m_Triggered = true;
            m_BICTutoMgr.NextPhase();
        }
    }
}