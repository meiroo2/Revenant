using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateName
{
    IDLE,
    WALK,
    ATTACK
}

public class BasicEnemy : Human
{
    // Visible Member Variables
    [field: SerializeField] public float p_VisionDistance { get; protected set; }
    [field: SerializeField] public float p_HearColSize { get; protected set; }
    [field: SerializeField] public int p_AngleLimit { get; protected set; } = 20;
    
    
    // Member Variables
    public Transform m_PlayerTransform { get; protected set; }
    public Rigidbody2D m_EnemyRigid { get; protected set; }
    public RaycastHit2D m_VisionHit { get; protected set; }
    protected Enemy_FSM m_CurEnemyFSM;
    protected EnemyStateName m_CurEnemyStateName;


    // Functions
    public virtual void RaycastVisionCheck()
    {
        if (m_isRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(transform.position, Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.right * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast( transform.position, -Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, -Vector2.right * p_VisionDistance, Color.red);
        }
    }
    public virtual void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log("상태 전이" + _name);
        m_CurEnemyStateName = _name;
        
        m_CurEnemyFSM.ExitState();
        switch (m_CurEnemyStateName)
        {
            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
    
    public void MoveToPoint_FUpdate(ref Vector2 _pos)
    {
        
    }

    public void MoveByDirection_FUpdate(bool _isRight)
    {
        if (_isRight)
        {
            if(!m_isRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_HumanFootNormal) * (m_Speed * Time.deltaTime);
        }
        else
        {
            if(m_isRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_HumanFootNormal) * (m_Speed * Time.deltaTime);
        }
    }
}