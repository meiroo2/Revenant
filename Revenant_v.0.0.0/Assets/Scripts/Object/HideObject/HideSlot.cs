using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSlot : MonoBehaviour
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.HIDEPOS;
    public bool m_isOn { get; set; } = false;
    

    // Member Variables
    public HideObj m_HideObj { get; set; }
    public bool m_isLeftSlot { get; set; } = true;

    // Constructors


    // Updates


    // Physics


    // Functions
    public void ActivateHideSlot(bool _true)
    {
        m_isOn = _true;
        m_HideObj.UpdateHideSlotInfo();
    }
}
