using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public HideSlot p_LSlot;
    public HideSlot p_RSlot;

    // Member Variables
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = false;
    private BoxCollider2D m_HitBox;
    private bool[] m_isSlotOn_States = new bool[2];
    private int m_SlotCount = 0;


    // Constructors
    private void Awake()
    {
        m_HitBox = GetComponent<BoxCollider2D>();
        m_HitBox.enabled = false;

        m_isSlotOn_States.Initialize();

        if (p_LSlot)
        {
            m_SlotCount++;
            p_LSlot.m_isLeftSlot = true;
            p_LSlot.m_HideObj = this;
        }
        if (p_RSlot)
        {
            m_SlotCount++;
            p_RSlot.m_isLeftSlot = false;
            p_RSlot.m_HideObj = this;
        }
    }


    // Updates


    // Physics


    // Functions
    public int HitHotBox(IHotBoxParam _param) 
    { 
        return 1; 
    }
    public void getHideSlotInfo(bool _isOn, bool _isLeftSlot)
    {
        if (_isLeftSlot)
            m_isSlotOn_States[0] = _isOn;
        else
            m_isSlotOn_States[1] = _isOn;

        if (m_isSlotFullOff() == true)
            m_HitBox.enabled = false;
        else
            m_HitBox.enabled = true;
    }
    private bool m_isSlotFullOff()
    {
        bool isFullOff = true;

        for(int i = 0; i < m_isSlotOn_States.Length; i++)
        {
            if(m_isSlotOn_States[i] == true)
            {
                isFullOff = false;
                break;
            }
        }

        return isFullOff;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
