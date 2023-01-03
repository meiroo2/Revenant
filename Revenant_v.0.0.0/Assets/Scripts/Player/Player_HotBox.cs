using System.Collections;
using Sirenix.OdinInspector;
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
    private SoundPlayer m_SoundPlayer;
    private ScreenEffect_UI m_ScreenEffectUI;
    private Player_MatMgr m_PlayerMatMgr;
    private HitSFXMaker m_HitSFXMaker;
    
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.BODY;
    public GameObject m_ParentObj { get; set; }

    [HideInInspector]
    public int m_HitCount = 0;

    
    // Coroutine Variables
    private Coroutine m_BlinkCoroutine;
    private Coroutine m_EvadeCoroutine;
    
    
    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_ParentObj = gameObject;
        m_PlayerHotBoxCol = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        var instanceMgr = InstanceMgr.GetInstance();
        
        m_HitSFXMaker = instanceMgr.GetComponentInChildren<HitSFXMaker>();
        m_UIMgr = instanceMgr.GetComponentInChildren<UIMgr>();
        m_PlayerUIMgr = instanceMgr.m_Player_UI;
        m_ScreenEffectUI = instanceMgr.m_MainCanvas.GetComponentInChildren<InGame_UI>().m_ScreenEffectUI;
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_PlayerMatMgr = GetComponentInParent<Player>().GetComponent<Player_MatMgr>();
    }

    public void setPlayerHotBoxCol(bool _isOn)
    {
        m_PlayerHotBoxCol.enabled = _isOn;
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        if (!m_Player.m_CanAttacked || m_IsPlayerBlinking)
            return 0;

        switch (m_hotBoxType)
        {
            case 0:
                m_ScreenEffectUI.ActivateScreenBloodEft();
                m_ScreenEffectUI.ActivateScreenColorDistortionEffect();
                m_SoundPlayer.PlayHitSoundByMatType(MatType.Flesh, m_Player.transform);
                
                m_HitSFXMaker.EnableNewObj(1, m_Player.GetPlayerCenterPos(), m_Player.m_IsRightHeaded);
                
                m_Player.setPlayerHp((m_Player.p_Hp - _param.m_Damage));
                if (m_Player.p_Hp <= 0)
                {
                    m_Player.ChangePlayerFSM(PlayerStateName.DEAD);

                    Debug.Log("플레이어 사망");
                    return 1;
                }

                if(m_BlinkCoroutine != null)
                    StopCoroutine(m_BlinkCoroutine);
        
                m_BlinkCoroutine = StartCoroutine(ActivatePlayerBlink());

                return 1;
                break;
            
            case 2:
                // Roll에게 신호 줘야 함
                m_HitCount++;
                break;
        }

        return 0;
    }
    private IEnumerator ActivatePlayerBlink()
    {
        m_PlayerMatMgr.ActivateBlink(true);
        
        m_IsPlayerBlinking = true;
        m_PlayerHotBoxCol.enabled = false;
        yield return new WaitForSeconds(m_Player.p_StunAlertSpeed);
        m_PlayerHotBoxCol.enabled = true;
        m_IsPlayerBlinking = false;
        
        m_PlayerMatMgr.ActivateBlink(false);
    }
}