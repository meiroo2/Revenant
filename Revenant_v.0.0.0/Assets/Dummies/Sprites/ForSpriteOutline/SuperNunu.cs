using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNunu : MonoBehaviour
{
    public int isSuper = 0;
    public SpriteRenderer m_Renderer;
    public Vector2 m_OriginPos;
    public Vector2 m_OriginScale;

    private float m_Timer = 0.5f;

    public float m_Speed = 1f;

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();

        m_OriginPos = transform.localPosition;
        m_OriginScale = transform.localScale;

        m_Renderer.enabled = false;
    }
    private void Update()
    {
        if (isSuper == 0)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                isSuper = 1;
                transform.localPosition = m_OriginPos;
                transform.localScale = m_OriginScale;
                m_Renderer.enabled = true;
                m_Timer = 0.5f;
            }
        }

        if (isSuper == 1)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, Time.deltaTime * m_Speed);
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1f, 1f), Time.deltaTime * m_Speed);
            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0f)
            {
                isSuper = 0;
                m_Renderer.enabled = false;
                transform.localPosition = m_OriginPos;
                transform.localScale = m_OriginScale;
            }
        }
    }
}
