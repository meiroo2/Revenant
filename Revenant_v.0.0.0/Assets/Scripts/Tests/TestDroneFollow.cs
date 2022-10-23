using System;
using UnityEngine;


public class TestDroneFollow : MonoBehaviour
{
    public Transform p_PlayerTransform;
    public Vector2 p_GapVec;

    public Transform p_DroneTransform;
    public SpriteRenderer p_VidRenderer;

    private bool m_IsTriggered = false;

    private Vector2 m_StuckPos;
    private Color m_Color = Color.white;

    private void Awake()
    {
        m_StuckPos = p_DroneTransform.position;
        
        m_Color.a = 0f;
        p_VidRenderer.color = m_Color;
    }

    private void Update()
    {
        if (m_IsTriggered)
        {
            m_Color.a += Time.deltaTime * 3f;
            p_VidRenderer.color = m_Color;

            p_DroneTransform.position = Vector2.Lerp(p_DroneTransform.position,
                m_StuckPos, Time.deltaTime * 5f);
        }
        else
        {
            m_Color.a -= Time.deltaTime * 3f;
            p_VidRenderer.color = m_Color;
            
            p_DroneTransform.position = Vector2.Lerp(p_DroneTransform.position,
                new Vector2(p_PlayerTransform.position.x + p_GapVec.x, p_PlayerTransform.position.y + p_GapVec.y), 
                Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        m_Color.a = 0f;
        m_IsTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_Color.a = 1f;
        m_IsTriggered = false;
    }
}