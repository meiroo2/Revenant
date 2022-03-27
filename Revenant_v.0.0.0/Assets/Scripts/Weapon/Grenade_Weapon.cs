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
        m_LeftBullet = m_BulletPerMag;
        m_LeftMag = m_Magcount;
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
    public override int Fire()
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
            return 1;
        }
        else
            return 0;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}
