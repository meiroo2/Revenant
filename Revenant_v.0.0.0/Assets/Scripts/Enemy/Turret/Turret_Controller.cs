using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controller : Enemy
{
    [SerializeField]
    WEAPON m_weapon;

    private void Awake()
    {
        gameObject.SetActive(false);

        HumanInit();
        Attack_1();
    }

    public override void Idle()
    {
        //등장
        //1초 대기
        //
    }

    public void Attack_1()
    {
        // 1회 발사
        m_weapon.Fire();
    }
    public void Attack_3()
    {
        // 3회 발사

    }


    public void Attacked()
    {
        Debug.Log("turret");
    }

    
}
