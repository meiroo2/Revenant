using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMgr_DefenseMap : EnemyMgr
{

    public static EnemyMgr_DefenseMap Instance { get; private set; } = null;

    // ���̺� ������ ����
    // ���� ��� ����� ��� ���� ���� Ȱ��ȭ

    // Ư�� Ű�� ������ ���� ������ �����ؾ� ��

    // �迭 ������ ���� �������̽�ȭ�� �غ��� ���� ��

    public List<IEnemySpawn> m_Wave1 { get; set; } = new List<IEnemySpawn>();

    
    public List<IEnemySpawn> m_Wave2 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave3 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave4 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave5 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave6 { get; set; } = new List<IEnemySpawn>();

    [field:SerializeField]
    public float m_spawnWaitTime { get; set; } = 3.0f; // ���� �� ���� ��� �ð�

    int m_waveIndex = 0; // ���̺� ��

    bool m_spawnActive = false;

    private void Awake()
    {
        if(null == Instance)
        {
            //Debug.Log("new DefenseMap Instance");
            Instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& !m_spawnActive)
        {
            m_spawnActive = true;
            //Spawn();
            Invoke(nameof(Spawn), m_spawnWaitTime);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(m_spawnActive)
            {
                // ���� ���̺� ����
                //Spawn();
                Invoke(nameof(Spawn), m_spawnWaitTime);
            }
            else
            {
                Debug.Log("Q ��ư�� ���� �����ּ���!!");
            }
        }
    }

    public void PlusDieCount()
    {
        if (m_spawnActive)
        {
            m_dieCount++;
            if (m_dieCount >= m_dieMaxCount)
            {
                Debug.Log("All Kill");
                Invoke(nameof(Spawn), m_spawnWaitTime);
                //Spawn();
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
                    if(m_Wave1.Count==0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");

                    m_dieMaxCount += m_Wave1.Count;

                    foreach (var e in m_Wave1) e.setActive();

                    break;
                case 1:
                    if (m_Wave2.Count == 0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");
                    
                    m_dieMaxCount += m_Wave2.Count;

                    foreach (var e in m_Wave2) e.setActive();

                    break;
                case 2:
                    if (m_Wave3.Count == 0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");

                    m_dieMaxCount += m_Wave3.Count;

                    foreach (var e in m_Wave3) e.setActive();

                    break;

                case 3:
                    if (m_Wave4.Count == 0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");

                    m_dieMaxCount += m_Wave4.Count;

                    foreach (var e in m_Wave4) e.setActive();

                    break;

                case 4:
                    if (m_Wave5.Count == 0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");

                    m_dieMaxCount += m_Wave5.Count;

                    foreach (var e in m_Wave5) e.setActive();

                    break;
                case 5:
                    if (m_Wave6.Count == 0) Debug.Log("<error> ���̺꿡 ���� �����ϴ�!! �������� �巡�����ּ���!");

                    m_dieMaxCount += m_Wave6.Count;

                    foreach (var e in m_Wave6) e.setActive();
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
