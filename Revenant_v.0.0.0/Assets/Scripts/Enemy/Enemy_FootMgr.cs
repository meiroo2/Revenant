using System;
using UnityEngine;


public class Enemy_FootMgr : MonoBehaviour
{
    // Visible Member Variablesb
    public Vector2 m_FootNormal { get; private set; }

    // Member Variables
    private RaycastHit2D m_FootHit;
    private GameObject m_Enemy;
    private Transform m_EnemyTransform;

    private Vector2 m_TempVecForRay;
    
    // Constructor
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>().gameObject;
        m_EnemyTransform = m_Enemy.transform;
    }

    // Updates
    private void Update()
    {
        CalEnemyFootRay();
      // Debug.Log(Vector2.Distance( m_EnemyTransform.position, m_FootHit.point));
    }

    // Functions
    public void CalEnemyStairNormal(ref StairPos _stairPos)
    {
        m_FootNormal = _stairPos.m_ParentStair.m_StairNormalVec;
    }
    private void CalEnemyFootRay()
    {
        Vector2 position = m_EnemyTransform.position;
        position.y -= 0.36f;
        
        m_FootHit = m_Enemy.layer switch
        {
            11 => Physics2D.Raycast(position, -transform.up, 0.5f, LayerMask.GetMask("Floor")),
            9 => Physics2D.Raycast(position, -transform.up, 0.5f, LayerMask.GetMask("Stair")),
            _ => m_FootHit
        };

        Debug.DrawRay(position, Vector2.down * 0.5f, new Color(1, 0, 0));

        if (!m_FootHit)
            return;

        
        switch (m_Enemy.layer)
        {
            case 11:
                m_FootNormal = m_FootHit.normal;
                if (Vector2.Distance(m_EnemyTransform.position, m_FootHit.point) > 0.64f)
                    m_EnemyTransform.position = new Vector2(m_EnemyTransform.position.x, m_FootHit.point.y + 0.61f);
                break;

            case 9:
                if (Vector2.Distance(m_EnemyTransform.position, m_FootHit.point) > 0.64f)
                    m_EnemyTransform.position = new Vector2(m_EnemyTransform.position.x, m_FootHit.point.y + 0.62f);
                break;
        }
        
    }
}