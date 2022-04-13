using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1_PillarDoor : MonoBehaviour, IDirect
{
    public int m_Idx { get; set; } = 0;
    private BoxCollider2D m_GateBoxCol;
    private Animator m_GateAni;

    private void Awake()
    {
        m_GateBoxCol = GetComponent<BoxCollider2D>();
        m_GateAni = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(m_Idx == 1)
        {
            if(m_GateAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                m_GateBoxCol.enabled = false;
                m_Idx++;
            }
        }
    }

    public int NextDirect()
    {
        m_GateAni.SetBool("isOpen", true);
        m_Idx++;
        return 0;
    }
}