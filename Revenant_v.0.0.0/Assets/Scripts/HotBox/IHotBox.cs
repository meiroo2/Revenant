using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    BULLET,
    GRENADE,
    KNIFE,
}

public enum HitBoxPoint
{
    HEAD,
    BODY,
    OBJECT
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

    public void ResetContactPoint(Vector2 _pos)
    {
        m_contactPoint = _pos;
    }
}

public interface IHotBox
{
    public GameObject m_ParentObj { get; set; }
    
    // 0 = No Hit, 1 = Hit
    public int m_hotBoxType { get; set; }
    public bool m_isEnemys { get; set; }
    public HitBoxPoint m_HitBoxInfo { get; set; }


    // 0 = No Hit, 1 = Hit
    public int HitHotBox(IHotBoxParam _param);
}