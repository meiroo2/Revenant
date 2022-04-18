using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour, IAttacked
{
    // Visible Member Variables
    public Player m_Player { get; private set; }
    public bool m_isOn = false;
    public bool m_isPlayerHide = false;

    // Member Variables
    private BoxCollider2D m_HideCollider;
    private SoundMgr_SFX m_SFXMgr;
    private SpriteRenderer m_SpriteRenderer;

    // Constructors
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_HideCollider = GetComponent<BoxCollider2D>();
        m_HideCollider.enabled = false;
        //m_SFXMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();
    }
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
    }
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public void Attacked(AttackedInfo _AttackedInfo)
    {
        m_SFXMgr.playAttackedSound(MatType.Target_Body, _AttackedInfo.m_ContactPoint);
    }
    public bool setPlayerStateToHide()
    {
        
        if ((m_Player.m_curPlayerState != playerState.HIDDEN) && (m_Player.m_curPlayerState != playerState.HIDDEN_STAND))
        {
            m_Player.changePlayerFSM(playerState.HIDDEN);
            // 숨는 OBJ 콜라이더 켜짐
            m_HideCollider.enabled = true;
            m_isOn = true;
            m_SpriteRenderer.sortingLayerName = "Stair";

            return true;
        }
        else
        {
            m_Player.changePlayerFSM(playerState.IDLE);
            // 숨는 OBJ 콜라이더 꺼짐
            m_HideCollider.enabled = false;
            m_isOn = false;
            m_SpriteRenderer.sortingLayerName = "Object";

            return false;
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
