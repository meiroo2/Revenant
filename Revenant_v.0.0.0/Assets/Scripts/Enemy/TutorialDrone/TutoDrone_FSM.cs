using System.Collections;
using UnityEngine;


public class TutoDrone_FSM : Enemy_FSM
{
    // Member Variables
    protected TutoDrone m_Enemy;

    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void NextPhase()
    {
        throw new System.NotImplementedException();
    }
}

public class IDLE_TutoDrone : TutoDrone_FSM
{
    // Member Variables
    private CoroutineElement m_AnimCheckElement = null;
    
    public IDLE_TutoDrone(TutoDrone _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_Enemy.SetHotBoxesActive(false);
        
       SafetyStopElement();

       m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(AnimCheck());
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
       SafetyStopElement();
    }

    private IEnumerator AnimCheck()
    {
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.
                    GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }

        m_Enemy.ChangeEnemyFSM(EnemyStateName.PATROL);
        
        yield break;
    }
    
    private void SafetyStopElement()
    {
        if (!ReferenceEquals(m_AnimCheckElement, null))
        {
            m_AnimCheckElement.StopCoroutine_Element();
            m_AnimCheckElement = null;
        }
    }
}

public class PATROL_TutoDrone : TutoDrone_FSM
{
    private Transform m_EnemyTransform;
    private int m_PatrolIdx = 0;
    private int m_Phase = 0;
    private readonly int Idle = Animator.StringToHash("Idle");

    public PATROL_TutoDrone(TutoDrone _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_Enemy.SetHotBoxesActive(true);
        
        m_EnemyTransform = m_Enemy.transform;
        
        m_Enemy.p_Animator.SetInteger(Idle, 1);
        m_Phase = 0;
        m_PatrolIdx = 0;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPoses[m_PatrolIdx].position);
                m_Enemy.SetRigidToPoint();
                m_Phase = 1;
                break;
            
            case 1:
                if (Vector2.Distance(m_EnemyTransform.position, 
                        m_Enemy.p_PatrolPoses[m_PatrolIdx].position) < 0.1f)
                {
                    m_Enemy.ResetRigid();
                    m_Phase = 2;
                }
                break;
            
            case 2:
                m_PatrolIdx++;
                if (m_PatrolIdx >= m_Enemy.p_PatrolPoses.Length)
                {
                    m_PatrolIdx = 0;
                }

                m_Phase = 0;
                break;
        }
    }

    public override void ExitState()
    {
       m_Enemy.ResetRigid();
    }
}

public class DEAD_TutoDrone : TutoDrone_FSM
{
    private readonly int Dead = Animator.StringToHash("Dead");
    private readonly int Hit = Animator.StringToHash("Hit");
    private CoroutineElement m_AnimCheckElement = null;
    
    public DEAD_TutoDrone(TutoDrone _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_Enemy.SetHotBoxesActive(false);
        
        m_Enemy.m_Animator.SetInteger(Hit, 1);
        
        SafetyStopElement();

        m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(AnimCheck());
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
       SafetyStopElement();
    }

    private IEnumerator AnimCheck()
    {
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        
        m_Enemy.m_Animator.SetInteger(Dead, 1);
        
        
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        
        SafetyStopElement();
        m_Enemy.gameObject.SetActive(false);
        
        yield break;
    }

    private void SafetyStopElement()
    {
        if (!ReferenceEquals(m_AnimCheckElement, null))
        {
            m_AnimCheckElement.StopCoroutine_Element();
            m_AnimCheckElement = null;
        }
    }
}