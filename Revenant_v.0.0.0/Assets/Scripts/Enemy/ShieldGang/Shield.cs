using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Shield : MonoBehaviour, IHotBox, IMatType
{
    // Visible Member Variables
    public int p_Shield_Hp = 30;
    public int p_ShieldDmgMulti = 1;
    [field: SerializeField]public MatType m_matType { get; set; }
    
    
    // Member Variables
    private ShieldGang m_ShieldGang;
    public GameObject m_ParentObj { get; set; }
    public int m_hotBoxType { get; set; } = 1;
    public bool m_isEnemys { get; set; } = true;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;

    private Collider2D m_Collider;
    private List<Rigidbody2D> m_SpriteRigids = new List<Rigidbody2D>();

    private SoundPlayer m_SoundPlayer;

    private SimpleEffectPuller m_SEPuller;
    
    // Constructors
    private void Awake()
    {
        m_ParentObj = gameObject;

        m_Collider = GetComponent<Collider2D>();

        var SpRenders = GetComponentsInChildren<SpriteRenderer>();
        foreach (var element in SpRenders)
        {
            var rigid = element.GetComponent<Rigidbody2D>();
            m_SpriteRigids.Add(rigid);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.isKinematic = true;
        }
        
        m_ShieldGang = GetComponentInParent<ShieldGang>();
    }

    private void Start()
    {
        m_SEPuller = InstanceMgr.GetInstance().GetComponentInChildren<SimpleEffectPuller>();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
    }


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_SEPuller.SpawnSimpleEffect(14, _param.m_contactPoint);
        
        m_SoundPlayer.PlayHitSoundByMatType(m_matType, transform.position);
        
        if (m_ShieldGang.UpdateShieldDmg(_param.m_Damage * m_ShieldGang.p_Shield_Dmg_Multi) == 1)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    
}