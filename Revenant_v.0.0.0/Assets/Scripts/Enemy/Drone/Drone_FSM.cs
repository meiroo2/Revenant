using System;
using System.Collections;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;


public class Drone_FSM : Enemy_FSM
{
    // Member Variables
    protected Drone m_Enemy;
    protected Animator m_Animator;

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

public class IDLE_Drone : Drone_FSM
{
    private float Timer = 0f;
    
    
    public IDLE_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {

    }

    public override void UpdateState()
    {
        if (m_Enemy.p_LookAround)
        {
            Timer += Time.deltaTime;
            if (Timer >= m_Enemy.p_LookAroundDelay)
            {
                m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                Timer = 0f;
            }
        }

        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.StartPlayerCognition();
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

public class PATROL_Drone : Drone_FSM
{
    private Transform m_EnemyTransform;
    private int m_PatrolIdx = 0;
    private int m_Phase = 0;

    public PATROL_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {

        m_EnemyTransform = m_Enemy.transform;
        m_Animator = m_Enemy.m_Animator;

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
        
        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.StartPlayerCognition();
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

public class FOLLOW_Drone : Drone_FSM
{
    // Member Variables
    private float m_DistanceBetPlayer;
    private int m_Phase;
    private CoroutineElement m_CoroutineElement;
    private CoroutineHandler m_CoroutineHandler;
    private readonly int Move = Animator.StringToHash("Move");

    private Transform m_EnemyTransform;
    private Transform m_PlayerTransform;
    private readonly int Stop = Animator.StringToHash("Stop");

    private bool m_FinishDecideMovePos = false;

    public FOLLOW_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {

        m_Animator = m_Enemy.m_Animator;
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        m_Phase = 0;

        m_FinishDecideMovePos = false;
        
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_Player.p_CenterTransform;

        m_Animator.SetInteger(Move, 1);
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();

        switch (m_Phase)
        {
            case 0:     // 초기 판정
                if (m_DistanceBetPlayer > m_Enemy.p_AtkDistance)
                {
                    m_Phase = 1;
                }
                else // MinFollowDistance 안쪽일 경우
                {
                    m_Phase = 2;
                }
                break;
            
            case 1:
                if (Vector2.Distance(m_EnemyTransform.position, m_PlayerTransform.position) >
                    m_Enemy.p_AtkDistance)
                {
                    m_Enemy.ResetMovePoint(m_PlayerTransform.position);
                    m_Enemy.SetRigidToPoint();
                }
                else
                {
                    m_Phase = 2;
                }
                break;
            
            case 2:     // 삐빅떄 Rush Pos 결정
                m_Enemy.ResetMovePoint(m_Enemy.m_Player.p_CenterTransform.position);
                m_Animator.SetInteger(Stop, 1);
                m_Enemy.VelocityBreak(true);
                m_Phase = 3;
                break;
            
            case 3:     // Stop 대기
                if (!m_FinishDecideMovePos)
                {
                    if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=
                        m_Enemy.p_DecidePositionPointTime)
                    {
                        m_Enemy.ResetMovePoint(m_Enemy.m_Player.m_PlayerFootMgr.GetFootRayHit().point);
                        m_FinishDecideMovePos = true;
                    }
                }
                
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.RUSH);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Animator.SetInteger(Move, 0);
        m_Animator.SetInteger(Stop, 0);
        
        m_Enemy.VelocityBreak(false);
    }

    public override void NextPhase()
    {
        m_Phase++;
    }
}

public class RUSH_Drone : Drone_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private Transform m_EnemyTransform;
    private static readonly int Attack = Animator.StringToHash("Attack");

    public RUSH_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_EnemyTransform = m_Enemy.transform;
        m_Phase = 0;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Animator.SetInteger(Attack, 1);
                m_Phase = 1;
                break;
            
            case 1:     // 좌표 계산
                if (Vector2.Distance(m_EnemyTransform.position,m_Enemy.GetMovePoint()) > 0.1f)
                {
                    m_Enemy.SetRigidToPoint(m_Enemy.p_RushSpeedRatio);
                }
                else
                {
                    m_Enemy.ResetRigid();
                    m_Enemy.SetHitBox(false);
                    m_Enemy.m_DeadReason = 2;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.DEAD);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Animator.SetInteger(Attack, 0);
    }

    public override void NextPhase()
    {
        m_Phase++;
    }
}

public class DEAD_Drone : Drone_FSM
{
    // Member Variables
    private AnimatorStateInfo m_StateInfo;

    private CoroutineHandler m_Handler;
    private CoroutineElement m_CoroutineElement;

    public DEAD_Drone(Drone _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.m_Animator;
    }
    
    public override void StartState()
    {
        m_Enemy.m_IsDead = true;
        

        m_CoroutineElement = null;
        m_Handler = GameMgr.GetInstance().p_CoroutineHandler;

        m_Animator = m_Enemy.m_Animator;
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
        
        switch (m_Enemy.m_DeadReason)
        {
            case 0: // 몸체 피격
                m_Enemy.m_SoundPlayer.PlayEnemySound(2, 1, m_Enemy.transform.position);

                
                m_Enemy.m_SimpleEffectPuller.SpawnSimpleEffect(8, new Vector2(m_Enemy.transform.position.x,
                    m_Enemy.transform.position.y - 0.3f));
                
                m_Animator.SetTrigger("Head");
                
                m_Enemy.m_WeponMgr.ChangeWeapon(0);
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                m_CoroutineElement = m_Handler.StartCoroutine_Handler(CheckAniEnd());
                m_Enemy.m_DeadReason = -1;
                break;
            
            case 1: // 폭탄 피격
                m_Enemy.m_SoundPlayer.PlayEnemySound(2, 1, m_Enemy.transform.position);

                m_Enemy.m_SimpleEffectPuller.SpawnSimpleEffect(8, new Vector2(m_Enemy.transform.position.x,
                    m_Enemy.transform.position.y - 0.3f));
                
                m_Animator.SetTrigger("Body");
                
                m_Enemy.m_WeponMgr.ChangeWeapon(0);
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                m_CoroutineElement = m_Handler.StartCoroutine_Handler(CheckAniEnd());
                m_Enemy.m_DeadReason = -1;
                break;
            
            case 2: // 잘 폭발
                m_Enemy.m_SoundPlayer.PlayEnemySound(2, 2, m_Enemy.transform.position);
                
                m_Enemy.m_SimpleEffectPuller.SpawnSimpleEffect(8, m_Enemy.transform.position);
                m_Enemy.m_Renderer.sortingLayerName = "BackGround03";
                m_Animator.SetInteger("Explode", 1);
                
                m_Enemy.m_WeponMgr.ChangeWeapon(0);
                m_Enemy.m_WeponMgr.m_CurWeapon.Fire();
                m_CoroutineElement = m_Handler.StartCoroutine_Handler(CheckAniEnd());
                m_Enemy.m_DeadReason = -1;
                break;
        }
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        m_Animator.SetInteger("Explode", 0);
        
        if (!ReferenceEquals(m_CoroutineElement, null))
        {
            m_CoroutineElement.StopCoroutine_Element();
            m_CoroutineElement = null;
        }
    }

    public override void NextPhase()
    {
       
    }

    private IEnumerator CheckAniEnd()
    {
        m_Enemy.m_Renderer.sortingLayerID = 0;
        
        yield return null;
        AnimatorStateInfo state;
        while (true)
        {
            state = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime >= 1f)
            {
                break;
            }
            yield return null;
        }

        if (!ReferenceEquals(m_CoroutineElement, null))
        {
            m_CoroutineElement.StopCoroutine_Element();
            m_CoroutineElement = null;
        }
        
        m_Enemy.gameObject.SetActive(false);
        yield break;
    }
}