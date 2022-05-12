using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    public GameObject m_HealthSlot;
    public GameObject[] m_WeaponSlots;
    public GameObject[] m_WeaponImgs;
    public GameObject[] m_WeaponTexts;

    public Text m_LeftRollCountTxt;
    public Text m_RollTimerTxt;
    public Text m_ReloadTimerTxt;

    [Space(20f)]
    public Sprite m_RedHealth;
    public Sprite m_GrayHealth;

    // Visible Member Variables
    RectTransform m_Player_Health;
    RectTransform m_MainWeaponSlot;
    RectTransform m_SubWeaponSlot;
    TextMeshProUGUI m_MainBulletText;
    TextMeshProUGUI m_SubBulletText;


    // Member Variables
    private float m_Timer = 3f;
    private bool m_DoLerpForSlot = false;

    private bool m_FrontSlotisMain = true;

    private Vector2 m_FrontSlotPos;
    private Vector2 m_BehindSlotPos;
    private Image[] m_HealthSlots;

    private bool m_RollTimerEnable = false;
    private float m_RollTimer = 0f;

    private bool m_ReloadTimerEnable = false;
    private float m_ReloadTimer = 0f;

    private void Awake()
    {
        m_Player_Health = m_HealthSlot.GetComponent<RectTransform>();
        m_MainWeaponSlot = m_WeaponSlots[0].GetComponent<RectTransform>();
        m_SubWeaponSlot = m_WeaponSlots[1].GetComponent<RectTransform>();
        m_MainBulletText = m_WeaponTexts[0].GetComponent<TextMeshProUGUI>();
        m_SubBulletText = m_WeaponTexts[1].GetComponent<TextMeshProUGUI>();

        m_FrontSlotPos = m_MainWeaponSlot.anchoredPosition;
        m_BehindSlotPos = m_SubWeaponSlot.anchoredPosition;
        m_HealthSlots = m_Player_Health.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        if (m_RollTimerEnable)
        {
            m_RollTimer -= Time.deltaTime;
            m_RollTimerTxt.text = "구르기 타이머 : " + m_RollTimer;
            if(m_RollTimer <= 0f)
            {
                m_RollTimer = 0f;
                m_RollTimerTxt.text = "구르기 타이머 : " + m_RollTimer;
                m_RollTimerEnable = false;
            }
        }

        if (m_ReloadTimerEnable)
        {
            m_ReloadTimer -= Time.deltaTime;
            m_ReloadTimerTxt.text = "남은 재장전 시간 : " + m_ReloadTimer;
            if (m_ReloadTimer <= 0f)
            {
                m_ReloadTimer = 0f;
                m_ReloadTimerTxt.text = "남은 재장전 시간 : " + m_ReloadTimer;
                m_ReloadTimerEnable = false;
            }
        }

        if (m_DoLerpForSlot)
        {
            if (m_FrontSlotisMain)
            {
                m_Timer -= Time.deltaTime;
                m_MainWeaponSlot.anchoredPosition = Vector2.Lerp(m_MainWeaponSlot.anchoredPosition, m_FrontSlotPos, Time.deltaTime * 4f);
                m_SubWeaponSlot.anchoredPosition = Vector2.Lerp(m_SubWeaponSlot.anchoredPosition, m_BehindSlotPos, Time.deltaTime * 4f);
                if (m_Timer <= 0f)
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

    // Functions
    public void setLeftBulletUI(int _LeftBullet, int _LeftMag, int _SlotNum)
    {
        switch (_SlotNum)
        {
            case 0:
                m_MainBulletText.text = _LeftBullet + " / " + _LeftMag;
                break;
            case 1:
                m_SubBulletText.text = _LeftBullet + " / " + _LeftMag;
                break;
        }
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
    public void UpdatePlayerHp(int _Hp)
    {
        for (int i = 0; i < _Hp; i++)
        {
            m_HealthSlots[i].sprite = m_RedHealth;
        }

        if (_Hp < m_HealthSlots.Length)
        {
            for (int i = _Hp; i < m_HealthSlots.Length; i++)
            {
                m_HealthSlots[i].sprite = m_GrayHealth;
            }
        }
    }
    public void UpdateRollCount(int _count)
    {
        m_LeftRollCountTxt.text = "구르기 횟수 : " + _count;
    }
    public void UpdateRollTimer(float _time)
    {
        m_RollTimer = _time;
        m_RollTimerEnable = true;
    }
    public void UpdateReloadTimer(float _time)
    {
        m_ReloadTimer = _time;
        m_ReloadTimerEnable = true;
    }
}