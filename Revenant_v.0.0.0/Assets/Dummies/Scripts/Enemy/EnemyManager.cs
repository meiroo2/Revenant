using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomNum
{
    EnemyTest,
    FireTest01,
    FireTest02,
}

public class EnemyManager : MonoBehaviour
{
    

    public int enemyNum { get; set; }
    public int dieCount { get; set; }

    [field: SerializeField]
    public float respawnTime { get; set; }

    SpawnPosition[] spawnPositions;

    RoomNum roomNum = RoomNum.EnemyTest;

    private void Awake()
    {
        spawnPositions = GetComponentsInChildren<SpawnPosition>();
    }

    private void Start()
    {
        //respawnTime = 1.0f;
        dieCount = 0;
    }

    public void PlusDieCount()
    {
        if(roomNum == RoomNum.EnemyTest)
        {

            // 모든 적이 사망
            if (++dieCount >= enemyNum)
            {
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
            spawnPositions[i].SpawnEnemy();
        }
    }
}
