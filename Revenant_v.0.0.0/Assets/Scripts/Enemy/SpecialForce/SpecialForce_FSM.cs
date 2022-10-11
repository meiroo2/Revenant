using System.Collections;
using FMOD.Studio;
using UnityEngine;


public class SpecialForce_FSM : Enemy_FSM
{
    // Member Variables
    protected SpecialForce m_Enemy;
    protected Animator m_Animator;
    protected Transform m_Transform;
    
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

    protected void InitFSM()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Transform = m_Enemy.transform;
    }
}

public class SpecialForce_IDLE : SpecialForce_FSM
{
    // Member Variables
    private int m_Phase;
    
    public SpecialForce_IDLE(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Phase = 0;
    }

    public override void UpdateState()
    {
        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
    }

    public override void ExitState()
    {
       
    }

    public override void NextPhase()
    {
       
    }
}

public class SpecialForce_FOLLOW : SpecialForce_FSM
{
    // Member Variables
    private int m_Phase = 0;

    public SpecialForce_FOLLOW(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        if (!m_Enemy.m_PlayerCognition)
        {
            m_Enemy.m_PlayerCognition = true;
            m_Phase = 0;
        }
        else
        {
            m_Phase = 1;
        }


    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Phase = -1;
                m_Enemy.m_AlertSystem.DoFadeIn(()=> m_Phase = 1);
                break;
            
            case 1:
                if (m_Enemy.m_IsMouseTouched)
                {
                    m_Enemy.m_IsMouseTouched = false;
                    if (!m_Enemy.IsFacePlayer())
                        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ROLL);
                    break;
                }
                
                if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_Attack_Distance)
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                }
                else
                {
                    Vector2 difference = m_Enemy.GetPositionDifferenceBetPlayer();
                    if (difference.x > 0)
                    {
                        m_Enemy.SetRigidByDirection(false);
                    }
                    else
                    {
                        m_Enemy.SetRigidByDirection(true);
                    }
                }
                break;
        }
    }

    public override void ExitState()
    {
       m_Enemy.ResetRigid();
    }

    public override void NextPhase()
    {
       
    }
}

public class SpecialForce_ROLL : SpecialForce_FSM
{
    // Member Variables
    private CoroutineElement m_Element;
    private readonly int Roll = Animator.StringToHash("Roll");

    public SpecialForce_ROLL(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
       m_Enemy.SetHotBoxesActive(false);
       m_Animator.SetInteger(Roll, 1);

       m_Element = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CheckRoll());
    }

    public override void UpdateState()
    {
        m_Enemy.SetRigidByDirection(m_Enemy.m_IsRightHeaded, m_Enemy.p_Roll_Speed_Multi);
    }

    public override void ExitState()
    {
        m_Enemy.ResetRigid();
        m_Animator.SetInteger(Roll, 0);
        m_Enemy.SetHotBoxesActive(true);
    }

    public override void NextPhase()
    {
       
    }

    private IEnumerator CheckRoll()
    {
        yield return null;
        
        while (true)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            
            yield return null;
        }

        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        m_Element.StopCoroutine_Element();
        yield break;
    }
}

public class SpecialForce_ATTACK : SpecialForce_FSM
{
    // Member Variables
    private CoroutineElement m_Element;
    private readonly int Roll = Animator.StringToHash("Roll");
    private float m_Timer;

    public SpecialForce_ATTACK(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Timer = 0f;
        
        if (!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        
        m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
    }

    public override void UpdateState()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_Enemy.p_Fire_Delay)
        {
            if (!m_Enemy.IsFacePlayer())
                m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
            m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
            m_Timer = 0f;
        }
        
    }

    public override void ExitState()
    {
    
    }

    public override void NextPhase()
    {
       
    }
}

public class SpecialForce_STUN : SpecialForce_FSM
{
    // Member Variables
    private int m_Phase;
    
    private CoroutineElement m_Element;
    
    public SpecialForce_STUN(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Phase = 0;

        if(!m_Enemy.m_PlayerCognition)
            m_Enemy.StartPlayerCognition();

        m_Element = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CalStun());
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

    private IEnumerator CalStun()
    {
        yield return new WaitForSeconds(m_Enemy.p_Stun_Time);
        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        m_Element.StopCoroutine_Element();
        yield break;
    }
}

public class SpecialForce_DEAD : SpecialForce_FSM
{
    public SpecialForce_DEAD(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
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