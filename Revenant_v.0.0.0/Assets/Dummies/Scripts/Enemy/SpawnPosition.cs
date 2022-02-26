using System.Collections;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    void Start()
    {
        if (enemyPrefab)
            Instantiate(enemyPrefab, transform);//enemySpawnPoint.transform.position, transform.rotation,);
        else
            Debug.Log("No Prefab");
    }

    void Update()
    {
        
    }
}
