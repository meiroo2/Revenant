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
    public void ActivateOutline(bool _isOn)
    {
        m_HideObj.p_Outline.outlineSize = _isOn ? 1 : 0;
    }
    public void ActivateHideSlot(bool _true)
    {
        m_isOn = _true;
        m_HideObj.UpdateHideSlotInfo();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
