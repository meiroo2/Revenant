using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hearCollider : MonoBehaviour
{
    EnemyA enemyA;
    private void Awake()
    {
        enemyA = GetComponentInParent<EnemyA>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyA.curEnemyState==EnemyState.IDLE || enemyA.curEnemyState == EnemyState.GUARD)
        {
            if (collision.CompareTag("Something"))
            {

                enemyA.sensorPos = collision.transform.position;
                enemyA.GuardAIState();
            }
        }
    }
}
