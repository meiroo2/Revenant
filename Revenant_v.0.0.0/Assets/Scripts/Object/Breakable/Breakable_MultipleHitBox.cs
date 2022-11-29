using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_MultipleHitBox : Breakable
{
    public override void GetHit(int _damage)
    {
        if (Hp <= 0)
            return;
        
        Hp -= _damage;
        
        if (Hp == 20)
        {
            //m_Animator.SetTrigger("Explode");
            Debug.Log("맞음 ==== 체력 20");
        }
        else if (Hp == 10)
        {
            //m_Animator.SetTrigger("Explode");
            Debug.Log("맞음 == 체력 10");
        }
        else if (Hp == 0)
        {
            m_Animator.SetTrigger("Break");
            m_HotBox.gameObject.SetActive(false);
            m_Coroutine = StartCoroutine(CheckExplodeEnd());
        }
    }
}
