using UnityEngine;


public class Enemy_WeaponMgr : WeaponMgr
{
    protected new void Awake()
    {
        base.Awake();
        for (int i = 0; i < p_Weapons.Count; i++)
        {
            p_Weapons[i].gameObject.SetActive(false);
        }
    }

    protected new void Start()
    {
        base.Start();
    }

    public override void ChangeWeapon(int _idx)
    {
        if (_idx < 0 || _idx >= p_Weapons.Count)
        {
            Debug.Log("WeaponMgr 총기교체 OOR ERROR");
            return;
        }
        
        
        if (p_Weapons[_idx] == m_CurWeapon)
        {
            if(!m_CurWeapon.gameObject.activeSelf)
                m_CurWeapon.gameObject.SetActive(true);
            
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