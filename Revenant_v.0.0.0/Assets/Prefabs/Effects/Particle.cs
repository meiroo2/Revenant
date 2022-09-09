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
    
    public void InitParticle(Transform _transform, float _speed, Action _action)
    {
        m_Trail.time = 0f;
        
        m_DestinationTransform = _transform;
        m_Speed = _speed;

        m_Action = _action;

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

        m_Action?.Invoke();
        
        gameObject.SetActive(false);
    }
}
