using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour, IAttacked
{
    // Visible Member Variables
    public Player m_Player;
    public bool m_isOn = false;

    // Member Variables
    private BoxCollider2D m_HideCollider;
    private SoundMgr_SFX m_SFXMgr;

    // Constructors
    private void Awake()
    {
        m_HideCollider = GetComponent<BoxCollider2D>();
        m_HideCollider.enabled = false;
        m_SFXMgr = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();
    }
    private void Start()
    {

    }
    /*
    <Ŀ���� �ʱ�ȭ �Լ��� �ʿ��� ���>
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
    public void setPlayerStateToHide()
    {
        
        if (m_Player.m_curPlayerState != playerState.HIDDEN)
        {
            m_Player.changePlayerFSM(playerState.HIDDEN);
            Debug.Log("����");
            // ���� OBJ �ݶ��̴� ����
            m_HideCollider.enabled = true;
        }
        else
        {
            m_Player.changePlayerFSM(playerState.IDLE);
            Debug.Log("����");
            // ���� OBJ �ݶ��̴� ����
            m_HideCollider.enabled = false;
        }
        

        /*
        if (!m_isOn)
        {
            m_isOn = true;
            m_HideCollider.enabled = true;
        }
        else
        {
            m_isOn = false;
            m_HideCollider.enabled = false;
        }
        */
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
