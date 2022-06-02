using UnityEngine;


public class ShieldGang_FSM : Enemy_FSM
{
    // Member Variables
    protected ShieldGang m_Enemy;
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

public class IDLE_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    
    
    // Constructor
    public IDLE_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class FOLLOW_ShieldGang : ShieldGang_FSM
{
    // Member Variables


    // Constructor
    public FOLLOW_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class ROTATION_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    

    // Constructor
    public ROTATION_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class ATTACK_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public ATTACK_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class CHANGE_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public CHANGE_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class DEAD_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public DEAD_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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

public class STUN_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public STUN_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
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