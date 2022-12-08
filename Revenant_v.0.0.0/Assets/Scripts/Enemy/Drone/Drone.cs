using System;
using System.Collections;
using FMOD;
using FMOD.Studio;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using FixedUpdate = Unity.VisualScripting.FixedUpdate;


public class Drone : BasicEnemy, ISpriteMatChange
{
    // Visible Member Variables
    //[field: SerializeField, BoxGroup("DroneGang Values")] public float p_AlertSpeedRatio = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_RushSpeedRatio { get; protected set; } = 2f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_WiggleSpeed { get; protected set; } = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_WigglePower { get; protected set; } = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public int p_BombHp { get; protected set; } = 20;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_BombRange { get; protected set; } = 2f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_BreakPower = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_DetectSpeed = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values"), Range(0f, 1f)]
    public float p_DecidePositionPointTime = 0f;
    
    [field: SerializeField, BoxGroup("DroneGang Values")] public bool p_LookAround = false;
    [field: SerializeField, BoxGroup("DroneGang Values")] public float p_LookAroundDelay = 1f;
    [field: SerializeField, BoxGroup("DroneGang Values")] public Transform[] p_PatrolPoses;

    
    [Space(20f)] [Header("확인용 변수 모음")] 
    public int m_DeadReason = -1;
    public BombWeapon_Enemy m_BombWeapon;
    public Enemy_HotBox p_HeadHotBox;
    public Enemy_HotBox p_BodyHotBox;
    public SpriteRenderer p_Renderer;
    
    // FSMs
    private IDLE_Drone m_IDLE;
    private PATROL_Drone m_PATROL;
    private FOLLOW_Drone m_FOLLOW;
    private RUSH_Drone m_RUSH;
    private DEAD_Drone m_DEAD;


    // Member Variables
    public delegate void CoroutineDelegate();
    private CoroutineDelegate m_CoroutineCallback = null;
    
    public Player m_Player { get; private set; }
    private Enemy_HotBox[] m_HotBoxes;
    public Enemy_WeaponMgr m_WeponMgr;
    public Drone_CrashCol m_CrashCol { get; private set; }

    public SimpleEffectPuller m_SimpleEffectPuller { get; private set; }

    private Coroutine m_Coroutine;

    private Coroutine m_VelocityBreakCoroutine;
    
    private Vector2 m_RushMoveVec;
    
    private float m_SinYValue = 0;
    private float m_Timer = 0;
    
    public delegate void UpdateDelegate();
    private UpdateDelegate m_Callback = null;

    private float m_VisionAngle = 0f;
    private static readonly int DetectSpeed = Animator.StringToHash("DetectSpeed");
    

    // Constructors
    public void Awake()
    {
        InitHuman();
        InitEnemy();
        
        //m_Animator.SetFloat("DetectSpeed", p_DetectSpeed);

        
        m_Renderer = p_Renderer;
        
        m_DeadReason = -1;
        
        m_HotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_CrashCol = GetComponentInChildren<Drone_CrashCol>();
        

        m_RushMoveVec = Vector2.zero;
        m_EnemyHotBoxes = GetComponentsInChildren<Enemy_HotBox>();
        m_EnemyRigid = GetComponent<Rigidbody2D>();

        m_IDLE = new IDLE_Drone(this);
        m_PATROL = new PATROL_Drone(this);
        m_FOLLOW = new FOLLOW_Drone(this);
        m_RUSH = new RUSH_Drone(this);
        m_DEAD = new DEAD_Drone(this);

        if (p_LookAround && p_PatrolPoses.Length > 0)
        {
            p_LookAround = false;
        }

        if (p_PatrolPoses.Length <= 0)
        {
            m_CurEnemyStateName = EnemyStateName.IDLE;
            m_CurEnemyFSM = m_IDLE;
        }
        else if(p_PatrolPoses.Length > 1 && !p_LookAround)
        {
            m_CurEnemyStateName = EnemyStateName.PATROL;
            m_CurEnemyFSM = m_PATROL;
        }
        
        InitISpriteMatChange();
    }

    public void Start()
    {
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        
        m_Animator.SetFloat(DetectSpeed, p_DetectSpeed);
        var instance = InstanceMgr.GetInstance();
        m_SimpleEffectPuller = InstanceMgr.GetInstance().GetComponentInChildren<SimpleEffectPuller>();
        m_Player = GameMgr.GetInstance().p_PlayerMgr.GetPlayer();
        m_PlayerTransform = m_Player.transform;
        
        m_CurEnemyFSM.StartState();
        
        m_WeponMgr.ChangeWeapon(0);
    }

    private void OnEnable()
    {
        m_Animator.SetFloat(DetectSpeed, p_DetectSpeed);
    }

    // Updates
    public void Update()
    {
        if (m_ObjectState == ObjectState.Pause)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public override void RaycastVisionCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = StaticMethods.GetRotatedVec(Vector2.right, m_IsRightHeaded ? m_VisionAngle : -m_VisionAngle);

        m_VisionAngle -= 1f;
        if (m_VisionAngle <= -60f)
            m_VisionAngle = 0f;
        
        if (m_IsRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(position, direction, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, direction * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast( position, -direction, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, (-direction * p_VisionDistance), Color.red);
        }
    }
    
    public override void StartPlayerCognition(bool _instant = false)
    {
        if (m_PlayerCognition)
            return;
        
        if (_instant)
        {
            m_PlayerCognition = true;
            ChangeEnemyFSM(EnemyStateName.FOLLOW);
        }
        else
        {
            m_PlayerCognition = true;
        }
    }
    
    public void SetCoroutineCallback(CoroutineDelegate _input)
    {
        m_CoroutineCallback = _input;
    }
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        var bombWeapon = GetComponentInChildren<BombWeapon_Enemy>();
        
        p_Hp = _mgr.D_HP;
        bombWeapon.p_BulletDamage = _mgr.D_BombDamage;
        bombWeapon.p_BombRadius = _mgr.D_BombRadius;
        p_BreakPower = _mgr.D_BreakPower;
        p_MoveSpeed = _mgr.D_Speed;
        p_RushSpeedRatio = _mgr.D_RushSpeedMulti;
        p_AtkDistance = _mgr.D_RushTriggerDistance;
        p_HeadHotBox.p_DamageMulti = _mgr.D_DroneDmgMulti;
        p_BodyHotBox.p_DamageMulti = _mgr.D_BombDmgMulti;
        p_DetectSpeed = _mgr.D_DetectSpeed;
        p_VisionDistance = _mgr.D_VisionDistance;
        p_DecidePositionPointTime = _mgr.D_DecidePositionPointTime;

        #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(p_HeadHotBox);
                EditorUtility.SetDirty(p_BodyHotBox);
                EditorUtility.SetDirty(bombWeapon);
        #endif
    }

    public void SetUpdateCallback(UpdateDelegate _input)
    {
        m_Callback = _input;
    }

    public void ResetCallback()
    {
        m_Callback = null;
    }
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;
        
        p_Hp -= _damage * (_point == HitBoxPoint.HEAD ? 2 : 1);
        m_CurStunValue += _stunValue;

        if (p_Hp <= 0)
        {
            if (_point == HitBoxPoint.HEAD)
            {
                m_DeadReason = 0;
            }
            else if (_point == HitBoxPoint.BODY)
            {
                m_DeadReason = 1;
            }

            SetHitBox(false);
            ChangeEnemyFSM(EnemyStateName.DEAD);
            return;
        }
    }
    /// <summary>해당 오브젝트의 모든 Hotbox를 켜거나 끕니다.</summary>
    /// <param name="_on">true = 켜기, false = 끄기</param>
    public void SetHitBox(bool _on)
    {
        foreach (Enemy_HotBox ele in m_HotBoxes)
        {
            ele.gameObject.SetActive(_on);
        }
    }
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        Debug.Log("상태 전이" + _name);
        m_CurEnemyStateName = _name;
        
        m_CurEnemyFSM.ExitState();
        
        switch (m_CurEnemyStateName)
        {
            case EnemyStateName.IDLE:
                m_CurEnemyFSM = m_IDLE;
                break;
            
            case EnemyStateName.FOLLOW:
                m_CurEnemyFSM = m_FOLLOW;
                break;
            
            case EnemyStateName.RUSH:
                m_CurEnemyFSM = m_RUSH;
                break;
            
            case EnemyStateName.DEAD:
                m_CurEnemyFSM = m_DEAD;
                break;

            default:
                Debug.Log("Enemy->ChangeEnemyFSM에서 존재하지 않는 상태 전이 요청");
                break;
        }
        
        m_CurEnemyFSM.StartState();
    }
    
    /// <summary>파라미터에 기반하여 왼쪽 혹은 오른쪽 방향대로 velocity를 수정합니다.</summary>
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
    
    public override void ResetMovePoint(Vector2 _destinationPos)
    {
        m_MovePoint = _destinationPos;
        //m_MovePoint = (_destinationPos - (Vector2)transform.position).normalized;
    }
    
    public override void SetRigidToPoint(float _addSpeed = 1f)
    {
        Vector2 direction = (m_MovePoint - (Vector2)transform.position).normalized;
        if (direction.x > 0)
        {
            setisRightHeaded(true);
            m_EnemyRigid.velocity = direction * (p_MoveSpeed * _addSpeed);
        }
        else
        {
            setisRightHeaded(false);
            m_EnemyRigid.velocity = direction * (p_MoveSpeed * _addSpeed);
        }
    }

    /// <summary>
    /// 드론의 속도를 점차 낮춥니다. (물리적)
    /// </summary>
    /// <param name="_isBreak">false일 경우 즉시 정지</param>
    public void VelocityBreak(bool _isBreak)
    {
        if (!ReferenceEquals(m_VelocityBreakCoroutine, null))
        {
            StopCoroutine(m_VelocityBreakCoroutine);
        }
        
        if (_isBreak)
        {
            m_VelocityBreakCoroutine = StartCoroutine(VelocitySlowDown());
        }
        else
        {
            m_EnemyRigid.velocity = Vector2.zero;
        }
        
    }

    private IEnumerator VelocitySlowDown()
    {
        Vector2 velocity = m_EnemyRigid.velocity;
        float power = 1f;
        
        while (true)
        {
            m_EnemyRigid.velocity = velocity * power;
            power -= Time.deltaTime * p_BreakPower;
            
            if (power <= 0f)
            {
                m_EnemyRigid.velocity = Vector2.zero;
                if (!ReferenceEquals(m_CoroutineCallback, null))
                {
                    m_CoroutineCallback();
                    m_CoroutineCallback = null;
                }

                yield break;
            }
            yield return null;
        }
    }
    
    
    /*
     public override void MoveByDirection_FUpdate(bool _isRight)
    {
        m_Timer += Time.deltaTime;
        m_SinYValue = Mathf.Sin(m_Timer * p_WiggleSpeed) * p_WigglePower;
        if (_isRight)
        {
            if(!m_IsRightHeaded)
                setisRightHeaded(true);
            
            //m_EnemyRigid.velocity = new Vector2(1, m_SinYValue).normalized * (p_Speed);
            m_EnemyRigid.velocity = Vector2.right * p_Speed;
        }
        else
        {
            if(m_IsRightHeaded)
                setisRightHeaded(false);
            
            //m_EnemyRigid.velocity = new Vector2(-1, m_SinYValue).normalized * (p_Speed);
            m_EnemyRigid.velocity = Vector2.left * p_Speed;
        }
    }
     */
    
    // For MatChanger
    public bool m_IgnoreMatChanger { get; set; }
    public SpriteType m_SpriteType { get; set; }
    public SpriteMatType m_CurSpriteMatType { get; set; }
    public Material p_OriginalMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_BnWMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_RedHoloMat { get; set; }
    [field: SerializeField, BoxGroup("ISpriteMatChange")] public Material p_DisappearMat { get; set; }
    private Coroutine m_MatTimeCoroutine = null;
    private readonly int ManualTimer = Shader.PropertyToID("_ManualTimer");
    
    public void ChangeMat(SpriteMatType _matType)
    {
        if (!ReferenceEquals(m_MatTimeCoroutine, null))
        {
            StopCoroutine(m_MatTimeCoroutine);
            m_MatTimeCoroutine = null;
        }
        
        if (m_IgnoreMatChanger || !gameObject.activeSelf)
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