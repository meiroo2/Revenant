using UnityEngine;
using Random = Unity.Mathematics.Random;


public class BossGang_FSM : Enemy_FSM
{
    // Member Variables
    protected BossGang m_Enemy;
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

public class Idle_BossGang : BossGang_FSM
{
    // Member Variables
    private float m_Timer = 0f;
    
    // Constructor
    public Idle_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;

        m_Timer = 0f;
    }

    public override void UpdateState()
    {
        m_Enemy.ChangeBossFSM(BossStateName.WALK);
    }

    public override void ExitState()
    {
       
    }
}

public class Walk_BossGang : BossGang_FSM
{
    // Member Variables
    private float m_Timer = 0f;
    private int m_Phase = 0;
    private readonly int Walk = Animator.StringToHash("Walk");

    // Constructor
    public Walk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Enemy.m_NextFSMForStealth = 0;
        m_Enemy.StartWalkSound(true, 1f);
        
        m_Phase = 0;
        m_Timer = 0f;
    }

    public override void UpdateState()
    {
        m_Timer += Time.deltaTime;
        
        switch (m_Phase)
        {
            case 0:
                if (m_Enemy.m_IsUltimateBooked == 1)
                {
                    m_Phase = -1;
                    m_Enemy.m_IsUltimateBooked = 2;
                    m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                }
                else
                {
                    m_Phase = 1;
                }
                m_Phase = 1;
                break;
            
            case 1:
                //  걷기
                if (m_Timer >= m_Enemy.p_Walk_Time)
                {
                    m_Animator.SetInteger(Walk, 2);
                    m_Phase = 2;
                    break;
                }

                if(!m_Enemy.IsFacePlayer())
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                
                m_Animator.SetInteger(Walk, 1);
                m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer());
                
                if (m_Enemy.GetDistanceBetPlayer() < m_Enemy.p_Walk_MinDistance)
                {
                    m_Enemy.ResetRigid();
                    m_Animator.SetInteger(Walk, 2);
                    m_Phase = 2;
                    break;
                }
                break;
            
            case 2:
                // 멈추기 시
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Animator.SetInteger(Walk, 0);
                    m_Phase = 3;
                    break;
                }
                break;
            
            case 3:
                // 멈추기 완료
                if (m_Timer >= m_Enemy.p_Walk_Time)
                {
                    m_Phase = 4;
                    break;
                }

                if(!m_Enemy.IsFacePlayer())
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                
                if (m_Enemy.GetDistanceBetPlayer() >= m_Enemy.p_Walk_MinDistance)
                {
                    m_Phase = 1;
                    break;
                }
                break;

            case 4:
                m_Phase = -1;

                float distance = m_Enemy.GetDistanceBetPlayer();
                float jumpMax = m_Enemy.p_JumpAtk_Distance_Max;
                float leapMin = m_Enemy.p_LeapAtk_Distance_Min;

                if (distance <= jumpMax && distance < leapMin)
                {
                    // Jump만 가능
                    m_Enemy.m_NextFSMForStealth = 1;
                    m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                    break;
                }
                else if (distance <= jumpMax && distance >= leapMin)
                {
                    // Jump & Leap 가능
                    int randomNum = UnityEngine.Random.Range(0, 2);
                
                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.LEAPATK);
                            break;
                    
                        case 1:
                            m_Enemy.m_NextFSMForStealth = 1;
                            m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                            break;
                    }
                }
                else if (distance > jumpMax && distance >= leapMin)
                {
                    // Leap만 됨
                    int randomNum = UnityEngine.Random.Range(0, 2);
                
                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.LEAPATK);
                            break;

                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                            break;
                    }
                }
                else
                {
                    // 예외상황 (Stealth만 발동)
                    m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                    break;
                }
                
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.StartWalkSound(false);
        
        m_Enemy.ResetRigid();
        m_Animator.SetInteger(Walk, 0);
    }
    
    /*
     public override void UpdateState()
    {
        m_Timer += Time.deltaTime;
        
        switch (m_Phase)
        {
            case 0:
                if (m_Enemy.m_IsUltimateBooked == 1)
                {
                    m_Phase = -1;
                    m_Enemy.m_IsUltimateBooked = 2;
                    m_Enemy.ChangeBossFSM(BossStateName.ULTIMATE);
                }
                else
                {
                    m_Phase = 1;
                }
                m_Phase = 1;
                break;
            
            case 1:
                if (m_Timer >= m_Enemy.p_Walk_Time)
                    m_Phase = 2;

                if(!m_Enemy.IsFacePlayer())
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                
                if (m_Enemy.GetDistanceBetPlayer() > m_Enemy.p_Walk_MinDistance)
                {
                    m_Animator.SetInteger(Walk, 1);
                    m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer());
                }
                else
                {
                    m_Animator.SetInteger(Walk, 0);
                }
                break;
            
            case 2:
                m_Phase = -1;

                float distance = m_Enemy.GetDistanceBetPlayer();
                float jumpMax = m_Enemy.p_JumpAtk_Distance_Max;
                float leapMin = m_Enemy.p_LeapAtk_Distance_Min;
                
                if (distance <= jumpMax && distance < leapMin)
                {
                    // Jump만 가능
                    m_Enemy.m_NextFSMForStealth = 1;
                    
                    int randomNum = UnityEngine.Random.Range(0, 2);
                
                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.JUMPATK);
                            break;
                    
                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                            break;
                    }
                }
                else if (distance <= jumpMax && distance >= leapMin)
                {
                    // Jump & Leap 가능
                    int randomNum = UnityEngine.Random.Range(0, 3);
                
                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.LEAPATK);
                            break;
                    
                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.JUMPATK);
                            break;
                    
                        case 2:
                            m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                            break;
                    }
                }
                else if (distance > jumpMax && distance >= leapMin)
                {
                    // Leap만 됨
                    int randomNum = UnityEngine.Random.Range(0, 2);
                
                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.LEAPATK);
                            break;

                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                            break;
                    }
                }
                else
                {
                    // 예외상황 (Stealth만 발동)
                    m_Enemy.ChangeBossFSM(BossStateName.STEALTH);
                    break;
                }
                
                break;
        }
    }
     */
}

public class JumpAtk_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_LerpPos = 0f;
    private Vector2 m_StartPoint;
    private Vector2 m_MovePoint;
    private float m_Acceleration = 1f;
    private float m_NormalTime = 0f;
    private readonly int Jump = Animator.StringToHash("Jump");
    private float m_Timer = 0f;
    
    // Constructor
    public JumpAtk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
        
        m_Animator = m_Enemy.m_Animator;
        m_Enemy.ResetRigid();
        m_Timer = 0f;
        
        m_Phase = 0;
        m_LerpPos = 0f;
        m_NormalTime = 0f;
        m_Acceleration = 1f;
        
        m_Enemy.m_WeaponMgr.ChangeWeapon(0);

        m_Enemy.p_RigidCol.isTrigger = true;
        m_Enemy.m_EnemyRigid.isKinematic = true;

        m_StartPoint = m_Enemy.transform.position;
        m_MovePoint = m_StartPoint;
        m_MovePoint.y += m_Enemy.p_JumpAtk_Height;

        m_Enemy.transform.position = m_MovePoint;
        
        m_Enemy.SetHotBoxesActive(true);
        m_Animator.SetInteger(Jump, 1);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                // Jump_Appear
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Timer += Time.deltaTime;
                    if (m_Timer >= m_Enemy.p_JumpAtk_DelayTime)
                    {
                        m_Animator.SetInteger(Jump, 2);
                        m_Phase = 1;
                        break;
                    }
                }
                break;
            
            case 1:
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 0.3f)
                {
                    m_Enemy.m_SEPuller.SpawnSimpleEffect(10, new Vector2(m_Enemy.transform.position.x,
                        m_Enemy.transform.position.y + 0.32f));
                    
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 3, m_Enemy.transform.position);
                    
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_Phase = 2;
                }
                break;
            
            case 2:
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_NormalTime = 0f;
                    m_Enemy.transform.position = m_StartPoint;
                    m_Animator.SetInteger(Jump, 3);
                    
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
                    
                    m_Phase = 3;
                }
                break;
            
            case 3:
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    m_Animator.SetInteger(Jump, 0);
                    m_Phase = -1;
                }
                break;
        }
    }
    
    public override void ExitState()
    {
        m_Animator.SetInteger(Jump, 0);
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
        m_Enemy.p_RigidCol.isTrigger = false;
        m_Enemy.m_EnemyRigid.isKinematic = false;
    }
    
    /*
     public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Enemy.p_FSMText.text = "JUMP_ING";
                
                m_Enemy.transform.position = Vector2.Lerp(m_StartPoint, m_MovePoint, m_LerpPos);
                m_LerpPos += Time.deltaTime * (m_Enemy.p_JumpAtk_Speed * m_Acceleration);
                m_Acceleration -= (Time.deltaTime * m_Enemy.p_JumpAtk_AccelSpeed);
                
                if (m_LerpPos >= 1f)
                {
                    m_LerpPos = 1f;
                    m_Enemy.transform.position = m_MovePoint;
                    m_Phase = 1;
                }
                break;
            
            case 1:
                m_Enemy.p_FSMText.text = "JUMP_WAIT";
                
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_JumpAtk_DelayTime)
                {
                    m_Enemy.m_SEPuller.SpawnSimpleEffect(7, m_Enemy.transform.position);
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_LerpPos = 0f;
                    m_Acceleration = 1f;
                    m_Phase = 2;
                    break;
                }
                break;
            
            case 2:
                m_Enemy.p_FSMText.text = "JUMP_LAND";
                
                m_Enemy.transform.position = Vector2.Lerp(m_MovePoint ,m_StartPoint, m_LerpPos);
                m_LerpPos += Time.deltaTime * (m_Enemy.p_JumpAtk_Speed * m_Acceleration);
                m_Acceleration += (Time.deltaTime * m_Enemy.p_JumpAtk_AccelSpeed);
                
                if (m_LerpPos >= 1f)
                {
                    m_LerpPos = 1f;
                    m_Enemy.transform.position = m_StartPoint;
                    m_Phase = 3;
                    break;
                }
                break;
            
            case 3:
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Phase = -1;
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                }
                break;
        }
    }
     */
    
}

public class LeapAtk_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private Transform m_EnemyTransform;
    private Vector2 m_ColSpawnPos;
    private Vector2 m_LandPos;
    private readonly int Leap = Animator.StringToHash("Leap");

    // Constructor
    public LeapAtk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
        
        m_EnemyTransform = m_Enemy.transform;
        m_Animator = m_Enemy.m_Animator;
        m_Phase = 0;
        m_Timer = 0f;

        m_Enemy.SetHotBoxesActive(false);
        m_Animator.SetInteger(Leap, 1);
        m_Enemy.m_EnemyRigid.isKinematic = true;
        
        m_Enemy.m_Renderer.material.SetColor("Color_1256EBB7", m_Enemy.m_LeapColor);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                // Leap_Stealth 뒤로 후퇴 (Leap이 1)
                m_Enemy.ForceSetRigid(!m_Enemy.m_IsRightHeaded);

                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);

                    
                    m_Phase = 1;
                    m_Timer = 0f;

                    m_Animator.SetInteger(Leap, 2);
                    m_Enemy.ResetRigid();
                    
                    // Leap 공중 포지션 결정
                    m_ColSpawnPos = m_EnemyTransform.position;
                    m_ColSpawnPos.x += m_Enemy.m_IsRightHeaded
                        ? m_Enemy.p_LeapAtk_MoveDistance
                        : -m_Enemy.p_LeapAtk_MoveDistance;
                    
                    m_EnemyTransform.position = GetJumpPos();
                    
                    m_Enemy.p_LeapColMaster.SpawnCols(m_Enemy.m_IsRightHeaded, m_ColSpawnPos);
                    
                    m_Enemy.SetHotBoxesActive(true);
                    break;
                }
                break;
                
            case 1:
                // Leap_Appear
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Leap, 3);
                    m_Phase = 2;
                }
                break;
            
            case 2:
                // Leap_Idle
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_LeapAtk_HoverTime)
                {
                    m_Timer = 0f;
                    
                    m_Enemy.m_Renderer.material.SetColor("Color_1256EBB7", m_Enemy.m_AlertColor);
                    m_Phase = 3;
                }
                break;
            
            case 3:
                // Color Change
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_LeapAtk_BeforeAtkDelay)
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Leap, 4);
                    
                    m_LandPos = m_Enemy.p_LeapColMaster.GetLandingPos(m_Enemy.m_Player.GetPlayerFootPos());
                    m_Enemy.p_LeapColMaster.ConvertSelectedCol();
                    
                    m_Phase = 4;

                    if (m_Enemy.p_LeapColMaster.IsSelectedShort())
                        m_Enemy.m_SEPuller.SpawnSimpleEffect(12, m_LandPos, m_Enemy.m_IsRightHeaded);
                    else
                        m_Enemy.m_SEPuller.SpawnSimpleEffect(9, m_LandPos, m_Enemy.m_IsRightHeaded);
                }
                break;
            
            case 4:
                // Leap_Start
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 5, m_Enemy.transform.position);

                    
                    m_Timer = 0f;
                    m_Animator.SetInteger(Leap, 5);
                    
                    m_EnemyTransform.position = m_LandPos;
                    m_Enemy.p_LeapColMaster.DoAttack();
                    
                    m_Phase = 5;
                }
                break;
            
            case 5:
                // Leap_End
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Leap, 6);
                    m_Phase = 6;
                }
                break;
            
            case 6:
                // PocketToIdle
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Leap, 0);
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_Renderer.material.SetColor("Color_1256EBB7", m_Enemy.m_BasicColor);
        m_Enemy.p_LeapColMaster.ReleaseAll();
        m_Enemy.SetHotBoxesActive(true);
        m_Enemy.m_EnemyRigid.isKinematic = false;
    }

    private Vector2 GetJumpPos()
    {
        Vector2 Pos = m_EnemyTransform.position;
        Pos.y += m_Enemy.p_LeapAtk_Height;
        return Pos;
    }
 
}
/*
 public class LeapAtk_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private float m_BeizerDistance = 0f;
    
    // Constructor
    public LeapAtk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Phase = 0;
        m_Timer = 0f;

        m_BeizerDistance = Vector2.Distance(m_Enemy.transform.position,
            m_Enemy.m_Player.m_PlayerFootMgr.GetFootRayHit().point);
        
        m_Enemy.m_WeaponMgr.ChangeWeapon(1);
        
        if(!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        
        m_Enemy.p_RigidCol.isTrigger = true;
        m_Enemy.p_BezierMove.SetAction(PhaseToOne);
        m_Enemy.p_BezierMove.MoveToPoint();
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 1:
                m_Phase = 2;
                m_Enemy.p_RigidCol.isTrigger = false;
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();

                Vector2 SlipPwr = Vector2.right * (m_Enemy.p_LeapAtk_SlipPwr *
                                                   (m_BeizerDistance * m_Enemy.p_LeapAtk_AdaptiveSlipValue));
                if (!m_Enemy.m_IsRightHeaded)
                    SlipPwr *= -1f;
                
                m_Enemy.m_EnemyRigid.AddForce(SlipPwr, ForceMode2D.Impulse);
                break;
            
            case 2:
                m_Phase = 3;
                break;
            
            case 3:
                m_Timer += Time.deltaTime;
                if (m_Timer >= 1f)
                    m_Phase = 4;
                break;
            
            case 4:
                m_Phase = -1;
                m_Enemy.ChangeBossFSM(BossStateName.WALK);
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.p_RigidCol.isTrigger = false;
        m_Enemy.p_BezierMove.ResetAction();
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
        m_Enemy.ResetRigid();
    }

    private void PhaseToOne()
    {
        m_Phase = 1;
    }
}
 */


public class Stealth_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_NormalTime = 0f;
    private readonly int Stealth = Animator.StringToHash("Stealth");
    private readonly int StealthSpeed = Animator.StringToHash("StealthSpeed");

    // Constructor
    public Stealth_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
        
        m_Animator = m_Enemy.m_Animator;
        // Speed 설정 및 애니메이션 행동 개시
        m_Animator.SetFloat(StealthSpeed, m_Enemy.p_Stealth_Speed);
        m_Animator.SetInteger(Stealth, 1);
        
        m_Phase = 0;
        m_NormalTime = 0f;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                // Stealth Anim Wait
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                
                if (m_NormalTime >= 1f)
                {
                    m_Enemy.SetHotBoxesActive(false);
                    
                    m_Phase = 1;
                    break;
                }
                break;
            
            case 1:
                if (m_Enemy.m_IsUltimateBooked == 2)
                {
                    m_Phase = -1;
                    m_Enemy.ChangeBossFSM(BossStateName.ULTIMATE);
                }
                else
                {
                    m_Phase = 2;
                }
                break;
            
            case 2:
                if (m_Enemy.m_NextFSMForStealth == 1)
                {
                    // Jump 포함
                    m_Enemy.m_NextFSMForStealth = 0;
                    int randomNum = UnityEngine.Random.Range(0, 3);

                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.HOLO);
                            break;
                    
                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.COUNTER);
                            break;
                        
                        case 2:
                            m_Enemy.ChangeBossFSM(BossStateName.JUMPATK);
                            break;
                    }
                }
                else
                {
                    int randomNum = UnityEngine.Random.Range(0, 2);

                    switch (randomNum)
                    {
                        case 0:
                            m_Enemy.ChangeBossFSM(BossStateName.HOLO);
                            break;
                    
                        case 1:
                            m_Enemy.ChangeBossFSM(BossStateName.COUNTER);
                            break;
                    }
                }
                
                
                m_Phase = -1;
                break;
        }
    }

    public override void ExitState()
    {
        m_Animator.SetInteger(Stealth, 0);
        m_Enemy.SetHotBoxesActive(false);
    }

    private void PhaseToOne()
    {
        m_Phase = 1;
    }
}

public class Holo_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private Vector2 m_PlayerPos;
    private float m_Timer = 0f;
    private Color m_Color = Color.white;

    private bool m_IsHoloFake = false;
    private int m_HoloSpawnCount = 0;
    
    private float m_HoloRealHotBoxTimer = 0f;
    private bool m_HoloRealHotBoxActive = false;
    
    private bool m_DoUpdate = true;
    private readonly int Holo = Animator.StringToHash("Holo");
    private readonly int HoloAppearSpeed = Animator.StringToHash("HoloAppearSpeed");


    // Constructor
    public Holo_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_HoloRealHotBoxTimer = 0f;
        m_HoloRealHotBoxActive = false;
        
        m_Animator = m_Enemy.m_Animator;
        m_Animator.SetFloat(HoloAppearSpeed, m_Enemy.p_Holo_AppearSpeed);
        m_Enemy.m_ActionOnHit_Holo = null;
        m_Enemy.m_ActionOnHit_Holo = GotoStun;
        m_DoUpdate = true;

        m_IsHoloFake = false;
        m_HoloSpawnCount = 0;
        m_Color = Color.white;
        m_Color.a = 0f;
        m_Timer = 0f;
        m_Phase = m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance);
        
        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.m_WeaponMgr.ChangeWeapon(2);
    }

    public override void UpdateState()
    {
        if (!m_DoUpdate)
            return;
        
        switch (m_Phase)
        {
            case 0:
                m_Enemy.p_FSMText.text = "HOLO_NoSPACE";
                
                // 빈 공간 없음
                Debug.Log("망함 ㅅㄱ");
                m_Phase = 4;
                break;
            
            case 1:
                m_Enemy.p_FSMText.text = "HOLO_LEFT";
                
                // 왼쪽만 빔
                Debug.Log("왼쪽 빔 확인");
                m_PlayerPos = m_Enemy.m_Player.transform.position;
                m_PlayerPos.x -= m_Enemy.p_Holo_Distance;
                m_PlayerPos.y -= 0.64f;
                m_Enemy.transform.position = m_PlayerPos;
                m_Enemy.setisRightHeaded(true);
                m_Phase = 4;
                break;
            
            case 2:
                m_Enemy.p_FSMText.text = "HOLO_RIGHT";
                
                // 오른쪽만 빔
                Debug.Log("오른쪽 빔 확인");
                m_PlayerPos = m_Enemy.m_Player.transform.position;
                m_PlayerPos.x += m_Enemy.p_Holo_Distance;
                m_PlayerPos.y -= 0.64f;
                m_Enemy.transform.position = m_PlayerPos;
                m_Enemy.setisRightHeaded(false);
                m_Phase = 4;
                break;
            
            case 3:
                // 양쪽 다 빔
                m_Phase = UnityEngine.Random.Range(1, 3);
                break;
            
            case 4:
                // 좌표 이동만 완료함, 현재 안보임 상태
                // 기본은 Holo_Fake
                
                // 이미 Fake 충분히 해서 무조건 Holo_Real
                if (m_HoloSpawnCount >= m_Enemy.p_Holo_MaxCount)
                {
                    m_IsHoloFake = false;
                    m_Animator.SetInteger(Holo, 4);
                    m_Phase = 8;
                    break;
                }

                // 랜덤 확률 탐색
                if (StaticMethods.GetProbabilityWinning(m_Enemy.p_Holo_FakeChance))
                {
                    m_HoloSpawnCount++;
                    m_IsHoloFake = true;
                    m_Timer = 0f;
                    m_Enemy.p_FSMText.text = "HOLO_FAKE";
                    
                    m_Animator.SetInteger(Holo, 1);
                    
                    m_Phase = 5;
                }
                else
                {
                    m_IsHoloFake = false;
                    
                    m_Animator.SetInteger(Holo, 4);
                    
                    m_Phase = 8;
                }
                break;
            
            case 5:
                // HoloFake_Appear
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer += Time.deltaTime;
                    if (m_Timer >= m_Enemy.p_Holo_BeforeAtkDelay)
                    {
                        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 2, m_Enemy.transform.position);
                        
                        m_Timer = 0f;
                        m_Animator.SetInteger(Holo, 2);
                        m_Phase = 6;
                    }
                }
                break;
                
            case 6:
                // HoloFake_Atk
                m_Timer += Time.deltaTime;
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Holo, 3);
                    m_Phase = 7;
                }
                break;
                
            case 7:
                // HoloFake_Disappear
                m_Timer += Time.deltaTime;
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Timer = 0f;
                    m_Phase = m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance);
                }
                break;
            
            
            case 8:
                // HoloReal_Appear
                m_HoloRealHotBoxTimer = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                switch (m_HoloRealHotBoxTimer)
                {
                    case >= 1f:
                        m_Timer += Time.deltaTime;
                        if (m_Timer >= m_Enemy.p_Holo_BeforeAtkDelay)
                        {
                            m_Enemy.m_SoundPlayer.PlayEnemySound(5, 2, m_Enemy.transform.position);

                            
                            m_Timer = 0f;
                            m_Animator.SetInteger(Holo, 5);
                            m_Phase = 9;
                        }
                        break;
                    
                    case > 0.3f:
                        if (m_HoloRealHotBoxActive)
                            break;
                        
                        m_HoloRealHotBoxActive = true;
                        m_Enemy.SetHotBoxesActive(true);
                        break;
                }
                break;
            
            
            case 9:
                // HoloReal_Atk
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
                {
                    m_Timer = 0f;
                    
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 2, m_Enemy.transform.position);
                    
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    
                    m_Phase = 10;
                }
                break;
            
            case 10:
                // HoloReal_End
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Animator.SetInteger(Holo, -1);
                    m_Phase = 11;
                }
                break;
            
            case 11:
                // HoloReal_SwordToPocket
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Animator.SetInteger(Holo, -2);
                    m_Phase = 12;
                }
                break;
            
            case 12:
                // PocketToIdle
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Animator.SetInteger(Holo, 0);
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_ActionOnHit_Holo = null;
        m_Enemy.SetHotBoxesActive(true);
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
    }

    private void GotoStun()
    {
        m_Animator.SetInteger(Holo, 6);
        m_Animator.Play("Stun", -1, 0f);
        m_DoUpdate = false;
        m_Enemy.ChangeBossFSM(BossStateName.STUN);
    }
}

public class Ultimate_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private Color m_Color = Color.white;
    
    private float m_Angle = 0f;
    private TimeSliceObj m_TimeSliceObj;
    private int m_UltimateCount = 0;

    private bool m_SkipTimeSliceObjActivate = false;
    private bool m_IsTimeCircleMoveCompleted = false;

    private float m_NormalTime = 0f;

    private bool m_CurScreenActive = false;
    
    private readonly int Ult = Animator.StringToHash("Ult");

    // Constructor
    public Ultimate_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;

        m_Enemy.m_ScreenCaptureMgr.m_MoveImgEndAction = null;
        m_SkipTimeSliceObjActivate = false;
        m_IsTimeCircleMoveCompleted = false;
        m_CurScreenActive = false;
        
        m_Phase = 0;
        m_Timer = 0f;
        m_NormalTime = 0f;
        m_Color = Color.white;
        m_Color.a = 0f;
        m_UltimateCount = 0;
        m_Enemy.SetHotBoxesActive(false);

        m_Enemy.m_IsUltimateBooked = 0;
        
        m_Animator.SetInteger(Ult, 1);
        m_Enemy.transform.position = m_Enemy.p_MapCenterTransform.position;
        
        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                // Ult Appear
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_NormalTime = 0f;
                    m_Enemy.SetHotBoxesActive(true);
                    m_Animator.SetInteger(Ult, 2);
                    m_Phase = 1;
                }
                break;
            
            case 1:
                // Ult Start
                if (!m_CurScreenActive)
                {
                    m_Enemy.ActivateScreenHexa(true);
                    m_CurScreenActive = true;
                }
                
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_NormalTime = 0f;
                    m_Enemy.SetHotBoxesActive(true);
                    m_Animator.SetInteger(Ult, 3);
                    m_Phase = 2;
                }
                break;
            
            case 2:
                // Ult Idle
                m_Angle = UnityEngine.Random.Range(m_Enemy.p_Ultimate_AngleLimit.x, m_Enemy.p_Ultimate_AngleLimit.y);
                m_TimeSliceObj =
                    m_Enemy.p_TimeSliceMgr.SpawnTimeSlice(m_Enemy.p_Ultimate_TimeSliceMoveSpeed,
                        m_Enemy.p_Ultimate_TImeSliceColorSpeed, m_Angle, m_Enemy.p_Ultimate_RemainTime);
                
                // TimeSlice Circle Setting
                m_SkipTimeSliceObjActivate = false;
                m_IsTimeCircleMoveCompleted = false;
                m_TimeSliceObj.p_CircleCol.gameObject.SetActive(true);
                m_TimeSliceObj.p_CircleCol.InitTimeSliceCircleCol(m_Enemy.p_Ultimate_CircleStartPos,
                    m_Enemy.p_Ultimate_CircleEndPos, m_Enemy.p_Ultimate_CircleSpeed, SuccessTimeCircleMoveCompleted);
                
                
                m_TimeSliceObj.StartFollow();
                m_TimeSliceObj.m_OnHitAction = SkipCurTimeSliceObjActivate;

                m_Timer = 0f;
                m_Phase = 3;
                break;
            
            case 3:
                if (m_SkipTimeSliceObjActivate)
                {
                    // 부서짐;;
                    m_Phase = 4;
                }
                else if (m_IsTimeCircleMoveCompleted)
                {
                    // TimeCircle이 도착 좌표에 이미 도달함
                    if (m_Timer == 0f)
                    {
                        // 우선 Stop
                        if (m_CurScreenActive)
                        {
                            m_Enemy.ActivateScreenHexa(false);
                            m_CurScreenActive = false;
                        }
                    
                        m_TimeSliceObj.p_CircleCol.gameObject.SetActive(false);
                        m_TimeSliceObj.Stop();
                    }
                    else if(m_Timer >= m_Enemy.p_Ultimate_DelayTimeAfterSetPos)
                    {
                        // Delay타임 이후 발동
                        m_Timer = 0f;

                        m_Enemy.m_Renderer.sortingLayerName = "UI";
                        m_Enemy.m_Renderer.sortingOrder = 20;
                        m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                    
                        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
                    
                        m_Animator.SetInteger(Ult, 4);
                    
                        m_Phase = 5;
                        break;
                    }
                    m_Timer += Time.deltaTime;
                }
                break;
            
            case 4:
                // 그냥 사라짐
                m_SkipTimeSliceObjActivate = false;
                m_UltimateCount++;
                
                if (m_UltimateCount < m_Enemy.p_Ultimate_RepeatCount)
                {
                    m_Animator.SetInteger(Ult, 3);
                    m_Timer = 0f;
                    m_Color = Color.white;
                    m_Color.a = 0f;
                    m_Phase = 1;
                }
                else
                {
                    m_Timer = 0f;
                    m_Animator.SetInteger(Ult, 7);
                    m_Phase = 8;
                }
                break;
            
            case 5:
                // Ulti_Start 대기
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_NormalTime = 0f;

                    m_Enemy.m_ScreenCaptureMgr.m_MoveImgEndAction = (() => m_Phase = 6);
                    
                    m_UltimateCount++;
                    m_TimeSliceObj.Activate();
                    m_Enemy.m_ScreenCaptureMgr.Capture(m_TimeSliceObj.transform.position.x,
                        m_TimeSliceObj.transform.position.y,
                        m_Angle);
                    
                    m_Phase = -1;
                }
                break;
            
            case 6:
                // 마지막인지 한번 더 해야 하는지 결정(Ult_Start 막프레임 상태)
                m_Enemy.m_ScreenCaptureMgr.m_MoveImgEndAction = null;
                m_Animator.updateMode = AnimatorUpdateMode.Normal;
                m_Timer = 0f;
                m_Color = Color.white;
                m_Color.a = 0f;
                
                if (m_UltimateCount < m_Enemy.p_Ultimate_RepeatCount)
                {
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 7, m_Enemy.transform.position);
                    
                    m_Animator.SetInteger(Ult, 5);
                }
                else
                {
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 7, m_Enemy.transform.position);
                    
                    m_Animator.SetInteger(Ult, 6);
                }
                
                m_Phase = 7;
                break;
            
            case 7:
                // Ult_End
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Enemy.m_Renderer.sortingOrder = -1;
                    
                    m_NormalTime = 0f;
                    if (m_UltimateCount < m_Enemy.p_Ultimate_RepeatCount)
                    {
                        m_Animator.SetInteger(Ult, 3);
                        m_Phase = 1;
                    }
                    else
                    {
                        m_Timer = 0f;
                        m_Animator.SetInteger(Ult, 7);
                        m_Phase = 8;
                    }
                }
                break;
            
            case 8:
                // PocketToIdle
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    m_Phase = -1;
                }
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.ActivateScreenHexa(false, false);
        m_Animator.SetInteger(Ult, 0);
    }

    private void SuccessTimeCircleMoveCompleted()
    {
        m_IsTimeCircleMoveCompleted = true;
    }
    private void SkipCurTimeSliceObjActivate()
    {
        m_SkipTimeSliceObjActivate = true;
    }
}

public class Counter_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private Vector2 m_CounterAppearPos;
    private float m_Timer = 0f;
    private Color m_Color = Color.white;

    private float m_NormalTime = 0f;
    
    private readonly int Counter = Animator.StringToHash("Counter");


    // Constructor
    public Counter_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Enemy.m_ActionOnHit_Counter = null;
        
        m_Color.a = 0f;
        m_Timer = 0f;
        m_Phase = 0;
        m_NormalTime = 0f;
        // 히트박스 콜백
        m_Enemy.m_ActionOnHit_Counter = (() => m_Phase = 2);

        Vector2 playerPos = m_Enemy.m_Player.transform.position;
        playerPos.y -= 0.64f;
        
        switch (m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance))
        {
            case 0:
                m_CounterAppearPos = m_Enemy.p_MapCenterTransform.position;
                break;
            
            case 1:
                playerPos.x -= m_Enemy.p_Holo_Distance;
                m_CounterAppearPos = playerPos;
                break;
            
            case 2:
                playerPos.x += m_Enemy.p_Holo_Distance;
                m_CounterAppearPos = playerPos;
                break;
            
            case 3:
                int randomNum = UnityEngine.Random.Range(1, 3);
                if (randomNum == 1)
                {
                    playerPos.x -= m_Enemy.p_Holo_Distance;
                    m_CounterAppearPos = playerPos;
                }
                else
                {
                    playerPos.x += m_Enemy.p_Holo_Distance;
                    m_CounterAppearPos = playerPos;
                }
                break;
        }

        m_Enemy.SetHotBoxesActive(false);
        m_Enemy.transform.position = m_CounterAppearPos;
        if(!m_Enemy.IsFacePlayer())
            m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        m_Animator.SetInteger(Counter, 1);
       
        m_Enemy.m_WeaponMgr.ChangeWeapon(3);
        
        
        m_Enemy.m_SoundPlayer.PlayEnemySound(5, 1, m_Enemy.transform.position);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                // Appear Anim Check
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_NormalTime = 0f;
                    m_Enemy.SetHotBoxesActive(true);
                    m_Animator.SetInteger(Counter, 2);
                    m_Phase = 1;
                }
                break;
            
            case 1: 
                // Counter Idle
                if(!m_Enemy.IsFacePlayer())
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);

                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_Counter_Time)
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    m_Phase = -1;
                    break;
                }
                break;
            
            case 2:
                // Counter
                m_Animator.SetInteger(Counter, 3);
                m_Enemy.m_ActionOnHit_Counter = null;
                m_Phase = 3;
                break;
            
            case 3:
                // Counter Anim Wait
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Enemy.m_SoundPlayer.PlayEnemySound(5, 2, m_Enemy.transform.position);
                    
                    m_NormalTime = 0f;
                    m_Animator.SetInteger(Counter, 4);
                    m_Phase = 4;
                }
                break;
            
            case 4:
                // Counter PointAttack 
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= m_Enemy.p_Counter_PointAtkTime)
                {
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                    m_Phase = 5;
                }
                break;
            
            case 5:
                // Counter Attack End Wait
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Animator.SetInteger(Counter, 5);
                    m_Phase = 6;
                }
                break;
            
            case 6:
                // To Pocket
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Animator.SetInteger(Counter, 6);
                    m_Phase = 7;
                }
                break;
            
            case 7:
                // To Idle
                m_NormalTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (m_NormalTime >= 1f)
                {
                    m_Animator.SetInteger(Counter, 0);
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    m_Phase = -1;
                }
                break;
        }
        
        
    }

    public override void ExitState()
    {
        m_Animator.SetInteger(Counter, 0);
        m_Enemy.m_ActionOnHit_Counter = null;
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
    }
}

public class Stun_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private readonly int Holo = Animator.StringToHash("Holo");


    // Constructor
    public Stun_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Timer = 0f;

        m_Enemy.m_Renderer.color = Color.white;
        m_Enemy.SetHotBoxesActive(true);
        
        m_Enemy.m_EnemyRigid.AddForce(m_Enemy.m_IsRightHeaded ? 
                Vector2.left * m_Enemy.p_StunBackPwr : Vector2.right * m_Enemy.p_StunBackPwr,
            ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        m_Timer += Time.deltaTime;

        if (m_Timer >= m_Enemy.p_StunTime)
        {
            m_Enemy.ChangeBossFSM(BossStateName.WALK);
        }
    }

    public override void ExitState()
    {
        m_Animator.SetInteger(Holo, 0);
    }
}

public class DEAD_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private Color m_Color;


    // Constructor
    public DEAD_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }


    // Functions
    public override void StartState()
    {
        m_Enemy.SendDeathAlarmToSpawner();
        
        m_Animator = m_Enemy.m_Animator;
        
        m_Phase = 0;
        m_Color = Color.white;

        m_Enemy.m_Animator.SetInteger("Dead", 1);
        m_Enemy.SetHotBoxesActive(false);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {

    }
}