using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEAPON_PlayerParam
{
    public float BulletSpeed { get; private set; }
    public int BulletDamage { get; private set; }
    public int StunValue { get; private set; }

    public float ShotDelay { get; private set; }
    public int BulletPerMag { get; private set; }
    public int MagCount { get; private set; }
    public WEAPON_PlayerParam(float _speed, int _damage, int _stunV, float _delay, int _bullet, int _mag)
    {
        BulletSpeed = _speed;
        BulletDamage = _damage;
        StunValue = _stunV;
        ShotDelay = _delay;
        BulletPerMag = _bullet;
        MagCount = _mag;
    }
}

public class WEAPON_Player : WEAPON
{
    // Visible Member Variables
    public float m_BulletSpeed = 15;
    public int m_BulletDamage = 1;
    public int m_StunValue = 1;

    [field: SerializeField] public float m_ShotDelay { get; private set; } = 0.5f;
    [field: SerializeField] public int m_WeaponType { get; protected set; } = 0; // 0 == Main, 1 == Sub, 2 == Throwable

    public int m_BulletPerMag = 10;
    public int m_Magcount = 5;


    // Member Variables
    public int m_LeftBullet { get; protected set; } = 0;
    public int m_LeftMag { get; protected set; } = 0;
    protected bool m_isDelayEnd = true;
    protected Transform m_Player_Arm;
    protected AimCursor m_aimCursor;
    protected Player m_Player;
    protected Player_Gun m_PlayerGun;

    protected Player_UI m_PlayerUIMgr;
    protected SoundMgr_SFX m_SoundMgrSFX;


    // Constructors
    public void InitWeapon(Transform _playerarm, AimCursor _aimcursor, Player _player, Player_Gun _playergun)
    {
        m_Player_Arm = _playerarm;
        m_aimCursor = _aimcursor;
        m_Player = _player;
        m_PlayerGun = _playergun;
    }

    // Updates


    // Physics


    // Functions
    public bool getPlayerWeaponCanReload()
    {
        if (m_LeftBullet <= m_BulletPerMag)
            return true;
        else
            return false;
    }
    public void setPlayerWeaponValue(WEAPON_PlayerParam _param)
    {
        m_BulletSpeed = _param.BulletSpeed;
        m_BulletDamage = _param.BulletDamage;
        m_StunValue = _param.StunValue;
        m_ShotDelay = _param.ShotDelay;
        m_BulletPerMag = _param.BulletPerMag;
        m_Magcount = _param.MagCount;

        m_LeftBullet = m_BulletPerMag;
        m_LeftMag = m_Magcount;
    }
    protected void setisDelayEndToTrue() { m_isDelayEnd = true; }

    // 기타 분류하고 싶은 것이 있을 경우
}