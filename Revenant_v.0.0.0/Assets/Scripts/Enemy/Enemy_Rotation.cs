using System;
using UnityEngine;



public class Enemy_Rotation : MonoBehaviour
{
    // Visible Memeber Variables
    [field: SerializeField] public int[] p_Angles { get; private set; } = new int[3];


    // Member Variables
    private Transform m_PlayerTransform;
    private BasicEnemy m_Enemy;
    private int m_AnglePhase;


    // Constructors
    private void Awake()
    {
        m_Enemy = GetComponentInParent<BasicEnemy>();
    }

    private void Start()
    {
        var Instance = InstanceMgr.GetInstance();
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
    }
    
    
    // Functions
    public void RotateEnemyArm()
    {
        /*
        Vector2 Distance = new Vector2(m_PlayerRealTransform.position.x - m_Enemy.transform.position.x,
            m_PlayerRealTransform.position.y - m_Enemy.transform.position.y);

        float ActualAngle;
        
        if (m_Enemy.m_isRightHeaded)
            ActualAngle = Mathf.Atan2(Distance.y, Distance.x) * Mathf.Rad2Deg;
        else
            ActualAngle = -Mathf.Atan2(Distance.y, Distance.x) * Mathf.Rad2Deg;

        if (ActualAngle > 90 - m_Enemy.p_AngleLimit)
            ActualAngle = 90 - m_Enemy.p_AngleLimit;
        else if (ActualAngle < -90 + m_Enemy.p_AngleLimit)
            ActualAngle = -90 + m_Enemy.p_AngleLimit;
        */

        m_AnglePhase = StaticMethods.getAnglePhase(m_Enemy.transform.position,
            m_PlayerTransform.position, 3, m_Enemy.p_AngleLimit);

        switch (m_AnglePhase)
        {
            case 0:
                transform.localRotation = Quaternion.Euler(0, 0, p_Angles[0]);
                break;
            
            case 1:
                transform.localRotation = Quaternion.Euler(0, 0, p_Angles[1]);
                break;
            
            case 2:
                transform.localRotation = Quaternion.Euler(0, 0, p_Angles[2]);
                break;
        }
    }
}