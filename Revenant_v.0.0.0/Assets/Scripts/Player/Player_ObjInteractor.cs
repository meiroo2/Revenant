using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_ObjInteractor : MonoBehaviour
{
    /*
    // Member Variables
    private List<Collider2D> m_HideSlotList;
    private Player_InputMgr m_InputMgr;
    private Player m_Player;
    
    private HideSlot m_CurHideSlot;
    private bool m_IsHide = false;


    // Constructors
    private void Awake()
    {
        m_HideSlotList = new List<Collider2D>();
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_InputMgr = m_Player.m_InputMgr;
    }


    // Updates


    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("HideSlot"))
        {
            m_HideSlotList.Add(col);
            
            HighlightbyDistance(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideSlot"))
        {
            m_HideSlotList.Remove(other);

            if (m_HideSlotList.Count > 0)
                HighlightbyDistance(true);
            else
                m_CurHideSlot = null;
        }
    }
    
    
    // Functions
    public int DoHide(bool _doHide)
    {
        if (_doHide)
        {
            if (m_IsHide || m_HideSlotList.Count <= 0)
                return 0;

            m_IsHide = true;
            m_HideSlotList[GetNearestHideSlotIdx()].TryGetComponent(out HideSlot slot);
            m_CurHideSlot = slot;
            m_CurHideSlot.ActivateHideSlot(true);
            return 1;
        }
        else
        {
            if (!m_IsHide)
                return 0;
            
            m_IsHide = false;
            m_CurHideSlot.ActivateHideSlot(false);
            m_CurHideSlot = null;
            return 1;
        }
    }
    public void ForceExitFromHideSlot()
    {
        if (!m_IsHide)
            return;
        
        m_CurHideSlot.ActivateHideSlot(false);
        m_CurHideSlot = null;
        m_IsHide = false;
    }

    /// <summary>
    /// 가장 가까운 Slot Idx을 리턴합니다.
    /// </summary>
    /// <returns>Nearest Slot Idx</returns>
    private int GetNearestHideSlotIdx()
    {
        float nearestDistance = 9999999999f;
        int nearestIdx = 0;

        for (int i = 0; i < m_HideSlotList.Count; i++)
        {
            float distanceBetSlot = Vector2.Distance(m_Player.GetPlayerCenterPos(), 
                m_HideSlotList[i].transform.position);

            if (distanceBetSlot < nearestDistance)
            {
                nearestDistance = distanceBetSlot;
                nearestIdx = i;
            }
        }

        return nearestIdx;
    }
    
    private void HighlightbyDistance(bool _isTrue)
    {
        if (m_HideSlotList.Count <= 0)
            return;
        
        Vector2 playerPos = transform.position;

        int minIdx = 0;
        float minDistance = 999999999f;
        
        foreach (var element in m_HideSlotList)
        {
            float distance = (playerPos - (Vector2)element.transform.position).sqrMagnitude;
            if(distance < minDistance)
            {
                minIdx = m_HideSlotList.IndexOf(element);
                minDistance = distance;
            }
        }

        m_CurHideSlot = m_HideSlotList[minIdx].GetComponent<HideSlot>();
    }
    */
}