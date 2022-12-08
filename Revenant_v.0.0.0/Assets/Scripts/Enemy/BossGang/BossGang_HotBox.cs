using Sirenix.OdinInspector;
using UnityEngine;


public class BossGang_HotBox : MonoBehaviour, IHotBox
{
    // Visual Member Variables
    [field: SerializeField, BoxGroup("HotBox Values")]
    public MatType p_MatType;
    
    [field: SerializeField, BoxGroup("HotBox Values")]
    public HitBoxPoint p_HitBoxPoint { get; private set; }

    [field: SerializeField, BoxGroup("HotBox Values")]
    public int p_DamageMulti { get; set; }
    
    
    // Member Variables
    public BossGang m_Enemy { get; private set; }
    private Player_UI m_PlayerUI;
    private RageGauge m_RageGauge;
    private SoundPlayer m_SoundPlayer;
    private Transform m_PlayerCenterTransform;
    
    
    // For SFX
    private ParticleMgr m_ParticleMgr;
    private HitSFXMaker m_HitSFXMaker;
    private SimpleEffectPuller m_SEPuller;
    
    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BossGang>();
        m_ParentObj = gameObject;
        m_HitBoxInfo = p_HitBoxPoint;
    }

    private void Start()
    {
        var instance = InstanceMgr.GetInstance();

        m_PlayerUI = instance.m_Player_UI;
        
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_PlayerCenterTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().p_CenterTransform;
        m_RageGauge = instance.m_MainCanvas.GetComponentInChildren<RageGauge>();
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
        if (m_Enemy.m_CurBossStateName == BossStateName.ULTIMATE)
        {
            return 0;
        }
        
        if (p_HitBoxPoint == HitBoxPoint.HEAD)
        {
            switch (_param.m_weaponType)
            {
                case WeaponType.BULLET_TIME:
                    m_SEPuller.SpawnSimpleEffect(5, _param.m_contactPoint);
                    break;
                
                case WeaponType.BULLET:
                    m_HitSFXMaker.EnableNewObj(1, _param.m_contactPoint);
                    break;
                
                case WeaponType.KNIFE:
                    m_SoundPlayer.PlayEnemySoundOnce(2, gameObject);
                    m_HitSFXMaker.EnableNewObj(1, _param.m_contactPoint);
                    break;
                
                default:
                    return 0;
                    break;
            }
            
            m_SoundPlayer.PlayHitSoundByMatType(p_MatType, transform);
            m_PlayerUI.ActiveHitmark(0);

            m_Enemy.AttackedByWeapon(p_HitBoxPoint, _param.m_Damage * p_DamageMulti,
                _param.m_stunValue, _param.m_weaponType);
            
            if (_param.m_MakeRageParticle)
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform,
                    () => m_RageGauge.AddGaugeValue(
                        (_param.m_Damage * p_DamageMulti) * m_RageGauge.p_Gauge_Refill_Attack_Multi
                    )
                );

            return 1;
        }
        else if (p_HitBoxPoint == HitBoxPoint.BODY)
        {
            switch (_param.m_weaponType)
            {
                case WeaponType.BULLET_TIME:
                    m_SEPuller.SpawnSimpleEffect(5, _param.m_contactPoint);
                    break;
                
                case WeaponType.BULLET:
                    m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
                    break;
                
                case WeaponType.KNIFE:
                    m_SoundPlayer.PlayEnemySoundOnce(2, gameObject);
                    m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
                    break;
                
                case WeaponType.GRENADE:
                    // Only Damage
                    break;
                
                default:
                    return 0;
                    break;
            }
            
            
            m_SoundPlayer.PlayHitSoundByMatType(p_MatType, transform);
            m_PlayerUI.ActiveHitmark(1);

            if (_param.m_MakeRageParticle)
                m_ParticleMgr.MakeParticle(_param.m_contactPoint, m_PlayerCenterTransform,
                    () => m_RageGauge.AddGaugeValue(
                        (_param.m_Damage * p_DamageMulti) * m_RageGauge.p_Gauge_Refill_Attack_Multi
                    )
                );
                
            m_Enemy.AttackedByWeapon(p_HitBoxPoint, _param.m_Damage * p_DamageMulti,
                _param.m_stunValue, _param.m_weaponType);
            
            return 1;
        }
        else if (p_HitBoxPoint == HitBoxPoint.COGNITION)
        {
            switch (_param.m_weaponType)
            {
                case WeaponType.MOUSE:
                    m_Enemy.MouseTouched(_param.m_Damage > 0 ? true : false);
                    break;
            }
            
            return 1;
        }

        return 0;
    }
}