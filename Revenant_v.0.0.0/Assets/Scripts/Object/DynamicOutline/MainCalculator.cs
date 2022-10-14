using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class MainCalculator : MonoBehaviour
{
    private Transform m_PlayerTransform;
    private Vector2 m_PlayerVec;
    private Coroutine m_Coroutine;

    private List<DynamicOutline> m_List = new List<DynamicOutline>();
    private DynamicOutline[] m_Arr;

    private void Start()
    {
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer()
            .transform;

        m_Arr = new DynamicOutline[m_List.Count];
        for (int i = 0; i < m_Arr.Length; i++)
        {
            m_Arr[i] = m_List[i];
        }
        m_Coroutine = StartCoroutine(FixedCoroutine());
    }
    
    public void Add(DynamicOutline _element)
    {
        m_List.Add(_element);
    }

    private IEnumerator FixedCoroutine()
    {
        while (true)
        {
            m_PlayerVec = m_PlayerTransform.position;

            for (int i = 0; i < m_Arr.Length; i++)
            {
                //m_Arr[i].m_Distance = m_Arr[i].m_CenterPos.position.x - m_PlayerVec.x;
            }

            yield return null;
        }
    }
}