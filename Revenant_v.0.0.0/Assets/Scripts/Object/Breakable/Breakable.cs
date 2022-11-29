using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int Health = 10;
    
    protected Breakable_Hotbox m_HotBox;
    protected Animator m_Animator;

    private void Awake()
    {
        m_HotBox = GetComponentInChildren<Breakable_Hotbox>();
        m_Animator = GetComponentInChildren<Animator>();

        m_HotBox.Breakable = this;
    }
    
    public virtual void GetHit(int _damage) { }
}
