using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    TutoEnemyMgr m_tutoEnemyMgr;

    [SerializeField]
    GameObject targetPrefab;

    [SerializeField]
    GameObject[] targetSpawnPoints;

    void Awake()
    {
        Init();
        

    }

    public void Init()
    {
        foreach (var target in targetSpawnPoints)
        {
            Instantiate(targetPrefab, target.transform);
        }
    }

    void Update()
    {
        
    }
}
