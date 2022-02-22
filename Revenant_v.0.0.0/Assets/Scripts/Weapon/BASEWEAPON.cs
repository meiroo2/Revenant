using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BASEWEAPON : MonoBehaviour
{
    // Visible Member Variables
    public GameObject m_BulletPrefab;
    public float m_BulletSpeed;
    public float m_BulletDamage;

    // Member Variables
    protected Transform m_Player_Arm;
    protected AimCursor m_aimCursor;
    protected Player m_Player;
    protected Player_Gun m_PlayerGun;

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
    public virtual void Fire() { }

    // 기타 분류하고 싶은 것이 있을 경우
}