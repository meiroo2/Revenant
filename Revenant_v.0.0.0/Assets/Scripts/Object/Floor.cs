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
    private SoundPlayer _mSoundPlayer;

    // Constructors
    private void Awake()
    {
        m_ParentObj = this.gameObject;
        m_ObjectType = ObjectType.Floor;
        m_CanAttacked = true;
    }
    private void Start()
    {
        _mSoundPlayer = InstanceMgr.GetInstance().GetComponentInChildren<SoundPlayer>();
    }

    // Updates


    // Physics


    // Functions
    public int HitHotBox(IHotBoxParam _param)
    {
        _mSoundPlayer.playAttackedSound(m_matType, _param.m_contactPoint);

        return 1;
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}