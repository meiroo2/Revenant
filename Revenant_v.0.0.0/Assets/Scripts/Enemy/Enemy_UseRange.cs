using System;
using UnityEngine;

public class Enemy_UseRange : MonoBehaviour
{
    // Visible Member Variables
    public Enemy_FootMgr p_Enemy_FootMgr;
    public bool p_UseEnemyUseRange = false;
    
    
    // Member Variables
    private BasicEnemy m_Enemy;
    private StairPos m_StairPos;

    
    // Functions
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!p_UseEnemyUseRange)
            return;
        
        // 우선 현재는 계단만 탐
        if (!col.CompareTag("StairSensor")) return;
        
        m_StairPos = col.GetComponent<StairPos>();
        switch (m_StairPos.StairDetectorDetected(gameObject.GetInstanceID()))
        {
            case 1:
                m_Enemy.gameObject.layer = 9;
                p_Enemy_FootMgr.CalEnemyStairNormal(ref m_StairPos);
                break;
            
            default:
                m_Enemy.gameObject.layer = 11;
                break;
        }
    }
}