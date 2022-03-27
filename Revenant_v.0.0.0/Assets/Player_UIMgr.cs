using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player_UIMgr : MonoBehaviour
{
    // Visible Member Variables
    public RectTransform m_MainWeaponSlot;
    public RectTransform m_SubWeaponSlot;
    public TextMeshProUGUI m_BulletText;
    public Image m_MainWeaponImg;
    public Image m_SubWeaponImg;


    // Member Variables
    private float m_Timer = 3f;
    private bool m_DoLerpForSlot = false;

    private bool m_FrontSlotisMain = true;

    private Vector2 m_FrontSlotPos;
    private Vector2 m_BehindSlotPos;


    // Constructors
    private void Awake()
    {
        m_FrontSlotPos = m_MainWeaponSlot.anchoredPosition;
        m_BehindSlotPos = m_SubWeaponSlot.anchoredPosition;
    }

    // Updates
    private void Update()
    {
        if (m_DoLerpForSlot)
        {
            if (m_FrontSlotisMain)
            {
                m_Timer -= Time.deltaTime;
                m_MainWeaponSlot.anchoredPosition = Vector2.Lerp(m_MainWeaponSlot.anchoredPosition, m_FrontSlotPos, Time.deltaTime * 4f);
                m_SubWeaponSlot.anchoredPosition = Vector2.Lerp(m_SubWeaponSlot.anchoredPosition, m_BehindSlotPos, Time.deltaTime * 4f);
                if(m_Timer <= 0f)
                {
                    m_DoLerpForSlot = false;
                    m_Timer = 3f;
                }
            }
            else
            {
                m_Timer -= Time.deltaTime;
                m_MainWeaponSlot.anchoredPosition = Vector2.Lerp(m_MainWeaponSlot.anchoredPosition, m_BehindSlotPos, Time.deltaTime * 4f);
                m_SubWeaponSlot.anchoredPosition = Vector2.Lerp(m_SubWeaponSlot.anchoredPosition, m_FrontSlotPos, Time.deltaTime * 4f);
                if (m_Timer <= 0f)
                {
                    m_DoLerpForSlot = false;
                    m_Timer = 3f;
                }
            }
        }
    }

    // Physics


    // Functions
    public void setBulletInfo(int _LeftBullet, int _LeftMag)
    {
        m_BulletText.text = _LeftBullet + " / " + _LeftMag;
    }
    public void changeWeapon(int _Num)
    {
        switch (_Num)
        {
            case 0: // Main
                if (!m_FrontSlotisMain)
                {
                    m_FrontSlotisMain = true;
                    m_DoLerpForSlot = true;
                    m_Timer = 3f;
                    m_SubWeaponSlot.SetAsFirstSibling();
                    m_MainWeaponSlot.SetAsLastSibling();
                }
                break;
            case 1: // Sub
                if (m_FrontSlotisMain)
                {
                    m_FrontSlotisMain = false;
                    m_DoLerpForSlot = true;
                    m_Timer = 3f;
                    m_SubWeaponSlot.SetAsLastSibling();
                    m_MainWeaponSlot.SetAsFirstSibling();
                }
                break;
            case 2: // Throw
                break;
        }
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
