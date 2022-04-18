using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    EnemyType m_enemyType;

    [SerializeField]
    TutoEnemyMgr m_tutoEnemyMgr;

    [SerializeField]
    TutoRoom03EnemyMgr m_tuto3EnemyMgr;

    [SerializeField]
    GameObject m_enemyPrefab;

    [SerializeField]
    GameObject[] m_enemySpawnPoints;

    [SerializeField]
    SpriteRenderer[] m_spawnSprites;

    void Awake()
    {
        foreach(var s in m_spawnSprites)
        {
            if (s)
                s.enabled = false;
        }
        
        Init();
        
    }
    private void Start()
    {
        
    }

    public void Init()
    {
        foreach (var p in m_enemySpawnPoints)
        {
            GameObject enemy = Instantiate(m_enemyPrefab);
            enemy.transform.position = p.transform.position;

            if(m_enemyType.m_stage == STAGE.TUTORIAL)
            {
                switch (m_enemyType.m_tutorialEnemyType)
                {
                    case TUTORIAL.TARGETBOARD:
                        m_tutoEnemyMgr.m_TargetList.Add(enemy.GetComponent<TargetBoard_Controller>());
                        break;
                    case TUTORIAL.DRONE:
                        m_tutoEnemyMgr.m_droneList.Add(enemy.GetComponent<Drone_Controller>());
                        break;
                    case TUTORIAL.TURRET:
                        m_tuto3EnemyMgr.m_turretList.Add(enemy.GetComponent<Turret_Controller>());
                        break;
                }
            }
        }
    }


    void Update()
    {
        
    }
}
