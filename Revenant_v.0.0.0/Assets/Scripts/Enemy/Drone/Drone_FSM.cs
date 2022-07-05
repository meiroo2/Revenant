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

        if (Mathf.Abs(m_DistanceBetPlayer.x) > m_Enemy.p_ToRushXDistance)
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
    private int m_Phase = 0;
    private Enemy_Alert m_Alert;
    private Transform m_EnemyTransform;
    private Vector2 m_TargetPos;

    public RUSH_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    public override void StartState()
    {
        m_EnemyTransform = m_Enemy.transform;
        m_Phase = 0;
        m_Enemy.ResetMovePoint(m_Enemy.m_PlayerTransform.position);
        m_TargetPos = m_Enemy.m_PlayerTransform.position;
        m_Alert = m_Enemy.m_Alert;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:     // 멈춤, 느낌표 띄움, 콜백함수 심음, 애니 재생, 위치는 저장함
                m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
                m_Alert.SetAlertActive(true);
                m_Alert.SetCallback(NextPhase, true);
                m_Alert.SetAlertFill(true);
                m_Enemy.m_Animator.SetBool("IsDetect", true);
                m_Phase = 1;
                break;
            
            case 1:     // 콜백 대기
                break;
            
            case 2:     // 느낌표 채움 완료 + 돌진 + 충돌 콜백 설정
                //m_Enemy.m_CrashCol.SetCrashCol(true);
                //m_Enemy.m_CrashCol.SetCallback(NextPhase, true);
                m_Bombed = false;
                m_Enemy.SetUpdateCallback(CalDistance);
                m_Phase = 3;
                break;
            
            case 3:
                m_Enemy.MoveToPoint_FUpdate();
                break;
            
            case 4:
                m_Enemy.SetHitBox(false);
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                m_Enemy.m_DieReasaon = 1;
                m_Enemy.ChangeEnemyFSM(EnemyStateName.DEAD);
                break;
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        m_Phase++;
    }

    private bool m_Bombed = false;
    private void CalDistance()
    {
        if (m_Bombed)
            return;
        
        if ((m_TargetPos - (Vector2)m_EnemyTransform.position).magnitude < 0.2f)
        {
            m_Bombed = true;
            m_Phase = 4;
            m_Enemy.ResetCallback();
        }
    }
}

public class DEAD_Drone : Drone_FSM
{
    // Member Variables

    public DEAD_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    public override void StartState()
    {
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.m_Alert.SetAlertActive(false);
        m_Enemy.m_Alert.gameObject.SetActive(false);
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
        switch (m_Enemy.m_DieReasaon)
        {
            case 0: // 몸체 피격
                m_Enemy.m_Animator.Play("Head");
                break;
            
            case 1: // 폭탄 피격
                m_Enemy.m_Animator.Play("Body");
                break;
        }
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