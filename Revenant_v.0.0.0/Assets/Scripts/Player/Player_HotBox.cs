using System.Collections;
using UnityEditor;
using UnityEngine;


public class Player_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    public bool m_IsPlayerBlinking { get; private set; } = false;

    private BoxCollider2D m_PlayerHotBoxCol;
    private UIMgr m_UIMgr;
    private Player_UI m_PlayerUIMgr;
    private Player m_Player;
    private SoundMgr_SFX m_SFXMgr;
    private ScreenEffect_UI m_ScreenEffectUI;
    
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.BODY;
    public GameObject m_ParentObj { get; set; }

    
    // Coroutine Variables
    private Coroutine m_BlinkCoroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_ParentObj = m_Player.gameObject;
        m_PlayerHotBoxCol = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        var instanceMgr = InstanceMgr.GetInstance();
        
        m_UIMgr = instanceMgr.GetComponentInChildren<UIMgr>();
        m_PlayerUIMgr = instanceMgr.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_ScreenEffectUI = instanceMgr.m_MainCanvas.GetComponentInChildren<InGame_UI>().m_ScreenEffectUI;
        m_SFXMgr = instanceMgr.GetComponentInChildren<SoundMgr_SFX>();
    }

    public void setPlayerHotBoxCol(bool _isOn)
    {
        m_PlayerHotBoxCol.enabled = _isOn;
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        if (!m_Player.m_CanAttacked || m_IsPlayerBlinking)
            return 0;

        m_ScreenEffectUI.ActivateScreenEdgeEffect();
        m_ScreenEffectUI.ActivateScreenColorDistortionEffect();
        m_SFXMgr.playPlayerSFXSound(3);

        m_Player.setPlayerHp((m_Player.p_Hp - _param.m_Damage));
        if (m_Player.p_Hp <= 0)
        {
            m_Player.ChangePlayerFSM(PlayerStateName.DEAD);
            m_UIMgr.m_GameOverUI.SetActive(true);

            if (m_Player.m_CurPlayerFSMName != PlayerStateName.DEAD)
                m_PlayerUIMgr.SetHp(m_Player.p_Hp);
            
            m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
            Debug.Log("플레이어 사망");
            return 1;
        }

        if(m_BlinkCoroutine != null)
            StopCoroutine(m_BlinkCoroutine);
        
        m_BlinkCoroutine = StartCoroutine(ActivatePlayerBlink());

        if (m_Player.m_CurPlayerFSMName != PlayerStateName.DEAD)
            m_PlayerUIMgr.SetHp(m_Player.p_Hp);
        
        m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
//        Debug.Log("플레이어에게 " + _param.m_Damage + "데미지!");
        return 1;
    }

    private IEnumerator ActivatePlayerBlink()
    {
        m_IsPlayerBlinking = true;
        m_PlayerHotBoxCol.enabled = false;
        yield return new WaitForSeconds(m_Player.p_stunTime);
        m_PlayerHotBoxCol.enabled = true;
        m_IsPlayerBlinking = false;
    }
}