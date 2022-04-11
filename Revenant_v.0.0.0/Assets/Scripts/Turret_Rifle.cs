using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rifle : WEAPON
{
    private void Awake()
    {
        m_isPlayers = false;
    }

    public override int Fire()
    {
        Debug.Log("turret fire");
        return 1;
    }

    public override int Reload()
    {
        return 0;
    }
}
