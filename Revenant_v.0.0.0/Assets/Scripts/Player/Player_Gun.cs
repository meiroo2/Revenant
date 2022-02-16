using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    // Member Variables
    // public -> private 순서로 적기
    // public 혹은 시리얼라이즈드 필드 쓸 때, bool값이 가장 위에 오게 쓰기(인스펙터에서 보기 편하게 하려고)
    // 그 후에 나머지 변수들 적기
    public GameObject m_BulletPrefab;
    public float m_BulletSpeed;
    public float m_BulletDamage;
    public bool m_canShot = true;

    private Player m_Player;

    // Constructors
    // 생성자 관련 함수들 적기(Awake->Start->기타 자체 생성자 순서)
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
    }

    // Updates
    // 업데이트 문 혹은 업데이트 때 호출되는 함수들 적기 (Update->FixedUpdate->자체 업데이트함수 순서)
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
    // 충돌 관련 함수들 적기

    // Functions
    // 
}