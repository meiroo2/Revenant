using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BASEWEAPON : MonoBehaviour
{
    // Visible Member Variables
    public SoundMgr_SFX m_SoundMgrSFX;
    public GameObject m_BulletPrefab;
    public float m_BulletSpeed = 15;
    public int m_BulletDamage = 1;
    public int m_StunValue = 1;
    public int m_LeftBullet = 1;


    // Member Variables
    protected bool m_isDelayEnd = true;
    protected Transform m_Player_Arm;
    protected AimCursor m_aimCursor;
    protected Player m_Player;
    protected Player_Gun m_PlayerGun;
    [field: SerializeField] public float m_ShotDelay { get; private set; } = 0.5f;
    [field: SerializeField] public int m_WeaponType { get; protected set; } = 0; // 0 == Main, 1 == Sub, 2 == Throwable

    // Constructors
    public void InitWeapon(Transform _playerarm, AimCursor _aimcursor, Player _player, Player_Gun _playergun)
    {
        m_Player_Arm = _playerarm;
        m_aimCursor = _aimcursor;
        m_Player = _player;
        m_PlayerGun = _playergun;
    }

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public virtual bool Fire() { return true; }
    protected void setisDelayEndToTrue() { m_isDelayEnd = true; }

    // 기타 분류하고 싶은 것이 있을 경우
}