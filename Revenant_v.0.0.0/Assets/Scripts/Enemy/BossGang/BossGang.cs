using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;

public enum BossStateName
{
    IDLE,
    WALK,
    LEAPATK,
    JUMPATK,
    STEALTH,
    HOLO,
    ULTIMATE,
    COUNTER,
    STUN,
    DEAD
}

public class BossGang : BasicEnemy, ISpriteMatChange
{
    // Visible Member Variables
    
    // Common
    [BoxGroup("BossGang Values")] public Transform p_MapCenterTransform;
    
    // Walk
    [BoxGroup("BossGang Values")] public float p_Walk_Time = 3f;
    [BoxGroup("BossGang Values")] public float p_Walk_MinDistance = 1f;
    
    // JumpAtk
    [BoxGroup("BossGang Values")] public float p_JumpAtk_Height = 1f;
    [BoxGroup("BossGang Values")] public float p_JumpAtk_DelayTime = 1f;
    [BoxGroup("BossGang Values")] public float p_JumpAtk_Speed = 1f;
    [BoxGroup("BossGang Values")] public float p_JumpAtk_AccelSpeed = 1f;
    
    // Stealth
    [BoxGroup("BossGang Values")] public float p_Stealth_Speed = 1f;

    // Holo
    [BoxGroup("BossGang Values")] public int p_Holo_MaxCount = 4;
    
    // Counter
    [BoxGroup("BossGang Values")] public float p_Counter_Time = 3f;
    
    // Stun
    [BoxGroup("BossGang Values"), Title("Stun"), PropertySpace(SpaceBefore = 1, SpaceAfter = 0)]
    public float p_StunTime = 1f;
    [BoxGroup("BossGang Values")] public float p_StunBackPwr = 1f;

    // Ultimate
    [BoxGroup("BossGang Values"), Title("Ultimate"), PropertySpace(SpaceBefore = 1, SpaceAfter = 0)] 
    public List<int> p_Ultimate_HpBookList = new List<int>();
    [BoxGroup("BossGang Values")] public Vector2 p_Ultimate_AngleLimit;
    [BoxGroup("BossGang Values")] public float p_Ultimate_SetPosTime = 1f;
    [BoxGroup("BossGang Values")] public float p_Ultimate_DelayTimeAfterSetPos = 1f;
    [BoxGroup("BossGang Values")] public int p_Ultimate_RepeatCount = 3;
    [BoxGroup("BossGang Values")] public float p_Ultimate_RemainTime = 5f;
    [BoxGroup("BossGang Values")] public float p_Ultimate_TimeSliceMoveSpeed = 1f;
    [BoxGroup("BossGang Values")] public float p_Ultimate_TImeSliceColorSpeed = 1f;


    public TextMeshProUGUI p_HpText;
    public TextMeshProUGUI p_FSMText;
    [field: SerializeField] public TimeSliceMgr p_TimeSliceMgr { get; private set; }
    [field: SerializeField] private SpriteRenderer p_Renderer;
    [field: SerializeField, Space(10f)] public Enemy_HotBox p_HeadBox;
    [field: SerializeField] public Enemy_HotBox p_BodyBox;
    [field: SerializeField] public BezierMove p_BezierMove;
    [field: SerializeField] public CircleCollider2D p_RigidCol;
    
    
    // Member Variables
    public ScreenCaptureMgr m_ScreenCaptureMgr { get; private set; }
    public SimpleEffectPuller m_SEPuller { get; private set; }
    public WeaponMgr m_WeaponMgr { get; private set; }
    private IHotBox[] m_HotBoxes;
    public Player m_Player { get; private set; }
    [HideInInspector] public Action m_ActionOnHit_Counter = null; 
    [HideInInspector] public Action m_ActionOnHit_Ultimate = null; 

    private BossStateName m_CurBossStateName;
    private Idle_BossGang m_IDLE;
    private Walk_BossGang m_WALK;
    private JumpAtk_BossGang m_JUMP_ATK;
    private LeapAtk_BossGang m_LEAP_ATK;
    private Stealth_BossGang m_STEALTH;
    private Holo_BossGang m_HOLO;
    private Ultimate_BossGang m_ULTIMATE;
    private Counter_BossGang m_COUNTER;
    private Stun_BossGang m_STUN;
    private DEAD_BossGang m_DEAD;

    
    /// <summary>
    /// Ultimate 예약을 위한 int, 예약 시 1, Walk에서 넘어갔을 시 2로 변함.
    /// Stealth에서 2라면 Ultimate 가면 됨
    /// </summary>
    [HideInInspector] public int m_IsUltimateBooked = 0;
    

    /// <summary>
    /// 0 == 에러, 1 == 머리, 2 == 몸통
    /// </summary>
    public int m_DeathReason { get; private set; } = 0;


    // Constructor
    private void Awake()
    {
        InitHuman();
        InitEnemy();

        m_Renderer = p_Renderer;
        m_Animator = GetComponentInChildren<Animator>();
        m_HotBoxes = GetComponentsInChildren<IHotBox>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();

        m_IDLE = new Idle_BossGang(this);
        m_WALK = new Walk_BossGang(this);
        m_JUMP_ATK = new JumpAtk_BossGang(this);
        m_LEAP_ATK = new LeapAtk_BossGang(this);
        m_STEALTH = new Stealth_BossGang(this);
        m_HOLO = new Holo_BossGang(this);
        m_ULTIMATE = new Ultimate_BossGang(this);
        m_COUNTER = new Counter_BossGang(this);
        m_STUN = new Stun_BossGang(this);
        
        m_CurEnemyFSM = m_IDLE;
        m_CurBossStateName = BossStateName.IDLE;
        m_CurEnemyFSM.StartState();

        p_FSMText.text = "IDLE";
        p_HpText.text = p_Hp.ToString();
        
        // ISpriteMatChange
        InitISpriteMatChange();
    }

    private void Start()
    {
        m_OriginPos = transform.position;

        Player tempPlayer = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_Player = tempPlayer;
        m_PlayerTransform = tempPlayer.transform;
        m_PlayerLocationSensor = tempPlayer.m_PlayerLocationSensor;
        m_SEPuller = InstanceMgr.GetInstance().GetComponentInChildren<SimpleEffectPuller>();

        m_ScreenCaptureMgr = InstanceMgr.GetInstance().GetComponentInChildren<ScreenCaptureMgr>();
    }


    // Updates
    private void FixedUpdate()
    {
        if (m_IsRightHeaded)
        {
            p_FSMText.transform.localScale = new Vector3(1f, 1f, 1f);
            p_HpText.transform.localScale = p_FSMText.transform.localScale;
        }
        else
        {
            p_FSMText.transform.localScale = new Vector3(-1f, 1f, 1f);
            p_HpText.transform.localScale = p_FSMText.transform.localScale;
        }
        m_CurEnemyFSM.UpdateState();
    }


    // Functions

    public override float GetDistanceBetPlayer()
    {
        return Vector2.Distance(m_PlayerTransform.position, new Vector2(transform.position.x, transform.position.y + 0.64f));
    }

    public override void SetRigidByDirection(bool _isRight, float _addSpeed = 1f)
    {
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);

            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * ((p_MoveSpeed) * _addSpeed);
        }
    }

    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr) 
            return;


        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(p_HeadBox);
            EditorUtility.SetDirty(p_BodyBox);
        #endif
    }
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue)
    {
        if (p_Hp <= 0)
            return;

        if (m_CurBossStateName != BossStateName.STUN && m_CurBossStateName != BossStateName.ULTIMATE)
        {
            m_CurStunValue += _stunValue;
            if (m_CurStunValue >= p_StunHp)
            {
                m_CurStunValue = 0;
                ChangeBossFSM(BossStateName.STUN);
            }
        }
        
        if (m_CurBossStateName == BossStateName.COUNTER)
        {
            m_ActionOnHit_Counter?.Invoke();
            Debug.Log("공격 반사!!");
        }
        else if (m_CurBossStateName == BossStateName.ULTIMATE)
        {
            m_ActionOnHit_Ultimate?.Invoke();
            Debug.Log("공격 반사!!");
        }
        else
        {
            p_Hp -= _damage;
            
            if (p_Ultimate_HpBookList.Count > 0)
            {
                if (p_Hp <= p_Ultimate_HpBookList[0])
                {
                    m_IsUltimateBooked = 1;
                    p_Ultimate_HpBookList.RemoveAt(0);
                }
            }
        }
        
        p_HpText.text = p_Hp.ToString();
    }

    public void ResetStunValue()
    {
        m_CurStunValue = 0;
    }

    /// <summary>
    /// 특별히 Boss를 위해 제작된 FSM Change 함수입니다.
    /// </summary>
    /// <param name="_stateName"></param>
    public void ChangeBossFSM(BossStateName _stateName)
    {
        Debug.Log(_stateName + "로의 상태 전이");
        m_CurEnemyFSM.ExitState();
        
        switch (_stateName)
        {
            case BossStateName.IDLE:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_IDLE;
                p_FSMText.text = "IDLE";
                break;
            
            case BossStateName.WALK:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_WALK;
                p_FSMText.text = "WALK";
                break;
            
            case BossStateName.JUMPATK:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_JUMP_ATK;
                p_FSMText.text = "JUMPATK";
                break;
            
            case BossStateName.LEAPATK:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_LEAP_ATK;
                p_FSMText.text = "LEAPATK";
                break;
            
            case BossStateName.STEALTH:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_STEALTH;
                p_FSMText.text = "STEALTH";
                break;
            
            case BossStateName.HOLO:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_HOLO;
                p_FSMText.text = "HOLO";
                break;
            
            case BossStateName.ULTIMATE:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_ULTIMATE;
                p_FSMText.text = "ULTIMATE";
                break;
            
            case BossStateName.COUNTER:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_COUNTER;
                p_FSMText.text = "COUNTER";
                break;
            
            case BossStateName.STUN:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_STUN;
                p_FSMText.text = "STUN";
                break;
            
            case BossStateName.DEAD:
                m_CurBossStateName = _stateName;
                m_CurEnemyFSM = m_DEAD;
                p_FSMText.text = "DEAD";
                break;
            
            default:
                Debug.Log("ERR : BossGang에서 의도되지 않은 상태 전이 발생");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }

    public override void SetRigidToPoint(float _addSpeed = 1f)
    {
        m_EnemyRigid.velocity = (m_MovePoint - (Vector2)transform.position).normalized * (p_MoveSpeed * _addSpeed);
    }
    
    
    

    // For MatChanger
    public bool m_IgnoreMatChanger { get; set; } = false;
    public SpriteType m_SpriteType { get; set; } = SpriteType.ENEMY;
    public SpriteMatType m_CurSpriteMatType { get; set; } = SpriteMatType.ORIGIN;
    public Material p_OriginalMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_BnWMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_RedHoloMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_DisappearMat { get; set; }
    private Coroutine m_MatTimeCoroutine = null;
    private readonly int ManualTimer = Shader.PropertyToID("_ManualTimer");
    
    public void ChangeMat(SpriteMatType _matType)
    {
        if (!isActiveAndEnabled)
            return;
        
        if (!ReferenceEquals(m_MatTimeCoroutine, null))
        {
            StopCoroutine(m_MatTimeCoroutine);
            m_MatTimeCoroutine = null;
        }
        
        if (m_IgnoreMatChanger)
            return;

        m_CurSpriteMatType = _matType;
        switch (_matType)
        {
            case SpriteMatType.ORIGIN:
                m_Renderer.material = p_OriginalMat;
                break;
            
            case SpriteMatType.BnW:
                m_Renderer.material = p_BnWMat;
                break;
            
            case SpriteMatType.REDHOLO:
                m_MatTimeCoroutine = StartCoroutine(MatTimeInput());
                m_Renderer.material = p_RedHoloMat;
                break;
            
            case SpriteMatType.DISAPPEAR:
                m_Renderer.material = p_DisappearMat;
                break;
        }
    }

    public void InitISpriteMatChange()
    {
        m_SpriteType = SpriteType.ENEMY;
        m_CurSpriteMatType = SpriteMatType.ORIGIN;
        p_OriginalMat = m_Renderer.material;
        
        if(!p_BnWMat)
            Debug.Log("Info : ISpriteMat BnWMat Null");
        if(!p_RedHoloMat)
            Debug.Log("Info : ISpriteMat RedHoloMat Null");
        if(!p_DisappearMat)
            Debug.Log("Info : ISpriteMat DisappearMat Null");
    }

    private IEnumerator MatTimeInput()
    {
        float timer = 0f;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            m_Renderer.material.SetFloat(ManualTimer, timer);
            yield return null;
        }
    }
}