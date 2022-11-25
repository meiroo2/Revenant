using System;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;


public class Target_HotBox : MonoBehaviour, IHotBox
{
    // Visible Member Variables
    public HitBoxPoint p_Point;
    [field: SerializeField] public int p_DamageMultiples { get; private set; } = 1;
    public MatType p_MatType;
    
    
    // Member Variables
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 0;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; }
    private Target m_Target;
    private RageGauge m_RageUI;

    private SoundPlayer m_SoundPlayer;

    private Player_UI m_PlayerUI;
    private ParticleMgr m_ParticleMgr;
    private HitSFXMaker m_HitSFXMaker;
    private Transform m_PlayerCenterTransform;
    private SimpleEffectPuller m_SEPuller;
    

    private void Awake()
    {
        m_ParentObj = this.gameObject;
        
        m_HitBoxInfo = p_Point;
        m_Target = GetComponentInParent<Target>();
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();
        
        m_RageUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge>();
        m_PlayerUI = instance.m_Player_UI;
        m_ParticleMgr = instance.GetComponentInChildren<ParticleMgr>();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_PlayerCenterTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().p_CenterTransform;
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_SEPuller = instance.GetComponentInChildren<SimpleEffectPuller>();
    }

    public int HitHotBox(IHotBoxParam _param)
    {
        EventInstance eventInstance;
        
        switch (p_Point)
        {
            case HitBoxPoint.HEAD:
                if (_param.m_weaponType != WeaponType.BULLET_TIME)
                    m_HitSFXMaker.EnableNewObj(1, _param.m_contactPoint);
                else
                {
                    m_SEPuller.SpawnSimpleEffect(5, _param.m_contactPoint);
                }
                
                m_PlayerUI.ActiveHitmark(0);

                m_SoundPlayer.PlayHitSoundByMatType(p_MatType, transform);
       

                m_Target.PrintTxt(p_Point + " " + _param.m_Damage * 2 + " DMG");
                
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 
                    () => m_RageUI.AddGaugeValue((_param.m_Damage * p_DamageMultiples) *
                                                 m_RageUI.p_Gauge_Refill_Attack_Multi) );
                
                break;
            
            case HitBoxPoint.BODY:
                m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
                m_PlayerUI.ActiveHitmark(1);
                
                m_SoundPlayer.PlayHitSoundByMatType(p_MatType, transform);

                m_Target.PrintTxt(p_Point + " " + _param.m_Damage + " DMG");
                
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 
                    () => m_RageUI.AddGaugeValue((_param.m_Damage * p_DamageMultiples) *
                                                 m_RageUI.p_Gauge_Refill_Attack_Multi) );
                
                break;
        }
        
        return 1;
    }
}