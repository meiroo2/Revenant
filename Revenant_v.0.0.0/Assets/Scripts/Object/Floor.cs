using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ObjectDefine, IMatType, IBulletHit
{
    // Visible Member Variables
    [field: SerializeField] public MatType m_matType { get; set; } = MatType.Normal;

    // Member Variables
    private SoundMgr m_SoundMgr;

    // Constructors
    private void Awake()
    {
        InitObjectDefine(ObjectType.Floor, true, false);
        m_SoundMgr = GameObject.FindWithTag("SoundMgr").GetComponent<SoundMgr>();
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
    public void BulletHit(float _damage, Vector2 _contactPoint, HitPoints _hitPoints)
    {
        m_SoundMgr.playBulletHitSound(m_matType, _contactPoint);
    }

    // 기타 분류하고 싶은 것이 있을 경우
}