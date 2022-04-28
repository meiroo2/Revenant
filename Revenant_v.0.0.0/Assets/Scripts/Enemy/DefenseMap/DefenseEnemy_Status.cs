using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseEnemy_Status : Enemy
{
    // 추격 선딜
    [field: SerializeField]
    public float m_preChaseTime { get; set; } = 1.0f;

    // 시야 (추격)
    [field: SerializeField]
    public float m_guardSightDistance { get; set; } = 2.0f;    // 추격 거리
    // 사정 거리 (전투)
    [field: SerializeField]
    public float m_fightSightDistance { get; set; } = 1.5f;   // 공격 거리

    // 경직정도
    [field: SerializeField]
    public float m_stunStack { get; set; } = 0;

    // 사격 선딜
    [field: SerializeField]
    public float m_preFightTime { get; set; } = 1.0f;
    
}
