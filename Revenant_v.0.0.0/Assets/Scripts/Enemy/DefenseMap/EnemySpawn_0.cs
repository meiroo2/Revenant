using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawn_0 : MonoBehaviour
{

    [SerializeField]
    SpawnPos[] m_spawnPositions;


    void Awake()
    {
        
        foreach(var p in m_spawnPositions)
        {
            if(p)
                p.AllWaveInit();
        }

        
    }
}
