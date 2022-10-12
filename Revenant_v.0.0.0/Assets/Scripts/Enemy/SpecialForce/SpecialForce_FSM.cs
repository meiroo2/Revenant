using System.Collections;
using FMOD.Studio;
using Unity.VisualScripting;
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
    private bool m_RollBook = false;
    private bool m_IsMouseCal = false;
    
    private CoroutineElement m_RollDelayElement;
    private CoroutineElement m_MouseOnElement;
    private SpecialForce_UseRange m_UseRange;

    private HideSlot m_Slot;

    public SpecialForce_FOLLOW(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Slot = null;
        m_UseRange = m_Enemy.m_UseRange;
        
        if (!m_Enemy.m_PlayerCognition)
        {
            m_Enemy.m_PlayerCognition = true;
            m_Phase = 0;
            m_Enemy.m_AlertSystem.DoFadeIn(()=> m_Phase = 1);
        }
        else
        {
            m_Phase = 1;
        }

        m_MouseOnElement = null;
        m_RollDelayElement = null;
        m_IsMouseCal = false;
        m_RollBook = false;
    }

    public override void UpdateState()
    {
        if(!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        
        WalkToPlayer(true);


        if (m_Enemy.m_IsMouseTouched && !m_IsMouseCal && !m_RollBook)
        {
            Debug.Log("시작시작");
            m_IsMouseCal = true;
            m_MouseOnElement = m_Enemy.m_CoroutineHandler.
                StartCoroutine_Handler(MouseOnCheck());
        }
        else if(!m_Enemy.m_IsMouseTouched)
        {
            if (m_IsMouseCal)
            {
                m_MouseOnElement.StopCoroutine_Element();
                m_MouseOnElement = null;
                m_IsMouseCal = false;
            }
        }

        switch (m_Phase)
        {
            case 1:
                if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_Attack_Distance)
                {
                    if (m_Enemy.m_UseHide)
                    {
                        m_Slot = m_UseRange.GetProperSlot();
                        if (ReferenceEquals(m_Slot, null) && !m_RollBook)
                        {
                            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                            m_Phase = -1;
                        }
                        else if(!ReferenceEquals(m_Slot, null))
                        {
                            m_Slot.m_isBooked = true;
                            m_Enemy.ResetMovePoint(m_Slot.transform.position);
                            m_Phase = 2;
                        }
                    }
                    else if(!m_RollBook)
                    {
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                        m_Phase = -1;
                    }
                }
                break;
            
            case 2:
                m_Enemy.SetRigidToPoint();
                if (Vector2.Distance(m_UseRange.transform.position, 
                        m_Slot.transform.position) < 0.2f)
                {
                    m_Enemy.m_CurHideSlot = m_Slot;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.HIDDEN);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        if (m_RollDelayElement != null)
            m_RollDelayElement.StopCoroutine_Element();

        if (m_MouseOnElement != null)
            m_MouseOnElement.StopCoroutine_Element();

        m_Enemy.ResetRigid();
    }

    public override void NextPhase()
    {
       
    }
    
    private IEnumerator MouseOnCheck()
    {
        while (true)
        {
            if (!m_Enemy.m_GlobalRollCooldown)
            {
                Debug.Log(m_Enemy.p_Roll_Chance + "퍼센트 시도");

                if (StaticMethods.GetProbabilityWinning(m_Enemy.p_Roll_Chance))
                {
                    Debug.Log(m_Enemy.p_Roll_Chance + "퍼센트 당첨 완료.");

                    m_RollBook = true;
                    m_RollDelayElement =
                        m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(RollDelay());
                    m_Enemy.RollCooldown();
                    break;
                }
            }
            
            yield return new WaitForSeconds(m_Enemy.p_Roll_Refresh);
        }

        m_IsMouseCal = false;
        yield break;
    }

    private IEnumerator RollDelay()
    {
        float randomTime = Random.Range(m_Enemy.p_Roll_Tick.x, m_Enemy.p_Roll_Tick.y);
        Debug.Log(randomTime + "초만큼 대기");
        yield return new WaitForSeconds(randomTime);
        Debug.Log("구르기 개시");
        
        if (!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        m_Enemy.ChangeEnemyFSM(EnemyStateName.ROLL);
        yield break;
    }
    
    private void WalkToPlayer(bool _toPlayer)
    {
        Vector2 difference = m_Enemy.GetPositionDifferenceBetPlayer();
        if (difference.x > 0)
        {
            m_Enemy.SetRigidByDirection(!_toPlayer);
        }
        else
        {
            m_Enemy.SetRigidByDirection(_toPlayer);
        }
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
        m_Animator = m_Enemy.m_Animator;
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
    private int m_Phase = 0;
    private float m_Timer = 0f;

    private const float m_MagicGap = 0.05f;


    public SpecialForce_ATTACK(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Phase = 0;
        m_Timer = 0f;

        m_Enemy.m_AlertSystem.AlertGaugeUp(() => m_Phase = 1);
    }

    public override void UpdateState()
    {
        if (m_Enemy.m_LastActionIsHide)
        {
            switch (m_Phase)
            {
                case 1:
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_Phase = 2;
                    break;
                
                case 2:
                    m_Timer += Time.deltaTime;
                    if (m_Timer > m_Enemy.p_Fire_Delay)
                    {
                        m_Enemy.m_LastActionIsHide = false;
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.HIDDEN);
                    }
                    break;
            }
        }
        else
        {
            if(!m_Enemy.IsFacePlayer())
                m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);

            float distance = m_Enemy.GetDistanceBetPlayer();
            if (!CheckIsInMagicGap(distance))
            {
                if (distance < m_Enemy.p_Gap_Distance)
                {
                    WalkToPlayer(false);
                        
                }
                else
                {
                    WalkToPlayer(true);
                }
            }
        
            switch (m_Phase)
            {
                case 0:
                    break;
            
                case 1:
                    m_Enemy.ResetRigid();
                    if (m_Enemy.GetDistanceBetPlayer() < m_Enemy.p_Melee_Distance)
                    {
                        // Melee Attack
                        m_Enemy.m_MeleeWeapon.Fire();
                    }
                    else
                    {
                        m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    }

                    m_Phase = 2;
                    break;
            
                case 2:
                    if (m_Enemy.GetDistanceBetPlayer() < m_Enemy.p_Attack_Distance)
                    {
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                        m_Phase = -1;
                    }
                    else
                    {
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                        m_Phase = -1;
                    }
                    break;
            }
        }
    }

    public override void ExitState()
    {

    }

    public override void NextPhase()
    {
       
    }

    private void WalkToPlayer(bool _toPlayer)
    {
        Vector2 difference = m_Enemy.GetPositionDifferenceBetPlayer();
        if (difference.x > 0)
        {
            m_Enemy.SetRigidByDirection(!_toPlayer, m_Enemy.p_Alert_Move_Speed_Multi);
        }
        else
        {
            m_Enemy.SetRigidByDirection(_toPlayer, m_Enemy.p_Alert_Move_Speed_Multi);
        }
    }

    private bool CheckIsInMagicGap(float _curDistance)
    {
        if (_curDistance < m_Enemy.p_Gap_Distance + m_MagicGap &&
            _curDistance > m_Enemy.p_Gap_Distance - m_MagicGap)
        {
            m_Enemy.ResetRigid();
            return true;
        }
        return false;
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

public class SpecialForce_HIDDEN : SpecialForce_FSM
{
    // Member Variables
    private Transform m_PlayerTransform;
    private CoroutineHandler m_Handler;
    private CoroutineElement m_Element;

    private const float m_XGap = 0.1f; 

    public SpecialForce_HIDDEN(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Handler = m_Enemy.m_CoroutineHandler;

        m_PlayerTransform = m_Enemy.m_PlayerTransform;
        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.m_CurHideSlot.m_isBooked = false;
        m_Enemy.m_CurHideSlot.ActivateHideSlot(true);
        m_Animator.SetInteger("Hide", 1);

        m_Element = m_Handler.StartCoroutine_Handler(CheckMinTimeHide());
    }

    public override void UpdateState()
    {
        if (m_Enemy.GetIsLeftThenPlayer())
        {
            if (m_PlayerTransform.position.x < m_Enemy.m_CurHideSlot.transform.position.x + m_XGap)
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            }
        }
        else
        {
            if (m_PlayerTransform.position.x > m_Enemy.m_CurHideSlot.transform.position.x - m_XGap)
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            }
        }
    }

    public override void ExitState()
    {
        if(!ReferenceEquals(m_Element, null))
            m_Element.StopCoroutine_Element();
        
        m_Enemy.SetHotBoxesActive(true);
        m_Enemy.m_CurHideSlot.ActivateHideSlot(false);
        m_Animator.SetInteger("Hide", 0);
    }

    public override void NextPhase()
    {
       
    }

    private IEnumerator CheckMinTimeHide()
    {
        yield return new WaitForSeconds(m_Enemy.p_Hide_Time_Min);
        
        float distance = m_Enemy.GetDistanceBetPlayer();

        if (distance > m_Enemy.p_Attack_Distance)
        {
            float timer = 0f;
            while (true)
            {
                timer += Time.deltaTime;

                if (timer > m_Enemy.p_Hide_Time_Cancel)
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                    break;
                }

                yield return null;
            }
        }
        else
        {
            m_Enemy.m_LastActionIsHide = true;
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }

        yield break;
    }
}