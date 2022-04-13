using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IHotBox
{
    // Visible Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Normal;

    // Member Variables
    public bool m_isEnemys { get; set; } = false;
    public int m_hotBoxType { get; set; } = 1;
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

    // 기타 분류하고 싶은 것이 있을 경우
}