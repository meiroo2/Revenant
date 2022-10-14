using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

public class HideSlot : MonoBehaviour
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.HIDEPOS;
    public bool m_isOn { get; private set; } = false;
    public bool m_isBooked = false;

    // Member Variables
    [HideInInspector] public HideObj m_HideObj;
    [Sirenix.OdinInspector.ReadOnly] public bool m_isLeftSlot = false;


    // Constructors


    // Updates


    // Physics


    // Functions
    public void ActivateHideSlot(bool _true)
    {
        m_isOn = _true;
        m_HideObj.UpdateHideSlotInfo();
    }

    public HideSlot GetOtherSideSlot()
    {
        return m_isLeftSlot ? m_HideObj.p_RSlot : m_HideObj.p_LSlot;
    }
}
