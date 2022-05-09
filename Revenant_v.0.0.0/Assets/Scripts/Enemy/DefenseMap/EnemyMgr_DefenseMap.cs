using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class EnemyMgr_DefenseMap : EnemyMgr
{
    public static EnemyMgr_DefenseMap Instance { get; private set; } = null;

    public List<IEnemySpawn> m_Wave1 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave2 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave3 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave4 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave5 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave6 { get; set; } = new List<IEnemySpawn>();

    public List<IEnemySpawn> m_Wave7 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave8 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave9 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave10 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave11 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave12 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave13 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave14 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave15 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave16 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave17 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave18 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave19 { get; set; } = new List<IEnemySpawn>();
    public List<IEnemySpawn> m_Wave20 { get; set; } = new List<IEnemySpawn>();


    public int m_waveIndex { get; set; } = 0;

    [field:SerializeField]
    public float m_spawnWaitTime { get; set; } = 3.0f; // 웨이브 간 스폰 딜레이

    bool m_spawnActive = false;

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& !m_spawnActive)
        {
            m_spawnActive = true;
            //Spawn();
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
                Invoke(nameof(Spawn), m_spawnWaitTime);
                //Spawn();
            }
        }

    }

    public void Spawn()
    {
        CancelInvoke(nameof(Spawn));
        if(m_waveIndex < 20)
        {

            Debug.Log("- WAVE "+(m_waveIndex + 1)+ " -");
            switch(m_waveIndex)
            {
                case 0:
                    if(m_Wave1.Count==0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");
                    //if(m_Waves[0] == null) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");
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

                case 6:
                    if (m_Wave6.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave7.Count;

                    foreach (var e in m_Wave7) e.setActive();
                    break;

                case 7:
                    if (m_Wave8.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave8.Count;

                    foreach (var e in m_Wave8) e.setActive();
                    break;

                case 8:
                    if (m_Wave9.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave9.Count;

                    foreach (var e in m_Wave9) e.setActive();
                    break;

                case 9:
                    if (m_Wave10.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave10.Count;

                    foreach (var e in m_Wave10) e.setActive();
                    break;

                case 10:
                    if (m_Wave11.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave11.Count;

                    foreach (var e in m_Wave11) e.setActive();
                    break;

                case 11:
                    if (m_Wave12.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave12.Count;

                    foreach (var e in m_Wave12) e.setActive();
                    break;

                case 12:
                    if (m_Wave13.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave6.Count;

                    foreach (var e in m_Wave13) e.setActive();
                    break;

                case 13:
                    if (m_Wave14.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave14.Count;

                    foreach (var e in m_Wave14) e.setActive();
                    break;
                case 14:
                    if (m_Wave15.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave15.Count;

                    foreach (var e in m_Wave15) e.setActive();
                    break;
                case 15:
                    if (m_Wave16.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave16.Count;

                    foreach (var e in m_Wave16) e.setActive();
                    break;
                case 16:
                    if (m_Wave17.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave17.Count;

                    foreach (var e in m_Wave17) e.setActive();
                    break;
                case 17:
                    if (m_Wave18.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave18.Count;

                    foreach (var e in m_Wave18) e.setActive();
                    break;
                case 18:
                    if (m_Wave19.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave19.Count;

                    foreach (var e in m_Wave19) e.setActive();
                    break;
                case 19:
                    if (m_Wave20.Count == 0) Debug.Log("<error> 웨이브에 적이 없습니다!! 프리팹을 드래그해주세요!");

                    m_dieMaxCount += m_Wave20.Count;

                    foreach (var e in m_Wave20) e.setActive();
                    break;

            }
            m_waveIndex++;
        }
        else
        {
            Debug.Log("<error> 20 웨이브를 초과했습니다!!");
        }
       
    }
}
