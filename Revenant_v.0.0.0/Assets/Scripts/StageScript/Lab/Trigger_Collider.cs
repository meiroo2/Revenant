using System;
using UnityEngine;


public class Trigger_Collider : MonoBehaviour
{
    // Visible Member Variables
    public Animator p_AnimatorToManipulate;
    public bool p_CanOpen = false;
    public bool p_CanClose = false;
   

    // Member Variables
    private bool m_IsUsed = false;
    private readonly int Open = Animator.StringToHash("Open");
    
    
    // Constructor
    private void Awake()
    {
        p_AnimatorToManipulate.SetInteger(Open, -1);
    }


    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_IsUsed)
            return;
        
        switch (p_CanOpen)
        {
            case true when p_CanClose:
                p_AnimatorToManipulate.SetInteger(Open, 1);
                break;
            
            case true when !p_CanClose:
                p_AnimatorToManipulate.SetInteger(Open, 1);
                m_IsUsed = true;
                break;
            
            case false when p_CanClose:
                p_AnimatorToManipulate.SetInteger(Open, 0);
                m_IsUsed = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (p_CanOpen)
        {
            case true when p_CanClose:
                p_AnimatorToManipulate.SetInteger(Open, 0);
                break;
            
            case true when !p_CanClose:
                break;
            
            case false when p_CanClose:
                break;
        }
    }
}