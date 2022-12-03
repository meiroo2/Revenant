using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

public class IDLE_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private int m_PatrolIdx = 0;
    private float m_InternalTimer = 0f;
    private Transform m_EnemyTransform;
    private int m_Phase = 0;
    private float m_Timer = 0;
    private bool m_IsFirst = true;
    
    private readonly int Turn = Animator.StringToHash("Turn");
    private readonly int Walk = Animator.StringToHash("Walk");

    // Constructor
    public IDLE_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_InternalTimer = m_Enemy.p_LookAroundDelay;

        m_PatrolIdx = 0;
        m_Phase = 0;
        m_Timer = 0f;
    }

    public override void UpdateState()
    {
        if (m_Enemy.p_IsLookAround && !m_Enemy.m_IsPatrol)
        {
           LookAround();
        }
        else if (!m_Enemy.p_IsLookAround && m_Enemy.m_IsPatrol)
        {
           Patrol();
        }

        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null) && !m_Enemy.m_PlayerCognition)
        {
            m_Enemy.StartPlayerCognition();
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_IsTurning = false;
        m_EnemyAnimator.SetInteger(Turn, 0);
    }

    public override void NextPhase()
    {
        
    }

    private void LookAround()
    {
        // 좌우 반전만
        switch (m_Phase)
        {
            case 0:
                m_EnemyAnimator.SetInteger(Turn, 1);
                m_Enemy.m_IsTurning = true;
                m_Phase = 1;
                break;
                
            case 1:
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_EnemyAnimator.SetInteger(Turn, 0);
                    m_Enemy.m_IsTurning = false;
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                    m_Phase = 2;
                }
                break;
                
            case 2:
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_LookAroundDelay)
                {
                    m_Timer = 0f;
                    m_Phase = 0;
                }
                break;
        }
    }

    private void Patrol()
    {
        // Patrol
           switch (m_Phase)
           {
               case 0:
                   // 방향 판단
                   m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPosArr[m_PatrolIdx].position);
                   if (m_Enemy.IsExistInEnemyView(m_Enemy.GetMovePoint()))
                   {
                       m_EnemyAnimator.SetInteger(Walk, 1);
                       m_Enemy.SetRigidToPoint();
                       m_Phase = 2;
                   }
                   else
                   {
                       m_Enemy.m_IsTurning = true;
                       m_EnemyAnimator.SetInteger(Turn, 1);
                       m_Phase = 1;
                   }
                   break;
               
               case 1:
                   if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                   {
                       m_Enemy.m_IsTurning = false;
                       m_EnemyAnimator.SetInteger(Turn, 0);
                       m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                       
                       m_EnemyAnimator.SetInteger(Walk, 1);
                       m_Enemy.SetRigidToPoint();
                       m_Phase = 2;
                   }
                   break;
               
               case 2:
                   // 도착
                   if (Mathf.Abs(m_EnemyTransform.position.x - m_Enemy.GetMovePoint().x) < 0.05f)
                   {
                       m_EnemyAnimator.SetInteger(Walk, 0);
                       m_Enemy.ResetRigid();
                       m_Phase = 3;
                   }
                   else if (!m_Enemy.IsExistInEnemyView(m_Enemy.GetMovePoint()))
                   {
                       m_EnemyAnimator.SetInteger(Walk, 0);
                       m_Enemy.ResetRigid();
                       m_Phase = 3;
                   }
                   break;
               
               case 3:
                   m_Timer += Time.deltaTime;
                   if (m_Timer <= m_Enemy.p_LookAroundDelay)
                   {
                       break;
                   }
                   
                   if (m_Enemy.p_PatrolPosArr.Length == 1)
                   {
                       m_Phase = 4;
                   }
                   else if (m_PatrolIdx < m_Enemy.p_PatrolPosArr.Length - 1)
                   {
                       m_PatrolIdx++;
                       m_Timer = 0f;
                       m_Phase = 0;
                   }
                   else
                   {
                       m_PatrolIdx = 0;
                       m_Timer = 0f;
                       m_Phase = 0;
                   }
                   break;
           }
    }
}

public class FOLLOW_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private float m_DistanceBetPlayer;
    private Transform m_EnemyTransform;
    private float m_OverlapTimer = 0f;
    private int m_Phase = 0;
    
    // Constructor
    public FOLLOW_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        _enemyState = EnemyState.Chase;
        
        if (m_Enemy.WayPointsVectorList.Count == 0 && !m_Enemy.bIsOnStair && m_Enemy.m_Player.bIsOnStair)
        {
            if (m_Enemy.IsPlayerUpper())
                m_Enemy.bMoveToUseStairUp = true;
            else
                m_Enemy.bMoveToUseStairDown = true;
        }
        
        m_Enemy.StartWalkSound(true, 0.3f);
        
        m_Phase = 0;
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();

        if (m_DistanceBetPlayer > m_Enemy.p_MeleeDistance)
        {
            m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer(), m_Enemy.p_FollowSpeedMulti);
            
            if (m_Enemy.WayPointsVectorList.Count != 0 && m_Enemy.WayPointsIndex < m_Enemy.WayPointsVectorList.Count)
            {
                m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x), m_Enemy.p_FollowSpeedMulti);
            }
            else
            {
                m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.transform.position.x), m_Enemy.p_FollowSpeedMulti);
            }

            if (m_Enemy.IsSameFloorWithPlayer(m_Enemy.bMoveToUsedDoor, m_Enemy.bIsOnStair, m_Enemy.m_Player.bIsOnStair))
            {
                m_Enemy.MoveToPlayer();
            }
            else if (m_Enemy.IsSameStairWithPlayer(m_Enemy.bIsOnStair, m_Enemy.m_Player.bIsOnStair, m_Enemy.EnemyStairNum, m_Enemy.m_Player.PlayerStairNum))
            {
                m_Enemy.MoveToPlayer();
            }
        }
        else // MinFollowDistance 안쪽일 경우
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
    }

    public override void ExitState()
    {
        m_Enemy.StartWalkSound(false);
        m_Enemy.ResetRigid();
    }

    public override void NextPhase()
    {
        
    }
}

public class ATTACK_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private readonly Transform m_EnemyTransform;
    private readonly Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private float m_Timer = 0f;

    // Constructor
    public ATTACK_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }

    public override void StartState()
    {
        _enemyState = EnemyState.Chase;
        
        if (m_Enemy.WayPointsVectorList.Count == 0 && !m_Enemy.bIsOnStair && m_Enemy.m_Player.bIsOnStair)
        {
            if (m_Enemy.IsPlayerUpper())
                m_Enemy.bMoveToUseStairUp = true;
            else
                m_Enemy.bMoveToUseStairDown = true;
        }
        
        m_Phase = 0;
        m_Timer = 0f;
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;

        // Attack 시작 시 우선 플레이어 바라봄
        m_Enemy.SetViewDirectionToPlayer();

        // Phase 0으로 초기화 & 공격 모션 시작
        m_Phase = 0;

        // 애니메이터 -> 공격시작
        m_EnemyAnimator.SetInteger("Attack", 1);
        
        // 사운드 플레이
        m_Enemy.m_SoundPlayer.PlayEnemySound(1, 1, m_Enemy.GetBodyCenterPos());
    }

    public override void UpdateState()
    {
        if (m_Enemy.IsSameFloorWithPlayer(m_Enemy.bMoveToUsedDoor, m_Enemy.bIsOnStair, m_Enemy.m_Player.bIsOnStair))
        {
            m_Enemy.MoveToPlayer();
        }
        else if (m_Enemy.IsSameStairWithPlayer(m_Enemy.bIsOnStair, m_Enemy.m_Player.bIsOnStair, m_Enemy.EnemyStairNum, m_Enemy.m_Player.PlayerStairNum))
        {
            m_Enemy.MoveToPlayer();
        }

        switch (m_Phase)
        {
            case 0:
                // AttackTiming에 공격판정
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > m_Enemy.p_AttackTiming)
                {
                    //m_Enemy.m_SoundPlayer.PlayEnemySound(1, 2, m_Enemy.GetBodyCenterPos());
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_Phase = 1;
                }
                break;
            
            case 1:
                // 끝나면 딜레이
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_EnemyAnimator.SetInteger("Attack", 2);
                    m_Phase = 2;
                }
                break;
            
            case 2:
                m_Timer += Time.deltaTime;
                if (m_Timer > m_Enemy.p_DelayAfterAttack)
                {
                    m_EnemyAnimator.SetInteger("Attack", 0);
                    m_Timer = 0f;
                    m_Phase = 3;
                }
                break;

            case 3:    
                if (m_Enemy.GetDistanceBetPlayer() < m_Enemy.p_MeleeDistance)
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
        m_EnemyAnimator.SetInteger("Attack", 0);
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void NextPhase()
    {
        m_Phase = 1;
    }
}

public class DEAD_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private float m_Time = 0f;
    private int m_Phase = 0;
    private bool m_DeathSoundPlayed = false;
    
    private Color m_WhiteColor;
    
    private float m_FadeValue = 1f;
    private readonly int Fade = Shader.PropertyToID("_Fade");
    
    // Constructor
    public DEAD_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
    public override void StartState()
    {
        m_WhiteColor = Color.white;
        
        m_Enemy.m_IsDead = true;
        
        m_DeathSoundPlayed = false;
        m_Phase = 0;
        m_Time = 0f;
        m_FadeValue = 1f;

        m_Enemy.SetEnemyHotBox(false);
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;

        switch (m_Enemy.m_DeathReason)
        {
            case 0:
                Debug.Log("ERR : MeleeGang DeathReason is 0");
                break;
            
            case 1:
                m_EnemyAnimator.SetInteger("Head", 1);
                break;

            case 2:
                m_EnemyAnimator.SetInteger("Body", 1);
                break;
        }
    }

    public override void UpdateState()
    {
        if (!m_DeathSoundPlayed)
        {
            if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                m_DeathSoundPlayed = true;
                m_Enemy.m_SoundPlayer.PlayEnemySound(1, 3, m_Enemy.GetBodyCenterPos());
            }
        }
        
        if (m_Time < 3f)
        {
            m_Time += Time.deltaTime;
            return;
        }

        switch (m_Enemy.m_DeadReasonForMat)
        {
            case 0:
                // 노말 사망
                m_Enemy.m_Renderer.color = m_WhiteColor;
                
                m_WhiteColor.a -= Time.deltaTime;
                if(m_WhiteColor.a <= 0f)
                    m_Enemy.gameObject.SetActive(false);
                break;
            
            case 1:
                // 불릿타임 사망(머터리얼은 이미 교체됨)
                if (m_Enemy.m_CurSpriteMatType is SpriteMatType.ORIGIN or SpriteMatType.DISAPPEAR)
                {
                    m_Enemy.m_Renderer.material.SetFloat(Fade, m_FadeValue);
                    m_FadeValue -= Time.deltaTime;
                    if (m_FadeValue <= 0f)
                    {
                        m_Enemy.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetInteger("Head", 0);
        m_EnemyAnimator.SetInteger("Body", 0);
        m_Enemy.SetEnemyHotBox(true);
    }

    public override void NextPhase()
    {
        
    }
}

public class CHANGE_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private readonly int Change = Animator.StringToHash("Change");
    
    
    // Constructor
    public CHANGE_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        _enemyState = EnemyState.Chase;
        m_Enemy.ResetRigid();
        m_EnemyAnimator.SetInteger(Change, 1);
    }

    public override void UpdateState()
    {
        if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }   
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetInteger(Change, 0);
        m_EnemyAnimator.runtimeAnimatorController = m_Enemy.p_FightAniCont;
    }

    public override void NextPhase()
    {
        
    }
}

public class STUN_MeleeGang : MeleeGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private readonly int Hit = Animator.StringToHash("Hit");
    

    public STUN_MeleeGang(MeleeGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_EnemyAnimator = m_Enemy.m_Animator;
        
        m_Phase = 0;
        m_Timer = 0f;
        
        m_EnemyAnimator.SetInteger(Hit, 1);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    if (m_Enemy.m_CurStunValue >= m_Enemy.p_StunHp)
                    {
                        m_Enemy.ResetStunValue();
                        m_Phase = 1;
                        break;
                    }

                    m_Phase = -1;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                }
                break;
            
            case 1:
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_StunWaitTime)
                {
                    m_Enemy.ResetStunValue();
                    m_Phase = -1;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetInteger(Hit, 0);
    }

    public override void NextPhase()
    {
        
    }
}