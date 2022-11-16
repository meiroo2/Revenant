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
    
    // Constructor
    public Walk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;

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
                break;
            
            case 1:
                if (m_Timer >= m_Enemy.p_Walk_Time)
                    m_Phase = 2;

                if (m_Enemy.GetDistanceBetPlayer() > m_Enemy.p_Walk_MinDistance)
                {
                    m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer());
                }
                break;
            
            case 2:
                m_Phase = -1;

                float distance = m_Enemy.GetDistanceBetPlayer();
                float jumpMax = m_Enemy.p_JumpAtk_Distance_Max;
                float leapMin = m_Enemy.p_LeapAtk_Distance_Min;
                
                m_Enemy.ChangeBossFSM(BossStateName.LEAPATK);
                break;
                
                if (distance <= jumpMax && distance < leapMin)
                {
                    // Jump만 가능
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
                }
                
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.ResetRigid();
    }
}

public class JumpAtk_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_LerpPos = 0f;
    private Vector2 m_StartPoint;
    private Vector2 m_MovePoint;
    private float m_Acceleration = 1f;
    private float m_Timer = 0f;
    
    // Constructor
    public JumpAtk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Phase = 0;
        m_LerpPos = 0f;
        m_Timer = 0f;
        m_Acceleration = 1f;
        
        m_Enemy.m_WeaponMgr.ChangeWeapon(0);

        m_Enemy.p_RigidCol.isTrigger = true;
        m_Enemy.m_EnemyRigid.isKinematic = true;

        m_StartPoint = m_Enemy.transform.position;
        m_MovePoint = m_StartPoint;
        m_MovePoint.y += m_Enemy.p_JumpAtk_Height;
        m_Enemy.ResetMovePoint(m_MovePoint);
    }

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
                m_Phase = -1;
                m_Enemy.ChangeBossFSM(BossStateName.WALK);
                break;
        }
    }
    
    public override void ExitState()
    {
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
        m_Enemy.p_RigidCol.isTrigger = false;
        m_Enemy.m_EnemyRigid.isKinematic = false;
    }
}

public class LeapAtk_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;
    private Transform m_EnemyTransform;
    
    // Constructor
    public LeapAtk_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_EnemyTransform = m_Enemy.transform;
        m_Animator = m_Enemy.m_Animator;
        m_Phase = 0;
        m_Timer = 0f;

        m_Enemy.m_EnemyRigid.isKinematic = true;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_EnemyTransform.position = new Vector2(m_EnemyTransform.position.x,
                    m_EnemyTransform.position.y + 1f);
                
                m_Enemy.p_LeapColMaster.SpawnCols(m_Enemy.m_IsRightHeaded, m_Enemy.m_Player.GetPlayerFootPos());
                m_Phase = 1;
                break;
            
            case 1:
                m_Timer += Time.deltaTime;
                if (m_Timer > 3f)
                    m_Phase = 2;
                break;
            
            case 2:
                Vector2 landPos = m_Enemy.p_LeapColMaster.GetLandingPos(m_Enemy.m_Player.GetPlayerFootPos());
                m_EnemyTransform.position = landPos;

                m_Phase = -1;
                break;
        }
    }

    public override void ExitState()
    {
        m_Enemy.m_EnemyRigid.isKinematic = false;
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
    private Color m_Color;
    
    // Constructor
    public Stealth_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Phase = 0;
        m_Color = Color.white;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Color.a -= Time.deltaTime * m_Enemy.p_Stealth_Speed;
                m_Enemy.m_Renderer.color = m_Color;
                if (m_Color.a <= 0f)
                {
                    m_Color.a = 0f;
                    m_Enemy.m_Renderer.color = m_Color;
                    
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
                m_Phase = -1;
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
                break;
        }
    }

    public override void ExitState()
    {
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
    
    private bool m_DoUpdate = true;
    
    // Constructor
    public Holo_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        m_Enemy.m_ActionOnHit_Holo = null;
        m_Enemy.m_ActionOnHit_Holo = GotoStun;
        m_DoUpdate = true;

        m_IsHoloFake = false;
        m_HoloSpawnCount = 0;
        m_Color = Color.white;
        m_Color.a = 0f;
        m_Timer = 0f;
        
        m_Enemy.SetHotBoxesActive(false);
        m_Phase = m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance);
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
                // 나타나는 구간
                if (m_HoloSpawnCount >= m_Enemy.p_Holo_MaxCount)
                {
                    m_Enemy.SetHotBoxesActive(true);
                    m_IsHoloFake = false;
                    m_Color = Color.white;
                    m_Color.a = 0f;
                    m_Phase = 7;
                    break;
                }

                int randomNum = StaticMethods.GetProbabilityWinning(m_Enemy.p_Holo_FakeChance) ? 0 : 1; 
                switch (randomNum)
                {
                    case 0:
                        // 홀로그램
                        m_HoloSpawnCount++;
                        m_IsHoloFake = true;
                        m_Color = Color.blue;
                        m_Color.a = 0f;
                        m_Phase = 5;
                        break;
                    
                    case 1:
                        // 진짜
                        m_Enemy.SetHotBoxesActive(true);
                        m_IsHoloFake = false;
                        m_Color = Color.white;
                        m_Color.a = 0f;
                        m_Phase = 7;
                        break;
                }
                break;
            
            case 5:
                m_Enemy.p_FSMText.text = "HOLO_FAKE";
                
                // 홀로그램으로 등장
                if (m_Color.a < 1f)
                {
                    m_Color.a += Time.deltaTime * m_Enemy.p_Holo_FadeSpeed;
                    m_Enemy.m_Renderer.color = m_Color;

                    if (m_Color.a >= 1f)
                    {
                        m_Color.a = 1f;
                        m_Enemy.m_Renderer.color = m_Color;
                        m_Timer = 0f;
                    }
                }
                else
                {
                    m_Timer += Time.deltaTime;
                    if (m_Timer > m_Enemy.p_Holo_BeforeDelay)
                    {
                        m_Phase = 6;
                    }
                }
                break;
                
            case 6:
                // 홀로그램 공격
                m_Phase = 9;
                break;
                
            case 7:
                m_Enemy.p_FSMText.text = "HOLO_REAL";
                
                // 진짜 본체 등장
                if (m_Color.a < 1f)
                {
                    m_Color.a += Time.deltaTime * m_Enemy.p_Holo_FadeSpeed;
                    m_Enemy.m_Renderer.color = m_Color;

                    if (m_Color.a >= 1f)
                    {
                        m_Color.a = 1f;
                        m_Enemy.m_Renderer.color = m_Color;
                        m_Timer = 0f;
                    }
                }
                else
                {
                    m_Timer += Time.deltaTime;
                    if (m_Timer > m_Enemy.p_Holo_BeforeDelay)
                    {
                        m_Timer = 0f;
                        m_Phase = 8;
                    }
                }
                break;
            
            case 8:
                // 진짜 본체 공격
                if (m_Timer == 0f)
                {
                    m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                }
                else
                {
                    if (m_Timer > m_Enemy.p_HoloReal_AfterDelay)
                    {
                        m_Phase = -1;
                        m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    }
                }
                m_Timer += Time.deltaTime;
                
                break;

            case 9:
                // 다시 숨기
                m_Color.a -= Time.deltaTime * m_Enemy.p_Holo_FadeSpeed;
                m_Enemy.m_Renderer.color = m_Color;
                
                if (m_Color.a <= 0f)
                {
                    m_Color.a = 0f;
                    m_Phase = m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance);
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
    
    // Constructor
    public Ultimate_BossGang(BossGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    // Functions
    public override void StartState()
    {
        m_Animator = m_Enemy.m_Animator;
        
        m_SkipTimeSliceObjActivate = false;
        m_IsTimeCircleMoveCompleted = false;
        
        m_Phase = 0;
        m_Timer = 0f;
        m_Color = Color.white;
        m_Color.a = 0f;
        m_UltimateCount = 0;
        m_Enemy.SetHotBoxesActive(true);

        m_Enemy.m_IsUltimateBooked = 0;
        
        m_Enemy.transform.position = m_Enemy.p_MapCenterTransform.position;
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Color.a += Time.deltaTime * m_Enemy.p_Ultimate_FadeSpeed;
                m_Enemy.m_Renderer.color = m_Color;
                if (m_Color.a >= 1f)
                {
                    m_Color.a = 1f;
                    m_Enemy.m_Renderer.color = m_Color;
                    m_Phase = 1;
                }
                break;
            
            case 1:
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
                
                m_Phase = 2;
                break;
            
            case 2:
                if (m_SkipTimeSliceObjActivate)
                {
                    m_Phase = 3;
                }
                else if (m_IsTimeCircleMoveCompleted)
                {
                    m_TimeSliceObj.p_CircleCol.gameObject.SetActive(false);
                    m_TimeSliceObj.Stop();
                    m_Timer = 0f;
                    m_Phase = 4;
                }
                break;
            
            case 3:
                // 그냥 사라짐
                m_SkipTimeSliceObjActivate = false;
                m_UltimateCount++;
                
                if (m_UltimateCount < m_Enemy.p_Ultimate_RepeatCount)
                {
                    m_Timer = 0f;
                    m_Color = Color.white;
                    m_Color.a = 0f;
                    m_Phase = 1;
                }
                else
                {
                    m_Phase = -1;
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                }
                break;
            
            case 4:
                // 정상 Activate
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_Enemy.p_Ultimate_DelayTimeAfterSetPos)
                {
                    m_UltimateCount++;
                    m_TimeSliceObj.Activate();
                    m_Enemy.m_ScreenCaptureMgr.Capture(m_TimeSliceObj.transform.position.x,
                        m_TimeSliceObj.transform.position.y,
                        m_Angle);

                    if (m_UltimateCount < m_Enemy.p_Ultimate_RepeatCount)
                    {
                        m_Timer = 0f;
                        m_Color = Color.white;
                        m_Color.a = 0f;
                        m_Phase = 1;
                    }
                    else
                    {
                        m_Phase = -1;
                        m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    }
                }
                break;
        }
    }

    public override void ExitState()
    {
        
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
    private Vector2 m_SpawnPos;
    private float m_TimerForFade = 0f;
    private float m_TimerForCounter = 0f;
    private Color m_Color = Color.white;
    
    
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
        m_TimerForFade = 0f;
        m_TimerForCounter = 0f;
        m_Phase = 0;


        Vector2 playerPos = m_Enemy.m_Player.transform.position;
        playerPos.y -= 0.64f;
        
        switch (m_Enemy.m_Player.GetIsEmptyNearPlayer(m_Enemy.p_Holo_Distance))
        {
            case 0:
                m_SpawnPos = m_Enemy.p_MapCenterTransform.position;
                break;
            
            case 1:
                playerPos.x -= m_Enemy.p_Holo_Distance;
                m_SpawnPos = playerPos;
                break;
            
            case 2:
                playerPos.x += m_Enemy.p_Holo_Distance;
                m_SpawnPos = playerPos;
                break;
            
            case 3:
                int randomNum = UnityEngine.Random.Range(1, 3);
                if (randomNum == 1)
                {
                    playerPos.x -= m_Enemy.p_Holo_Distance;
                    m_SpawnPos = playerPos;
                }
                else
                {
                    playerPos.x += m_Enemy.p_Holo_Distance;
                    m_SpawnPos = playerPos;
                }
                break;
        }
        
        
        m_Enemy.SetHotBoxesActive(false);

        m_Enemy.m_ActionOnHit_Counter = ToAttack;
        m_Enemy.m_WeaponMgr.ChangeWeapon(3);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0: 
                // 최초 좌표 이동
                if (IsCounterTimeEnd())
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    break;
                }
                
                m_Enemy.SetHotBoxesActive(true);
                m_Enemy.transform.position = m_SpawnPos;
                if(!m_Enemy.IsFacePlayer())
                    m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
                m_Phase = 1;
                break;
            
            case 1:
                // 밝아지기 시작
                if (IsCounterTimeEnd())
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    break;
                }

                m_Color.a += Time.deltaTime * m_Enemy.p_Counter_FadeSpeed;
                m_Enemy.m_Renderer.color = m_Color;

                if (m_Color.a >= 1f)
                {
                    m_Color.a = 1f;
                    m_Enemy.m_Renderer.color = m_Color;
                    
                    m_Phase = 2;
                }
                break;
            
            case 2:
                // 대기
                if (IsCounterTimeEnd())
                {
                    m_Enemy.ChangeBossFSM(BossStateName.WALK);
                    break;
                }
                break;
            
            case 3:
                m_Phase = -1;
                m_Color.a = 1f;
                m_Enemy.m_Renderer.color = m_Color;
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                m_Enemy.ChangeBossFSM(BossStateName.WALK);
                break;
        }
        
        
    }

    public override void ExitState()
    {
        m_Enemy.m_ActionOnHit_Counter = null;
        m_Enemy.m_WeaponMgr.ReleaseWeapon();
    }

    private bool IsCounterTimeEnd()
    {
        m_TimerForCounter += Time.deltaTime;
        if (m_TimerForCounter >= m_Enemy.p_Counter_Time)
        {
            return true;
        }

        return false;
    }

    private void ToAttack()
    {
        m_Phase = 3;
    }
}

public class Stun_BossGang : BossGang_FSM
{
    // Member Variables
    private int m_Phase = 0;
    private float m_Timer = 0f;


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
        m_Animator = m_Enemy.m_Animator;
        
        m_Phase = 0;
        m_Color = Color.white;

        m_Enemy.SetHotBoxesActive(false);
    }

    public override void UpdateState()
    {
        switch (m_Phase)
        {
            case 0:
                m_Color.a -= Time.deltaTime * m_Enemy.p_Stealth_Speed;
                m_Enemy.m_Renderer.color = m_Color;
                if (m_Color.a <= 0f)
                {
                    m_Phase = -1;
                    m_Color.a = 0f;
                    m_Enemy.m_Renderer.color = m_Color;
                    m_Enemy.gameObject.SetActive(false);
                }
                break;
        }
    }

    public override void ExitState()
    {

    }
}