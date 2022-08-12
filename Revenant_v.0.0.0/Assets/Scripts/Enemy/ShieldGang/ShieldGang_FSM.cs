using System.Collections;
using Unity.VisualScripting;
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
        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.m_IsFoundPlayer = true;
            m_Enemy.StartPlayerCognition();
        }
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
    private float m_DistanceBetPlayer;

    // Constructor
    public FOLLOW_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    
    public override void StartState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();

        // 쉴드 깨졌을 떄랑 구분
        if (m_Enemy.m_IsShieldBroken)
        {
            if (m_DistanceBetPlayer > m_Enemy.p_AttackDistance)
            {
                m_Enemy.MoveByDirection(m_Enemy.GetIsLeftThenPlayer());
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            }
        }
        else
        {
            if (Mathf.Abs(m_DistanceBetPlayer - m_Enemy.p_GapDistance) > 0.03f)
            {
                // 이격거리보다 더 멀 경우
                if (m_DistanceBetPlayer > m_Enemy.p_GapDistance)
                {
                    m_Enemy.MoveByDirection(m_Enemy.GetIsLeftThenPlayer());
                }
                // 이격거리보다 가까이 있음
                else
                {
                    m_Enemy.MoveByDirection(!m_Enemy.GetIsLeftThenPlayer());
                }
            }
            else
            {
                m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
            }
            
            if(m_DistanceBetPlayer < m_Enemy.p_AttackDistance)
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
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

public class ROTATION_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private bool m_RotateComplete = false;
    private CoroutineElement m_Element;
    

    // Constructor
    public ROTATION_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    
    public override void StartState()
    {
        m_Enemy.m_Shield.gameObject.SetActive(false);
        m_RotateComplete = false;
        m_Element = GameMgr.GetInstance().p_CoroutineHandler.StartCoroutine_Handler(DoRotate());
    }

    public override void UpdateState()
    {
        if (m_RotateComplete)
        {
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }

    private IEnumerator DoRotate()
    {
        yield return new WaitForSeconds(1.5f);
        m_Enemy.m_Shield.gameObject.SetActive(true);
        m_RotateComplete = true;
        
        m_Element.StopCoroutine_Element(); 
        yield break;
    }
}

public class ATTACK_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private CoroutineElement m_CoroutineElement;
    private CoroutineHandler m_CoroutineHandler;
    private HitSFXMaker m_HitSFXMaker;
    
    private bool m_IsAttackEnd = false;
    
    
    // Constructor
    public ATTACK_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;

        var instance = InstanceMgr.GetInstance();
        m_HitSFXMaker = instance.GetComponentInChildren<HitSFXMaker>();
    }

    
    public override void StartState()
    {
        m_IsAttackEnd = false;

        m_CoroutineElement = 
            m_CoroutineHandler.StartCoroutine_Handler(m_Enemy.m_IsShieldBroken ? AttackCheck() : ShieldAttackCheck());
    }

    public override void UpdateState()
    {
        if (!m_IsAttackEnd)
            return;

        if (m_Enemy.m_IsShieldBroken)
        {
            if (m_Enemy.GetDistanceBetPlayer() < m_Enemy.p_AttackDistance)
            {
                StartState();
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
            }
        }
        else
        {
            if ((m_Enemy.m_IsRightHeaded && !m_Enemy.GetIsLeftThenPlayer()) ||
                !m_Enemy.m_IsRightHeaded && m_Enemy.GetIsLeftThenPlayer())
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ROTATION);
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
            }
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }

    private IEnumerator AttackCheck()
    {
        Debug.Log("무기를 든다.");
        yield return new WaitForSeconds(0.5f);

        Debug.Log("공격 판정 개시");
        m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
        m_HitSFXMaker.EnableNewObj(2, m_Enemy.m_WeaponMgr.m_CurWeapon.transform.position);

        m_IsAttackEnd = true;
        m_CoroutineElement.StopCoroutine_Element();
        yield break;
    }

    private IEnumerator ShieldAttackCheck()
    {
        Debug.Log("방패를 내리고 무기를 꺼낸다");
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("방패 콜라이더 꺼짐");
        m_Enemy.m_Shield.gameObject.SetActive(false);
        
        Debug.Log("무기를 꺼낸 팔을 높이 든 채로 대기");
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("공격 판정 개시");
        m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
        m_HitSFXMaker.EnableNewObj(2, m_Enemy.m_WeaponMgr.m_CurWeapon.transform.position);
        
        yield return new WaitForSeconds(0.5f);
        Debug.Log("방패를 다시 들었다.");
        m_Enemy.m_Shield.gameObject.SetActive(true);

        m_IsAttackEnd = true;
        
        m_CoroutineElement.StopCoroutine_Element();
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
        m_Enemy.gameObject.SetActive(false);
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

public class BREAK_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private CoroutineElement m_CoroutineElement;
    private CoroutineHandler m_CoroutineHandler;
    
    // Constructor
    public BREAK_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();

        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
    }
    
    public override void StartState()
    {
        Debug.Log("방패 파괴중...");
        m_CoroutineElement= m_CoroutineHandler.StartCoroutine_Handler(BreakingCheck());
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

    private IEnumerator BreakingCheck()
    {
        yield return new WaitForSeconds(1);
        
        Debug.Log("방패 파괴 끝");
        m_CoroutineElement.StopCoroutine_Element();
        
        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
    }
}