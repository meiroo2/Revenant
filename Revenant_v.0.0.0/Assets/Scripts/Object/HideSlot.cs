using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSlot : MonoBehaviour, IUseableObj
{
    // Visible Member Variables
    public UseableObjList m_ObjProperty { get; set; } = UseableObjList.HIDEPOS;
    public bool m_isOn { get; set; } = false;

    // Member Variables
    public HideObj m_HideObj { get; set; }
    public bool m_isLeftSlot { get; set; } = true;
    private int m_HideObjID = 0;    // 슬롯 주인 판별용고유 ID

    // Constructors


    // Updates


    // Physics


    // Functions
    public int useObj(IUseableObjParam _param)
    {
        if (!m_isOn)  // 빈 슬롯일 경우
        {
            m_isOn = true;
            m_HideObj.getHideSlotInfo(true, m_isLeftSlot);
            m_HideObjID = _param.m_ObjInstanceNum;
            return 1;
        }
        else
        {
            // 숨기 해제 시도가 본인일 경우
            if(_param.m_ObjInstanceNum == m_HideObjID)
            {
                m_isOn = false;
                m_HideObj.getHideSlotInfo(false, m_isLeftSlot);
                return 2;
            }
        }

        return 0;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
