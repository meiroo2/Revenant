using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    // Visible Member Variables
    public BASEWEAPON[] m_MainWeapons;
    public BASEWEAPON[] m_SubWeapons;
    public BASEWEAPON[] m_Throwables;

    [Space(30f)]
    [Header("For IK")]
    public Transform m_OutArmEffectorPos;
    public Transform m_OutArmEffectorOriginPos;

    public Transform m_InArmEffectorPos;
    public Transform m_InArmEffectorOriginPos;

    public Transform m_GunPos;
    public Transform m_GunOriginPos;

    // Member Variables
    private NoiseMaker m_NoiseMaker;
    private Player_UIMgr m_PlayerUIMgr;
    private AimCursor m_aimCursor;

    private PlayerSoundnAni m_PlayerSoundnAni;
    private Player m_Player;
    private Transform m_Player_Arm;

    private BASEWEAPON m_curMainWeapon;
    private BASEWEAPON m_curSubWeapon;
    private BASEWEAPON m_curThrowable;

    private BASEWEAPON m_ActiveWeapon;

    private bool doRecoil = false;
    private bool m_isCastingThrow = false;
    private int m_ActiveWeaponType = 0; // 0 == Main, 1 == Sub, 2 == Throwable

    // Constructors
    private void Start()
    {
        m_Player = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player;
        m_Player_Arm = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerRotation.transform;
        m_PlayerSoundnAni = GameManager.GetInstance().GetComponentInChildren<Player_Manager>().m_Player.m_playerSoundnAni;
        m_NoiseMaker = GameManager.GetInstance().GetComponentInChildren<NoiseMaker>();
        m_PlayerUIMgr = GameManager.GetInstance().GetComponentInChildren<Player_UIMgr>();
        m_aimCursor = GameManager.GetInstance().GetComponentInChildren<AimCursor>();

        if (m_MainWeapons.Length != 0)
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
        m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);

        GameManager.GetInstance().GetComponentInChildren<Player_UIMgr>().setLeftBulletUI(m_curMainWeapon.m_LeftBullet, m_curMainWeapon.m_LeftMag, 0);
        //GameManager.GetInstance().GetComponentInChildren<Player_UIMgr>().setLeftBulletUI(m_curSubWeapon.m_LeftBullet, m_curSubWeapon.m_LeftMag, 0);
    }

    // Updates
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_ActiveWeapon.m_WeaponType == 0)
            {   // To Sub
                /*
                m_PlayerUIMgr.changeWeapon(1);
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
                */
                changeWeapon(1);
            }
            else if (m_ActiveWeapon.m_WeaponType == 1)
            {
                // To Main
                /*
                m_PlayerUIMgr.changeWeapon(0);
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curMainWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
                */
                changeWeapon(0);
            }
            else
            {
                // To Sub
                /*
                m_PlayerUIMgr.changeWeapon(1);
                m_ActiveWeapon.gameObject.SetActive(false);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
                */
                changeWeapon(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            /*
            m_ActiveWeapon.gameObject.SetActive(false);
            m_ActiveWeapon = m_curThrowable;
            m_ActiveWeapon.gameObject.SetActive(true);
            m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
            */
            changeWeapon(2);
        }

        if (m_Player.m_canShot)
        {
            if (Input.GetMouseButton(0) && !m_isCastingThrow)
            {
                switch (m_ActiveWeapon.Fire())
                {
                    case 0: // 발사 실패(딜레이)
                        break;
                    case 1: // 발사 성공
                        m_PlayerSoundnAni.playShotAni();

                        if (Vector2.Distance(m_OutArmEffectorPos.position, m_OutArmEffectorOriginPos.position) <= 0.05f)
                        {
                            m_OutArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                            m_InArmEffectorPos.Translate(new Vector2(-0.04f, 0f));
                            m_GunPos.Translate(new Vector2(-0.04f, 0f));
                        }
                        doRecoil = true;

                        // 소음 발생
                        m_NoiseMaker.MakeNoise(NoiseType.FIREARM, new Vector2(7f, 1.5f), m_Player.transform.position, true);
                        break;
                    case 2: // 총알 없음
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_ActiveWeapon.Reload();
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
    private void changeWeapon(int _input)
    {
        exitWeapon();
        // 0 == To Main, 1 == To Sub, 2 == To Throw
        switch (_input)
        {
            case 0:
                m_PlayerUIMgr.changeWeapon(0);
                m_ActiveWeapon = m_curMainWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeaponType = 0;
                break;

            case 1:
                m_PlayerUIMgr.changeWeapon(1);
                m_ActiveWeapon = m_curSubWeapon;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeaponType = 1;
                break;

            case 2:
                m_PlayerSoundnAni.changeArmMode(false);
                m_ActiveWeapon = m_curThrowable;
                m_ActiveWeapon.gameObject.SetActive(true);
                m_ActiveWeaponType = 2;
                break;
        }
        m_ActiveWeapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
    }
    private void exitWeapon()
    {
        switch (m_ActiveWeaponType)
        {
            case 0:
                m_ActiveWeapon.gameObject.SetActive(false);
                break;
            case 1:
                m_ActiveWeapon.gameObject.SetActive(false);
                break;
            case 2:
                m_PlayerSoundnAni.changeArmMode(true);
                m_ActiveWeapon.gameObject.SetActive(false);
                break;
        }
    }
}