using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;


public class Player_UI : MonoBehaviour
{
    // Visible Member Variables
    [Header("Objs")]
    public Image p_HpGauge;
    public Image p_RollGauge;
    public Image p_LeftRoundsImg;
    public Text p_LeftMagTxt;

    [Space(10f)] 
    public Sprite[] p_LeftRoundsImgArr;

    
    [Space(30f)] [Header("AimUI")] 
    public Image p_MainAimImg;
    public Sprite p_ReloadAimImg;
    public Image p_ReloadCircle;
    public Image p_Hitmark;
    
    [Space(10f)] 
    public Sprite[] p_AimImgArr;
    public Sprite[] p_HitmarkArr;





    // Member Variables
    private SoundMgr_SFX m_SoundMgr;
    private CameraMove m_Maincam;
    public float m_ReloadSpeed { get; set; } = 1f;
    public float m_HitmarkRemainTime { get; set; } = 0.2f;
    private bool m_IsReloading = false;
    private int m_MaxHp = 0;
    private float m_HpUnit = 0;
    private Transform m_AimTransform;
    
    public delegate void PlayerUIDelegate();
    private PlayerUIDelegate m_Callback = null;

    private Coroutine m_CurCoroutine;
    private Coroutine m_NullCoroutine = null;

    private Color m_HitmarkColor = new Color(1, 1, 1, 1);
    private Vector2 m_HitmarkOriginScale;
    
    
    // Constructors
    private void Awake()
    {
        m_Maincam = Camera.main.GetComponent<CameraMove>();
        m_AimTransform = p_MainAimImg.transform;
        p_Hitmark.enabled = false;
        p_ReloadCircle.fillAmount = 0f;
        
        m_HitmarkOriginScale = p_Hitmark.rectTransform.localScale;
        
        p_LeftMagTxt.text = "0";
        m_MaxHp = 0;
        m_HpUnit = 0;
    }

    private void Start()
    {
        m_SoundMgr = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }


    private void Update()
    {
        m_AimTransform.position = Input.mousePosition;
    }


    // Functions
    public void ForceStopReload()
    {
        StopCoroutine(Internal_Reload());
        p_ReloadCircle.fillAmount = 0f;
        m_IsReloading = false;
    }
    public void StartReload()
    {
        if (!m_IsReloading)
        {
            p_MainAimImg.sprite = p_ReloadAimImg;
            StartCoroutine(Internal_Reload());
        }
    }

    private IEnumerator Internal_Reload()
    {
        m_IsReloading = true;
        while (p_ReloadCircle.fillAmount < 1)
        {
            p_ReloadCircle.fillAmount += Time.deltaTime * m_ReloadSpeed;
            yield return null;
        }

        p_ReloadCircle.fillAmount = 0f;
        m_IsReloading = false;
        m_Callback();
    }
    
    
    public void AddCallback(PlayerUIDelegate _input)
    {
        m_Callback += _input;
    }
    public void ResetCallback()
    {
        m_Callback = null;
    }
    
    public void SetMaxHp(int _maxHp)
    {
        m_MaxHp = _maxHp;
        m_HpUnit = (float)1 / (float)m_MaxHp;
    }
    public void SetHp(int _curHp)
    {
        p_HpGauge.fillAmount = _curHp * m_HpUnit;
    }
    public void SetRollGauge(float _curRoll)
    {
        p_RollGauge.fillAmount = _curRoll / 3.0f;
    }
    public void SetLeftMag(int _magCount)
    {
        p_LeftMagTxt.text = _magCount.ToString();
    }
    public void SetLeftRounds(int _rounds)
    {
        if (_rounds is < 0 or > 8)
            return;

        p_LeftRoundsImg.sprite = p_LeftRoundsImgArr[_rounds];
        p_MainAimImg.sprite = p_AimImgArr[_rounds];
    }
    public void SetLeftRoundsNMag(int _rounds, int _magCount)
    {
        SetLeftRounds(_rounds);
        SetLeftMag(_magCount);
    }

    public void ActiveHitmark(int _type)
    {
        if (m_CurCoroutine != m_NullCoroutine)
            StopCoroutine(m_CurCoroutine);

        // 기본 히트를 Body라 생각
        p_Hitmark.enabled = true;
        // 불투명하게 만들기 위한 것
        m_HitmarkColor = Color.white;
        p_Hitmark.color = m_HitmarkColor;

        switch (_type)
        {
            case 0:     // Head
                p_Hitmark.sprite = p_HitmarkArr[0];
                m_Maincam.DoCamShake(true);
                m_SoundMgr.playUISound(0);
                // 원본 크기 == 큰 크기임
                p_Hitmark.rectTransform.localScale = m_HitmarkOriginScale;
                m_CurCoroutine = StartCoroutine(DisableHitMark_Head());
                break;
            
            case 1:     // Body
                p_Hitmark.sprite = p_HitmarkArr[1];
                m_Maincam.DoCamShake(false);
                m_SoundMgr.playUISound(1);
                // 2f, 2f가 작은 버전의 크기
                p_Hitmark.rectTransform.localScale = new Vector2(2f, 2f);
                m_CurCoroutine = StartCoroutine(DisableHitMark_Body());
                break;
        }
    }

    private IEnumerator DisableHitMark_Body()   // 몸통 Hit 코루틴
    {
        while (true)
        {
            if (m_HitmarkColor.a <= 0f)
                break;
            
            // Fade out
            m_HitmarkColor.a -= Time.deltaTime * 7f;
            p_Hitmark.color = m_HitmarkColor;
            yield return null;
        }
        
        p_Hitmark.enabled = false;
    }

    private IEnumerator DisableHitMark_Head()
    {
        while (true)
        {
            if (m_HitmarkColor.a <= 0f)
                break;
            
            
            // 1.5f Scale까지 작아지면 그냥 0으로 스케일 줄여버리기
            p_Hitmark.rectTransform.localScale = p_Hitmark.rectTransform.localScale.x >= 1.5f ?
                Vector2.Lerp(p_Hitmark.rectTransform.localScale, Vector2.zero, Time.deltaTime * 6f) : Vector2.zero;
            
            m_HitmarkColor.a -= Time.deltaTime * 7f;
            p_Hitmark.color = m_HitmarkColor;
            yield return null;
        }
        
        p_Hitmark.enabled = false;
    }
    
    
    // Legacy Codes
    /*
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
            if (i < m_HealthSlots.Length)
                m_HealthSlots[i].sprite = m_RedHealth;
        }

        if (_Hp < m_HealthSlots.Length)
        {
            for (int i = _Hp; i < m_HealthSlots.Length; i++)
            {
                if (i < m_HealthSlots.Length)
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
    */
}