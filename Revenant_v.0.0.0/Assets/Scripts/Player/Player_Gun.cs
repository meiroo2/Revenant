using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    // Visible Member Variables
    public AimCursor m_aimCursor;
    public BASEWEAPON[] m_MainWeapons;
    public BASEWEAPON[] m_SubWeapons;
    public BASEWEAPON[] m_Throwables;
    [field: SerializeField] public int m_ActiveWeaponType { get; private set; } = 0; // 0 == Main, 1 == Sub, 2 == Throwable

    [Space (30f)]
    [Header("For IK")]
    public Transform m_OutArmEffectorPos;
    public Transform m_OutArmEffectorOriginPos;

    public Transform m_InArmEffectorPos;
    public Transform m_InArmEffectorOriginPos;

    public Transform m_GunPos;
    public Transform m_GunOriginPos;

    // Member Variables
    private Player m_Player;
    private Transform m_Player_Arm;

    private BASEWEAPON m_curMainWeapon;
    private BASEWEAPON m_curSubWeapon;
    private BASEWEAPON m_curThrowable;

    private BASEWEAPON m_ActiveWeapon;

    private bool doRecoil = false;
    private bool m_isCastingThrow = false;

    // Constructors
    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
        m_Player_Arm = GetComponentInParent<PlayerRotation>().gameObject.transform;

        if(m_MainWeapons.Length != 0)
        {
            foreach (BASEWEAPON element in m_MainWeapons)
            {
                element.gameObject.SetActive(false);
            }
            m_curMainWeapon = m_MainWeapons[0];
        }

        if (m_SubWeapons.Length != 0)
        {
            foreach (BASEWEAPON element in m_SubWeapons)
            {
                element.gameObject.SetActive(false);
            }
            m_curSubWeapon = m_SubWeapons[0];
        }

        if (m_Throwables.Length != 0)
        {
            foreach (BASEWEAPON element in m_Throwables)
            {
                element.gameObject.SetActive(false);
            }
            m_curThrowable = m_Throwables[0];
        }


        m_curMainWeapon.gameObject.SetActive(true);
        m_ActiveWeapon = m_curMainWeapon;
    }
    private void Start()
    {
        m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_ActiveWeapon.m_WeaponType == 0)
            {
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            }
            else if (m_ActiveWeapon.m_WeaponType == 1)
            {
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curMainWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            }
            else
            {
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            m_ActiveWeapon.gameObject.SetActive(false);
            m_ActiveWeapon = m_curThrowable;
            m_ActiveWeapon.gameObject.SetActive(true);
            m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
        }

        if (m_Player.m_canShot)
        {
            if (Input.GetMouseButton(0) && !m_isCastingThrow)
            {
                if(m_ActiveWeapon.Fire() == true)
                {
                    if (Vector2.Distance(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position) <= 0.05f)
                    {
                        m_OutArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                        m_InArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                        m_GunPos.Translate(new Vector2(-0.04f, 0f));
                    }

                    doRecoil = true;
                }
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


    // ��Ÿ �з��ϰ� ���� ���� ���� ���
    private void changeWeapon(int _input)
    {
        exitWeapon();
        // 0 == To Main, 1 == To Sub, 2 == To Throw
        switch (_input)
        {
            case 0:
                m_curMainWeapon.gameObject.SetActive(true);
                m_ActiveWeapon = m_curMainWeapon;
                m_ActiveWeaponType = 0;
                break;

            case 1:
                m_curSubWeapon.gameObject.SetActive(true);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeaponType = 1;
                break;

            case 2:
                m_curThrowable.gameObject.SetActive(true);
                m_ActiveWeapon = m_curThrowable;
                m_ActiveWeaponType = 2;
                break;
        }
    }
    private void exitWeapon()
    {
        switch (m_ActiveWeaponType)
        {
            case 0:
                m_curMainWeapon.gameObject.SetActive(false);
                break;
            case 1:
                m_curSubWeapon.gameObject.SetActive(false);
                break;
            case 2:
                m_curThrowable.gameObject.SetActive(false);
                break;
        }
    }
}