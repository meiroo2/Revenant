using System.Collections;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


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
    private float m_Timer;
    private readonly int Cognition = Animator.StringToHash("Cognition");

    public SpecialForce_IDLE(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
        _enemyState = EnemyState.Idle;
    }
    
    public override void StartState()
    {
        m_Timer = 0f;

        if (m_Enemy.p_PatrolPos.Length > 0)
        {
            m_Phase = -1;
            m_Enemy.ChangeEnemyFSM(EnemyStateName.PATROL);
        }
        else
        {
            m_Phase = 0;
        }
    }

    public override void UpdateState()
    {
        if (m_Enemy.m_PlayerCognition)
        {
            switch (m_Phase)
            {
                case 0:
                    m_Phase = -1;
                    m_Enemy.SetSpriteMode(1);
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                    break;
            }
        }
        else
        {
            switch (m_Phase)
            {
                case 0:
                    if (m_Enemy.p_IsLookAround)
                        m_Phase = 1;
                    else
                        m_Phase = -1;
                    break;
            
                case 1:
                    m_Timer += Time.deltaTime;
                    if (m_Timer >= m_Enemy.p_LookAroundDelay)
                    {
                        m_Timer = 0f;
                        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                        m_Phase = 0;
                    }
                    break;
            }
            
            m_Enemy.RaycastVisionCheck();
            if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
            {
                m_Phase = 0;
                m_Enemy.m_PlayerCognition = true;
            }
        }
    }

    public override void ExitState()
    {
       
    }

    public override void NextPhase()
    {
       
    }
}

public class SpecialForce_PATROL : SpecialForce_FSM
{
    // Member Variables
    private int m_Phase;
    private int m_PatrolIdx = 0;
    private float m_Timer = 0f;
    private readonly int Cognition = Animator.StringToHash("Cognition");
    private static readonly int Walk = Animator.StringToHash("Walk");

    public SpecialForce_PATROL(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Phase = 0;
        m_PatrolIdx = 0;
        m_Timer = 0f;
    }

    public override void UpdateState()
    {
        if (m_Enemy.m_PlayerCognition)
        {
            switch (m_Phase)
            {
                case 0:
                    m_Phase = -1;
                    m_Enemy.SetSpriteMode(1);
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                    break;
            }
        }
        else
        {
            switch (m_Phase)
            {
                case 0:
                    // Patrol Pos 결정
                    m_Enemy.ResetMovePoint(m_Enemy.p_PatrolPos[m_PatrolIdx].position);

                    if(!m_Enemy.IsExistInEnemyView(m_Enemy.GetMovePoint()))
                        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                    
                    m_Enemy.m_Animator.SetInteger(Walk, 1);

                    m_Phase = 1;
                    break;
            
                case 1:
                    // 이동
                    m_Enemy.SetRigidToPoint();
                    if (Mathf.Abs(m_Enemy.transform.position.x - m_Enemy.GetMovePoint().x) < 0.05f)
                    {
                        m_Enemy.m_Animator.SetInteger(Walk, 0);
                        m_Enemy.ResetRigid();
                        m_Phase = 2;
                    }
                    else if (!m_Enemy.IsExistInEnemyView(m_Enemy.GetMovePoint()))
                    {
                        m_Enemy.m_Animator.SetInteger(Walk, 0);
                        m_Enemy.ResetRigid();
                        m_Phase = 2;
                    }
                    break;
                
                case 2:
                    m_Timer += Time.deltaTime;
                    if (m_Timer > m_Enemy.p_LookAroundDelay)
                    {
                        m_Timer = 0f;
                        m_Phase = 3;
                    }
                    break;
                
                case 3:
                    if (m_Enemy.p_PatrolPos.Length == 1)
                    {
                        m_Phase = -1;
                    }
                    else if (m_PatrolIdx < m_Enemy.p_PatrolPos.Length - 1)
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
            
            m_Enemy.RaycastVisionCheck();
            if (!ReferenceEquals(m_Enemy.m_VisionHit.collider, null))
            {
                m_Phase = 0;
                m_Enemy.m_PlayerCognition = true;
            }
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
    private int m_WalkVal = 0;
    
    private CoroutineElement m_RollDelayElement;
    private CoroutineElement m_MouseOnElement;
    private CoroutineElement m_WalkStartCheckElement;
    
    private SpecialForce_UseRange m_UseRange;

    private HideSlot m_Slot;
    private readonly int Walk = Animator.StringToHash("Walk");
    private readonly int Cognition = Animator.StringToHash("Cognition");

    public SpecialForce_FOLLOW(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        _enemyState = EnemyState.Chase;
        m_Slot = null;
        m_UseRange = m_Enemy.m_UseRange;

        m_Enemy.m_PlayerCognition = true;
        m_WalkVal = 0;

        m_WalkStartCheckElement = null;
        
        
        m_MouseOnElement = null;
        m_RollDelayElement = null;
        m_IsMouseCal = false;
        m_RollBook = false;


        m_Phase = 0;
        if (!m_Enemy.p_AlertSystem.m_IsOnline)
        {
            m_Phase = -1;
            m_Enemy.p_AlertSystem.SetFadeInSpeed(m_Enemy.p_Alert_Fade_Speed);
            
            m_Enemy.p_AlertSystem.DoFadeIn(() =>
            {
                m_Enemy.SetSpriteMode(1);
                
                m_WalkStartCheckElement =
                    m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(WalkStartCheck());
                
                m_Phase = 0;
            });
        }
        else
        {
            m_Enemy.SetSpriteMode(1);
            m_Enemy.p_FullAnimator.SetInteger(Cognition, 0);
            m_WalkStartCheckElement =
                m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(WalkStartCheck());
        }
        
        m_Enemy.StartWalkSound(true, 0.25f);
    }

    public override void UpdateState()
    {
        if(!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);

        if (m_Enemy.IsSameFloorWithPlayer(m_Enemy.bMoveToUsedDoor, m_Enemy.bIsOnStair,
                m_Enemy.m_Player.bIsOnStair))
        {
            m_Enemy.MoveToPlayer();
        }
        
        switch (m_Phase)
        {
            case 0:
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
                
                if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_AtkDistance)
                {
                    m_Enemy.ResetRigid();
                    if (!ReferenceEquals(m_WalkStartCheckElement, null))
                    {
                        m_WalkStartCheckElement.StopCoroutine_Element();
                        m_WalkStartCheckElement = null;
                    }
                    m_WalkStartCheckElement = 
                        m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(WalkEndCheck());
                    
                    m_Phase = -1;
                }
                else
                {
                    if (m_Enemy.WayPointsVectorList.Count != 0 && m_Enemy.WayPointsIndex < m_Enemy.WayPointsVectorList.Count)
                    {
                        m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.WayPointsVectorList[m_Enemy.WayPointsIndex].x),
                            m_Enemy.p_RunSpeedMulti);
                    }
                    else
                    {
                        m_Enemy.SetRigidByDirection(!(m_Enemy.transform.position.x > m_Enemy.m_Player.transform.position.x),
                            m_Enemy.p_RunSpeedMulti);
                    }
                }
                break;
        }
    }

    public override void ExitState()
    {
        if (!ReferenceEquals(m_RollDelayElement, null))
        {
            m_RollDelayElement.StopCoroutine_Element();
            m_RollDelayElement = null;
        }

        if (!ReferenceEquals(m_MouseOnElement, null))
        {
            m_MouseOnElement.StopCoroutine_Element();
            m_MouseOnElement = null;
        }

        if (!ReferenceEquals(m_WalkStartCheckElement, null))
        {
            m_WalkStartCheckElement.StopCoroutine_Element();
            m_WalkStartCheckElement = null;
        }
        
        m_Enemy.ResetRigid();
        m_Enemy.StartWalkSound(false);
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

    private IEnumerator WalkStartCheck()
    {
        Animator FullAnimator = m_Enemy.p_FullAnimator;
        m_WalkVal = 1;
        FullAnimator.SetInteger(Walk, m_WalkVal);
        
        while (true)
        {
            yield return null;
            
            if (FullAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                m_WalkVal = 2;
                FullAnimator.SetInteger(Walk, m_WalkVal);
                break;
            }
        }
        
        yield break;
    }

    private IEnumerator WalkEndCheck()
    {
        Animator FullAnimator = m_Enemy.p_FullAnimator;
        m_WalkVal = 3;
        FullAnimator.SetInteger(Walk, m_WalkVal);
        
        while (true)
        {
            yield return null;
            
            if (FullAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                m_WalkVal = 0;
                FullAnimator.SetInteger(Walk, m_WalkVal);
                break;
            }
        }
        m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        yield break;
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
        m_Enemy.m_SoundPlayer.PlayEnemySound(4,1,m_Enemy.transform.position);
        
        m_Animator = m_Enemy.m_Animator;
        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.SetSpriteMode(0);
        m_Animator.runtimeAnimatorController = m_Enemy.p_FightFullCont;
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

        if (m_Enemy.m_ATK_RollState == 2)
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            m_Element.StopCoroutine_Element();
            yield break;
        }
        
        if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_AtkDistance)
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
        else
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
           
        
        m_Element.StopCoroutine_Element();
        yield break;
    }
}

public class SpecialForce_ATTACK : SpecialForce_FSM
{
    // Member Variables
    private CoroutineElement m_FullAnimCheckElement;
    private CoroutineElement m_ArmAnimCheckElement;
    private bool m_IsWeaponReady = false;
    private float m_Timer = 0f;
    private readonly int Cognition = Animator.StringToHash("Cognition");

    private Transform m_EnemyTransform;
    
    private int m_GapMoveState = 0;
    private float m_Distance = 0f;
    private int m_AtkPhase = 0;
    private int m_BulletCount = 0;
    
    private int m_AtkInputVal = 0;

    private int m_Phase = 0;
    
    private readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

    private const float m_MagicGap = 0.05f;


    public SpecialForce_ATTACK(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        _enemyState = EnemyState.Chase;

        m_EnemyTransform = m_Enemy.transform;
        m_IsWeaponReady = false;
        m_Timer = 0f;
        
        m_BulletCount = 2;
        m_GapMoveState = 0;
        m_Distance = 0f;
        m_AtkInputVal = 0;
        
        m_FullAnimCheckElement = null;
        m_ArmAnimCheckElement = null;

        m_Phase = 0;
        
        m_AtkPhase = 0;
        m_Enemy.p_AlertSystem.SetAlertSpeed(m_Enemy.p_Alert_Speed);
        // First
        switch (m_Enemy.m_ATK_RollState)
        {
            case 0:
                // Roll에서 넘어오지 않음
                m_FullAnimCheckElement = 
                    m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CheckCognitionAnim());
                
                m_Enemy.p_AlertSystem.AlertGaugeUp(() => m_Phase = 2);
                break;
            
            case 1:
                // 개시 Roll에서 넘어옴
                m_Phase = 1;
                m_Enemy.m_ATK_RollState = 0;
                
                m_FullAnimCheckElement = 
                    m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CheckCognitionAnim());
                
                m_Enemy.p_AlertSystem.AlertGaugeUp(() => m_Phase = 2);
                
                break;
            
            case 2:
                // 게이지 채웠는데 근거리여서 구른 후
                m_Enemy.m_ATK_RollState = 0;
                m_Enemy.SetSpriteMode(2);
                m_IsWeaponReady = true;
                FacePlayer();
                m_Phase = 3;
                break;
        }
        
       
    }

    public override void UpdateState()
    {
        if (m_Enemy.IsSameFloorWithPlayer(m_Enemy.bMoveToUsedDoor, m_Enemy.bIsOnStair,
                m_Enemy.m_Player.bIsOnStair))
        {
            m_Enemy.MoveToPlayer();
        }

        
        m_Distance = m_Enemy.GetDistanceBetPlayer();

        switch (m_Phase)
        {
            case 0:
                // 첫 조우 시 마우스 닿았는지 1회 측정
                if (m_Enemy.m_IsMouseTouched)
                {
                    if (StaticMethods.GetProbabilityWinning(m_Enemy.p_Roll_Chance))
                    {
                        Debug.Log("첫 ATK_Distance 조우시 구르기 확률당첨");
                        m_Enemy.m_ATK_RollState = 1;

                        if (!m_Enemy.IsFacePlayer())
                            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                        
                        m_Enemy.p_AlertSystem.CancelGaugeUp();
                        
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.ROLL);
                    }
                }
                
                m_Phase = 1;
                break;
            
            case 1:
                // Gap Move(Alert 올라가는 중)
                FacePlayer();
                GapMove();
                break;
            
            case 2:
                // 아무튼 게이지를 다 채움
                m_Enemy.m_SoundPlayer.PlayEnemySound(4,3,m_EnemyTransform.position);
                FacePlayer();
                
                if (m_Distance <= m_Enemy.p_MeleeRollDistance)
                {
                    // 게이지 채웠는데 거리 내부
                    m_Enemy.m_ATK_RollState = 2;
                    m_Enemy.ChangeEnemyFSM(EnemyStateName.ROLL);
                    m_Phase = -1;
                    break;
                }
                else
                {
                    if (!m_IsWeaponReady)
                    {
                        m_Enemy.SetSpriteMode(2);
                        m_IsWeaponReady = true;
                    }
                    m_Phase = 3;
                }
                break;
            
            case 3:
                // 1회차 발사
                m_Enemy.ResetRigid();
                StopAnim();
                m_AtkPhase = 0;
                m_ArmAnimCheckElement =
                    m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CheckArmAnim());
                m_Phase = 4;
                break;
            
            case 4:
                // 2회차 발사
                if (m_AtkPhase == 1)
                {
                    m_AtkPhase = 0;
                    m_ArmAnimCheckElement =
                        m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(CheckArmAnim());
                    m_Phase = 5;
                }
                break;
            
            case 5:
                // 2회차 사격 끙남
                if (m_AtkPhase == 1)
                {
                    FacePlayer();
                    GapMove();
                    m_Timer += Time.deltaTime;

                    if (m_Timer < m_Enemy.p_Fire_Delay)
                        break;

                    m_Timer = 0f;
                    if (m_Distance <= m_Enemy.p_AtkDistance)
                    {
                        m_Phase = -1;
                        StartState();
                    }
                    else
                    {
                        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
                        m_Phase = -1;
                    }
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.ResetRigid();
        m_Enemy.p_FullAnimator.SetInteger(Cognition, 0);
        if (!ReferenceEquals(m_FullAnimCheckElement, null))
        {
            m_FullAnimCheckElement.StopCoroutine_Element();
            m_FullAnimCheckElement = null;
        }

        if (!ReferenceEquals(m_ArmAnimCheckElement, null))
        {
            m_ArmAnimCheckElement.StopCoroutine_Element();
            m_ArmAnimCheckElement = null;
        }
    }

    private void StopAnim()
    {
        m_Enemy.p_BodyAnimator.SetInteger(Walk, 0);
        m_Enemy.p_LegAnimator.SetInteger(Walk, 0);
        m_Enemy.p_ArmAnimator.SetInteger(Walk, 0);
    }

    private void FacePlayer()
    {
        if(!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
    }

    private void GapMove()
    {
        if (m_Distance > m_Enemy.p_GapDistance + m_MagicGap)
        {
            // Gap 맞추러 플레이어로 다가가야 함
            m_GapMoveState = 0;

            m_Enemy.p_BodyAnimator.SetInteger(Walk, 1);
            m_Enemy.p_LegAnimator.SetInteger(Walk, 1);
            m_Enemy.p_ArmAnimator.SetInteger(Walk, 1);

            m_Enemy.SetRigidByDirection(m_Enemy.m_IsRightHeaded);
            
            m_Enemy.StartWalkSound(true, 0.5f);
        }
        else if (m_Distance > m_Enemy.p_GapDistance - m_MagicGap &&
                 m_Distance < m_Enemy.p_GapDistance + m_MagicGap)
        {
            // MagicGap 이내 - 멈춤
            m_GapMoveState = 1;

            m_Enemy.p_BodyAnimator.SetInteger(Walk, 0);
            m_Enemy.p_LegAnimator.SetInteger(Walk, 0);
            m_Enemy.p_ArmAnimator.SetInteger(Walk, 0);

            m_Enemy.ResetRigid();
            
            m_Enemy.StartWalkSound(false);
        }
        else if (m_Distance < m_Enemy.p_GapDistance - m_MagicGap)
        {
            // Gap 맞추러 뒷걸음질
            m_GapMoveState = 2;
            m_Enemy.p_BodyAnimator.SetInteger(Walk, 1);
            m_Enemy.p_LegAnimator.SetInteger(Walk, -1);
            m_Enemy.p_ArmAnimator.SetInteger(Walk, 1);
            m_Enemy.SetRigidByDirection(!m_Enemy.m_IsRightHeaded);
            
            m_Enemy.StartWalkSound(true, 0.5f);
        }
    }

    private IEnumerator CheckArmAnim()
    {
        Animator armAnimator = m_Enemy.p_ArmAnimator;
        BasicWeapon weapon = m_Enemy.p_SingleRifleWeapon;
        armAnimator.SetFloat(AttackSpeed, m_Enemy.p_FireAnimSpeed);
        
        if (m_BulletCount == 2)
        {
            // Max치일 떄
            armAnimator.SetInteger(Attack, 1);
            weapon.Fire();
            while (true)
            {
                yield return null;
                if (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }

            m_BulletCount--;
            m_AtkPhase = 1;
            yield break;
        }
        else
        {
            // 마지막 발
            armAnimator.Play("Attack", -1, 0f);
            weapon.Fire();
            while (true)
            {
                yield return null;
                if (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }
            
            armAnimator.SetInteger(Attack, 2);
            while (true)
            {
                yield return null;
                if (armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    break;
                }
            }
            armAnimator.SetInteger(Attack, 0);
            m_BulletCount--;
            m_AtkPhase = 1;
            yield break;
        }
        
        yield break;
    }
    
    private IEnumerator CheckCognitionAnim()
    {
        Animator FullAnimator = m_Enemy.p_FullAnimator;
        FullAnimator.SetInteger(Cognition, 1);
        while (true)
        {
            yield return null;

            if (FullAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }

        m_Enemy.SetSpriteMode(2);
        m_IsWeaponReady = true;

        yield break;
    }
}

public class SpecialForce_STUN : SpecialForce_FSM
{
    // Member Variables
    private int m_Phase;
    
    private readonly int Stun = Animator.StringToHash("Stun");

    public SpecialForce_STUN(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Phase = 0;
        m_Enemy.p_AlertSystem.CancelGaugeUp();
        m_Enemy.p_AlertSystem.SetStunAlertSpeed(m_Enemy.p_StunAlertSpeed);
        m_Enemy.p_AlertSystem.DoStun(StunFinalStep);
        
        m_Enemy.SetSpriteMode(1);
        m_Enemy.p_FullAnimator.SetInteger(Stun, 1);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        m_Enemy.p_FullAnimator.SetInteger(Stun, 0);
    }

    public override void NextPhase()
    {
       
    }

    private void StunFinalStep()
    {
        if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_AtkDistance)
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
        }
        else
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
    }
}

public class SpecialForce_DEAD : SpecialForce_FSM
{
    // Member Variables
    private float m_Time = 0f;
    private int m_Phase = 0;
    private bool m_DeathSoundPlayed = false;
    
    private Color m_WhiteColor = Color.white;
    
    private float m_FadeValue = 1f;
    private readonly int Fade = Shader.PropertyToID("_Fade");
    
    // Hash Variables
    private readonly int Dead = Animator.StringToHash("Dead");

    
    public SpecialForce_DEAD(SpecialForce _enemy)
    {
        m_Enemy = _enemy;
        InitFSM();
    }
    
    public override void StartState()
    {
        m_Enemy.m_IsDead = true;
        m_Time = 0f;
        
        m_Enemy.StartWalkSound(false);
        
        m_Enemy.SetHotBoxesActive(false);
        
        m_Enemy.p_AlertSystem.gameObject.SetActive(false);
        m_Enemy.SetSpriteMode(1);
        
        m_Enemy.p_FullAnimator.SetInteger(Dead, 1);
    }

    public override void UpdateState()
    {
        /*
        if (!m_DeathSoundPlayed)
        {
            if (m_Enemy.p_FullAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
            {
                m_DeathSoundPlayed = true;
                m_Enemy.m_SoundPlayer.PlayEnemySound(1, 3, m_Enemy.GetBodyCenterPos());
            }
        }
        */
        
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

        if (distance > m_Enemy.p_AtkDistance)
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