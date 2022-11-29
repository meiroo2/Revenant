using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingleRifle : BasicWeapon_Enemy
{
 // Visible Member Variables
    public float p_BulletSpread = 5f;
    public float p_FireDelay = 0.05f;
    public ObjectPuller p_MuzFlashPuller;
    public Transform m_ShellPos;
    public Sprite p_BulletSprite;
    
    
    // Member Variables
    private BulletPuller m_Puller;
    private BulletParam m_BulletParam;
    private int m_LeftFire = 3;

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
            m_Enemy_Arm.rotation, p_BulletDamage, p_BulletSpeed);
    }

    public override int Fire()
    {
        m_SoundPlayer.PlayEnemySoundOnce(0, gameObject);

        m_BulletParam.m_IsRightHeaded = m_Enemy.m_IsRightHeaded;
        m_BulletParam.m_Position = m_Enemy_Arm.position;
        m_BulletParam.m_Rotation = m_Enemy_Arm.rotation;
        
        // 불릿 랜덤 로테이션
        float randomRotation = Random.Range(-p_BulletSpread, p_BulletSpread);
        m_BulletParam.m_Rotation = Quaternion.Euler(m_BulletParam.m_Rotation.eulerAngles.x,
            m_BulletParam.m_Rotation.eulerAngles.y, m_BulletParam.m_Rotation.eulerAngles.z + randomRotation);
        
        m_Puller.MakeBullet(m_BulletParam);
        
        p_MuzFlashPuller.EnableNewObj();
        
        if(m_Enemy.m_IsRightHeaded)
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(-0.8f, -1.5f), Random.Range(0.8f, 1.5f)));
        else
            m_ShellMgr.MakeShell(m_ShellPos.transform.position,
                new Vector2(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f)));
        
        return 1;
    }

    public override void Reload()
    {
        //return 0;
    }

    public override void InitWeapon()
    {
        
    }

    public override void ExitWeapon()
    {
        
    }
}