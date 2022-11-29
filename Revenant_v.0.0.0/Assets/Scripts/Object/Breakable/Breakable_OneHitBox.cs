using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_OneHitBox : Breakable
{
    public override void GetHit(int _damage)
    {
        if (Health <= 0)
            return;
        
        Health -= _damage;
        
        if (Health <= 0)
        {
            m_Animator.SetTrigger("Break");
            m_HotBox.gameObject.SetActive(false);
        }
    }
}
