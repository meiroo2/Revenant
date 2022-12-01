using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Breakable_BreakBox : Breakable
{
    [SerializeField]
    private Object DestructionRefObject;

    public override void GetHit(int _damage)
    {
        if (Health <= 0)
            return;
        
        Health -= _damage;
        
        if (Health == 10)
        {
            m_Animator.SetInteger("Hp", Health);
        }
        else if (Health == 0)
        {
            m_Animator.SetInteger("Hp", Health);
            DestructObject();
            m_HotBox.gameObject.SetActive(false);
            //m_Coroutine = StartCoroutine(CheckExplodeEnd());
            gameObject.SetActive(false);
        }
    }

    void DestructObject()
    {
        GameObject Destructable = (GameObject)Instantiate(DestructionRefObject);

        Destructable.transform.position = transform.position;
        Destructable.transform.localScale = transform.localScale;
    }
}
