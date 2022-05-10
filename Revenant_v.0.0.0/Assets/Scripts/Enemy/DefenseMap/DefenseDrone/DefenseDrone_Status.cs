using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDrone_Status : Enemy
{
    // 돌진 이동속도
    [field: Header("돌진 이동속도")]
    [field: SerializeField]
    public float m_rushSpeed { get; set; } = 0.8f;

    // 폭파 데미지
    [field: Header("폭파 데미지")]
    [field: SerializeField]
    public int m_damage { get; set; } = 10;

    [field: Header("폭파 딜레이 (n초 후에 데미지)")]
    [field: SerializeField]
    // 폭파 데미지 들어가는 딜레이(n초 후에 데미지)
    public float m_explodeDelayTime { get; set; } = 0.5f;

    [field: Header("폭파 딜레이 (원의 반지름)")]
    [field: SerializeField]
    // 폭파 범위
    public float m_explosionRange { get; set; } = 0.5f;

    [field: Header("준비->돌진 대기시간")]
    [field: SerializeField]
    // 돌진 준비 시간
    public float m_rushDelayTime { get; set; } = 0.75f;

    // 드론 인지 범위 (사정거리)
    [field: Header("드론 인지 범위 (사정거리)")]
    [field: SerializeField]
    public float m_detectRange { get; set; } = 1.2f;

}
