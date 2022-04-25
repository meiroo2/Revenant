using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr_DefenseMap : MonoBehaviour
{
    // ���̺� ������ ����
    // ���� ��� ����� ��� ���� ���� Ȱ��ȭ

    // Ư�� Ű�� ������ ���� ������ �����ؾ� ��

    // �迭 ������ ���� �������̽�ȭ�� �غ��� ���� ��

    public List<IEnemyType> m_Wave1 { get; set; } = new List<IEnemyType>();

    
    public List<IEnemyType> m_Wave2 { get; set; } = new List<IEnemyType>();

    public List<IEnemyType> m_Wave3 { get; set; } = new List<IEnemyType>();

    public List<IEnemyType> m_Wave4 { get; set; } = new List<IEnemyType>();
    public List<IEnemyType> m_Wave5 { get; set; } = new List<IEnemyType>();
    public List<IEnemyType> m_Wave6 { get; set; } = new List<IEnemyType>();

    [SerializeField]
    float m_spawnWaitTime = 3.0f; // ���� �� ���� ��� �ð�

    int m_waveIndex = 0; // ���̺� ��

    bool m_spawnActive = false;

    private void Awake()
    {
        
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& !m_spawnActive)
        {
            m_spawnActive = true;
            Spawn();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(m_spawnActive)
            {
                // ���� ���̺� ����
                Spawn();
            }
            else
            {
                Debug.Log("Q ��ư�� ���� �����ּ���!!");
            }
        }
    }

    public void Spawn()
    {
        if(m_waveIndex < 6)
        {
            Debug.Log("- WAVE "+(m_waveIndex + 1)+ " -");
            switch(m_waveIndex)
            {
                case 0:
                    if(m_Wave1.Count==0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    //Debug.Log(m_Wave1.Count);
                    foreach (var e in m_Wave1)
                    {
                        //e.getInfo();
                        e.setActive();
                    }
                
                    break;
                case 1:
                    if (m_Wave2.Count == 0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    //Debug.Log(m_Wave2.Count);
                    foreach (var e in m_Wave2)
                    {
                        
                        //e.getInfo();
                        e.setActive();
                    }
                    break;
                case 2:
                    if (m_Wave3.Count == 0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    foreach (var e in m_Wave3)
                    {
                        //e.getInfo();
                        e.setActive();
                    }
                    break;
                case 3:
                    if (m_Wave4.Count == 0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    foreach (var e in m_Wave4)
                    {
                        //e.getInfo();
                        e.setActive();
                    }
                    break;
                case 4:
                    if (m_Wave5.Count == 0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    foreach (var e in m_Wave5)
                    {
                        //e.getInfo();
                        e.setActive();
                    }
                    break;
                case 5:
                    if (m_Wave6.Count == 0)
                    {
                        Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    }
                    foreach (var e in m_Wave6)
                    {
                        //e.getInfo();
                        e.setActive();
                    }
                    break;
            }
            m_waveIndex++;
        }
        else
        {
            Debug.Log("<error> 6 ���̺긦 �ʰ��߽��ϴ�!!");
        }
       
    }
}
