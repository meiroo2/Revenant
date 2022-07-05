using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class LocationSensor : MonoBehaviour
{
    // Visible Member Variables
    public bool m_isPlayers { get; set; }
    //public LocationInfo p_Location; 
    

    // Member Variables
    private LocationMgr m_LocationMgr;
    
    private Human m_ParentHuman;

    private List<GameObject> m_CurLocationInfos = new List<GameObject>();   // 현재 접촉 중인 Location Collider List
    private List<GameObject> m_TempLocations = new List<GameObject>();      // 함수 내부에서 사용할 Temp List


    // Constructors
    private void Awake()
    {
        m_ParentHuman = GetComponentInParent<Human>();

        if (GetComponentInParent<Player>())
            m_isPlayers = true;

    }
    private void Start()
    {
       m_LocationMgr  = InstanceMgr.GetInstance().GetComponentInChildren<LocationMgr>();
    }


    // Updates


    // Functions
    public LocationInfo GetLocation()
    {
        m_TempLocations.Clear();
        if (m_isPlayers)
        {
            switch (m_ParentHuman.gameObject.layer)
            {
                case 12:    // 계단 밖
                    foreach (var obj in m_CurLocationInfos.Where(obj => obj.CompareTag("RoomLocation")))
                    {
                        m_TempLocations.Add(obj);
                    }
                    break;
                
                case 10:    // 계단 안
                    foreach (var obj in m_CurLocationInfos.Where(obj => obj.CompareTag("StairLocation")))
                    {
                        m_TempLocations.Add(obj);
                    }
                    break;
            }
        }
        else
        {
            switch (m_ParentHuman.gameObject.layer)
            {
                case 11:    // 계단 밖
                    foreach (var obj in m_CurLocationInfos.Where(obj => obj.CompareTag("RoomLocation")))
                    {
                        m_TempLocations.Add(obj);
                    }
                    break;
                
                case 9:     // 계단 안
                    foreach (var obj in m_CurLocationInfos.Where(obj => obj.CompareTag("StairLocation")))
                    {
                        m_TempLocations.Add(obj);
                    }
                    break;
            }
        }

        return m_TempLocations.Count switch
        {
            > 1 => m_TempLocations[^1].GetComponent<LocationInfo>(),    // 2개 이상 접촉의 경우 가장 마지막에 들어온 것 반환
            1 => m_TempLocations[0].GetComponent<LocationInfo>(),       // 1개 접촉의 경우 그냥 0번째 반환
            _ => m_LocationMgr.GetDefaultLocation()                     // 에러 검출용 null 반환
        };
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!(col.CompareTag("RoomLocation") || col.CompareTag("StairLocation")))
            return;
        
        m_CurLocationInfos.Add(col.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.CompareTag("RoomLocation") || other.CompareTag("StairLocation")))
            return;
        
        m_CurLocationInfos.Remove(other.gameObject);
    }
}