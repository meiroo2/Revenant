using System;
using System.Collections.Generic;
using UnityEngine;


public class Player_MeleeAttack : MonoBehaviour
{
    // Visible Member Variables
    public int m_Damage = 10;
    public int m_StunValue = 10;
    
    // Member Variables
    private SoundPlayer m_SoundPlayer;
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
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
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

                m_Player.m_SimpleEffectPuller.SpawnSimpleEffect(7, col.ClosestPoint(transform.position),
                    !m_Player.m_IsRightHeaded);
               // m_SFXMaker.EnableNewObj(3, col.ClosestPoint(transform.position), m_Player.m_IsRightHeaded);
                m_SFXMaker.EnableNewObj(2, col.ClosestPoint(transform.position),m_Player.m_IsRightHeaded);
               
                m_SoundPlayer.PlayPlayerSoundOnce(3);
                
                m_MeleeCol.enabled = false;
            }
        }
        else
        {
            Debug.Log("ERR : Player_MeleeAttack에서 IHotBox 탐색 불가");
        }
    }
}