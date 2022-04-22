using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr_DefenseMap : MonoBehaviour
{
    // �ϳ��� ���� ������ ���ο� ���� ����ؾ� �ؼ�
    // �� ������ ����Ǿ�� ��.
    // ���� ��� ����� ��� ���� ���� ���

    // Ư�� Ű�� ������ ���� ������ �����ؾ� ��

    // �迭 ������ ���� �������̽�ȭ�� �غ��� ���� ��

    public List<IEnemyType> m_Wave1 { get; set; } = new List<IEnemyType>();

    
    public List<IEnemyType> m_Wave2 { get; set; } = new List<IEnemyType>();

    [SerializeField]
    float m_spawnWaitTime = 3.0f; // ���� �� ���� ��� �ð�

    int m_groupIndex = 0; // ���� ����
    public void ToggleEnemyList()
    {

        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("- WAVE 1 -");
            foreach(var e in m_Wave1)
            {
                e.getInfo();
            }
            Debug.Log("- WAVE 2 -");
            foreach(var e in m_Wave2)
            {
                e.getInfo();
            }
        }
    }
}
