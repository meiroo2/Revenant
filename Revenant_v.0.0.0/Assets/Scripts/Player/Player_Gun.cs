using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    // Visible Member Variables
    public bool m_canShot = true;
    public Transform m_Player_Arm;
    public AimCursor m_aimCursor;
    public Player m_Player;
    public BASEWEAPON[] m_Weapons;

    // Member Variables
    private int m_WeaponIdx = 0;
    private BASEWEAPON m_Weapon;

    // Constructors
    private void Awake()
    {
        for(int i = 0; i < m_Weapons.Length; i++)
        {
            m_Weapons[i].gameObject.SetActive(false);
        }
        m_Weapons[0].gameObject.SetActive(true);
    }
    private void Start()
    {
        m_Weapon = m_Weapons[0];
        m_Weapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(m_WeaponIdx + 1 < m_Weapons.Length)
            {
                m_Weapons[m_WeaponIdx].gameObject.SetActive(false);
                m_WeaponIdx++;
                m_Weapons[m_WeaponIdx].gameObject.SetActive(true);
                m_Weapon = m_Weapons[m_WeaponIdx];
                m_Weapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            }
            else
            {
                m_Weapons[m_WeaponIdx].gameObject.SetActive(false);
                m_WeaponIdx = 0;
                m_Weapons[m_WeaponIdx].gameObject.SetActive(true);
                m_Weapon = m_Weapons[m_WeaponIdx];
                m_Weapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_Weapon.Fire();
        }
    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}