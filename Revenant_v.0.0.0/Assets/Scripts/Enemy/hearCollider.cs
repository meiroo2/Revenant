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
                Debug.Log("���� ������ ������ " + temp.m_NoiseType + "�̸�, �÷��̾� ������ ���δ� " + temp.m_isPlayer + "�Դϴ�.");
                enemyA.GuardAIState();
            }
        }
    }
}
