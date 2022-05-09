using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private HBox_Door m_DoorCollision;
    private Animator m_DoorAnimator;
    private bool m_isOpen = false;

    private void Awake()
    {
        m_DoorAnimator = GetComponentInChildren<Animator>();
        m_DoorCollision = GetComponentInChildren<HBox_Door>();
    }

    public void useDoor()
    {
        if(m_DoorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (m_isOpen)
            {
                m_isOpen = false;
                m_DoorCollision.EnableCol(true);
                m_DoorAnimator.SetInteger("isOpen", 0);
            }
            else
            {
                m_isOpen = true;
                m_DoorCollision.EnableCol(false);
                m_DoorAnimator.SetInteger("isOpen", 1);
            }
        }
    }
}