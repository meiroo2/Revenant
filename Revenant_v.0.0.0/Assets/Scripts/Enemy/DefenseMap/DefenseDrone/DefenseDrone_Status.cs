using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDrone_Status : Enemy
{
    // ���� �̵��ӵ�
    [field: Header("���� �̵��ӵ�")]
    [field: SerializeField]
    public float m_rushSpeed { get; set; } = 0.8f;

    // ���� ������
    [field: Header("���� ������")]
    [field: SerializeField]
    public int m_damage { get; set; } = 10;

    [field: Header("���� ������ (n�� �Ŀ� ������)")]
    [field: SerializeField]
    // ���� ������ ���� ������(n�� �Ŀ� ������)
    public float m_explodeDelayTime { get; set; } = 0.5f;

    [field: Header("���� ������ (���� ������)")]
    [field: SerializeField]
    // ���� ����
    public float m_explosionRange { get; set; } = 0.5f;

    [field: Header("�غ�->���� ���ð�")]
    [field: SerializeField]
    // ���� �غ� �ð�
    public float m_rushDelayTime { get; set; } = 0.75f;

    // ��� ���� ���� (�����Ÿ�)
    [field: Header("��� ���� ���� (�����Ÿ�)")]
    [field: SerializeField]
    public float m_detectRange { get; set; } = 1.2f;

}
