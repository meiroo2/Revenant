using System;
using UnityEngine;


public class Drone_FSM : Enemy_FSM
{
    // Member Variables
    protected Drone m_Enemy;
    protected Animator m_EnemyAnimator;

    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
       
    }

    public override void ExitState()
    {
       
    }

    public override void NextPhase()
    {
       
    }
}

public class FOLLOW_Drone : Drone_FSM
{
    // Member Variables
    private Vector2 m_DistanceBetPlayer;
    
    public FOLLOW_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        if (Mathf.Abs(m_DistanceBetPlayer.x) > m_Enemy.p_ToRushDistance)
        {
            if (m_DistanceBetPlayer.x > 0) // 적이 더 오른쪽
            {
                m_Enemy.MoveByDirection_FUpdate(false);
            }
            else // 적이 더 왼쪽
            {
                m_Enemy.MoveByDirection_FUpdate(true);
            }
        }
        else // MinFollowDistance 안쪽일 경우
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.RUSH);
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
    }

    public override void NextPhase()
    {
       
    }
}

public class RUSH_Drone : Drone_FSM
{
    // Member Variables

    public RUSH_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    public override void StartState()
    {
        m_Enemy.ResetMovePoint(m_Enemy.m_PlayerTransform.position);
    }

    public override void UpdateState()
    {
        m_Enemy.MoveToPoint_FUpdate();
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
       
    }
}