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
        if (collision.CompareTag("Noise"))
        {
            NoisePrefab temp = collision.GetComponent<NoisePrefab>();
            if (enemyA.curEnemyState == EnemyState.IDLE || enemyA.curEnemyState == EnemyState.GUARD)
            {
                enemyA.m_sensorPos = temp.transform.position;
                Debug.Log("적이 감지한 소음은 " + temp.m_NoiseType + "이며, 플레이어 소음의 여부는 " + temp.m_isPlayer + "입니다.");
                enemyA.GuardAIState();
            }
        }
    }
}
