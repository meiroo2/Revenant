using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class TutorialBot_Weapon : BasicWeapon_Enemy
{
    // Visible Member Variables
    public ObjectPuller p_MuzFlashPuller;
    public Transform m_ShellPos;
    public Sprite p_BulletSprite;
    
    // Member Variables
    private BulletPuller m_Puller;
    private BulletParam m_BulletParam;

    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
        m_Enemy_Arm = GetComponentInParent<WeaponMgr>().transform;
    }

    private void Start()
    {
        var tempIns = InstanceMgr.GetInstance();
        
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_ShellMgr = tempIns.GetComponentInChildren<ShellMgr>();
        m_Puller = tempIns.GetComponentInChildren<BulletPuller>();

        m_BulletParam = new BulletParam(false, p_BulletSprite, true, m_Enemy_Arm.position,
            m_Enemy_Arm.rotation, 0, p_BulletSpeed);
    }

    public override int Fire()
    {
        m_BulletParam.m_IsRightHeaded = m_Enemy.m_IsRightHeaded;
        m_BulletParam.m_Position = m_Enemy_Arm.position;
        m_BulletParam.m_Rotation = m_Enemy_Arm.rotation;
        
        m_Puller.MakeBullet(m_BulletParam);
        
        m_SoundPlayer.PlayEnemySoundOnce(0, gameObject);
        p_MuzFlashPuller.EnableNewObj();
        
        if(m_Enemy.m_IsRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));
        
        return 1;
    }
}