using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_HotBox : MonoBehaviour, IHotBox
{
    // Visual Member Variables
    [field: SerializeField, BoxGroup("HotBox Values")]
    public HitBoxPoint p_HitBoxPoint { get; private set; }

    [field: SerializeField, BoxGroup("HotBox Values")]
    public int p_DamageMulti { get; set; }


    // Member Variables
    private BasicEnemy m_Enemy;
    private Player_UI m_PlayerUI;
    private RageGauge_UI m_RageUI;
    private SoundPlayer m_SoundMgr;
    private Transform m_PlayerCenterTransform;
    
    // For SFX
    private ParticleMgr m_ParticleMgr;
    private HitSFXMaker m_HitSFXMaker;
    private SimpleEffectPuller m_SEPuller;
    
    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
        m_ParentObj = gameObject;
        m_HitBoxInfo = p_HitBoxPoint;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();

        m_PlayerUI = instance.m_MainCanvas.GetComponentInChildren<Player_UI>();
        m_RageUI = instance.m_MainCanvas.GetComponentInChildren<RageGauge_UI>();
        m_SoundMgr = instance.GetComponentInChildren<SoundPlayer>();
        m_PlayerCenterTransform = instance.GetComponentInChildren<Player_Manager>().m_Player.p_CenterTransform;
        m_SEPuller = instance.GetComponentInChildren<SimpleEffectPuller>();
        m_ParticleMgr = instance.GetComponentInChildren<ParticleMgr>();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
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
                if (_param.m_weaponType != WeaponType.BULLET_TIME)
                    m_HitSFXMaker.EnableNewObj(1, _param.m_contactPoint);
                else
                {
                    m_SEPuller.SpawnSimpleEffect(5, _param.m_contactPoint);
                }
                
                m_SoundMgr.playAttackedSound(MatType.Normal,  new Vector3(transform.position.x,transform.position.y, 0) );
                m_PlayerUI.ActiveHitmark(0);

                if (_param.m_MakeRageParticle)
                    m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 
                        () => m_RageUI.ChangeGaugeValue(m_RageUI.m_CurGaugeValue +
                                                        (_param.m_Damage * p_DamageMulti) *
                                                        m_RageUI.p_Gauge_Refill_Attack_Multi));
                
                break;
            
            case HitBoxPoint.BODY:
                if (_param.m_weaponType != WeaponType.BULLET_TIME)
                    m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
                else
                {
                    m_SEPuller.SpawnSimpleEffect(5, _param.m_contactPoint);
                }
                
                m_SoundMgr.playAttackedSound(MatType.Normal, new Vector3(transform.position.x,transform.position.y, 0) );
                m_PlayerUI.ActiveHitmark(1);

                if (_param.m_MakeRageParticle)
                    m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform, 
                        () => m_RageUI.ChangeGaugeValue(m_RageUI.m_CurGaugeValue +
                                                        (_param.m_Damage * p_DamageMulti) *
                                                        m_RageUI.p_Gauge_Refill_Attack_Multi));
                
                break;
            
            case HitBoxPoint.COGNITION:
                
                break;
        }
        
        // 파티클 생성 필요(함수 넘겨야 함)
        
        
        m_Enemy.AttackedByWeapon(p_HitBoxPoint, _param.m_Damage * p_DamageMulti, _param.m_stunValue);

        return 1;
    }
}