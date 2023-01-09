using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Serialization;


public class Player_UI : MonoBehaviour
{
    // Visible Member Variables
    [TabGroup("Bullets")] public Sprite p_FullImg;
    [TabGroup("Bullets")] public Sprite[] p_7AnimArr;
    [TabGroup("Bullets")] public Sprite[] p_6AnimArr;
    [TabGroup("Bullets")] public Sprite[] p_5AnimArr;
    [TabGroup("Bullets")] public Sprite[] p_4AnimArr;
    [TabGroup("Bullets")] public Sprite[] p_3AnimArr;
    [TabGroup("Bullets")] public Sprite[] p_2AnimArr;
    [TabGroup("Bullets")] public Sprite p_NoneImg;
    [TabGroup("Bullets")] public int p_RoundAnimSpeed = 4;

    [TabGroup("AimUI")] public RectTransform p_AimTransform;
    [TabGroup("AimUI")] public Image p_MainAimImg;
    [TabGroup("AimUI")] public Image p_ReloadCircle;
    [TabGroup("AimUI")] public Image p_Hitmark;
    [TabGroup("AimUI")] public Image[] p_LeftRoundsImgArr;

    [Space(20f)]
    public int p_FrameSpeed = 4;
    private Coroutine m_UIAniCoroutine;
    public Sprite[] p_NormalSpriteArr;
    public Sprite[] p_RedSpriteArr;
    
    
    
    [Space(10f)] 
    public Sprite[] p_AimImgArr;
    public Sprite[] p_HitmarkArr;
    

    // Member Variables

    private Coroutine m_RoundsCoroutine;

    private SoundPlayer m_SoundPlayer;
    private CameraMgr m_Maincam;
    private Player_ArmMgr m_ArmMgr;
    
    public float m_HitmarkRemainTime { get; set; } = 0.2f;
    private int m_MaxHp = 0;
    private float m_HpUnit = 0;

    public delegate void PlayerUIDelegate();
    private PlayerUIDelegate m_Callback = null;

    private Coroutine m_CurCoroutine;
    private Coroutine m_ReloadCoroutine;

    private Color m_HitmarkColor = new Color(1, 1, 1, 1);
    private Vector2 m_HitmarkOriginScale;

    private Sprite m_ReloadBackupSprite = null;

    private CanvasRenderer[] m_AllVisibleObjs;

    private Player m_Player;
    private float m_ReloadSpeed = 1f;

    private LeftBullet_WUI m_LeftBulletWUI;

    private RectTransform m_CamCanvas;
    private Camera m_OverlayCam;
    private Camera MainCam;
    private Vector2 m_Pos = new Vector2();
    private Vector3 m_AimPos = new Vector3();
    
    
    // Constructors
    private void Awake()
    {
        m_LeftBulletWUI = GameObject.FindGameObjectWithTag("@Player").GetComponent<Player>().p_LeftBullet_WUI;
        
        m_OverlayCam = GameObject.FindWithTag("OverlayCam").GetComponent<Camera>();
        MainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
        m_CamCanvas = GameObject.FindWithTag("MainCanvas").GetComponent<RectTransform>();
        
        m_AllVisibleObjs = this.gameObject.GetComponentsInChildren<CanvasRenderer>();

        m_Maincam = Camera.main.GetComponent<CameraMgr>();
        p_Hitmark.enabled = false;
        p_ReloadCircle.fillAmount = 0f;
        
        m_HitmarkOriginScale = p_Hitmark.rectTransform.localScale;
        
        m_MaxHp = 0;
        m_HpUnit = 0;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_ArmMgr = m_Player.m_ArmMgr;
    }


    private void Update()
    {
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(p_AimTransform, Input.mousePosition,
            m_OverlayCam, out m_Pos);
        */
        m_AimPos = m_OverlayCam.ScreenToWorldPoint(Input.mousePosition);
        m_AimPos.z = 10f;
        p_AimTransform.position = m_AimPos;
    }


    // Functions
    public void SetPlayerUIVisible(bool _isVisible)
    {
        for (int i = 0; i < m_AllVisibleObjs.Length; i++)
        {
            m_AllVisibleObjs[i].SetAlpha(_isVisible ? 1 : 0);
        }
    }

    /// <summary>
    /// 플레이어 UI의 재장전 모드를 활성/비활성화합니다.
    /// </summary>
    /// <param name="_isOn">활성화 여부</param>
    public void ActivateReloadMode(bool _isOn)
    {
        if (_isOn)
        {
            m_ReloadBackupSprite = p_MainAimImg.sprite;
            p_ReloadCircle.fillAmount = 0f;
        }
        else
        {
            if (!ReferenceEquals(m_ReloadBackupSprite, null))
            {
                //p_MainAimImg.sprite = m_ReloadBackupSprite;
            }

            p_ReloadCircle.fillAmount = 0f;
        }
    }


  
    public void SetLeftRounds(int _rounds)
    {
        m_LeftBulletWUI.SetLeftBulletUI(_rounds);
    }

    public void PlayRoundsAnim(int _leftRounds)
    {
        SafetyStopRoundsCoroutine();
        
        switch (_leftRounds)
        {
            case 0:
                ChangeImageForArr(p_LeftRoundsImgArr, p_NoneImg);
                break;
            
            case 1:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_2AnimArr)); 
                break;
            
            case 2:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_3AnimArr)); 
                break;
            
            case 3:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_4AnimArr)); 
                break;
            
            case 4:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_5AnimArr)); 
                break;
            
            case 5:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_6AnimArr)); 
                break;
            
            case 6:
                m_RoundsCoroutine = StartCoroutine(CheckRounds(p_LeftRoundsImgArr, p_7AnimArr)); 
                break;
            
            case 7:
                ChangeImageForArr(p_LeftRoundsImgArr, p_FullImg);
                break;
            
            case 8:
                ChangeImageForArr(p_LeftRoundsImgArr, p_FullImg);
                break;
            
            default:
                Debug.Log("ERR : Player_UI에서 OOR");
                break;
        }
    }

    private void ChangeImageForArr(Image[] _imgArr, Sprite _sprite)
    {
        for (int i = 0; i < _imgArr.Length; i++)
        {
            _imgArr[i].sprite = _sprite;
        }
    }

    private void SafetyStopRoundsCoroutine()
    {
        if (!ReferenceEquals(m_RoundsCoroutine, null))
        {
            StopCoroutine(m_RoundsCoroutine);
        }

        m_RoundsCoroutine = null;
    }

    private IEnumerator CheckRounds(Image[] _imageArr, Sprite[] _spriteArr)
    {
        int curFrame = p_RoundAnimSpeed;
        int idx = 0;
        while (true)
        {
            if (curFrame > p_RoundAnimSpeed)
            {
                ChangeImageForArr(_imageArr, _spriteArr[idx]);
                idx++;
                if (idx >= _spriteArr.Length)
                {
                    break;
                }
                curFrame = 0;
            }
            else
            {
                curFrame++;
            }
            yield return new WaitForFixedUpdate();
        }

        yield break;
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
                m_Maincam.DoQuickZoom(m_Maincam.p_HeadZoomPower, m_Maincam.p_ZoomSpeed);
                m_Maincam.DoCamShake(true);
                m_SoundPlayer.PlayUISoundOnce(0);
                
                // 원본 Scale로 함
                p_Hitmark.rectTransform.localScale = new Vector2(2f, 2f);
                m_CurCoroutine = StartCoroutine(DisableHitMark_Head());

                if (!ReferenceEquals(m_UIAniCoroutine, null))
                {
                    StopCoroutine(m_UIAniCoroutine);
                }
                m_UIAniCoroutine = StartCoroutine(ChangeAim(true));
                break;
            
            case 1:     // Body
                p_Hitmark.sprite = p_HitmarkArr[1];
                m_Maincam.DoCamShake(false);
                m_SoundPlayer.PlayUISoundOnce(1);
                
                // scale 2배로 시작
                p_Hitmark.rectTransform.localScale = new Vector2(2f, 2f);
                m_CurCoroutine = StartCoroutine(DisableHitMark_Body());

                if (!ReferenceEquals(m_UIAniCoroutine, null))
                {
                    StopCoroutine(m_UIAniCoroutine);
                }
                m_UIAniCoroutine = StartCoroutine(ChangeAim(false));
                break;
        }
    }

    private IEnumerator ChangeAim(bool _isHead)
    {
        int frameLimit = p_FrameSpeed;
        int frameCount = frameLimit;
        int idx = 0;

        if (_isHead)
        {
            while (true)
            {
                if (frameCount >= frameLimit)
                {
                    frameCount = 0;
                
                    p_MainAimImg.sprite = p_RedSpriteArr[idx];
                    idx++;
                    if (idx >= p_RedSpriteArr.Length)
                    {
                        break;
                    }
                }
                
                yield return new WaitForFixedUpdate();
                frameCount++;
            }
        }
        else
        {
            while (true)
            {
                if (frameCount >= frameLimit)
                {
                    frameCount = 0;
                
                    p_MainAimImg.sprite = p_NormalSpriteArr[idx];
                    idx++;
                    if (idx >= p_NormalSpriteArr.Length)
                    {
                        break;
                    }
                }
                
                yield return new WaitForFixedUpdate();
                frameCount++;
            }
        }
        
        
        yield break;
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