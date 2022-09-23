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
    
    public void InitParticle(Transform _transform, float _speed,float _waitTime, Action _action)
    {
        m_Trail.time = 0f;
        
        m_DestinationTransform = _transform;
        m_Speed = _speed;
        m_WaitTime = _waitTime;
        m_Action = _action;
        
        m_Coroutine = StartCoroutine(Following());
    }


    // Updates



    // Functions
    private IEnumerator Following()
    {
        Vector2 direction = (m_DestinationTransform.position - transform.position).normalized;
        yield return new WaitForSeconds(m_WaitTime);

        while (true)
        {
            direction = (m_DestinationTransform.position - transform.position).normalized;
            m_Trail.time += Time.deltaTime;
            
            transform.Translate(direction * m_Speed);

            if (Vector2.Distance(transform.position, 
                    m_DestinationTransform.position) < 0.1f)
                break;
            
            yield return new WaitForFixedUpdate();
        }

        m_Action?.Invoke();
        
        gameObject.SetActive(false);

        yield break;
    }
}
