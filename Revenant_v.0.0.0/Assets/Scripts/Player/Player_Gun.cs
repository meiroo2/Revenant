using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    // Member Variables
    // public -> private ������ ����
    // public Ȥ�� �ø��������� �ʵ� �� ��, bool���� ���� ���� ���� ����(�ν����Ϳ��� ���� ���ϰ� �Ϸ���)
    // �� �Ŀ� ������ ������ ����
    public GameObject m_BulletPrefab;
    public float m_BulletSpeed;
    public float m_BulletDamage;
    public bool m_canShot = true;

    private Player m_Player;

    // Constructors
    // ������ ���� �Լ��� ����(Awake->Start->��Ÿ ��ü ������ ����)
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
    }

    // Updates
    // ������Ʈ �� Ȥ�� ������Ʈ �� ȣ��Ǵ� �Լ��� ���� (Update->FixedUpdate->��ü ������Ʈ�Լ� ����)
    private void Update()
    {
        if (m_canShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject InstancedBullet = Instantiate(m_BulletPrefab);
                InstancedBullet.GetComponent<Player_Bullet>().InitBullet(m_BulletSpeed, m_BulletDamage);

                if (m_Player.m_isRightHeaded == false)
                    InstancedBullet.GetComponent<Player_Bullet>().InitBullet(-m_BulletSpeed, m_BulletDamage);

                InstancedBullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }
    }

    // Physics
    // �浹 ���� �Լ��� ����

    // Functions
    // 
}