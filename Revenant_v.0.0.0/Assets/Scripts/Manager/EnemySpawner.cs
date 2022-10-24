using System;
using System.Collections;
using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    // Visible Member Variables
    public bool p_CheckPlayerCollision = false;
    public float p_SpawnDelay = 0f;
    
    [Space(20f)]
    [Header("죽여야 할 적 목록")]
    public GameObject[] p_SpecificEnemysToDie;

    [Space(20f)]
    [Header("스폰될 적 목록(즉시 추격 상태로 시작)")]
    public GameObject[] p_WillSpawnEnemys;


    // Member Variables
    private bool m_SpawnerUsed = false;
    
    private BasicEnemy[] m_WillSpawnEnemyScripts;

    private bool m_IsPlayerCollide = false;
    private bool[] m_EnemyDeathChecker;
    
    private Coroutine m_DeActiveCoroutine;
    
    
    // Constructors
    protected void Awake()
    {
        m_WillSpawnEnemyScripts = new BasicEnemy[p_WillSpawnEnemys.Length];
        for (int i = 0; i < p_WillSpawnEnemys.Length; i++)
        {
            m_WillSpawnEnemyScripts[i] = p_WillSpawnEnemys[i].GetComponent<BasicEnemy>();
        }
        
        for (int i = 0; i < p_SpecificEnemysToDie.Length; i++)
        {
            p_SpecificEnemysToDie[i].GetComponent<BasicEnemy>().AddEnemySpawner(this);
        }

        m_EnemyDeathChecker = new bool[p_SpecificEnemysToDie.Length];
        for (int i = 0; i < m_EnemyDeathChecker.Length; i++)
        {
            m_EnemyDeathChecker[i] = false;
        }

        m_IsPlayerCollide = !p_CheckPlayerCollision;
    }

    protected void Start()
    {
        m_DeActiveCoroutine = StartCoroutine(DeActive());
    }

    private IEnumerator DeActive()
    {
        yield return null;
        
        for (int i = 0; i < p_WillSpawnEnemys.Length; i++)
        {
            p_WillSpawnEnemys[i].transform.parent = this.gameObject.transform;
            p_WillSpawnEnemys[i].SetActive(false);
        }
        
        yield break;
    }
    
    
    // Functions
    public void AchieveCollisionTrigger(bool _isOn)
    {
        m_IsPlayerCollide = _isOn;
        
        CheckEnemySpawn();
    }

    public virtual void AchieveEnemyDeath(GameObject _enemy)
    {
        Debug.Log(_enemy.name);
        int idx = -1;
        for (int i = 0; i < p_SpecificEnemysToDie.Length; i++)
        {
            if (p_SpecificEnemysToDie[i] == _enemy)
            {
                idx = i;
                break;
            }
        }

        if (idx != -1)
            m_EnemyDeathChecker[idx] = true;
        
        CheckEnemySpawn();
    }

    protected virtual void CheckEnemySpawn()
    {
        if (m_SpawnerUsed)
            return;

        for (int i = 0; i < m_EnemyDeathChecker.Length; i++)
        {
            if (m_EnemyDeathChecker[i] == false)
                return;
        }

        if (!m_IsPlayerCollide) 
            return;

        
        if (p_SpawnDelay == 0f)
        {
            SpawnAllEnemys();
        }
        else
        {
            StopCoroutine(SpawnCoroutine());
            StartCoroutine(SpawnCoroutine());
        }

        m_SpawnerUsed = true;
    }
    
    protected virtual void SpawnAllEnemys()
    {
        for (int i = 0; i < p_WillSpawnEnemys.Length; i++)
        {
            p_WillSpawnEnemys[i].SetActive(true);
        }
        for (int i = 0; i < p_WillSpawnEnemys.Length; i++)
        {
            m_WillSpawnEnemyScripts[i].StartPlayerCognition(true);
        }
    }

    protected virtual IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(p_SpawnDelay);
        SpawnAllEnemys();
    }
}