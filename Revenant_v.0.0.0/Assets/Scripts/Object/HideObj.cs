using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObj : MonoBehaviour
{
    // Visible Member Variables
    public Player m_Player;

    // Member Variables


    // Constructors
    private void Awake()
    {

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
    public void setPlayerStateToHide()
    {
        if (m_Player.m_curPlayerState != playerState.HIDDEN)
        {
            m_Player.changePlayerFSM(playerState.HIDDEN);
            // ���� OBJ �ݶ��̴� ����
        }
        else
        {
            m_Player.changePlayerFSM(playerState.IDLE);
            // ���� OBJ �ݶ��̴� ����
        }
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
