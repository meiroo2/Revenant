using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox_Troom2 : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_isOpen = false;
    private GameObject m_ProgressMgrObj;

    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_ProgressMgrObj = GameObject.FindGameObjectWithTag("ProgressMgr");
    }

    private void Update()
    {
        if (!m_isOpen)
        {
            if ( m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Gunbox_Giveani") &&m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                m_isOpen = true;
                m_ProgressMgrObj.SendMessage("NextProgress");
            }
        }
    }
}
