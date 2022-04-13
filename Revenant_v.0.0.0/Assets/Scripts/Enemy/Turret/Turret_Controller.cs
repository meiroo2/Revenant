using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controller : Enemy
{
    [SerializeField]
    WEAPON weapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Attack();
    }

    public override void Idle()
    {

    }



    public void Attacked()
    {
        Debug.Log("turret");
    }

    public void Attack()
    {
        weapon.Fire();
    }
}
