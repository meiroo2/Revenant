using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMgr_DefenseMap : EnemyMgr
{

    public static EnemyMgr_DefenseMap Instance { get; private set; } = null;

    // 웨이브 단위로 진행
    // 조가 모두 사망할 경우 다음 조를 활성화

    // 특정 키를 누르면 다음 스폰을 진행해야 함

    // 배열 관리를 위해 인터페이스화를 해봐도 좋을 듯

    public List<IEnemySpawn> m_Wave1 { get; set; } = new List<IEnemySpawn>();

    
    public List<IEnemySpawn> m_Wave2 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave3 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave4 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave5 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave6 { get; set; } = new List<IEnemySpawn>();

    [SerializeField]
    float m_spawnWaitTime = 3.0f; // 시작 후 스폰 대기 시간

    int m_waveIndex = 0; // 웨이브 수

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
            Spawn();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(m_spawnActive)
            {
                // 강제 웨이브 스폰
                Spawn();
            }
            else
            {
                Debug.Log("Q 버튼을 먼저 눌러주세요!!");
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
                Spawn();
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
                    if(m_Wave1.Count==0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave1.Count;

                    foreach (var e in m_Wave1) e.setActive();

                    break;
                case 1:
                    if (m_Wave2.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");
                    
                    m_dieMaxCount += m_Wave2.Count;

                    foreach (var e in m_Wave2) e.setActive();

                    break;
                case 2:
                    if (m_Wave3.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave3.Count;

                    foreach (var e in m_Wave3) e.setActive();

                    break;

                case 3:
                    if (m_Wave4.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave4.Count;

                    foreach (var e in m_Wave4) e.setActive();

                    break;

                case 4:
                    if (m_Wave5.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave5.Count;

                    foreach (var e in m_Wave5) e.setActive();

                    break;
                case 5:
                    if (m_Wave6.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave6.Count;

                    foreach (var e in m_Wave6) e.setActive();
                    break;
            }
            m_waveIndex++;
        }
        else
        {
            Debug.Log("<error> 6 웨이브를 초과했습니다!!");
        }
       
    }
}
