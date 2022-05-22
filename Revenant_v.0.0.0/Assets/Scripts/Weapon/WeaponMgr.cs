using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WeaponMgr : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public List<BasicWeapon> p_Weapons { get; private set; }

    // Member Variables
    public bool m_isPlayers { get; protected set; } = false;

    public BasicWeapon m_CurWeapon { get; private set; }

    // Constructors
    protected void Awake()
    {
        if (p_Weapons.Count > 0)
            m_CurWeapon = p_Weapons[0];
        else
            Debug.Log("WeaponMgr에 할당된 BasicWeapon이 존재하지 않습니다.");
    }

    // Functions
    protected void ChangeWeapon(BasicWeapon _weapon)
    {
        m_CurWeapon.ExitWeapon();
        m_CurWeapon = _weapon;
        m_CurWeapon.InitWeapon();
    }
}