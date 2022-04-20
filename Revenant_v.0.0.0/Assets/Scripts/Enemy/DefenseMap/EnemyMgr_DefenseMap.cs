using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr_DefenseMap : MonoBehaviour
{
    // �ϳ��� ���� ������ ���ο� ���� ����ؾ� �ؼ�
    // �� ������ ����Ǿ�� ��.
    // ���� ��� ����� ��� ���� ���� ���


    // �迭 ������ ���� �������̽�ȭ�� �غ��� ���� ��
    public List<Enemy00> m_enemy00List { get; set; } = new List<Enemy00>();
    public List<Enemy01> m_enemy01List { get; set; } = new List<Enemy01>();
    public List<Enemy02> m_enemy02List { get; set; } = new List<Enemy02>();


    [SerializeField]
    float m_spawnWaitTime = 3.0f; // ���� �� ���� ��� �ð�

    int m_groupIndex = 0; // ���� ����
    public void ToggleEnemyList() // �� ó���� 0, 1�� �Ѵ� ����
                                  // �� ������ �ϳ��� ����(������)
    {
        switch(m_groupIndex)
        {
            case 0:
                foreach(var e in m_enemy00List)
                {
                    e.enabled = true;
                }
                break;
            case 1:
                foreach (var e in m_enemy01List)
                {
                    e.enabled = true;
                }
                break;
            case 2:
                foreach (var e in m_enemy02List)
                {
                    e.enabled = true;
                }
                break;
            default:
                Debug.Log("<Error> Group Index");
                break;
        }
    }
}
