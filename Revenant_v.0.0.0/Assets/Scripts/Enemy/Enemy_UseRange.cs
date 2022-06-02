using System;
using UnityEngine;

public class Enemy_UseRange : MonoBehaviour
{
    // Visible Member Variables
    public Enemy_FootMgr p_Enemy_FootMgr;


    // Member Variables
    private BasicEnemy m_Enemy;
    private StairPos m_StairPos;
    private int m_CountEnemyUseRange = 0;   // 주어진 횟수만큼 적이 상호작용하기 위해서(대체로 1번)

    
    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    
    // Functions
    public void AddEnemyUseRangeCount(int _count)
    {
        if (_count > 0)
            m_CountEnemyUseRange += _count;
        else
            Debug.Log("ERR : AddEnemyUseRangeCount, Param below 0");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (m_CountEnemyUseRange == 0)
            return;

        // 우선 현재는 계단만 탐
        if (!col.CompareTag("StairSensor"))
            return;
        
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
        
        m_CountEnemyUseRange--;
    }
}