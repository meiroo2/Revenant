using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WEAPON_Player
{
    // Visible Member Variables
    public ObjectPuller m_ObjPullerForMuzFlash;

    [Space(30f)]
    [Header("For Effect")]
    public GameObject m_ShellPos;

    // Member Variables
    private HitSFXMaker m_HitSFXMaker;
    private ShellMgr m_ShellMgr;
    private Player_BulletMgr m_BulletMgr;

    // Constructors
    private void Awake()
    {
        m_LeftBullet = m_BulletPerMag;
        m_LeftMag = m_Magcount;
    }
    private void Start()
    {
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
        m_PlayerUIMgr = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
        m_ShellMgr = InstanceMgr.GetInstance().GetComponentInChildren<ShellMgr>();
        m_BulletMgr = InstanceMgr.GetInstance().GetComponentInChildren<Player_BulletMgr>();
    }
    private void OnEnable()
    {
        //m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }

    // Updates


    // Physics


    // Functions
    public override int Fire()
    {
        if (m_isDelayEnd)
        {
            if(m_LeftBullet > 0)
            {
                m_LeftBullet--;
                Internal_Fire();
                return 1;
            }
            else
            {
                return 2;
            }
        }
        return 0;
    }
    public override int Reload()
    {
        if (m_LeftMag > 0)
        {
            if (m_LeftBullet <= m_BulletPerMag && m_LeftBullet > 0)
            {
                m_LeftMag--;
                m_LeftBullet = m_BulletPerMag + 1;
                m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
                return 1;
            }
            else if (m_LeftBullet == 0)
            {
                m_LeftMag--;
                m_LeftBullet = m_BulletPerMag;
                m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
                return 1;
            }
            else
                return 0;
        }
        else
            return 0;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
    private void Internal_Fire()
    {
        m_isDelayEnd = false;

        Invoke(nameof(setisDelayEndToTrue), m_ShotDelay);

        m_SoundMgrSFX.playGunFireSound(0, m_Player.gameObject);


        m_BulletMgr.MakeBullet(m_Player.m_isRightHeaded, m_BulletDamage, m_BulletSpeed,
            m_Player_Arm.position, m_Player_Arm.rotation);

        // ������ ����Ű�� �ִ� ���� ����� ��Ʈ�ڽ��� �������� �Ѿ����� �Ѱ���
        Debug.Log(m_aimCursor.AimedObjName + ", " + m_aimCursor.AimedObjid + "�� ����");

        m_ObjPullerForMuzFlash.EnableNewObj();

        if (m_Player.m_isRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position, new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position, new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));

        m_PlayerUIMgr.setLeftBulletUI(m_LeftBullet, m_LeftMag, m_WeaponType);
    }
}