using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rifle : Enemy_Gun
{
    //public float m_firePreDelay { get; set; } = 1.0f;

    //public int m_fireCount { get; set; } = 0;//첫발은 딜레이가 없다

    private void Awake()
    {
        Init();
    }
    private void Update()
    {

    }
    public override int Fire()
    {
        Debug.Log("turret fire");

        //if(m_fireCount < 0)//첫발은 딜레이가 없다
        //else
        //{
            //Invoke(nameof(BulletCreate), m_firePreDelay);
        //}
        return 1;
    }

    public override int Reload()
    {
        return 0;
    }
}
