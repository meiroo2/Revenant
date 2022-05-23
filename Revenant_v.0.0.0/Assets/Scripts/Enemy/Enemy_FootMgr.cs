using System;
using UnityEngine;


public class Enemy_FootMgr : MonoBehaviour
{
    // Visible Member Variablesb
    

    // Member Variables
    private BasicEnemy m_Enemy;
    private RaycastHit2D m_FootHit;
    
    // Constructor
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    // Updates
    private void Update()
    {
        CalculateMoveVec();
    }

    // Functions
    public void CalculateMoveVec()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.5f, new Color(0, 1, 0));
        m_FootHit = Physics2D.Raycast(transform.position, Vector2.down,
            0.5f, LayerMask.GetMask("Floor"));

        if (m_FootHit)
        {
            m_Enemy.m_HumanFootNormal = m_FootHit.normal;
        }
    }
}