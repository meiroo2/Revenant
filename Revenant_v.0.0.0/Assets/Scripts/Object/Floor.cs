using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IBulletHit
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
    public void BulletHit(BulletHitInfo _bulletHitInfo)
    {
        m_SoundMgrSFX.playBulletHitSound(m_matType, _bulletHitInfo.m_ContactPoint);
    }

    // 기타 분류하고 싶은 것이 있을 경우
}