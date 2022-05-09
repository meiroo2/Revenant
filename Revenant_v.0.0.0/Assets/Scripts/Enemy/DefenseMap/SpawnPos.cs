using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{

    [SerializeField]
    GameObject m_wave1Prefab;

    [SerializeField]
    GameObject m_wave2Prefab;

    [SerializeField]
    GameObject m_wave3Prefab;

    [SerializeField]
    GameObject m_wave4Prefab;

    [SerializeField]
    GameObject m_wave5Prefab;

    [SerializeField]
    GameObject m_wave6Prefab;

    [SerializeField]
    EnemyMgr_DefenseMap m_enemyMgr;

    SpriteRenderer m_sprite;

    private void Awake()
    {
        // Square Áö¿ì±â
        m_sprite = GetComponent<SpriteRenderer>();
        m_sprite.enabled = false;
    }

    public void InitAll()
    {
        //wave 1 ~ 2 spawn
        GameObject prefab;

        if (m_wave1Prefab)
        {

            prefab = Instantiate(m_wave1Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave1.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }

        if (m_wave2Prefab)
        {
            prefab = Instantiate(m_wave2Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave2.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if(m_wave3Prefab)
        {
            prefab = Instantiate(m_wave3Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave3.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave4Prefab)
        {
            prefab = Instantiate(m_wave4Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave4.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave5Prefab)
        {
            prefab = Instantiate(m_wave5Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave5.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave6Prefab)
        {
            prefab = Instantiate(m_wave6Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave6.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
    }

    public void Init()
    {
        
    }
}
