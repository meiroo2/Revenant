﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_HotBox : MonoBehaviour, IHotBox
{
    // Visual Member Variables
    public HitBoxPoint p_HitBoxPoint;
    
    // Member Variables
    private BasicEnemy m_Enemy;
    private Player_UI m_PlayerUI;
    private SoundMgr_SFX m_SoundMgr;
    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
        m_ParentObj = m_Enemy.gameObject;
        m_HitBoxInfo = p_HitBoxPoint;
    }
    private void Start()
    {
        m_PlayerUI = InstanceMgr.GetInstance().m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_SoundMgr = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.BODY;
    
    public int HitHotBox(IHotBoxParam _param)
    {
        switch (p_HitBoxPoint)
        {
            case HitBoxPoint.HEAD:
                m_SoundMgr.playAttackedSound(MatType.Target_Head, transform.position);
                m_PlayerUI.ActiveHitmark(0);
                break;
            
            case HitBoxPoint.BODY:
                m_SoundMgr.playAttackedSound(MatType.Target_Body, transform.position);
                m_PlayerUI.ActiveHitmark(1);
                break;
        }
        
        m_Enemy.AttackedByWeapon(p_HitBoxPoint, _param.m_Damage,
            _param.m_stunValue);

        return 1;
    }

    private IEnumerator TempFunc_StopTimeScale()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }
}