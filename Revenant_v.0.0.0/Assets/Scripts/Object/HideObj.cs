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
    public void setPlayerStateToHide()
    {
        if (m_Player.m_curPlayerState != playerState.HIDDEN)
        {
            m_Player.changePlayerFSM(playerState.HIDDEN);
            // 숨는 OBJ 콜라이더 켜짐
        }
        else
        {
            m_Player.changePlayerFSM(playerState.IDLE);
            // 숨는 OBJ 콜라이더 꺼짐
        }
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
