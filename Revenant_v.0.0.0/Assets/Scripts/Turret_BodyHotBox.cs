using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_BodyHotBox : MonoBehaviour, IHotBox
{
    Turret turret;

    private void Awake()
    {
        turret = GetComponentInParent<Turret>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        turret.Attacked();
    }
}
