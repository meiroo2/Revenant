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
    private UnityEvent m_Event;

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
        m_Event = null;
        if (!ReferenceEquals(m_Coroutine, null))
        {
            StopCoroutine(m_Coroutine);
        }
    }
    public void InitParticle(Transform _transform, float _speed, UnityAction _action)
    {
        m_Trail.time = 0f;
        
        m_DestinationTransform = _transform;
        m_Speed = _speed;

        if (!ReferenceEquals(_action, null))
            m_Event.AddListener(_action);

        m_Rigid.AddForce(StaticMethods.GetRotatedVec(Vector2.up, Random.Range(0f, 360f)) * 0.3f, ForceMode2D.Impulse);
        m_Coroutine = StartCoroutine(Waiting());
    }
    
    
    // Updates



    // Functions
    private IEnumerator Waiting()
    {
        float Timer = 0f;
        while (true)
        {
            yield return null;
            Timer += Time.deltaTime;

            transform.localScale = new Vector3(transform.localScale.x + Timer * 0.02f, transform.localScale.y + Timer * 0.02f,
                transform.localScale.z);
            
            if (Timer >= 1f)
                break;
        }

        m_Rigid.isKinematic = false;
        
        float duration = m_Speed * 0.1f;
        float time = 0.0f;
        Vector3 start = transform.position;
        Vector3 end = m_DestinationTransform.position;

        while(time < duration)
        {
            m_Trail.time += Time.deltaTime;
            end = m_DestinationTransform.position;
            time += Time.deltaTime;
            float linearT = time / duration;
            float heightT = m_ParticleMgr.p_Curve.Evaluate(linearT);

            float height = Mathf.Lerp(0.0f, 1f, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0.0f, height);

            yield return null;
        }
        if (!ReferenceEquals(m_Event, null))
            m_Event.Invoke();
            
        gameObject.SetActive(false);
        /*
        m_Rigid.AddForce(StaticMethods.GetRotatedVec(Vector2.up, Random.Range(-10f, 10f)) * 1f, ForceMode2D.Impulse);
        
        float distance;
        while (true)
        {
            yield return null;
            m_Trail.time += Time.deltaTime;
            Timer -= Time.deltaTime;

            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(transform.localScale.x - Timer * 2f,
                    transform.localScale.y - Timer * 2f,
                    transform.localScale.z);

            distance = (transform.position - m_DestinationTransform.position).sqrMagnitude;
           
            switch (distance)
            {
                case <= 0.03f:
                    if (!ReferenceEquals(m_Event, null))
                        m_Event.Invoke();
            
                    gameObject.SetActive(false);
                    break;
                
                case <= 0.2f:
                    m_Rigid.velocity = (m_DestinationTransform.position -transform.position ) * m_Speed;
                    break;
                
                default:
                    m_Rigid.AddForce((m_DestinationTransform.position - transform.position).normalized * 2f);
                    break;
            }
            
        }
        */
    }
}
