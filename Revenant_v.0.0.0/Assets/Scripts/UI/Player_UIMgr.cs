using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player_UIMgr : MonoBehaviour
{
    [field: SerializeField] private Player_UI m_PlayerUIPrefab;
    public Player_UI m_PlayerUI { get; private set; }

    [Space (20f)]
    public Sprite m_RedHealth;
    public Sprite m_GrayHealth;

    // Visible Member Variables
    RectTransform m_Player_Health;
    RectTransform m_MainWeaponSlot;
    RectTransform m_SubWeaponSlot;
    TextMeshProUGUI m_MainBulletText;
    TextMeshProUGUI m_SubBulletText;
    Image m_MainWeaponImg;
    Image m_SubWeaponImg;


    // Member Variables
    private float m_Timer = 3f;
    private bool m_DoLerpForSlot = false;

    private bool m_FrontSlotisMain = true;

    private Vector2 m_FrontSlotPos;
    private Vector2 m_BehindSlotPos;
    private Image[] m_HealthSlots;


    // Constructors
    private void Start()
    {
        m_PlayerUI = Instantiate(m_PlayerUIPrefab, GameManager.GetInstance().GetComponent<GameManager>().m_MainCanvas.transform);

        m_Player_Health = m_PlayerUI.m_HealthSlot.GetComponent<RectTransform>();
        m_MainWeaponSlot = m_PlayerUI.m_WeaponSlots[0].GetComponent<RectTransform>();
        m_SubWeaponSlot = m_PlayerUI.m_WeaponSlots[1].GetComponent<RectTransform>();
        m_MainBulletText = m_PlayerUI.m_WeaponTexts[0].GetComponent<TextMeshProUGUI>();
        m_SubBulletText = m_PlayerUI.m_WeaponTexts[1].GetComponent<TextMeshProUGUI>();
        m_MainWeaponImg = m_PlayerUI.m_WeaponImgs[0].GetComponent<Image>();
        m_SubWeaponImg = m_PlayerUI.m_WeaponImgs[1].GetComponent<Image>();

        m_FrontSlotPos = m_MainWeaponSlot.anchoredPosition;
        m_BehindSlotPos = m_SubWeaponSlot.anchoredPosition;
        m_HealthSlots = m_Player_Health.GetComponentsInChildren<Image>();
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
        for(int i = 0; i < _Hp; i++)
        {
            m_HealthSlots[i].sprite = m_RedHealth;
        }

        if(_Hp < m_HealthSlots.Length)
        {
            for(int i = _Hp; i < m_HealthSlots.Length; i++)
            {
                m_HealthSlots[i].sprite = m_GrayHealth;
            }
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
