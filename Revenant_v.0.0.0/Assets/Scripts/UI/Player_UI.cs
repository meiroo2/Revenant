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
    private SoundPlayer m_SoundMgr;
    private CameraMove m_Maincam;
    private Player_ArmMgr m_ArmMgr;
    public float m_ReloadSpeed { get; set; } = 1f;
    public float m_HitmarkRemainTime { get; set; } = 0.2f;
    private int m_MaxHp = 0;
    private float m_HpUnit = 0;
    private Transform m_AimTransform;
    
    public delegate void PlayerUIDelegate();
    private PlayerUIDelegate m_Callback = null;

    private Coroutine m_CurCoroutine;
    private Coroutine m_ReloadCoroutine;

    private Color m_HitmarkColor = new Color(1, 1, 1, 1);
    private Vector2 m_HitmarkOriginScale;

    private Sprite m_ReloadBackupSprite;

    private CanvasRenderer[] m_AllVisibleObjs;
    
    
    // Constructors
    private void Awake()
    {
        m_AllVisibleObjs = this.gameObject.GetComponentsInChildren<CanvasRenderer>();

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
        var instance = InstanceMgr.GetInstance();
        m_SoundMgr = instance.GetComponentInChildren<SoundPlayer>();
        m_ArmMgr = instance.GetComponentInChildren<Player_Manager>().m_Player.m_ArmMgr;
    }


    private void Update()
    {
        m_AimTransform.position = Input.mousePosition;
    }


    // Functions
    public void SetPlayerUIVisible(bool _isVisible)
    {
        for (int i = 0; i < m_AllVisibleObjs.Length; i++)
        {
            m_AllVisibleObjs[i].SetAlpha(_isVisible ? 1 : 0);
        }
    }
    public void ForceStopReload()
    {
        if (!ReferenceEquals(m_ReloadCoroutine, null))
            StopCoroutine(m_ReloadCoroutine);

        p_MainAimImg.sprite = m_ReloadBackupSprite;
        p_ReloadCircle.fillAmount = 0f;
        m_ArmMgr.m_IsReloading = false;
    }
    public void StartReload()
    {
        m_ArmMgr.m_IsReloading = true;
        m_ReloadBackupSprite = p_MainAimImg.sprite;
        p_MainAimImg.sprite = p_ReloadAimImg;
        m_ReloadCoroutine = StartCoroutine(Internal_Reload());
    }

    private IEnumerator Internal_Reload()
    {
        while (p_ReloadCircle.fillAmount < 1)
        {
            p_ReloadCircle.fillAmount += Time.deltaTime * m_ReloadSpeed;
            yield return null;
        }

        p_ReloadCircle.fillAmount = 0f;
        m_ArmMgr.m_IsReloading = false;
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
        if (m_CurCoroutine != null)
            StopCoroutine(m_CurCoroutine);

        // �⺻ ��Ʈ�� Body�� ����
        p_Hitmark.enabled = true;
        // �������ϰ� ����� ���� ��
        m_HitmarkColor = Color.white;
        p_Hitmark.color = m_HitmarkColor;

        switch (_type)
        {
            case 0:     // Head
                p_Hitmark.sprite = p_HitmarkArr[0];
                m_Maincam.DoCamShake(true);
                m_SoundMgr.playUISound(0);
                // ���� ũ�� == ū ũ����
                p_Hitmark.rectTransform.localScale = m_HitmarkOriginScale;
                m_CurCoroutine = StartCoroutine(DisableHitMark_Head());
                break;
            
            case 1:     // Body
                p_Hitmark.sprite = p_HitmarkArr[1];
                m_Maincam.DoCamShake(false);
                m_SoundMgr.playUISound(1);
                // 2f, 2f�� ���� ������ ũ��
                p_Hitmark.rectTransform.localScale = new Vector2(2f, 2f);
                m_CurCoroutine = StartCoroutine(DisableHitMark_Body());
                break;
        }
    }

    private IEnumerator DisableHitMark_Body()   // ���� Hit �ڷ�ƾ
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
            
            
            // 1.5f Scale���� �۾����� �׳� 0���� ������ �ٿ�������
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
            m_RollTimerTxt.text = "������ Ÿ�̸� : " + m_RollTimer;
            if(m_RollTimer <= 0f)
            {
                m_RollTimer = 0f;
                m_RollTimerTxt.text = "������ Ÿ�̸� : " + m_RollTimer;
                m_RollTimerEnable = false;
            }
        }

        if (m_ReloadTimerEnable)
        {
            m_ReloadTimer -= Time.deltaTime;
            m_ReloadTimerTxt.text = "���� ������ �ð� : " + m_ReloadTimer;
            if (m_ReloadTimer <= 0f)
            {
                m_ReloadTimer = 0f;
                m_ReloadTimerTxt.text = "���� ������ �ð� : " + m_ReloadTimer;
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
        m_LeftRollCountTxt.text = "������ Ƚ�� : " + _count;
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