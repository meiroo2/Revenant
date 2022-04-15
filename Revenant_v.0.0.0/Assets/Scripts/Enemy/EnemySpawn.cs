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
    GameObject m_enemyPrefab;

    [SerializeField]
    GameObject[] m_enemySpawnPoints;

    

    void Awake()
    {
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
                        m_tutoEnemyMgr.m_turretList.Add(enemy.GetComponent<Turret_Controller>());
                        break;
                }
            }
        }
    }


    void Update()
    {
        
    }
}
