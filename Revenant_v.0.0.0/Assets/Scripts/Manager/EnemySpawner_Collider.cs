using System;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySpawner_Collider : MonoBehaviour
{
    // Visible Member Variables
    private EnemySpawner m_ParentEnemySpawner;
    

    // Constructors
    private void Awake()
    {
        m_ParentEnemySpawner = GetComponentInParent<EnemySpawner>();
    }


    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("@Player"))
        {
            m_ParentEnemySpawner.AchieveCollisionTrigger(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("@Player"))
        {
            m_ParentEnemySpawner.AchieveCollisionTrigger(false);
        }
    }
}