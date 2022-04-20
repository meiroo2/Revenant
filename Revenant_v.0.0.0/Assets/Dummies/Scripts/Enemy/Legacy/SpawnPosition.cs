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
        // 트레이닝 룸에서만
        if(enemyManager.roomNum == RoomNum.EnemyTest)
        {
            // 맨 처음 적 생성, 적 관리자에게 적 수를 추가
            enemyManager.enemyNum++;
            Invoke(nameof(spawnFirstEnemy), 3f);
        }
        
        
    }

    void spawnFirstEnemy()
    {
        if (enemyManager.enemyNum > 0)
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
