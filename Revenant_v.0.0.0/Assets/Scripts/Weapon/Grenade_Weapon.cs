using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Weapon : BASEWEAPON
{
    // Visible Member Variables
    public GameObject m_GrenadePrefab;

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
    public override bool Fire()
    {
        if (m_LeftBullet > 0 && m_isDelayEnd)
        {
            m_isDelayEnd = false;
            Invoke(nameof(setisDelayEndToTrue), m_ShotDelay);

            GameObject InstancedGren = GameObject.Instantiate(m_GrenadePrefab);
            InstancedGren.transform.SetPositionAndRotation(m_Player_Arm.position, m_Player_Arm.localRotation);
            if (m_Player.m_isRightHeaded)
                InstancedGren.GetComponent<Rigidbody2D>().AddForce(transform.up * 15f);
            else
                InstancedGren.GetComponent<Rigidbody2D>().AddForce(transform.up * 15f);
            return true;
        }
        else
            return false;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}
