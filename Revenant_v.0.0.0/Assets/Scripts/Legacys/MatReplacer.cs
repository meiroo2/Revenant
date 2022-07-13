using System;
using System.Collections.Generic;
using UnityEngine;


public class MatReplacer : MonoBehaviour
{
    // Visible Member Variables
    public Material p_MatForReplace;
    
    
    // Member Variables
    protected SpriteRenderer[] m_Renderers;
    protected Material m_OriginalMat;
    
    
    // Constructors
    protected void Awake()
    {
        m_Renderers = GetComponentsInChildren<SpriteRenderer>();
        m_OriginalMat = m_Renderers[0].material;
        
        CheckError();
    }
    
    
    // Functions
    public void ChangeMat(bool _toOrigin)
    {
        Material replaceMat = _toOrigin ? m_OriginalMat : p_MatForReplace;
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            m_Renderers[i].material = replaceMat;
        }
    }

    private void CheckError()
    {
        if(m_Renderers.Length <= 0)
            Debug.Log("MatReplacer Arr length below 0 error");
        
        if(p_MatForReplace == null)
            Debug.Log("MatReplacer Assign error");
    }
}