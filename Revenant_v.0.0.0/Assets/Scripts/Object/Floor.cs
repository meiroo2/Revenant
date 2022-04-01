using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IAttacked
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
    /*
    <커스텀 초기화 함수가 필요할 경우>
    public void Init()
    {

    }
    */

    // Updates
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    // Physics


    // Functions
    public void Attacked(AttackedInfo _AttackedInfo)
    {
        m_SoundMgrSFX.playAttackedSound(m_matType, _AttackedInfo.m_ContactPoint);
    }

    // 기타 분류하고 싶은 것이 있을 경우
}