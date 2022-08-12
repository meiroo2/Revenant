﻿using System;
using Unity.VisualScripting;
using UnityEngine;


public class Target_HotBox : MonoBehaviour, IHotBox
{
    public HitBoxPoint p_Point;
    
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; }
    private Target m_Target;
    private RageGauge_UI m_RageUI;

    private void Awake()
    {
        m_HitBoxInfo = p_Point;
        m_Target = GetComponentInParent<Target>();
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        
        m_RageUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        switch (p_Point)
        {
            case HitBoxPoint.HEAD:
                m_RageUI.AddValueToRageGauge(0.2f);
                m_Target.PrintTxt(p_Point + " " + _param.m_Damage * 2 + " DMG");
                break;
            
            case HitBoxPoint.BODY:
                m_RageUI.AddValueToRageGauge(0.1f);
                m_Target.PrintTxt(p_Point + " " + _param.m_Damage + " DMG");
                break;
        }
        
        return 1;
    }
}