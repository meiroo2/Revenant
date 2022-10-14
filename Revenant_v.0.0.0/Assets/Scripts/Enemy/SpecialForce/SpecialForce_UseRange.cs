using System;
using System.Collections.Generic;
using UnityEngine;


public class SpecialForce_UseRange : MonoBehaviour
{
    // Member Variables
    private SpecialForce m_Enemy;
    private Transform m_PlayerTransform;
    private Dictionary<Collider2D, HideSlot> m_HideSlotDic = new Dictionary<Collider2D, HideSlot>();

    private HideSlot m_NullSlot = null;

    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<SpecialForce>();
    }

    private void Start()
    {
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().p_CenterTransform;
    }


    // Functions
    public HideSlot GetProperSlot()
    {
        if (m_HideSlotDic.Count <= 0)
            return m_NullSlot;

        foreach (var COLLECTION in m_HideSlotDic)
        {
            if (IsThisSlotProperToHide(COLLECTION.Value))
            {
                return COLLECTION.Value;
            }
        }

        return m_NullSlot;
    }

    private bool IsThisSlotProperToHide(HideSlot _slot)
    {
        if (_slot.m_isBooked)
            return false;
        
        var otherSlot = _slot.GetOtherSideSlot();
        
        // 적이 플레이어보다 더 왼쪽
        if (m_Enemy.GetIsLeftThenPlayer())
        {
            // 반대쪽 슬롯보다 파라미터 슬롯이 더 왼쪽에 있어야
            return _slot.transform.position.x < otherSlot.transform.position.x;
        }
        else
        {   // 적이 플레이어보다 더 오른쪽
            
            // 반대쪽 슬롯보다 파라미터 슬롯이 더 오른쪽에 있어야
            return _slot.transform.position.x > otherSlot.transform.position.x;
        }
    }
    
    public bool GetIsThereAnySlot()
    {
        return m_HideSlotDic.Count > 0;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("HideSlot"))
            return;
        
        if (col.TryGetComponent(out HideSlot slot))
        {
            m_HideSlotDic.Add(col, slot);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("HideSlot"))
            return;

        m_HideSlotDic.Remove(other);
    }
}