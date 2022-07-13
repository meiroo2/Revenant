using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashpart : MonoBehaviour
{
    private Transform PlayerTransform;
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Animator.Play("DashPart",-1,0f);
            Vector2 pos = PlayerTransform.position;
            pos.y -= 0.4f;
            transform.position = pos;
        }
    }
}
