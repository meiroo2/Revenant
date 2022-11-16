using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class TutorialBot_FSM : Enemy_FSM
{
    // Member Variables
    protected TutorialBot m_Enemy;
    protected CoroutineElement m_AnimCheckElement = null;

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

    protected void SafetyDeleteCoroutineElement()
    {
        if (!ReferenceEquals(m_AnimCheckElement, null))
        {
            m_AnimCheckElement.StopCoroutine_Element();
            m_AnimCheckElement = null;
        }
    }
}

public class TutorialBot_IDLE : TutorialBot_FSM
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    // Member Variables

    public TutorialBot_IDLE(TutorialBot _enemy)
    {
        m_Enemy = _enemy;
    }

    public override void StartState()
    {
        m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(RespawnEndCheck());
    }
    
    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
       SafetyDeleteCoroutineElement();
    }

    private IEnumerator RespawnEndCheck()
    {
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }

        m_Enemy.m_Animator.SetInteger(Idle, 1);
        
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        
       // m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        yield break;
    }
}

public class TutorialBot_ATK : TutorialBot_FSM
{
    // Member Variables
    private readonly int AtkSpeed = Animator.StringToHash("AtkSpeed");
    private readonly int Fire = Animator.StringToHash("Fire");

    public TutorialBot_ATK(TutorialBot _enemy)
    {
        m_Enemy = _enemy;
    }

    public override void StartState()
    {
        m_Enemy.m_Animator.SetFloat(AtkSpeed, m_Enemy.p_AtkSpeed);
        m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(AtkEndCheck());
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        SafetyDeleteCoroutineElement();
    }

    private void ResetFire()
    {
        m_Enemy.m_Animator.Play("TutorialBot_Atk", -1, 0);
        
        SafetyDeleteCoroutineElement();
        m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(AtkEndCheck());
    }
    
    private IEnumerator AtkEndCheck()
    {
        bool fired = false;
        float normalTime = 0f;
        m_Enemy.m_Animator.SetInteger(Fire, 1);
        
        while (true)
        {
            yield return null;
            normalTime = m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (!fired && normalTime >= m_Enemy.p_PointAtkTime)
            {
                fired = true;
                m_Enemy.m_Weapon.Fire();
            }
            
            if (normalTime >= 1f)
            {
                break;
            }
        }
        
        ResetFire();
        yield break;
    }
}

public class TutorialBot_DEAD : TutorialBot_FSM
{
    private CoroutineElement m_AnimCheckElement = null;
    private readonly int Dead = Animator.StringToHash("Dead");

    public TutorialBot_DEAD(TutorialBot _enemy)
    {
        m_Enemy = _enemy;
    }

    public override void StartState()
    {
        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.m_Animator.SetInteger(Dead, 1);
        m_AnimCheckElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(
            DeadAnimCheck());
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        SafetyDeleteCoroutineElement();
    }

    private IEnumerator DeadAnimCheck()
    {
        while (true)
        {
            yield return null;

            if (m_Enemy.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        m_Enemy.gameObject.SetActive(false);
        yield break;
    }
}