using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomNum
{
    EnemyTest,
    Stage01,
    Stage01_Test
}

public class EnemyManager : MonoBehaviour
{
    [field: SerializeField]
    public GameObject[] enemyPrefab { get; set; }


    public int enemyNum { get; set; }
    public int dieCount { get; set; }

    public int enemyWave { get; set; }

    public Player player { get; private set; }

    [field: SerializeField]
    public float respawnTime { get; set; }

    SpawnPosition[] spawnPositions;
    [field: SerializeField]
    public RoomNum roomNum { get; set; } = RoomNum.EnemyTest;

    private void Awake()
    {
        spawnPositions = GetComponentsInChildren<SpawnPosition>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        //respawnTime = 1.0f;
        dieCount = 0;
        enemyWave = 0;
        if (roomNum == RoomNum.Stage01_Test && enemyPrefab != null && spawnPositions !=null)
            Instantiate(enemyPrefab[0], spawnPositions[0].gameObject.transform);
    }

    public void PlusDieCount()
    {
        if(roomNum == RoomNum.EnemyTest)
        {

            // 모든 적이 사망
            if (++dieCount >= enemyNum)
            {
                // 웨이브 증가
                enemyWave++;
                dieCount = 0;
                // 리스폰
                Invoke(nameof(RespawnAllEnemy), respawnTime);
            }

        }
        
    }

    void RespawnAllEnemy()
    {
        for(int i = 0; i < enemyNum; i++)
        {
            // 일반 적 -> 스나이퍼 포함 적 -> 일반 적 순
            if (enemyWave % 2 == 1 && i == 2)
            {
                spawnPositions[i].SpawnEnemy(enemyPrefab[1]);
            }
            else
                spawnPositions[i].SpawnEnemy(enemyPrefab[0]);
        }
    }
}
