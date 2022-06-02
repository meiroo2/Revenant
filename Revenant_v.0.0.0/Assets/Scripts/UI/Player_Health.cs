using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    // Visible Member Variables
    public Player m_Player;
    public Image m_Health;

    // Member Variables
    private float m_PlayerHp;

    // Constructors
    private void Awake()
    {

    }
    private void Start()
    {
        m_PlayerHp = m_Player.p_Hp;
        m_Health.fillAmount = m_PlayerHp / 10f;
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
    public void UpdatePlayerUI()
    {
        m_PlayerHp = m_Player.p_Hp;
        m_Health.fillAmount = m_PlayerHp / 10f;
    }


    // 기타 분류하고 싶은 것이 있을 경우
}
