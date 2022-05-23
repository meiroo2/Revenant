using UnityEngine;


public class NormalGang_FSM : Enemy_FSM
{
    // Member Variables
    protected NormalGang m_Enemy;
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

public class IDLE_NormalGang : NormalGang_FSM
{
    public IDLE_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        m_EnemyAnimator.SetBool("isWalk", false);
    }

    public override void UpdateState()
    {
        m_Enemy.RaycastVisionCheck();
        
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.WALK);
        }
    }

    public override void ExitState()
    {
        
    }

    public override void NextPhase()
    {
        
    }
}

public class WALK_NormalGang : NormalGang_FSM
{
    private Vector2 m_DistanceBetPlayer;
    private Transform m_EnemyTransform;

    public WALK_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_EnemyAnimator.SetBool("isWalk", true);
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
        
        if (m_DistanceBetPlayer.magnitude > m_Enemy.p_MinFollowDistance)
        {
            if (m_DistanceBetPlayer.x > 0)  // 적이 더 오른쪽
            {
                m_Enemy.MoveByDirection_FUpdate(false);
            }
            else   // 적이 더 왼쪽
            {
                m_Enemy.MoveByDirection_FUpdate(true);
            }
        }
        else    // MinFollowDistance 안쪽일 경우
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetBool("isWalk", false);
    }

    public override void NextPhase()
    {
        
    }
}


public class ATTACK_NormalGang : NormalGang_FSM
{
    private readonly SuperArmorMgr m_SuperArmor;
    private readonly Transform m_EnemyTransform;
    private readonly Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private int m_Angle = 0;

    public ATTACK_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_SuperArmor = m_EnemyAnimator.GetComponent<SuperArmorMgr>();
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }

    public override void StartState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Phase = 0;

        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
        
        if(m_DistanceBetPlayer.x > 0 && m_Enemy.m_isRightHeaded == true)
            m_Enemy.setisRightHeaded(false);
        else if (m_DistanceBetPlayer.x < 0 && m_Enemy.m_isRightHeaded == false)
            m_Enemy.setisRightHeaded(true);
        
        m_SuperArmor.doSuperArmor();
        m_SuperArmor.SetCallback(NextPhase);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 1:
                m_Angle = StaticMethods.getAnglePhase(m_Enemy.p_GunPos.position,
                    m_PlayerTransform.position, 3, 20);
                m_EnemyAnimator.SetInteger("FireAngle", m_Angle);
                Debug.Log(m_Angle);
                m_Phase = 2;
                break;
            
            case 2:
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                    m_Phase = 3;
                break;
            
            case 3:
                if (m_Enemy.GetDistBetPlayer().magnitude > m_Enemy.p_MinFollowDistance)
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.WALK);
                }
                else
                {
                    this.StartState();
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_EnemyAnimator.SetInteger("FireAngle", -1);
    }

    public override void NextPhase()
    {
        m_Phase++;
    }
}