using System;
using System.Collections;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public Rigidbody2D m_Rigid;
    public Collider2D m_Collider;

    private Coroutine m_BallCoroutine = null;
    
    private void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();

        m_Collider.enabled = false;
    }

    public void InitBall(Vector2 _position, float _deadTime)
    {
        if (!ReferenceEquals(m_BallCoroutine, null))
        {
            StopCoroutine(m_BallCoroutine);
            m_BallCoroutine = null;
        }

        transform.position = _position;
        m_Collider.enabled = true;
        m_Rigid.velocity = Vector2.zero;
        m_BallCoroutine = StartCoroutine(WaitForDead(_deadTime));
    }

    private IEnumerator WaitForDead(float _deadTime)
    {
        yield return new WaitForSecondsRealtime(_deadTime);
        m_Collider.enabled = false;
    }
}