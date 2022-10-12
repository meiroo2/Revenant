using System;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySpawner_Collider : MonoBehaviour
{
    // Visible Member Variables
    private EnemySpawner m_ParentEnemySpawner;
    
    
    // Member Variables
    private bool m_IsCollided = false;


    // Constructors
    private void Awake()
    {
        m_ParentEnemySpawner = GetComponentInParent<EnemySpawner>();
        m_IsCollided = false;
    }


    // Functions
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_IsCollided)
            return;

        if (col.CompareTag("@Player"))
        {
            m_IsCollided = true;
            m_ParentEnemySpawner.AchieveCollisionTrigger();
        }
    }
}