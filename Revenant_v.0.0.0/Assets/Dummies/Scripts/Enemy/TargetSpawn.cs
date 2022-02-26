using System.Collections;
using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject targetPrefab;

    [SerializeField]
    GameObject[] targetSpawnPoints;

    void Start()
    {
        foreach(var target in targetSpawnPoints)
        {
            Instantiate(targetPrefab, target.transform);
        }
        

    }

    void Update()
    {
        
    }
}
