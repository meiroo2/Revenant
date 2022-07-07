using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI : MonoBehaviour
{
    // Member Variables
    public ScreenEffect_UI m_ScreenEffectUI { get; private set; }
    
    
    // Constructors
    private void Awake()
    {
        m_ScreenEffectUI = GetComponentInChildren<ScreenEffect_UI>();
    }
}