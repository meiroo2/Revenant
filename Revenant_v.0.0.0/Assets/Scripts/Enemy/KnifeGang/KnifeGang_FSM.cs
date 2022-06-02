using System.Collections;
using UnityEngine;

public class KnifeGang_FSM : Enemy_FSM
{
    // Member Variables
    protected KnifeGang m_Enemy;
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

public class IDLE_KnifeGang : KnifeGang_FSM
{
    // Member Variables
    
    // Constructor
    public IDLE_KnifeGang(KnifeGang _enemy)
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

public class FOLLOW_KnifeGang : KnifeGang_FSM
{
    // Member Variables
    private Vector2 m_DistanceBetPlayer;
    
    // Constructor
    public FOLLOW_KnifeGang(KnifeGang _enemy)
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

        if (m_DistanceBetPlayer.magnitude > m_Enemy.p_MinFollowDistance)
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
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }
}

public class ATTACK_KnifeGang : KnifeGang_FSM
{
    // Member Variables
    private readonly SuperArmorMgr m_SuperArmor;
    private readonly Transform m_EnemyTransform;
    private readonly Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private int m_Angle = 0;

    private float m_Timer = 1f;
    private bool m_AttackSuccess = false;

    // Constructor
    public ATTACK_KnifeGang(KnifeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_SuperArmor = m_EnemyAnimator.GetComponent<SuperArmorMgr>();
    }

    public override void StartState()
    {
        m_AttackSuccess = false;
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
        
        if(m_DistanceBetPlayer.x > 0 && m_Enemy.m_IsRightHeaded == true)
            m_Enemy.setisRightHeaded(false);
        else if (m_DistanceBetPlayer.x < 0 && m_Enemy.m_IsRightHeaded == false)
            m_Enemy.setisRightHeaded(true);
        
        m_Enemy.m_IsFoundPlayer = true;
        
        // 전조증상 시작과 동시에 콜백함수로 NextPhase 넘김
        m_Timer = 1f;
        m_Phase = 0;
        m_SuperArmor.doSuperArmor();
        m_SuperArmor.SetCallback(NextPhase);
        
        Debug.Log("Start");
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 1:
                // 공격 모션 시작
                m_Phase = 2;
                break;
            
            case 2:
                // 공격 모션 진행 중
                m_Timer -= Time.fixedDeltaTime;
                if (m_Timer <= 0.1f && !m_AttackSuccess)
                {
                    // 마지막 초 쯤 공격 판정 들어감 (1회)
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_AttackSuccess = true;
                }
                else if (m_Timer <= 0f)
                {
                    // 0.1초간 공격 판정 후 다음 페이지
                    m_Phase = 3;
                }
                break;
            
            case 3:
                // 공격 전부 끝난 후 거리 계산해서 다시 시작 혹은 추적 페이즈 돌입
                m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
                if (m_DistanceBetPlayer.magnitude <= m_Enemy.p_MinFollowDistance)
                {
                    if(m_DistanceBetPlayer.x > 0 && m_Enemy.m_IsRightHeaded == true)
                        m_Enemy.setisRightHeaded(false);
                    else if (m_DistanceBetPlayer.x < 0 && m_Enemy.m_IsRightHeaded == false)
                        m_Enemy.setisRightHeaded(true);

                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                }
                else
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                }
                m_Phase = 4;
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void NextPhase()
    {
        m_Phase = 1;
    }
}

public class DEAD_KnifeGang : KnifeGang_FSM
{
    // Member Variables
    
    // Constructor
    public DEAD_KnifeGang(KnifeGang _enemy)
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