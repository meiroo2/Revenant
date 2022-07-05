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
        m_HideObj.m_OutlineScript.outlineSize = _isOn ? 1 : 0;
    }
    public int ActivateHideSlot(bool _true)
    {
        if (_true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
        
        
    }

    public bool GetCanHide()
    {
        return true;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
