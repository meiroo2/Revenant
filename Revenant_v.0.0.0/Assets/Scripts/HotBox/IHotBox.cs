using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    BULLET,
    GRENADE,
    KNIFE,
    AXE,
    BULLET_TIME,
    MOUSE
}

public enum HitBoxPoint
{
    HEAD,
    BODY,
    OBJECT,
    FLOOR,
    COGNITION
}

/// <summary>
/// HotBox에 히트 판정을 내고자 할 때 파라미터로 쓰이는 클래스입니다.
/// </summary>
public class IHotBoxParam
{
    public int m_Damage { get; private set; }
    public int m_stunValue { get; private set; }
    public Vector2 m_contactPoint { get; set; }
    public WeaponType m_weaponType { get; private set; }
    public bool m_MakeRageParticle;

    public IHotBoxParam(int _damage, int _stunValue, Vector2 _contactPt, WeaponType _weaponType, bool _makeParticle = true)
    {
        m_Damage = _damage;
        m_stunValue = _stunValue;
        m_contactPoint = _contactPt;
        m_weaponType = _weaponType;
        m_MakeRageParticle = _makeParticle;
    }

    public void ResetContactPoint(Vector2 _pos)
    {
        m_contactPoint = _pos;
    }
}

/// <summary>
/// 피격 가능한 히트박스용 클래스입니다.
/// </summary>
public interface IHotBox
{
    [field: SerializeField] public GameObject m_ParentObj { get; set; }
    
    
    /// <summary>
    /// HotBox의 타입을 결정합니다. 0일 경우 조준해야 히트, 1일 경우 강제 히트, 2일 경우 정보는 주되 지나갑니다.
    /// </summary>
    public int m_hotBoxType { get; set; }
    public bool m_isEnemys { get; set; }
    public HitBoxPoint m_HitBoxInfo { get; set; }
    
    
    // 0 = No Hit, 1 = Hit
    public int HitHotBox(IHotBoxParam _param);
}