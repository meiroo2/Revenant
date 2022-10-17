using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Particle : MonoBehaviour
{
    // Member Variables
    private Rigidbody2D m_Rigid;
    private TrailRenderer m_Trail;

    private ParticleMgr m_ParticleMgr;
    private Transform m_DestinationTransform;
    private float m_Speed;
    private float m_WaitTime;

    private Action m_Action;

    private Vector2 m_Direction;
    private Coroutine m_Coroutine;
    
    private Vector2[] p_BezierPositions = new Vector2[4];
    private float m_BezierTime = 0f;    
    public float posA = 0.85f;
    public float posB = 0.75f;


    // Constructors
    private void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        m_ParticleMgr = GetComponentInParent<ParticleMgr>();
        
        
    }

    private void OnDisable()
    {
        m_Action = null;
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }
    }
    
    public void InitParticle(Transform _transform, float _speed, float _waitTime, Action _action)
    {
        m_Trail.time = 0f;
        
        m_DestinationTransform = _transform;
        m_Speed = _speed;
        m_WaitTime = _waitTime;
        m_Action = _action;

        m_BezierTime = 0f;
        p_BezierPositions[0] = transform.position;
        p_BezierPositions[1] = SetBezierPoint(transform.position);
        p_BezierPositions[2] = SetBezierPoint(m_DestinationTransform.position);
        p_BezierPositions[3] = m_DestinationTransform.position;

        if(!ReferenceEquals(m_Coroutine, null))
            StopCoroutine(m_Coroutine);
        m_Coroutine = StartCoroutine(Following());
    }


    // Updates



    // Functions

    private float FourPointBezier(float _a, float _b, float _c, float _d) {
        return Mathf.Pow((1 - m_BezierTime), 3) * _a
               + Mathf.Pow((1 - m_BezierTime), 2) * 3 * m_BezierTime * _b
               + Mathf.Pow(m_BezierTime, 2) * 3 * (1 - m_BezierTime) * _c
               + Mathf.Pow(m_BezierTime, 3) * _d;
    }

    
    private Vector2 GetNewBezierPos()
    {
        return new Vector2(
            FourPointBezier(p_BezierPositions[0].x, p_BezierPositions[1].x, p_BezierPositions[2].x,
                p_BezierPositions[3].x),
            FourPointBezier(p_BezierPositions[0].y, p_BezierPositions[1].y, p_BezierPositions[2].y,
                p_BezierPositions[3].y)
        );
    }


    private Vector2 SetBezierPoint(Vector2 origin)
    {
        Vector2 returnPos;
        returnPos.x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        returnPos.y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
        return returnPos;
    }
    
    
    
    private IEnumerator Following()
    {
        yield return new WaitForSeconds(m_WaitTime);

        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            
            m_Trail.time += time;
            
            m_BezierTime += time * m_Speed;
            // Update Target Pos
            p_BezierPositions[3] = m_DestinationTransform.position;
            transform.position = GetNewBezierPos();

            if (m_BezierTime >= 1f)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        m_Action?.Invoke();
        
        gameObject.SetActive(false);

        yield break;
    }
}
