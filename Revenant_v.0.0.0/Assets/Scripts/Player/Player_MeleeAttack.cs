using System;
using System.Collections.Generic;
using UnityEngine;


public class Player_MeleeAttack : MonoBehaviour
{
    // Visible Member Variables
    public int m_Damage = 10;
    public int m_StunValue = 10;
    
    // Member Variables
    private SoundPlayer m_SoundSFXMgr;
    private HitSFXMaker m_SFXMaker;
    private BoxCollider2D m_MeleeCol;
    private Player m_Player;


    // Constructors
    private void Awake()
    {
        // Assignment
        m_MeleeCol = GetComponent<BoxCollider2D>();
        
        // Init
        m_MeleeCol.enabled = false;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_SFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SoundSFXMgr = instance.GetComponentInChildren<SoundPlayer>();
        m_Player = instance.GetComponentInChildren<Player_Manager>().m_Player;
    }


    // Functions
    public void StartMelee()
    {
        m_MeleeCol.enabled = true;
    }

    public void StopMelee()
    {
        m_MeleeCol.enabled = false;
    }
    
    
    
    // Physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IHotBox hotBox))
        {
            if (hotBox.m_HitBoxInfo is HitBoxPoint.BODY or HitBoxPoint.OBJECT)
            {
                hotBox.HitHotBox(new IHotBoxParam(m_Damage, m_StunValue,
                    transform.position, WeaponType.KNIFE));
                
                m_SFXMaker.EnableNewObj(3, col.ClosestPoint(transform.position), m_Player.m_IsRightHeaded);
                m_SFXMaker.EnableNewObj(2, col.ClosestPoint(transform.position),m_Player.m_IsRightHeaded);
                m_SoundSFXMgr.playGunFireSound(0, col.transform.position);
                m_MeleeCol.enabled = false;
            }
        }
        else
        {
            Debug.Log("ERR : Player_MeleeAttack에서 IHotBox 탐색 불가");
        }
    }
}