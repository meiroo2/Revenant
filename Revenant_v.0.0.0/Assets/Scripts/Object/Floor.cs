using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IHotBox
{
    // Visible Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Normal;

    // Member Variables
    public GameObject m_ParentObj { get; set; }
    public bool m_isEnemys { get; set; } = false;
    public int m_hotBoxType { get; set; } = 1;
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.FLOOR;
    private SoundPlayer m_SoundPlayer;
    
    
    // Member Variables
    private HitSFXMaker m_HitSFXMaker;
    

    // Constructors
    private void Awake()
    {
        m_ParentObj = this.gameObject;
        m_ObjectType = ObjectType.Floor;
        m_CanAttacked = true;
    }
    private void Start()
    {
        var instance = InstanceMgr.GetInstance();

        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
    }

    // Updates


    // Physics


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_SoundPlayer.PlayHitSoundByMatType(m_matType, _param.m_contactPoint);

        m_HitSFXMaker.EnableNewObj(0, _param.m_contactPoint);
        
        return 1;
    }
}