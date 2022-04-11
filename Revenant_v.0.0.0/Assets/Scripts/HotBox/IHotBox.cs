using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    BULLET,
    GRENADE,
}

public class IHotBoxParam
{
    public int m_Damage { get; private set; }
    public int m_stunValue { get; private set; }
    public Vector2 m_contactPoint { get; private set; }
    public WeaponType m_weaponType { get; private set; }

    public IHotBoxParam(int _damage, int _stunValue, Vector2 _contactPt, WeaponType _weaponType)
    {
        m_Damage = _damage;
        m_stunValue = _stunValue;
        m_contactPoint = _contactPt;
        m_weaponType = _weaponType;
    }
}

public interface IHotBox
{
    public void HitHotBox(IHotBoxParam _param);
}