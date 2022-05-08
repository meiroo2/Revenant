using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ArmMgr : MonoBehaviour
{
    // Visible Member Variables
    public Player_VisualPartMgr[] p_Arms;
    public float p_ArmChangeDistance = 1f;

    // Member Variables
    public int m_curArmIdx { get; private set; } = 0;
    private Player_Gun m_PlayerGun;
    private AimCursor m_AimCursor;
    private PlayerRotation m_PlayerRotation;
    private Vector2[] m_ArmOriginPos;


    // Constructors
    private void Awake()
    {
        m_PlayerRotation = GetComponent<PlayerRotation>();
    }
    private void Start()
    {
        GameObject _instance = GameManager.GetInstance();
        m_PlayerGun = _instance.GetComponentInChildren<Player_Manager>().m_Player.m_playerGun;
        m_AimCursor = _instance.GetComponentInChildren<AimCursor>();
    }

    // Updates
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            switch (m_PlayerGun.Fire_PlayerGun())
            {
                case 0:
                    // �߻� ����
                    break;

                case 1:
                    // �߻� ����
                    // �� �ݵ� �ʿ�
                    /*
                     if (doRecoil)
        {
            m_OutArmEffectorPos.position = Vector2.Lerp(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position, Time.deltaTime * 6f);
            m_InArmEffectorPos.position = Vector2.Lerp(m_InArmEffectorPos.position, m_InArmEffectorOriginPos.position, Time.deltaTime * 6f);
            m_GunPos.position = Vector2.Lerp(m_GunPos.position, m_GunOriginPos.position, Time.deltaTime * 6f);

            if (Vector2.Distance(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position) <= 0.0005f)
                doRecoil = false;
        }
                     */
                    break;

                case 2:
                    // �Ѿ� ����
                    break;
            }
        }
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}