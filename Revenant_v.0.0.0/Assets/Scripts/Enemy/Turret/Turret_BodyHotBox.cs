using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_BodyHotBox : MonoBehaviour, IHotBox
{
    public int m_hotBoxType { get; set; }
    Turret_Controller turret;

    private void Awake()
    {
        turret = GetComponentInParent<Turret_Controller>();
    }

    public void HitHotBox(IHotBoxParam _param)
    {
        turret.Attacked();
    }
}
