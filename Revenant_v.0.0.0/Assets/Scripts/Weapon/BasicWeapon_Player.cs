using System;
using System.Collections;
using UnityEngine;
using UnityEditor;



public class BasicWeapon_Player : BasicWeapon
{
    // Visible Member Variables
    [HideInInspector] public float p_ReloadTime;

    
    // Member Variables
    protected HitSFXMaker m_HitSFXMaker;
    protected ShellMgr m_ShellMgr;
    protected Transform m_Player_Arm;
    protected AimCursor m_AimCursor;
    protected Player m_Player;
    protected Player_UI m_PlayerUI;
    protected BallLauncher m_BallLauncher;
    
    public bool m_isReloading { get; protected set; }
    
    
    // Constructors
    protected void Start()
    {
        var instance = InstanceMgr.GetInstance();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
    }


    // Functions
    protected virtual void Internal_Reload()
    {
    }

    public virtual void ReloadWeaponData()
    {
    }
}