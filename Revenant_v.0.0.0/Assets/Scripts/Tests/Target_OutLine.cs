using System;
using UnityEngine;


public class Target_OutLine : MonoBehaviour
{
    public Transform p_Head;
    public Transform p_Body;
    
    private Transform m_AimCursor;

    public SpriteOutline m_HeadOut;
    public SpriteOutline m_BodyOut;
    

    private void Start()
    {
        m_AimCursor = InstanceMgr.GetInstance().GetComponentInChildren<AimCursor>().transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_BodyOut.outlineSize = 0;
        m_HeadOut.outlineSize = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var position = m_AimCursor.position;
        float aimToHead = Vector2.Distance(position, p_Head.position);
        float aimToBody = Vector2.Distance(position, p_Body.position);
        
        if (aimToHead > aimToBody)
        {
            m_BodyOut.outlineSize = 2;
            m_HeadOut.outlineSize = 0;
        }
        else
        {
            m_BodyOut.outlineSize = 0;
            m_HeadOut.outlineSize = 2;
        }
    }
}