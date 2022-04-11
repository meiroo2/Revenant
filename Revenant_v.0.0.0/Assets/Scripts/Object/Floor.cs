using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IHotBox
{
    // Visible Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Normal;

    // Member Variables
    private SoundMgr_SFX m_SoundMgrSFX;

    // Constructors
    private void Awake()
    {
        InitObjectDefine(ObjectType.Floor, true, false);
    }
    private void Start()
    {
        m_SoundMgrSFX = GameManager.GetInstance().GetComponentInChildren<SoundMgr_SFX>();
    }

    // Updates


    // Physics


    // Functions
    public void HitHotBox(IHotBoxParam _param)
    {
        m_SoundMgrSFX.playAttackedSound(m_matType, _param.m_contactPoint);
    }

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}