using System.Collections;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    
    EnemyManager enemyManager;

    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Start()
    {
        // �� ó�� �� ����, �� �����ڿ��� �� ���� �߰�
        enemyManager.enemyNum++;
        Invoke(nameof(spawnFirstEnemy), 3f);
    }

    void spawnFirstEnemy()
    {
        if (enemyManager.enemyPrefab[0])
        {
            Instantiate(enemyManager.enemyPrefab[0], transform);//enemySpawnPoint.transform.position, transform.rotation,);

        }
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        

            if (enemyPrefab)
            {
                Instantiate(enemyPrefab, transform);//enemySpawnPoint.transform.position, transform.rotation,);

            }
            else
                Debug.Log("No Prefab");

        
    }
}
