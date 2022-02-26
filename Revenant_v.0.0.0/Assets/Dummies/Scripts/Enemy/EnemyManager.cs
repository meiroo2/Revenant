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
    [SerializeField]
    RoomNum roomNumber;

    GameObject[] spawnedPosList;
    GameObject[] spawnedEnemyList;
    int spawnedEnemyNum;

    private void Update()
    {
        EnemyManage();
    }
    public void EnemyManage()
    {
        switch (roomNumber)
        {
            case RoomNum.EnemyTest:
                checkEnemyAllDie();
                break;
            default:
                break;
        }
    }
    void checkEnemyAllDie()
    {

    }
}
