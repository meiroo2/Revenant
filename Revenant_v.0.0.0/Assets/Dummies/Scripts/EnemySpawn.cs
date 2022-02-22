using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    GameObject[] enemySpawnPoints;

    void Start()
    {
        foreach(var enemySpawnPoint in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, enemySpawnPoint.transform);//enemySpawnPoint.transform.position, transform.rotation,);
        }
        

    }

    void Update()
    {
        
    }
}
