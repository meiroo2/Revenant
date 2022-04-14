using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rifle : Enemy_Gun
{

    

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
        BulletCreate();
        //Invoke(nameof(BulletCreate), m_burstTime);
        return 1;
    }

    public override int Reload()
    {
        return 0;
    }
}
