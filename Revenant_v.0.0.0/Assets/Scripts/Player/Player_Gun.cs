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

    public GameObject m_Grenade;

    public Transform m_OutArmEffectorPos;
    public Transform m_OutArmEffectorOriginPos;

    public Transform m_InArmEffectorPos;
    public Transform m_InArmEffectorOriginPos;

    public Transform m_GunPos;
    public Transform m_GunOriginPos;


    // Member Variables
    private int m_WeaponIdx = 0;
    private BASEWEAPON m_Weapon;

    private bool doRecoil = false;

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

        if (m_Player.m_canShot)
        {
            if (Input.GetMouseButton(0))
            {
                m_Weapon.Fire();

                if (Vector2.Distance(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position) <= 0.05f)
                {
                    m_OutArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                    m_InArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                    m_GunPos.Translate(new Vector2(-0.04f, 0f));
                }

                doRecoil = true;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {

            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                GameObject InstancedGren = GameObject.Instantiate(m_Grenade);
                InstancedGren.transform.SetPositionAndRotation(m_Player_Arm.position, m_Player_Arm.rotation);
                if (m_Player.m_isRightHeaded)
                    InstancedGren.GetComponent<Rigidbody2D>().AddForce(transform.right * 15f);
                else
                    InstancedGren.GetComponent<Rigidbody2D>().AddForce(-transform.right * 15f);
            }
        }
    }
    private void FixedUpdate()
    {
        if (doRecoil)
        {
            m_OutArmEffectorPos.position = Vector2.Lerp(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position, Time.deltaTime * 3f);
            m_InArmEffectorPos.position = Vector2.Lerp(m_InArmEffectorPos.position, m_InArmEffectorOriginPos.position, Time.deltaTime * 3f);
            m_GunPos.position = Vector2.Lerp(m_GunPos.position, m_GunOriginPos.position, Time.deltaTime * 3f);

            if (Vector2.Distance(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position) <= 0.0005f)
                doRecoil = false;
        }
    }

    // Physics


    // Functions


    // 기타 분류하고 싶은 것이 있을 경우
}