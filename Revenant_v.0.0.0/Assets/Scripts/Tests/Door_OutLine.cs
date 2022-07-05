using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door_OutLine : MonoBehaviour
{
    public float distance = 0f;
    private Transform m_PlayerTransform;
    private SpriteOutline m_Outline;

    private void Start()
    {
        m_Outline = GetComponent<SpriteOutline>();
        m_PlayerTransform = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.transform;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(m_PlayerTransform.position, transform.position) <= distance)
        {
            m_Outline.outlineSize = 2;
        }
        else
        {
            m_Outline.outlineSize = 0;
        }
    }
}