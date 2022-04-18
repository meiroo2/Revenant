using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controller : Enemy
{

    [field: SerializeField]
    public Turret_Animator m_turretAnimator { get; set; }

    [SerializeField]
    Enemy_Gun m_gun;

    [field: SerializeField]
    public float m_firePreDelay { get; set; } = 1.0f;

    [field: SerializeField]
    public float m_waitTime { get; set; } = 1.0f;// 사격하기 전 기다릴 시간

    private void Awake()
    {
        gameObject.SetActive(false);
        

        HumanInit();
    }
    public override void Attack()
    {
        
    }

    public void Attack_1()
    {
        // 1회 발사
        m_gun.m_autoMaxCount = 1;
        m_gun.Fire();
        
    }
    public void Attack_3()
    {
        // 3회 발사
        m_gun.m_autoMaxCount = 3;
        m_gun.Fire();
        //Invoke(nameof(Attack_1), m_firePreDelay);
    }

    public void WaitToAttack(int i)
    {
        if(i == 1)
        {
            Invoke(nameof(Attack_1), m_waitTime);
        }
        else if(i == 3)
        {
            Invoke(nameof(Attack_3), m_waitTime);
        }
    }

    public void Attacked()
    {
        Debug.Log("turret");
    }

}
