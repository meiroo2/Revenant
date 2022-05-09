using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDrone_BodyHotBox : MonoBehaviour, IHotBox
{
    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; } = 0;
    DefenseDrone_Controller drone;


    private void Awake()
    {
        drone = GetComponentInParent<DefenseDrone_Controller>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        drone.BodyAttacked();
        drone.Damaged(_param.m_stunValue, _param.m_Damage);


        return 1;
    }
}
