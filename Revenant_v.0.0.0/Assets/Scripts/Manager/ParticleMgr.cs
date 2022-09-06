using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ParticleMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_ParticlePrefab;
    public int p_PullingLimit = 10;
    public AnimationCurve p_Curve;

    
    // Member Variables
    private List<Particle> m_PulledPaticleList;
    private int m_Idx = 0;
    
    
    // Constructors
    private void Awake()
    {
        m_PulledPaticleList = new List<Particle>();
        if (ReferenceEquals(p_ParticlePrefab, null))
        {
            Debug.Log("ERR : ParticleMgr에서 풀링할 오브젝트가 없습니다.");
        }

        for (int i = 0; i < p_PullingLimit; i++)
        {
            var instance = Instantiate(p_ParticlePrefab).GetComponent<Particle>();
            instance.gameObject.transform.parent = transform;
            m_PulledPaticleList.Add(instance);
            instance.gameObject.SetActive(false);
        }
    }

    // Functions
    public void MakeParticle(Vector2 _position, Transform _transform, float _speed, UnityAction _action = null)
    {
        var instance = m_PulledPaticleList[m_Idx];
        instance.gameObject.SetActive(false);
        instance.gameObject.SetActive(true);

        instance.transform.position = _position;
        instance.InitParticle(_transform, _speed, _action);
        
        m_Idx++;

        if (m_Idx >= p_PullingLimit)
            m_Idx = 0;
    }
}
