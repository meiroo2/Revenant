using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitPoints
{
    HEAD,
    BODY,
    OTHER
}

public struct AttackedInfo
{
    public bool m_IsPlayer;
    public float m_Damage;
    public float m_StunValue;
    public Vector2 m_ContactPoint;
    public HitPoints m_HitPoint;

    public AttackedInfo(bool _isPlayer, float _damage, float _stunvalue, Vector2 _contactpoint, HitPoints _hitPoints)
    {
        m_IsPlayer = _isPlayer;
        m_Damage = _damage;
        m_StunValue = _stunvalue;
        m_ContactPoint = _contactpoint;
        m_HitPoint = _hitPoints;
    }
}

public interface IAttacked
{ 
    public void Attacked(AttackedInfo _AttackedInfo);
}