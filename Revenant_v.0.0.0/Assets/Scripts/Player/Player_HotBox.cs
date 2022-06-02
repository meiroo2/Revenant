using UnityEditor;
using UnityEngine;


public class Player_HotBox : MonoBehaviour, IHotBox
{
    // Member Variables
    private BoxCollider2D m_PlayerHotBoxCol;
    private UIMgr m_UIMgr;
    private Player_UI m_PlayerUIMgr;
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
        m_UIMgr = InstanceMgr.GetInstance().GetComponentInChildren<UIMgr>();
        m_Player = InstanceMgr.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_PlayerUIMgr = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_SFXMgr = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    public void setPlayerHotBoxCol(bool _isOn)
    {
        m_PlayerHotBoxCol.enabled = _isOn;
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        if (!m_Player.m_CanAttacked)
            return 0;

        if (m_Player.m_isPlayerBlinking)
        {
            Debug.Log("플레이어 깜빡이상태");
            return 0;
        }

        m_Player.setPlayerHp((m_Player.p_Hp - _param.m_Damage));
        if (m_Player.p_Hp <= 0)
        {
            m_Player.ChangePlayerFSM(PlayerStateName.DEAD);
            m_UIMgr.m_GameOverUI.SetActive(true);
            m_PlayerUIMgr.UpdatePlayerHp(Mathf.RoundToInt(m_Player.p_Hp / 10f));
            m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
            Debug.Log("플레이어 사망");
            return 1;
        }

        m_Player.DoPlayerBlink();
        m_PlayerUIMgr.UpdatePlayerHp(Mathf.RoundToInt(m_Player.p_Hp / 10f));
        m_SFXMgr.playAttackedSound(MatType.Normal, _param.m_contactPoint);
        Debug.Log("플레이어에게 " + _param.m_Damage + "데미지!");
        return 1;
    }
}