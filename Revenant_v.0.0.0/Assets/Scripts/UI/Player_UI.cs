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
    private CameraMgr m_Maincam;
    private Player_ArmMgr m_ArmMgr;
    
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

    private Player m_Player;
    private float m_ReloadSpeed = 1f;
    
    
    // Constructors
    private void Awake()
    {
        m_AllVisibleObjs = this.gameObject.GetComponentsInChildren<CanvasRenderer>();

        m_Maincam = Camera.main.GetComponent<CameraMgr>();
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
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
        m_ArmMgr = m_Player.m_ArmMgr;
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
        m_ReloadSpeed = m_Player.m_Negotiator.p_ReloadTime;
        
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

        // Body 기준
        p_Hitmark.enabled = true;
        
        m_HitmarkColor = Color.white;
        p_Hitmark.color = m_HitmarkColor;

        switch (_type)
        {
            case 0:     // Head
                p_Hitmark.sprite = p_HitmarkArr[0];
                m_Maincam.DoCamShake(true);
                m_SoundMgr.playUISound(0);
                // 원본 Scale로 함
                p_Hitmark.rectTransform.localScale = m_HitmarkOriginScale;
                m_CurCoroutine = StartCoroutine(DisableHitMark_Head());
                break;
            
            case 1:     // Body
                p_Hitmark.sprite = p_HitmarkArr[1];
                m_Maincam.DoCamShake(false);
                m_SoundMgr.playUISound(1);
                // scale 2배로 시작
                p_Hitmark.rectTransform.localScale = new Vector2(2f, 2f);
                m_CurCoroutine = StartCoroutine(DisableHitMark_Body());
                break;
        }
    }

    private IEnumerator DisableHitMark_Body()
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
            
            
            // Scale 갈수록 줄임
            p_Hitmark.rectTransform.localScale = p_Hitmark.rectTransform.localScale.x >= 1.5f ?
                Vector2.Lerp(p_Hitmark.rectTransform.localScale, Vector2.zero, Time.deltaTime * 6f) : Vector2.zero;
            
            m_HitmarkColor.a -= Time.deltaTime * 7f;
            p_Hitmark.color = m_HitmarkColor;
            yield return null;
        }
        
        p_Hitmark.enabled = false;
    }
}