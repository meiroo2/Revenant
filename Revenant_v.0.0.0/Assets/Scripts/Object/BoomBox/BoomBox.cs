using System;
using System.Collections;
using UnityEngine;


public class BoomBox : MonoBehaviour
{
    // Visible Member Variables
    public int p_Hp = 30;
    public int p_Damage = 10;
    public int p_StunValue = 10;
    
    
    // Member Variables
    private BoomBox_HotBox m_HotBox;
    private BoomBox_Bomb m_Bomb;
    private Animator m_Animator;
    
    // Constructors
    private void Awake()
    {
        m_HotBox = GetComponentInChildren<BoomBox_HotBox>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Bomb = GetComponentInChildren<BoomBox_Bomb>();
    }
    
    
    // Functions
    public void GetHit(int _damage)
    {
        p_Hp -= _damage;
        if (p_Hp <= 0)
        {
            m_Bomb.Explode(p_Damage, p_StunValue);
            m_Animator.SetTrigger("Explode");
            m_HotBox.gameObject.SetActive(false);
            StartCoroutine(CheckExplodeEnd());
        }
    }

    private IEnumerator CheckExplodeEnd()
    {
        while (true)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                gameObject.SetActive(false);
            }
            yield return null;
        }

        yield break;
    }
}