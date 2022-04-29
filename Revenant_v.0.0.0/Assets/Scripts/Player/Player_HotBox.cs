using UnityEditor;
using UnityEngine;


public class Player_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    private BoxCollider2D m_PlayerHotBoxCol;
    private UIMgr m_UIMgr;
    private Player_UIMgr m_PlayerUIMgr;
    private Player m_Player;
    private SoundMgr_SFX m_SFXMgr;
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = false;

    private void Awake()
    {
        m_PlayerHotBoxCol = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        m_UIMgr = GameManager.GetInstance().GetComponentInChildren<UIMgr>();
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerUIMgr = GameManager.GetInstance().GetComponentInChildren<Player_UIMgr>();
        m_SFXMgr = GameManager.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    public void setPlayerHotBoxCol(bool _isOn)
    {
        if (_isOn)
            m_PlayerHotBoxCol.enabled = true;
        else
            m_PlayerHotBoxCol.enabled = false;
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        if (m_Player.m_canAttacked && m_Player.m_curPlayerState != playerState.DEAD)
        {
            if (m_Player.m_isPlayerBlinking)
            {
                Debug.Log("플레이어 피격무적상태");
                return 0;
            }
            if(m_Player.m_curPlayerState == playerState.ROLL)
            {
                Debug.Log("플레이어 구르기상태");
                return 0;
            }

            m_Player.setPlayerHp((int)(m_Player.m_Hp - _param.m_Damage));
            if(m_Player.m_Hp <= 0)
            {
                m_Player.changePlayerFSM(playerState.DEAD);
                m_UIMgr.m_GameOverUI.SetActive(true);
                m_PlayerUIMgr.UpdatePlayerHp(Mathf.RoundToInt(m_Player.m_Hp / 10f));
                m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
                Debug.Log("플레이어 사망");
                return 1;
            }
            else
            {
                m_Player.DoPlayerBlink();
                m_PlayerUIMgr.UpdatePlayerHp(Mathf.RoundToInt(m_Player.m_Hp / 10f));
                m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
                Debug.Log("플레이어에게 " + _param.m_Damage + "데미지!");
                return 1;
            }
        }
        else
        {
            Debug.Log("플레이어 피격불가상태");
            return 0;
        }
    }
}