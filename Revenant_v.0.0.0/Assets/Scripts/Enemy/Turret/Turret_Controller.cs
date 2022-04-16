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
        //����
        //1�� ���
        //
    }

    public void Attack_1()
    {
        // 1ȸ �߻�
        m_weapon.Fire();
    }
    public void Attack_3()
    {
        // 3ȸ �߻�

    }


    public void Attacked()
    {
        Debug.Log("turret");
    }

    
}
