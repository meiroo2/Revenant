using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TimeSliceMgr : MonoBehaviour
{
    // Visible Member Variables
    public GameObject p_TimeSlicePrefab;

    // Member Variables
    private List<TimeSliceObj> m_TimeSliceObjList = new List<TimeSliceObj>();
    private int m_PoolLength = 5;
    private int m_Idx = 0;
    
    // Constructors
    private void Awake()
    {
        for (int i = 0; i < m_PoolLength; i++)
        {
            var go = Instantiate(p_TimeSlicePrefab, transform);
            go.transform.position = Vector2.zero;
            m_TimeSliceObjList.Add(go.GetComponent<TimeSliceObj>());
            go.SetActive(false);
        }
    }

    private void Start()
    {
        Player player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        foreach (var VARIABLE in m_TimeSliceObjList)
        {
            VARIABLE.p_TimeSliceMgr = this;
            VARIABLE.m_Player = player;
            VARIABLE.gameObject.SetActive(false);
        }
    }

    public TimeSliceObj SpawnTimeSlice(float _moveSpeed, float _colorSpeed, float _angle, float _remainTime)
    {
        TimeSliceObj returnObj = m_TimeSliceObjList[m_Idx];
        returnObj.gameObject.SetActive(true);
        returnObj.transform.parent = null;
        returnObj.ResetTimeSliceObj(_moveSpeed, _colorSpeed, _angle, _remainTime);

        m_Idx++;
        if (m_Idx >= m_PoolLength)
        {
            m_Idx = 0;
        }
        
        return returnObj;
    }
}