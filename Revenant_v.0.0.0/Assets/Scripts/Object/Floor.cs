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
    public HitBoxPoint m_HitBoxInfo { get; set; } = HitBoxPoint.OBJECT;
    private SoundMgr_SFX m_SoundMgrSFX;

    // Constructors
    private void Awake()
    {
        m_ParentObj = this.gameObject;
        m_ObjectType = ObjectType.Floor;
        m_CanAttacked = true;
    }
    private void Start()
    {
        m_SoundMgrSFX = InstanceMgr.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    // Updates


    // Physics


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        m_SoundMgrSFX.playAttackedSound(m_matType, _param.m_contactPoint);

        return 1;
    }

    // 기타 분류하고 싶은 것이 있을 경우
}