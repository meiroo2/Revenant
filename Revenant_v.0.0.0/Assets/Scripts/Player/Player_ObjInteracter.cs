using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_ObjInteracter : MonoBehaviour
{
    // Member Variables
    private List<Collider2D> m_HideSlotList;
    private Player_InputMgr m_InputMgr;
    private Player m_Player;
    
    private HideSlot m_CurHideSlot;


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
    private void Update()
    {
        // 3. 키다운 or 키업 처리
        if (m_InputMgr.m_IsPushHideKey && m_Player.m_CanHide && !ReferenceEquals(m_CurHideSlot, null))
        {
            m_CurHideSlot.ActivateHideSlot(true);
            m_Player.ChangePlayerFSM(PlayerStateName.HIDDEN);
        }
        else if(!m_InputMgr.m_IsPushHideKey && m_Player.m_CurPlayerFSMName == PlayerStateName.HIDDEN)
        {
            m_CurHideSlot.ActivateHideSlot(false);
            m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
        }
    }

    
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
            //m_HideSlotList.Find(x => x == other).GetComponent<IObjHighlight>().ActivateOutline(false);
            m_HideSlotList.Remove(other);

            if (m_HideSlotList.Count > 0)
                HighlightbyDistance(true);
            else
                m_CurHideSlot = null;
        }
    }
    
    
    // Functions
    public void ForceExitFromHideSlot()
    {
        if (ReferenceEquals(m_CurHideSlot, null))
            return;
        
        m_CurHideSlot.ActivateHideSlot(false);
        m_Player.ChangePlayerFSM(PlayerStateName.IDLE);
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
            
           // element.GetComponent<IObjHighlight>().ActivateOutline(false);
        }

        m_CurHideSlot = m_HideSlotList[minIdx].GetComponent<HideSlot>();
        //m_HideSlotList[minIdx].GetComponent<IObjHighlight>().ActivateOutline(true);
    }
}