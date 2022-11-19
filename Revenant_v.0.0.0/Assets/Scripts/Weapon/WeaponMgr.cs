using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class WeaponMgr : MonoBehaviour
{
    // Visible Member Variables
    [field: SerializeField] public List<BasicWeapon> p_Weapons { get; private set; }

    // Member Variables
    public bool m_isPlayers { get; protected set; } = false;
    public BasicWeapon m_CurWeapon { get; protected set; }

    // Constructors
    protected void Awake()
    {
        m_isPlayers = !GetComponentInParent<BasicEnemy>();

        if (p_Weapons.Count > 0)
            m_CurWeapon = p_Weapons[0];
        else
            Debug.Log("WeaponMgr에 할당된 BasicWeapon이 존재하지 않습니다.");
    }

    protected void Start()
    {
        SoundPlayer soundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        for (int i = 0; i < p_Weapons.Count; i++)
        {
            p_Weapons[i].m_SoundPlayer = soundPlayer;
        }
    }

    // Functions
    public virtual void ChangeWeapon(int _idx)
    {
        if (_idx < 0 || _idx >= p_Weapons.Count)
        {
            Debug.Log("WeaponMgr 총기교체 OOR ERROR");
            return;
        }
        
        m_CurWeapon.ExitWeapon();
        m_CurWeapon.gameObject.SetActive(false);
        m_CurWeapon = p_Weapons[_idx];
        m_CurWeapon.gameObject.SetActive(true);
        m_CurWeapon.InitWeapon();
    }

    public virtual void ReleaseWeapon()
    {
        m_CurWeapon.gameObject.SetActive(false);
        m_CurWeapon = null;
    }
}