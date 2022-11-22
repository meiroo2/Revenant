using System;
using System.Collections;
using UnityEngine;


public class Subway_SubwayMove : MonoBehaviour
{
    public Transform p_MoveDestTransform;
    public float p_Speed = 1f;

    private float m_LerpVal = 0f;
    private Vector2 m_StartPos;
    private Vector2 m_EndPos;

    private Coroutine m_MoveCoroutine = null;
    
    private void Awake()
    {
        m_StartPos = transform.position;
        m_EndPos = m_StartPos;
        m_EndPos.x = p_MoveDestTransform.position.x;
    }

    public void MoveToDest()
    {
        if (!ReferenceEquals(m_MoveCoroutine, null))
        {
            StopCoroutine(m_MoveCoroutine);
        }

        m_MoveCoroutine = StartCoroutine(MoveEnumerator());
    }

    private IEnumerator MoveEnumerator()
    {
        while (true)
        {
            transform.position = Vector2.Lerp(m_StartPos, m_EndPos, m_LerpVal);
            m_LerpVal += Time.deltaTime * p_Speed;

            if (m_LerpVal >= 1f)
            {
                m_LerpVal = 1f;
                transform.position = Vector2.Lerp(m_StartPos, m_EndPos, m_LerpVal);
                break;
            }
            yield return null;
        }
    }
}