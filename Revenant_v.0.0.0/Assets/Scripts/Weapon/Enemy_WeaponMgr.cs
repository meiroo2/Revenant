using UnityEngine;


public class Enemy_WeaponMgr : WeaponMgr
{
    protected new void Awake()
    {
        base.Awake();
    }

    protected new void Start()
    {
        base.Start();
        m_CurWeapon.gameObject.SetActive(false);
        m_CurWeapon = null;
    }

    public override void ChangeWeapon(int _idx)
    {
        if (_idx < 0 || _idx >= p_Weapons.Count)
        {
            Debug.Log("WeaponMgr 총기교체 OOR ERROR");
            return;
        }

        if (!ReferenceEquals(m_CurWeapon, null))
        {
            m_CurWeapon.ExitWeapon();
            m_CurWeapon.gameObject.SetActive(false);
        }

        m_CurWeapon = p_Weapons[_idx];
        m_CurWeapon.gameObject.SetActive(true);
        m_CurWeapon.InitWeapon();
    }
}