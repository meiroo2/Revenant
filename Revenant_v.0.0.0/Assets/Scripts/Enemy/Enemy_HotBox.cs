using System;
using UnityEngine;


public class Enemy_HotBox : MonoBehaviour,IHotBox
{
    // Visual Member Variables
    public HitBoxPoint p_HitBoxPoint;
    
    // Member Variables
    private BasicEnemy m_Enemy;
    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public int HitHotBox(IHotBoxParam _param)
    {
        m_Enemy.AttackedByWeapon(p_HitBoxPoint, _param.m_Damage,
            _param.m_stunValue);
        
        return 1;
    }
}