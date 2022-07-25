using System;
using System.Collections;
using UnityEngine;


public class Drone_FSM : Enemy_FSM
{
    // Member Variables
    protected Drone m_Enemy;
    protected Animator m_Animator;

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

public class IDLE_Drone : Drone_FSM
{
    public IDLE_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_Animator.SetBool("IsMove", false);
        m_Animator.SetBool("IsStop", false);
        m_Animator.SetBool("IsExplode", false);
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
    private int m_Phase;
    private CoroutineElement m_CoroutineElement;
    private CoroutineHandler m_CoroutineHandler;
    
    public FOLLOW_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        m_Phase = 0;

        m_Animator.SetBool("IsMove", true);
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        switch (m_Phase)
        {
            case 0:     // 초기 판정
                if (Mathf.Abs(m_DistanceBetPlayer.x) > m_Enemy.p_ToRushXDistance)
                {
                    if (m_DistanceBetPlayer.x > 0) // 적이 더 오른쪽
                    {
                        m_CoroutineElement =
                            m_CoroutineHandler.StartCoroutine_Handler(MoveCoroutine(false));
                        m_Phase = 1;
                    }
                    else // 적이 더 왼쪽
                    {
                        m_CoroutineElement =
                            m_CoroutineHandler.StartCoroutine_Handler(MoveCoroutine(true));
                        m_Phase = 2;
                    }
                }
                else // MinFollowDistance 안쪽일 경우
                {
                    m_Phase = 3;
                }
                break;
            
            case 1:     // 오른쪽 이동
                if (Mathf.Abs(m_DistanceBetPlayer.x) < m_Enemy.p_ToRushXDistance)
                {
                    m_CoroutineElement.StopCoroutine_Element();
                    m_Phase = 3;
                }
                break;
            
            case 2:     // 왼쪽 이동
                if (Mathf.Abs(m_DistanceBetPlayer.x) < m_Enemy.p_ToRushXDistance)
                {
                    m_CoroutineElement.StopCoroutine_Element();
                    m_Phase = 3;
                }
                break;
            
            case 3:     // Stop 대기
                m_Enemy.SetCoroutineCallback(NextPhase);
                m_Enemy.VelocityBreak();
                m_Animator.SetBool("IsStop", true);
                m_Phase = 4;
                break;
            
            case 5:
                m_Enemy.ChangeEnemyFSM(EnemyStateName.RUSH);
                break;
        }
    }

    public override void ExitState()
    {
        m_Animator.SetBool("IsStop", false);
        m_Animator.SetBool("IsMove", false);
    }

    public override void NextPhase()
    {
        m_Phase++;
    }

    private IEnumerator MoveCoroutine(bool _isRight)
    {
        bool isRight = _isRight;
        while (true)
        {
            m_Enemy.MoveByDirection_FUpdate(isRight);
            yield return new WaitForFixedUpdate();
        }
        
    }
}

public class RUSH_Drone : Drone_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private Transform m_EnemyTransform;
    private Vector2 m_TargetPos;
    private AnimatorStateInfo m_Stateinfo;

    public RUSH_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_EnemyTransform = m_Enemy.transform;
        m_Phase = 0;
        m_TargetPos = m_Enemy.m_Player.GetPlayerFootPos();

        m_Animator.SetBool("IsDetect", true);
        m_Enemy.ResetMovePoint(m_TargetPos);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:     // 애니메이션 Attack까지 대기
                m_Stateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (m_Stateinfo.IsName("At"))
                {
                    m_Enemy.MoveToPoint_FUpdate();
                    m_Phase = 1;
                }
                break;
            
            case 1:     // 좌표 계산
                if ((m_EnemyTransform.position.y - m_TargetPos.y) <= 0.2f)
                {
                    m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
                    m_Enemy.SetHitBox(false);
                    m_Enemy.m_DieReasaon = 2;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.DEAD);

                    m_Phase = 2;
                }
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
}

public class DEAD_Drone : Drone_FSM
{
    // Member Variables
    private AnimatorStateInfo m_StateInfo;
    
    
    public DEAD_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
        switch (m_Enemy.m_DieReasaon)
        {
            case 0: // 몸체 피격
                m_Enemy.m_Animator.Play("Head");
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                break;
            
            case 1: // 폭탄 피격
                m_Enemy.m_Animator.Play("Body");
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                break;
            
            case 2: // 잘 폭발
                m_Animator.SetBool("IsExplode", true);
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                break;
        }
    }

    public override void UpdateState()
    {
        m_StateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_StateInfo.normalizedTime >= 1f)
        {
            m_Enemy.gameObject.SetActive(false);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
       
    }
}