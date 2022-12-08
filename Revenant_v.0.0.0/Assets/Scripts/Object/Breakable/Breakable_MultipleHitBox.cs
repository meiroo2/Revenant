using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_MultipleHitBox : Breakable
{
    public override void GetHit(int _damage)
    {
        if (Health <= 0)
            return;
        
        Health -= _damage;
        
        if (Health == 20)
        {
            m_Animator.SetInteger("Hp", Health);
        }
        else if (Health == 10)
        {
            m_Animator.SetInteger("Hp", Health);
        }
        else if (Health == 0)
        {
            m_Animator.SetInteger("Hp", Health);
            m_HotBox.gameObject.SetActive(false);
        }
    }
}
