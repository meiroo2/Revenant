using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseEnemy_Status : Enemy
{
    // �߰� ����
    [field: SerializeField]
    public float m_preChaseTime { get; set; } = 1.0f;

    // �þ� (�߰�)
    [field: SerializeField]
    public float m_guardSightDistance { get; set; } = 2.0f;    // �߰� �Ÿ�
    // ���� �Ÿ� (����)
    [field: SerializeField]
    public float m_fightSightDistance { get; set; } = 1.5f;   // ���� �Ÿ�

    // ��������
    [field: SerializeField]
    public float m_stunStack { get; set; } = 0;

    // ��� ����
    [field: SerializeField]
    public float m_preFightTime { get; set; } = 1.0f;
    
}
