using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BodyHotBox : MonoBehaviour,IHotBox
{
    public bool m_isEnemys { get; set; } = true;
    public int m_hotBoxType { get; set; } = 0;


    Enemy m_enemy;

    private void Awake()
    {
        m_enemy = GetComponentInParent<Enemy>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        m_enemy.Damaged(_param.m_stunValue, _param.m_Damage);
        return 1;
    }
}
