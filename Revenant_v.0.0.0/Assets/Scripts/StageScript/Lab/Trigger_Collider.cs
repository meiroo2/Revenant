using System;
using System.Collections.Generic;
using UnityEngine;


public class Trigger_Collider : MonoBehaviour
{
    // Visible Member Variables
    public Animator p_AnimatorToManipulate;
    public bool p_CanOpen = false;
    public bool p_CanClose = false;
    public bool p_InitOpen = true;
    public List<GameObject> p_AfterActiveObjList = new List<GameObject>();
    
    // Member Variables
    private bool m_IsUsed = false;
    private readonly int Open = Animator.StringToHash("Open");
    
    
    // Constructor
    private void Awake()
    {
        CheckObjList();
        SetActiveObjList(false);
        p_AnimatorToManipulate.SetInteger(Open, p_InitOpen ? 1 : 0);
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
                SetActiveObjList(true);
                m_IsUsed = true;
                break;
            
            case false when p_CanClose:
                p_AnimatorToManipulate.SetInteger(Open, 0);
                SetActiveObjList(true);
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
    
    // Functions
    private void CheckObjList()
    {
        foreach (var VARIABLE in p_AfterActiveObjList)
        {
            if(!VARIABLE)
                Debug.LogError("ERR : Trigger_Collider NULL");
        }
    }
    
    public void SetActiveObjList(bool _toActive)
    {
        foreach (var VARIABLE in p_AfterActiveObjList)
        {
            VARIABLE.SetActive(_toActive);
        }
    }
}