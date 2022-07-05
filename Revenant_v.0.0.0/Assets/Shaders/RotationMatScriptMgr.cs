using System;
using UnityEngine;


public class RotationMatScriptMgr : MonoBehaviour
{
    // Member Variables
    private RotationMatScript[] m_RotaionMatScripts;
    
    
    // Constructors
    private void Awake()
    {
        m_RotaionMatScripts = GetComponentsInChildren<RotationMatScript>();
    }
    
    
    // Functions
    public void FlipAllNormalsToRight(bool _isTrue)
    {
        foreach (var ele in m_RotaionMatScripts)
        {
            ele.SetNormalToRight(_isTrue);
        }
    }
}