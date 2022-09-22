using System;
using Unity.VisualScripting;
using UnityEngine;


public class Target_HotBox : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public HitBoxPoint p_Point;
    [field: SerializeField] public int p_DamageMultiples { get; private set; } = 1;
    
    
    // Member Variables
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; }
    private Target m_Target;
    private RageGauge_UI m_RageUI;

    private SoundPlayer m_SoundPlayer;

    private Player_UI m_PlayerUI;
    private ParticleMgr m_ParticleMgr;
    private HitSFXMaker m_HitSFXMaker;
    private Transform m_PlayerCenterTransform;
    

    private void Awake()
    {
        m_ParentObj = this.gameObject;
        
        m_HitBoxInfo = p_Point;
        m_Target = GetComponentInParent<Target>();
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        
        m_RageUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
        m_PlayerUI = instance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_ParticleMgr = instance.GetComponentInChildren<ParticleMgr>();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_PlayerCenterTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.p_CenterTransform;
        m_SoundPlayer = instance.GetComponentInChildren<SoundPlayer>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        switch (p_Point)
        {
            case HitBoxPoint.HEAD:
                m_HitSFXMaker.EnableNewObj(1, _param.m_contactPoint);
                m_PlayerUI.ActiveHitmark(0);
                m_SoundPlayer.playAttackedSound(MatType.Normal,  transform.position );
                m_Target.PrintTxt(p_Point + " " + _param.m_Damage * 2 + " DMG");
                
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 8f,
                    () => m_RageUI.ChangeGaugeValue(m_RageUI.m_CurGaugeValue +
                                                    (_param.m_Damage * p_DamageMultiples) *
                                                    m_RageUI.p_Gauge_Refill_Attack_Multi));
                
                break;
            
            case HitBoxPoint.BODY:
                m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
                m_PlayerUI.ActiveHitmark(1);
                m_SoundPlayer.playAttackedSound(MatType.Normal, transform.position) ;
                m_Target.PrintTxt(p_Point + " " + _param.m_Damage + " DMG");
                
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 8f,
                    () => m_RageUI.ChangeGaugeValue(m_RageUI.m_CurGaugeValue +
                                                    (_param.m_Damage * p_DamageMultiples) *
                                                    m_RageUI.p_Gauge_Refill_Attack_Multi));
                
                break;
        }
        
        return 1;
    }
}