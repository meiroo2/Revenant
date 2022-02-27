using System.Collections;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;
    EnemyManager enemyManager;

    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    void Start()
    {
        // �� ó�� �� ����, �� �����ڿ��� �� ���� �߰�
        enemyManager.enemyNum++;
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab)
        {
            Instantiate(enemyPrefab, transform);//enemySpawnPoint.transform.position, transform.rotation,);
            
        }
        else
            Debug.Log("No Prefab");
    }
}