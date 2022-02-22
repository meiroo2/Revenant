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

    public BASEWEAPON m_Weapon;

    // Member Variables


    // Constructors
    private void Awake()
    {
        
    }
    private void Start()
    {
        m_Weapon.InitWeapon(m_Player_Arm, m_aimCursor, m_Player, this);
    }

    // Updates
    private void Update()
    {
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


    // 기타 분류하고 싶은 것이 있을 경우
}