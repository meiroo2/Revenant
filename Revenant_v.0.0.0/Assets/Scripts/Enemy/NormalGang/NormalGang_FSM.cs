﻿using System.Collections;
using FMODUnity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class NormalGang_FSM : Enemy_FSM
{
    // Member Variables
    protected NormalGang m_Enemy;
    protected Animator m_Animator;
    protected Rigidbody2D m_Rigid;
    protected Transform m_Transform;

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

    protected void InitFSM()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Rigid = m_Enemy.m_EnemyRigid;
        m_Transform = m_Enemy.transform;
    }
}

public class IDLE_NormalGang : NormalGang_FSM
{
    private AnimatorStateInfo m_CurAnimState;
    private bool m_isPatrol = false;
    private bool m_isLookAround = false;
    private int m_PatrolIdx = 0;
    private int m_Phase = 0;

    private CoroutineHandler m_CoroutineHandler;
    private CoroutineElement m_CoroutineElement;

    private readonly int IsWalk = Animator.StringToHash("IsWalk");
    private readonly int IsTurn = Animator.StringToHash("IsTurn");

    public IDLE_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
        _enemyState = EnemyState.Idle;
    }

    public override void StartState()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        m_isPatrol = false;
        m_isLookAround = m_Enemy.p_IsLookAround;
        m_Phase = 0;
        m_PatrolIdx = 0;
        m_Enemy.ChangeAnimator(true);
        m_Animator.SetBool(IsWalk, false);

        if (m_Enemy.p_PatrolPos.Length > 0)
        {
            m_isPatrol = true;
            m_PatrolIdx = 0;
            m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPos[m_PatrolIdx].position);
            m_Animator.SetBool(IsWalk, true);
        }
    }

    public override void UpdateState()
    {
        switch (m_isPatrol)
        {
            case true when !m_isLookAround: // Patrol
                switch (m_Phase)
                {
                    case 0: // 해당 포지션으로 이동
                        m_Enemy.SetRigidToPoint();
                        if (Mathf.Abs(m_Transform.position.x - m_Enemy.p_PatrolPos[m_PatrolIdx].position.x) < 0.1f)
                        {
                            m_PatrolIdx++;

                            if (m_PatrolIdx >= m_Enemy.p_PatrolPos.Length)
                                m_PatrolIdx = 0;

                            m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPos[m_PatrolIdx].position);

                            m_Phase = 1;
                        }

                        break;

                    case 1: // 포지션 도착(좌우 돌아야 함)
                        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
                        m_Animator.SetBool(IsWalk, false);
                        m_Animator.SetBool(IsTurn, true);
                        m_Phase = 2;
                        break;

                    case 2: // 애니메이션 종료 체크
                        CheckTurn();
                        break;

                    case 3: // Turn 끝
                        m_Animator.SetBool(IsTurn, false);
                        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                        m_CoroutineElement =
                            m_CoroutineHandler.StartCoroutine_Handler(LookDelay(m_Enemy.p_LookAroundDelay));
                        m_Phase = 4;
                        break;

                    case 4: // 대기시간
                        break;

                    case 5:
                        m_Animator.SetBool(IsWalk, true);
                        m_Phase = 0;
                        break;
                }

                break;

            case false when m_isLookAround: // 가만히 서서 배회
                switch (m_Phase)
                {
                    case 0: // Turn 시작
                        m_Animator.SetBool(IsTurn, true);
                        m_Phase = 1;
                        break;

                    case 1: // Turn 체크
                        CheckTurn();
                        break;

                    case 2: // Turn 끝
                        m_Animator.SetBool(IsTurn, false);
                        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                        m_CoroutineElement =
                            m_CoroutineHandler.StartCoroutine_Handler(LookDelay(m_Enemy.p_LookAroundDelay));
                        m_Phase = 3;
                        break;

                    case 3: // 코루틴 대기
                        break;

                    case 4:
                        m_Phase = 0;
                        break;
                }

                break;

            case false when !m_isLookAround: // 가만히 있음
                break;
        }

        // 시각 감지 체크(감지하는 순간 바로 FOLLOW 상태전환)
        m_Enemy.RaycastVisionCheck();
        if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
    }

    public override void ExitState()
    {
        m_Rigid.velocity = Vector2.zero;
        m_Animator.SetBool(IsWalk, false);
        m_Animator.SetBool(IsTurn, false);
    }

    public override void NextPhase()
    {
        m_Phase++;
    }

    public IEnumerator LookDelay(float _time)
    {
        yield return new WaitForSeconds(_time);
        NextPhase();

        m_CoroutineElement.StopCoroutine_Element();
    }

    private void CheckTurn()
    {
        m_CurAnimState = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_CurAnimState.IsName("Turn") && m_CurAnimState.normalizedTime >= 1f)
        {
            NextPhase();
        }
    }
}

public class FOLLOW_NormalGang : NormalGang_FSM // 추격입니다
{
    private Vector2 m_DistanceBetPlayer;
    private AnimatorStateInfo m_CurAnimState;
    private float m_StuckTimer = 0f;
    private int m_Phase = 0;
    private bool m_IsFirst = true;
    private readonly int IsChange = Animator.StringToHash("IsChange");
    private readonly int IsWalk = Animator.StringToHash("IsWalk");

    public FOLLOW_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }

    public override void StartState()
    {
        m_Animator.SetBool(IsWalk, true);
        m_IsFirst = true;

        if (!m_Enemy.m_PlayerCognition)
            m_Phase = 0;
        else
            m_Phase = 3;
    }

    public override void UpdateState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        switch (m_Phase)
        {
            case 0: // 체인지 애니메이션 대기 + 느낌표 출력
                _enemyState = EnemyState.Alert;
                m_Enemy.m_Alert.SetAlertActive(true);
                m_Animator.SetTrigger(IsChange);
                m_Phase = 1;
                break;

            case 1: // 체인지 애니메이션 체크
                CheckChange();
                break;

            case 2: // 체인지 끝
                m_Enemy.ChangeAnimator(false);
                m_Animator.SetBool(IsWalk, true);
                m_Enemy.m_PlayerCognition = true;
                m_Phase = 3;
                break;

            case 3: // 인식은 했으나 사정거리 안에 들어오지 못함
                //Debug.Log("사정거리 밖");
                //m_Enemy.GoToPlayerRoom();
                
                // 적 상태 CHASE
                _enemyState = EnemyState.Chase;
                
                // 적이 계단 위에 있고 플레이어가 계단 위에 있으면
                if (m_Enemy.bIsOnStair && m_Enemy.m_Player.bIsOnStair)
                {
                    // 계단으로 향해 이동하지 않음 - 플레이어에게 이동
                    m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.transform.position.x));
                }
                // 플레이어가 계단 밖으로 나갔지만 적이 계단 위에 있으면
                else if (m_Enemy.m_Player.bIsOutOfStair && m_Enemy.bIsOnStair)
                {
                    // 계단 밖 센서로 이동
                    m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.PlayerOutOfStairVector.x));
                }
                // 적이 문으로 향해 이동할 때 혹은 계단으로 향해 이동할 때 (오브젝트 사용 관련 조건문)
                else if (m_Enemy.bMoveToUsedDoor || m_Enemy.bMoveToUsedStair)
                {
                    // 플레이어가 사용한 오브젝트로 이동
                    m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.PlayerUsedObjectVector.x));
                }
                else
                {
                    m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.transform.position.x));
                }
                
                // 플레이어와 적이 같은 층에 있다면 문 사용 X
                if (Mathf.Abs(m_Enemy.transform.position.y - m_Enemy.m_Player.transform.position.y) <= 0.5f)
                {
                    m_Enemy.bMoveToUsedDoor = false;
                }

                if (m_DistanceBetPlayer.magnitude < m_Enemy.p_AttackDistance)
                    m_Phase = 4;
                break;
            case 4: // 사정거리 도달
                _enemyState = EnemyState.Attack;
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                m_Phase = 5;
                break;
        }
    }

    public override void ExitState()
    {
        m_Animator.SetBool(IsWalk, false);
        m_Phase = 0;
    }

    public override void NextPhase()
    {
        m_Phase++;
    }

    private void CheckChange()
    {
        m_CurAnimState = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_CurAnimState.IsName("Change") && m_CurAnimState.normalizedTime >= 1f)
        {
            NextPhase();
        }
    }

    private void MoveTowardPlayer()
    {
        if (!(m_DistanceBetPlayer.magnitude > m_Enemy.p_AttackDistance)) return;
        m_Enemy.SetRigidByDirection(!(m_DistanceBetPlayer.x > 0));
    }
}


public class ATTACK_NormalGang : NormalGang_FSM
{
    private readonly Transform m_EnemyTransform;
    private Transform m_PlayerTransform;
    private Vector2 m_DistanceBetPlayer;
    private int m_Phase = 0;
    private int m_Angle = 0;
    private float m_Timer = 0.2f;

    private AnimatorStateInfo m_AnimatorState;
    private readonly int IsWalk = Animator.StringToHash("IsWalk");
    private static readonly int Melee = Animator.StringToHash("Melee");
    private static readonly int FireAngle = Animator.StringToHash("FireAngle");

    public ATTACK_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.GetComponentInChildren<Animator>();
        m_EnemyTransform = m_Enemy.transform;
    }

    public override void StartState()
    {
        m_PlayerTransform = m_Enemy.m_PlayerTransform;
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        m_Phase = 0;

        m_DistanceBetPlayer = m_Enemy.GetDistBetPlayer();

        if (m_DistanceBetPlayer.x > 0 && m_Enemy.m_IsRightHeaded == true)
            m_Enemy.setisRightHeaded(false);
        else if (m_DistanceBetPlayer.x < 0 && m_Enemy.m_IsRightHeaded == false)
            m_Enemy.setisRightHeaded(true);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0: // 콜백 설정 + 느낌표 채우기 시작 + 좌우반전
                m_Enemy.m_Alert.SetCallback(NextPhase, true);
                m_Enemy.m_Alert.SetAlertFill(true);

                m_Animator.SetInteger(FireAngle, -1);
                m_Enemy.SetViewDirectionToPlayer();
                m_Animator.SetBool(IsWalk, false);
                m_Phase = 1;
                break;

            case 1: // 느낌표 채우는 중 (CallBack 대기)
                //Debug.Log("콜백 대기 느낌표 채움");
                break;

            case 2: // 근접공격 사거리 = 3, 총 사거리 = 4
                m_Phase = m_Enemy.GetDistBetPlayer().magnitude <= m_Enemy.p_MeleeDistance ? 3 : 5;
                break;

            case 3: // 칼로 공격 
                m_Animator.SetTrigger(Melee);
                m_Enemy.p_MeleeWeapon.Fire();
                m_Phase = 4;
                break;

            case 4: // 칼 공격 중(애니메이션 종료 대기)
                m_AnimatorState = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (m_AnimatorState.normalizedTime >= 1f)
                {
                    m_Phase = 8;
                }

                break;

            case 5: // 총으로 공격 
                m_Enemy.m_EnemyRotation.RotateEnemyArm();
                m_Enemy.m_WeaponMgr.ChangeWeapon(0); // 총으로 무기 변경
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();

                m_Angle = StaticMethods.getAnglePhase(m_Enemy.m_GunPos.position,
                    m_PlayerTransform.position, 3, 20);

                m_Animator.SetInteger(FireAngle, m_Angle);
                m_Phase = 6;
                break;

            case 6: // 즉시 -1로 바꿔 재발사 금지
                m_Animator.SetInteger(FireAngle, -1);
                m_Phase = 7;
                break;

            case 7: //  후딜레이 애니 계산
                CheckFireAniEnd();
                break;

            case 8: // 거리 재판별
                m_Enemy.m_Alert.SetAlertFill(false);
                m_Phase = 9;
                if (m_Enemy.GetDistBetPlayer().magnitude > m_Enemy.p_AttackDistance)
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                }
                else
                {
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                }

                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Animator.SetInteger(FireAngle, -2);
        m_Animator.SetBool(IsWalk, true);

        m_Enemy.m_Alert.SetAlertFill(false);
    }

    public override void NextPhase()
    {
        //Debug.Log("Next호출");
        m_Phase++;
    }

    private void CheckFireAniEnd()
    {
        m_AnimatorState = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (m_AnimatorState.IsName("up_r") || m_AnimatorState.IsName("front_r") ||
            m_AnimatorState.IsName("down_r"))
        {
            if (m_AnimatorState.normalizedTime >= 1f)
            {
                m_Animator.SetInteger("FireAngle", -2);
                NextPhase();
            }
        }
    }

    private IEnumerator ForDelay(float _delay, int _phase)
    {
        yield return new WaitForSeconds(_delay);
        m_Phase = _phase;
    }
}


public class STUN_NormalGang : NormalGang_FSM
{
    private int m_Phase;
    private Enemy_Alert m_Alert;

    public STUN_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_Alert = m_Enemy.m_Alert;
        InitFSM();
    }

    public override void StartState()
    {
        m_Phase = 0;
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;

        m_Alert.SetCallback(NextPhase, true);
        m_Alert.SetAlertSpeed(m_Enemy.p_StunAlertSpeed);
        m_Alert.SetAlertStun();
    }

    public override void UpdateState()
    {
        if (m_Phase == 0)
            return;

        if (m_Enemy.m_PlayerCognition)
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        else
        {
            m_Enemy.StartPlayerCognition();
        }
    }

    public override void ExitState()
    {
        m_Alert.SetAlertSpeed(m_Enemy.p_AlertSpeed);
    }

    public override void NextPhase()
    {
        m_Phase = 1;
    }
}


public class DEAD_NormalGang : NormalGang_FSM
{
    private float m_Time = 3f;

    public DEAD_NormalGang(NormalGang _enemy)
    {
        m_Enemy = _enemy;
        m_Animator = m_Enemy.GetComponentInChildren<Animator>();
    }

    public override void StartState()
    {
        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.m_Alert.gameObject.SetActive(false);
        m_Enemy.m_EnemyRigid.velocity = Vector2.zero;
        m_Enemy.SendDeathAlarmToSpawner();
    }

    public override void UpdateState()
    {
        m_Time -= Time.deltaTime;
        if (m_Time <= 0f)
            m_Enemy.gameObject.SetActive(false);
    }

    public override void ExitState()
    {
    }

    public override void NextPhase()
    {
    }
}