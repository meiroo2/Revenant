using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [SerializeField]
    GameObject m_enemyPrefab;

    [SerializeField]
    GameObject m_wave1Prefab;

    [SerializeField]
    GameObject m_wave2Prefab;



    [SerializeField]
    EnemyMgr_DefenseMap m_enemyMgr;

    SpriteRenderer m_sprite;

    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_sprite.enabled = false;
    }

    public void AllWaveInit()
    {
        //wave 1 ~ 2 spawn
        GameObject prefab;

        if (m_wave1Prefab)
        {

            prefab = Instantiate(m_wave1Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave1.Add(prefab.GetComponent<IEnemyType>());
        }

        if (m_wave2Prefab)
        {
            prefab = Instantiate(m_wave2Prefab);
            prefab.transform.position = transform.position;

            m_enemyMgr.m_Wave2.Add(prefab.GetComponent<IEnemyType>());
        }
            


    }

    public void Init()
    {
        
    }
}
