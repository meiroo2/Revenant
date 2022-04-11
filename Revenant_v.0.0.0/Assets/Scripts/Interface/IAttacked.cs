using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitPoints
{
    HEAD,
    BODY,
    OTHER
}

public class AttackedInfo
{
    public bool m_IsPlayer;
    public int m_Damage;
    public int m_StunValue;
    public Vector2 m_ContactPoint;
    public HitPoints m_HitPoint;
    public WeaponType m_WeaponType;

    public AttackedInfo(bool _isPlayer, int _damage, int _stunvalue, Vector2 _contactpoint, HitPoints _hitPoints, WeaponType _weaponType)
    {
        m_IsPlayer = _isPlayer;
        m_Damage = _damage;
        m_StunValue = _stunvalue;
        m_ContactPoint = _contactpoint;
        m_HitPoint = _hitPoints;
        m_WeaponType = _weaponType;
    }
}

public interface IAttacked
{ 
    public void Attacked(AttackedInfo _AttackedInfo);
}