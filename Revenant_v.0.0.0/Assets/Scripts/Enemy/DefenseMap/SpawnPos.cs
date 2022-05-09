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
    GameObject m_wave7Prefab;

    [SerializeField]
    GameObject m_wave8Prefab;

    [SerializeField]
    GameObject m_wave9Prefab;

    [SerializeField]
    GameObject m_wave10Prefab;

    [SerializeField]
    GameObject m_wave11Prefab;

    [SerializeField]
    GameObject m_wave12Prefab;

    [SerializeField]
    GameObject m_wave13Prefab;

    [SerializeField]
    GameObject m_wave14Prefab;

    [SerializeField]
    GameObject m_wave15Prefab;

    [SerializeField]
    GameObject m_wave16Prefab;

    [SerializeField]
    GameObject m_wave17Prefab;

    [SerializeField]
    GameObject m_wave18Prefab;

    [SerializeField]
    GameObject m_wave19Prefab;

    [SerializeField]
    GameObject m_wave20Prefab;

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
        if (m_wave7Prefab)
        {
            prefab = Instantiate(m_wave7Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave7.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave7Prefab)
        {
            prefab = Instantiate(m_wave7Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave7.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave8Prefab)
        {
            prefab = Instantiate(m_wave8Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave8.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave9Prefab)
        {
            prefab = Instantiate(m_wave9Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave9.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave10Prefab)
        {
            prefab = Instantiate(m_wave10Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave10.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave11Prefab)
        {
            prefab = Instantiate(m_wave11Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave11.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave12Prefab)
        {
            prefab = Instantiate(m_wave12Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave12.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave13Prefab)
        {
            prefab = Instantiate(m_wave13Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave13.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave14Prefab)
        {
            prefab = Instantiate(m_wave14Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave14.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave15Prefab)
        {
            prefab = Instantiate(m_wave15Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave15.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave16Prefab)
        {
            prefab = Instantiate(m_wave16Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave16.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave17Prefab)
        {
            prefab = Instantiate(m_wave17Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave17.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave18Prefab)
        {
            prefab = Instantiate(m_wave18Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave18.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave19Prefab)
        {
            prefab = Instantiate(m_wave19Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave19.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
        if (m_wave20Prefab)
        {
            prefab = Instantiate(m_wave20Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave20.Add(prefab.GetComponent<IEnemySpawn>());

            prefab.SetActive(false);
        }
    }

    public void Init()
    {
        
    }
}
