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
        m_PlayerHp = m_Player.m_Hp;
        m_Health.fillAmount = m_PlayerHp / 10f;
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
    public void UpdatePlayerUI()
    {
        m_PlayerHp = m_Player.m_Hp;
        m_Health.fillAmount = m_PlayerHp / 10f;
    }


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
