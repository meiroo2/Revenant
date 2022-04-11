using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    WEAPON weapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Attack();
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
