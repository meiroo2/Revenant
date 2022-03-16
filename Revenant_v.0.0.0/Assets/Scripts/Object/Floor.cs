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
        m_SoundMgrSFX = GameObject.FindWithTag("SoundMgr").GetComponent<SoundMgr_SFX>();
    }
    private void Start()
    {

    }
    /*
    <Ŀ���� �ʱ�ȭ �Լ��� �ʿ��� ���>
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

    // ��Ÿ �з��ϰ� ���� ���� ���� ���
}