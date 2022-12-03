using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


public class ShieldGang_FSM : Enemy_FSM
{
    // Member Variables
    protected ShieldGang m_Enemy;
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

public class IDLE_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    
    
    // Constructor
    public IDLE_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.m_Animator;
    }


    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
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

public class FOLLOW_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private float m_DistanceBetPlayer;

    private const float m_MagicGap = 0.1f;
    private static readonly int Walk = Animator.StringToHash("Walk");

    private bool m_IsMoving = false;
    private CoroutineElement m_CoroutineElement;
    
    private float m_WalkSoundDelay = 1f;
    private CoroutineElement m_WalkSoundCoroutineElement = null;
    private MatType m_MatType;


    // Constructor
    public FOLLOW_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
        
        m_EnemyAnimator.SetInteger(Walk, 0);
    }
    
    
    public override void StartState()
    {
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();

        m_IsMoving = false;

        if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
        {
            m_WalkSoundCoroutineElement.StopCoroutine_Element();
            m_WalkSoundCoroutineElement = null;
        }
        
        if (m_Enemy.m_IsShieldBroken)
        {
            m_EnemyAnimator.SetInteger(Walk, 1);
            m_WalkSoundCoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(PlayWalkSound());
        }
    }

    public override void UpdateState()
    {
        if (!m_Enemy.IsFacePlayer())
        {
            m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            return;
        }
        
        m_DistanceBetPlayer = m_Enemy.GetDistanceBetPlayer();

        // 쉴드 깨졌을 떄랑 구분
        if (m_Enemy.m_IsShieldBroken)
        {
            
            if (m_DistanceBetPlayer > m_Enemy.p_AttackDistance)
            {
                m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer(), m_Enemy.p_BrokenSpeedMulti);
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
            }
            
        }
        else
        {
            if (m_DistanceBetPlayer < m_Enemy.p_AttackDistance)
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ATTACK);
                return;
            }

            if (m_IsMoving)
                return;
            
            
            if (m_DistanceBetPlayer > m_Enemy.p_GapDistance + m_MagicGap ||
                m_DistanceBetPlayer < m_Enemy.p_GapDistance - m_MagicGap)
            {
                // 이격거리보다 더 멀 경우 - 전진
                if (m_DistanceBetPlayer > m_Enemy.p_GapDistance)
                {
                    m_CoroutineElement = m_Enemy.m_CoroutineHandler.
                        StartCoroutine_Handler(GapBaseMove(true));

                    if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
                    {
                        m_WalkSoundCoroutineElement.StopCoroutine_Element();
                        m_WalkSoundCoroutineElement = null;
                    }
                    m_WalkSoundCoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(PlayWalkSound());
                    
                    m_IsMoving = true;
                }
                // 이격거리보다 가까이 있음 - 후진
                else
                {
                    if (m_Enemy.p_WallSensor.m_IsTouch)
                        return;
                    
                    m_CoroutineElement = m_Enemy.m_CoroutineHandler.
                        StartCoroutine_Handler(GapBaseMove(false));

                    if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
                    {
                        m_WalkSoundCoroutineElement.StopCoroutine_Element();
                        m_WalkSoundCoroutineElement = null;
                    }
                    m_WalkSoundCoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(PlayWalkSound());
                    
                    m_IsMoving = true;
                }
            }
        }
    }

    private IEnumerator GapBaseMove(bool _toFoward)
    {
        if (_toFoward)  // 전진
        {
            m_EnemyAnimator.SetInteger(Walk, 1);

            while (true)
            {
                m_Enemy.SetRigidByDirection(m_Enemy.GetIsLeftThenPlayer());

                if (m_Enemy.GetDistanceBetPlayer() <= m_Enemy.p_GapDistance)
                {
                    if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
                    {
                        m_WalkSoundCoroutineElement.StopCoroutine_Element();
                        m_WalkSoundCoroutineElement = null;
                    }
                    
                    m_Enemy.ResetRigid();
                    break;
                }
                
                yield return null;
            }
        }
        else
        {               // 후진
            m_EnemyAnimator.SetInteger(Walk, -1);

            while (true)
            {
                // 뒤에 뭐 있어
                if (m_Enemy.p_WallSensor.m_IsTouch)
                {
                    if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
                    {
                        m_WalkSoundCoroutineElement.StopCoroutine_Element();
                        m_WalkSoundCoroutineElement = null;
                    }
                    m_Enemy.ResetRigid();   
                    
                    if (m_Enemy.GetDistanceBetPlayer() >= m_Enemy.p_GapDistance)
                        break;
                    
                    m_EnemyAnimator.SetInteger(Walk, 0);
                    m_IsMoving = false;
                    
                    yield return null;
                    continue;
                }
                
                m_Enemy.SetRigidByDirection(!m_Enemy.GetIsLeftThenPlayer(), m_Enemy.p_BackMoveSpeedMulti);

                if (m_Enemy.GetDistanceBetPlayer() >= m_Enemy.p_GapDistance)
                {
                    if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
                    {
                        m_WalkSoundCoroutineElement.StopCoroutine_Element();
                        m_WalkSoundCoroutineElement = null;
                    }
                    
                    m_Enemy.ResetRigid();
                    break;
                }

                yield return null;
            }
        }

        m_IsMoving = false;
        m_EnemyAnimator.SetInteger(Walk, 0);
        
        m_CoroutineElement.StopCoroutine_Element();
        m_CoroutineElement = null;
        
        yield break;
    }

    public override void ExitState()
    {
        m_EnemyAnimator.SetInteger(Walk, 0);
        m_Enemy.ResetRigid();

        if (!ReferenceEquals(m_CoroutineElement, null))
        {
            m_CoroutineElement.StopCoroutine_Element();
            m_CoroutineElement = null;
        }
        
        if (!ReferenceEquals(m_WalkSoundCoroutineElement, null))
        {
            m_WalkSoundCoroutineElement.StopCoroutine_Element();
            m_WalkSoundCoroutineElement = null;
        }
    }

    private IEnumerator PlayWalkSound()
    {
        SoundPlayer player = GameMgr.GetInstance().p_SoundPlayer;

        while (true)
        {
            yield return new WaitForSeconds(m_WalkSoundDelay);
            
            player.PlayEnemySound(3, 0, m_Enemy.GetBodyCenterPos());
        }
    }
}

public class ROTATION_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private bool m_RotateComplete = false;
    private CoroutineElement m_Element;
    private static readonly int Turn = Animator.StringToHash("Turn");


    // Constructor
    public ROTATION_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    
    public override void StartState()
    {
        m_EnemyAnimator = m_Enemy.m_Animator;
        
        m_Enemy.m_Shield.gameObject.SetActive(false);
        m_RotateComplete = false;

        if (m_Enemy.m_IsShieldBroken)
        {
            m_Element = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(DoRotate());
        }
        else
        {
            m_Element = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(DoShieldRotate());
        }
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        if(!ReferenceEquals(m_Element, null))
            m_Element.StopCoroutine_Element();
        
        m_EnemyAnimator.SetInteger(Turn, 0);
    }

    public override void NextPhase()
    {
        
    }

    private IEnumerator DoRotate()
    {
        m_EnemyAnimator.SetInteger(Turn, 1);
        while (true)
        {
            yield return null;

            if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }
        
        m_EnemyAnimator.SetInteger(Turn, 0);
        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);

        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);

        m_Element.StopCoroutine_Element();
        m_Element = null;
        
        yield break;
    }
    
    private IEnumerator DoShieldRotate()
    {
        // 쉴드 해제
        m_Enemy.m_Shield.gameObject.SetActive(false);
        
        m_EnemyAnimator.SetInteger(Turn, 1);
        while (true)
        {
            yield return null;

            if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
        }
        
        m_EnemyAnimator.SetInteger(Turn, 0);
        m_Enemy.setisRightHeaded(!m_Enemy.m_IsRightHeaded);
        m_Enemy.m_Shield.gameObject.SetActive(true);
        
        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);

        m_Element.StopCoroutine_Element();
        m_Element = null;

        yield break;
    }
}

public class ATTACK_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private CoroutineElement m_CoroutineElement;
    private HitSFXMaker m_HitSFXMaker;

    private Vector2 m_OriginHotBoxLocalPos;
    private bool m_IsAttackEnd = false;
    private static readonly int Attack = Animator.StringToHash("Attack");


    // Constructor
    public ATTACK_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.m_Animator;
        m_HitSFXMaker = m_Enemy.m_HitSFXMaker;
    }

    
    public override void StartState()
    {
        m_OriginHotBoxLocalPos = m_Enemy.p_HotBoxesTransform.localPosition;
        m_IsAttackEnd = false;

        if (m_Enemy.m_IsShieldBroken)
        {
            m_CoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(
                AttackCheck());
        }
        else
        {
            m_CoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(
                ShieldAttackCheck());
        }
    }

    public override void UpdateState()
    {
        if (!m_IsAttackEnd)
            return;

        if (m_Enemy.m_IsShieldBroken)
        {
            // 쉴드 없음
            if ((m_Enemy.m_IsRightHeaded && !m_Enemy.GetIsLeftThenPlayer()) ||
                !m_Enemy.m_IsRightHeaded && m_Enemy.GetIsLeftThenPlayer())
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ROTATION);
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
            }
        }
        else
        {
            // 쉴드 있음
            if ((m_Enemy.m_IsRightHeaded && !m_Enemy.GetIsLeftThenPlayer()) ||
                !m_Enemy.m_IsRightHeaded && m_Enemy.GetIsLeftThenPlayer())
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.ROTATION);
            }
            else
            {
                m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
            }
        }
    }

    public override void ExitState()
    {
        if (!ReferenceEquals(m_CoroutineElement, null))
        {
            m_CoroutineElement.StopCoroutine_Element();
            m_CoroutineElement = null;
        }
    }

    public override void NextPhase()
    {
        
    }

    private IEnumerator AttackCheck()
    {
        AnimatorStateInfo aniInfo;
        Animator animator = m_Enemy.m_Animator;
        
        // 방패 드는 모션
        animator.SetInteger(Attack, 1);
        
        // 애니 종료시점 체크
        while (true)
        {
            yield return null;
            
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }

        // 공격 홀드
        yield return new WaitForSeconds(m_Enemy.p_AtkHoldTime);
        
        // 공격 개시
        bool atkFinished = false;
        animator.SetInteger(Attack, 2);

        // 트랜스폼
        Transform hotboxTransform = m_Enemy.p_HotBoxesTransform;
        float normalTime = 0f;
        Vector2 localPos = new Vector2(m_OriginHotBoxLocalPos.x + 0.29f, m_OriginHotBoxLocalPos.y);
        float deadLine = m_Enemy.p_AtkAniLerpPoint;

        m_Enemy.m_SoundPlayer.PlayEnemySound(3, 1, m_Enemy.GetBodyCenterPos());
        while (true)
        {
            yield return null;

            normalTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            hotboxTransform.localPosition = StaticMethods.GetLerpPosByNormalizedTime(m_OriginHotBoxLocalPos,
                localPos, deadLine, normalTime);
            
            if (!atkFinished && normalTime >= m_Enemy.p_PointAtkTime)
            {
                atkFinished = true;
                // 사운드 재생
                GameMgr.GetInstance().p_SoundPlayer.PlayEnemySound(3, 2,
                    m_Enemy.m_PlayerTransform.position);
                
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                //m_HitSFXMaker.EnableNewObj(2, m_Enemy.m_WeaponMgr.m_CurWeapon.transform.position);
            }
            else if (normalTime >= 1f)
            {
                break;
            }
        }
        
        
        // Idle로 돌림
        animator.SetInteger(Attack, 0);
        m_IsAttackEnd = true;
        
        m_CoroutineElement.StopCoroutine_Element();
        m_CoroutineElement = null;

        yield break;
    }

    private IEnumerator ShieldAttackCheck()
    {
        AnimatorStateInfo aniInfo;
        Animator animator = m_Enemy.m_Animator;

        // 쉴드 제거
        m_Enemy.m_Shield.gameObject.SetActive(false);

        // 방패 드는 모션
        animator.SetInteger(Attack, 1);
        
        // 애니 종료시점 체크
        while (true)
        {
            yield return null;
            
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }

        // 공격 홀드
        yield return new WaitForSeconds(m_Enemy.p_AtkHoldTime);
        
        // 공격 개시
        bool atkFinished = false;
        animator.SetInteger(Attack, 2);

        // 트랜스폼
        Transform hotboxTransform = m_Enemy.p_HotBoxesTransform;
        float normalTime = 0f;
        Vector2 localPos = new Vector2(m_OriginHotBoxLocalPos.x + 0.29f, m_OriginHotBoxLocalPos.y);
        float deadLine = m_Enemy.p_AtkAniLerpPoint;

        // 사운드 재생
        m_Enemy.m_SoundPlayer.PlayEnemySound(3, 1, m_Enemy.GetBodyCenterPos());
        
        while (true)
        {
            yield return null;

            normalTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            hotboxTransform.localPosition = StaticMethods.GetLerpPosByNormalizedTime(m_OriginHotBoxLocalPos,
                localPos, deadLine, normalTime);
            
            if (!atkFinished && normalTime >= m_Enemy.p_PointAtkTime)
            {
                // 사운드 재생
                GameMgr.GetInstance().p_SoundPlayer.PlayEnemySound(3, 2,
                    m_Enemy.m_PlayerTransform.position);
                
                atkFinished = true;
                m_Enemy.m_WeaponMgr.m_CurWeapon.Fire();
                //m_HitSFXMaker.EnableNewObj(2, m_Enemy.m_WeaponMgr.m_CurWeapon.transform.position);
            }
            else if (normalTime >= 1f)
            {
                break;
            }
        }
        
        
        // Idle로 돌림
        animator.SetInteger(Attack, 0);

        // 쉴드 재생성
        m_Enemy.m_Shield.gameObject.SetActive(true);
        m_IsAttackEnd = true;
        
        m_CoroutineElement.StopCoroutine_Element();
        m_CoroutineElement = null;

        yield break;
    }
}

public class CHANGE_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public CHANGE_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }

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

public class DEAD_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private float m_Time = 0f;
    private float m_FadeValue = 1f;
    private Color m_WhiteColor;
    private readonly int Fade = Shader.PropertyToID("_Fade");
    
    private readonly int Dead = Animator.StringToHash("Dead");

    // Constructor
    public DEAD_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_EnemyAnimator = m_Enemy.m_Animator;
        
        m_WhiteColor = Color.white;
        m_FadeValue = 1f;
        m_Time = 0f;
        m_Enemy.m_IsDead = true;
        
        m_Enemy.m_Animator.SetInteger(Dead, 1);
        m_Enemy.m_SoundPlayer.PlayEnemySound(3, 5, m_Enemy.GetBodyCenterPos());
        
        m_Enemy.SendDeathAlarmToSpawner();
        m_Enemy.SetHotBoxesActive(false);
    }

    public override void UpdateState()
    {
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
        m_Enemy.SetHotBoxesActive(true);
    }

    public override void NextPhase()
    {
        
    }
}

public class STUN_ShieldGang : ShieldGang_FSM
{
    // Member Variables

    
    // Constructor
    public STUN_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
        m_EnemyAnimator = m_Enemy.GetComponentInChildren<Animator>();
    }
    
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

public class HIT_ShieldGang : ShieldGang_FSM
{
    // Member Variables
    private CoroutineElement m_CoroutineElement;
    private static readonly int Hit = Animator.StringToHash("Hit");

    // Constructor
    public HIT_ShieldGang(ShieldGang _enemy)
    {
        m_Enemy = _enemy;
    }
    
    public override void StartState()
    {
        m_EnemyAnimator = m_Enemy.m_Animator;

        if (m_Enemy.m_IsShieldBroken)
        {
            
        }
        else
        {
            if (m_Enemy.p_Shield_Hp > 0)
            {
                m_EnemyAnimator.SetInteger(Hit, 1);
                m_CoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(NormalHit());
            }
            else if (m_Enemy.p_Shield_Hp <= 0 && !m_Enemy.m_IsShieldBroken)
            {
                m_Enemy.BreakShield();
                m_Enemy.m_Shield.gameObject.SetActive(false);
                m_EnemyAnimator.SetInteger(Hit, 2);
                m_CoroutineElement = m_Enemy.m_CoroutineHandler.StartCoroutine_Handler(ShieldBreakHit());
            }
        }
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        if(!ReferenceEquals(m_CoroutineElement, null))
            m_CoroutineElement.StopCoroutine_Element();
        
        m_EnemyAnimator.SetInteger(Hit, 0);
    }

    public override void NextPhase()
    {
        
    }

    private IEnumerator NormalHit()
    {
        while (true)
        {
            yield return null;

            if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        
        m_EnemyAnimator.SetInteger(Hit, 0);

        m_CoroutineElement.StopCoroutine_Element();
        m_CoroutineElement = null;
        
        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        
        yield break;
    }

    private IEnumerator ShieldBreakHit()
    {
        while (true)
        {
            yield return null;
            
            if (m_EnemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }
        }
        m_CoroutineElement.StopCoroutine_Element();
        m_CoroutineElement = null;

        m_EnemyAnimator.runtimeAnimatorController = m_Enemy.p_NudeAnimator;
        m_EnemyAnimator.SetFloat(m_Enemy.AtkSpeed, m_Enemy.p_AtkAniSpeedMulti);
        m_Enemy.ChangeEnemyFSM(EnemyStateName.FOLLOW);
        
        yield break;
    }
}