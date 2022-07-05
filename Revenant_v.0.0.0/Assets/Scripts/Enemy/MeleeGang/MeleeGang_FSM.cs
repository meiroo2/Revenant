using System.Collections;
using UnityEngine;

public class MeleeGang_FSM : Enemy_FSM
{
    // Member Variables
    protected MeleeGang m_Enemy;
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

public class IdleMeleeGang : MeleeGang_FSM
{
    // Member Variables
    private bool m_isPatrol = false;
    private int m_PatrolIdx = 0;
    private float m_InternalTimer = 0f;
    private Transform m_EnemyTransform;
    
    // Constructor
    public IdleMeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        //m_EnemyAnimator.SetBool("isWalk", false);
        m_InternalTimer = m_Enemy.p_LookAroundDelay;
    }

    public override void UpdateState()
    {
        m_InternalTimer -= Time.deltaTime;
        if (m_InternalTimer <= 0f)
        {
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
            m_InternalTimer = m_Enemy.p_LookAroundDelay;
        }
        
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

public class FollowMeleeGang : MeleeGang_FSM
{
    // Member Variables
    private Vector2 m_DistanceBetPlayer;
    private Transform m_EnemyTransform;
    private float m_OverlapTimer = 0f;
    private int m_Phase = 0;
    
    // Constructor
    public FollowMeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_EnemyAnimator.SetBool("IsFollow", true);
        m_Phase = 0;
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        if (m_DistanceBetPlayer.magnitude > m_Enemy.p_MinFollowDistance)
        {
            m_Enemy.MoveByDirection_FUpdate(!(m_DistanceBetPlayer.x > 0));
        }
        else // MinFollowDistance 안쪽일 경우
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetBool("IsFollow", false);
    }

    public override void NextPhase()
    {
        
    }
}

public class AttackMeleeGang : MeleeGang_FSM
{
    // Member Variables
    private readonly Transform m_EnemyTransform;
    private readonly Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private int m_Angle = 0;
    private float m_Timer = 1f;

    // Constructor
    public AttackMeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }

    public override void StartState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
        
        // Attack 시작 시 우선 플레이어 바라봄
        m_Enemy.SetViewDirectionToPlayer();

        // Phase 0으로 초기화 & 공격 모션 시작
        m_Phase = 0;
        Debug.Log(m_Phase);
        
        // 애니메이터 -> 공격시작
        m_EnemyAnimator.SetBool("IsAttackStart", true);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:     // 휘두르는 애니 재생 중
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= m_Enemy.p_PointAttackTime)
                {
                    // 0.9f 이상이면 칼 콜라이더 생성하고 1페이즈로
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_Phase = 1;
                }
                break;
            
            case 1:     // 휘두르는 애니 재생 끝나면
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    // 애니 끝나면 Attack_Idle 재생, 0.5초후 Follow 가야 함
                    m_EnemyAnimator.SetBool("IsAttackEnd", true);
                    m_Timer = 0.5f;
                    m_Phase = 2;
                }
                break;
            
            case 2:     // 0.5초 대기
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                {
                    m_Phase = 3;
                }
                break;
            
            case 3:     // 공격 종료
                m_EnemyAnimator.SetBool("IsAttackEnd", false);
                // 사정거리 이내인지 계산
                if (m_Enemy.GetDistBetPlayer().magnitude <= m_Enemy.p_MinFollowDistance)
                {
                    // 플레이어 방향 바라보고 다시 공격페이즈
                    m_Enemy.SetViewDirectionToPlayer();
                    
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                    break;
                }
                else
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                    break;
                }
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        m_EnemyAnimator.SetBool("IsAttackEnd", false);
        m_EnemyAnimator.SetBool("IsAttackStart", false);
    }

    public override void NextPhase()
    {
        m_Phase = 1;
    }
}

public class DeadMeleeGang : MeleeGang_FSM
{
    // Member Variables
    private float m_Time = 3f;
    
    // Constructor
    public DeadMeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        m_Time -= Time.deltaTime;
        if(m_Time <= 0f)
            m_Enemy.gameObject.SetActive(false);
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }
}