using System.Collections;
using UnityEditor;
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
    private bool m_isPatrol = false;
    private int m_PatrolIdx = 0;
    private float m_InternalTimer = 0f;
    private Transform m_EnemyTransform;
    
    public IDLE_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;

        if (m_Enemy.p_PatrolPos.Length > 0)
            m_isPatrol = true;
    }

    public override void StartState()
    {
        m_EnemyAnimator.SetBool("isWalk", false);
        m_InternalTimer = m_Enemy.p_LookAroundDelay;

        if (m_Enemy.p_PatrolPos.Length > 0)
        {
            m_isPatrol = true;
            m_PatrolIdx = 0;
            m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPos[m_PatrolIdx].position);
        }
    }

    public override void UpdateState()
    {
        switch (m_isPatrol)
        {
            case true when !m_Enemy.p_IsLookAround:
                m_Enemy.MoveToPoint_FUpdate();
                if (Mathf.Abs(m_EnemyTransform.position.x - m_Enemy.p_PatrolPos[m_PatrolIdx].position.x) < 0.2f)
                {
                    m_PatrolIdx++;
                
                    if (m_PatrolIdx >= m_Enemy.p_PatrolPos.Length)
                        m_PatrolIdx = 0;

                    m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPos[m_PatrolIdx].position);
                }
                break;
            
            case false when m_Enemy.p_IsLookAround:
                m_InternalTimer -= Time.deltaTime;
                if (m_InternalTimer <= 0f)
                {
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                    m_InternalTimer = m_Enemy.p_LookAroundDelay;
                }
                break;
        }
        
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

public class WALK_NormalGang : NormalGang_FSM   // 추격입니다
{
    private Vector2 m_DistanceBetPlayer;
    private Transform m_EnemyTransform;
    private float m_StuckTimer = 0f;
    private int m_Phase = 0;

    public WALK_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_EnemyAnimator.SetBool("isWalk", true);
        m_StuckTimer = Random.Range(0f, 1f);
        m_Phase = 0;
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        switch (m_Phase)
        {
            case 0: // 인식은 했으나 사정거리 안에 들어오지 못함
                m_Enemy.GoToPlayer();
                if (m_DistanceBetPlayer.magnitude < m_Enemy.p_MinFollowDistance)
                    m_Phase = 1;
                break;
            
            case 1: // 사정거리 안에서 겹침을 막기 위해 랜덤타임만큼 더 감
                MoveTowardPlayer();
                m_StuckTimer -= Time.deltaTime;
                if (m_StuckTimer <= 0f)
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                break;
        }
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetBool("isWalk", false);
        m_Phase = 0;
    }

    public override void NextPhase()
    {
        
    }

    private void MoveTowardPlayer()
    {
        if (!(m_DistanceBetPlayer.magnitude > m_Enemy.p_MinFollowDistance)) return;
        m_Enemy.MoveByDirection_FUpdate(!(m_DistanceBetPlayer.x > 0));
    }
}


public class ATTACK_NormalGang : NormalGang_FSM
{
    private readonly Transform m_EnemyTransform;
    private readonly Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private int m_Angle = 0;
    private float m_Timer = 0.2f;

    public ATTACK_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
    }

    public override void StartState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Phase = 0;

        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();
        
        if(m_DistanceBetPlayer.x > 0 && m_Enemy.m_IsRightHeaded == true)
            m_Enemy.setisRightHeaded(false); 
        else if (m_DistanceBetPlayer.x < 0 && m_Enemy.m_IsRightHeaded == false)
            m_Enemy.setisRightHeaded(true);

        m_Enemy.m_IsFoundPlayer = true;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0: // 근접공격 판단(이미 원거리 공격 사거리 이내에는 들어옴)
                m_Phase = m_Enemy.GetDistBetPlayer().magnitude <= m_Enemy.p_CloseAttackDistance ? 2 : 1;
                break;
            
            case 1: // 사격
                m_Enemy.m_EnemyRotation.RotateEnemyArm();
                m_Enemy.m_WeaponMgr.ChangeWeapon(0);    // 총으로 무기 변경
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                m_Angle = StaticMethods.getAnglePhase(m_Enemy.p_GunPos.position,
                    m_PlayerTransform.position, 3, 20);
                m_EnemyAnimator.SetInteger("FireAngle", m_Angle);
                m_Phase = 3;
                break;
            
            case 2:
                m_Enemy.m_WeaponMgr.ChangeWeapon(1);    // 칼로 무기 변경
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                m_Timer = 0.2f;
                m_Phase = 5;
                break;
            
            case 3: // 사격 애니메이션 계산
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    m_EnemyAnimator.SetInteger("FireAngle", -1);
                    m_Phase = 4;
                }
                break;
            
            case 4: // 사격 이후 후딜레이 애니메이션 계산
                if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    m_Phase = 5;
                }
                break;
            
            case 5: // 공격 이후 0.2초 후딜레이 계산
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                {
                    m_Enemy.SetViewDirectionToPlayer();
                    m_Timer = m_Enemy.p_AttackDelay;
                    m_Phase = 6;
                }
                break;
            
            case 6: // 공격 딜레이 계산
                m_Timer -= Time.deltaTime;
                if (m_Timer <= 0f)
                    m_Phase = 7;
                break;
            
            case 7: // 사거리 안쪽이면 공격 처음부터, 아니면 따라가기(WALK)
                if (m_Enemy.GetDistBetPlayer().magnitude > m_Enemy.p_MinFollowDistance)
                {
                    m_EnemyAnimator.SetBool("isNear", false);
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.WALK);
                }
                else
                {
                    m_EnemyAnimator.SetBool("isNear", true);
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
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
        Debug.Log("Next호출");
        m_Phase++;
    }

    private IEnumerator ForDelay(float _delay, int _phase)
    {
        yield return new WaitForSeconds(_delay);
        m_Phase = _phase;
    }
}


public class STUN_NormalGang : NormalGang_FSM
{
    public STUN_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        CoroutineHandler.Start_Coroutine(ExitStun());
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

    private IEnumerator ExitStun()
    {
        yield return new WaitForSeconds(m_Enemy.p_stunTime);
        m_Enemy.ChangeEnemyFSM(EnemyStateName.WALK);
    }
}


public class DEAD_NormalGang : NormalGang_FSM
{
    public DEAD_NormalGang(NormalGang _enemy)
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