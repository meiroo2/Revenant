using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMoving : MonoBehaviour
{
    private Vector2 m_OriginVec;
    private Vector2 m_MoveVec;

    private float Timer = 1f;
    private bool m_isUp = true;

    private void Awake()
    {
        m_OriginVec = transform.position;
        m_MoveVec = new Vector2(m_OriginVec.x, m_OriginVec.y + 0.2f);
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0f)
        {
            if (m_isUp)
                m_isUp = false;
            else
                m_isUp = true;

            Timer = 1f;
        }
        else
        {
            if (m_isUp)
                transform.position = Vector2.Lerp(transform.position, m_MoveVec, Time.deltaTime);
            else
                transform.position = Vector2.Lerp(transform.position, m_OriginVec, Time.deltaTime);
        }
    }
}