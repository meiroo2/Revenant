using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomNum
{
    EnemyTest,
    FireTest01,
    FireTest02,
    Stage01
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
    [SerializeField]
    RoomNum roomNum = RoomNum.EnemyTest;

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
    }

    public void PlusDieCount()
    {
        if(roomNum == RoomNum.EnemyTest)
        {

            // ��� ���� ���
            if (++dieCount >= enemyNum)
            {
                // ���̺� ����
                enemyWave++;
                dieCount = 0;
                // ������
                Invoke(nameof(RespawnAllEnemy), respawnTime);
            }

        }
        
    }

    void RespawnAllEnemy()
    {
        for(int i = 0; i < enemyNum; i++)
        {
            // �Ϲ� �� -> �������� ���� �� -> �Ϲ� �� ��
            if (enemyWave % 2 == 1 && i == 2)
            {
                spawnPositions[i].SpawnEnemy(enemyPrefab[1]);
            }
            else
                spawnPositions[i].SpawnEnemy(enemyPrefab[0]);
        }
    }
}
