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
        if (Input.GetKeyDown(KeyCode.K))
        {
            Fire();
        }
    }
    public override int Fire()
    {
        Debug.Log("turret fire");
        BulletCreate();
        return 1;
    }

    public override int Reload()
    {
        return 0;
    }
}
