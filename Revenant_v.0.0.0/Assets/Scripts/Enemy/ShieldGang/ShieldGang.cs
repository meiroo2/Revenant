using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


public class ShieldGang : BasicEnemy, ISpriteMatChange
{
    // Visible Member Variables
    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public int p_Shield_Hp = 10;
    
    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public int p_Shield_Dmg_Multi = 1;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_AttackDistance = 0.4f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public int p_AttackDamage = 10;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_GapDistance = 1f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_AtkAniSpeedMulti = 1f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_BackMoveSpeedMulti = 0.8f;
    
    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_BrokenSpeedMulti = 1.5f;

    [field: SerializeField, BoxGroup("ShieldGang Values"), Range(0f, 1f)]
    public float p_PointAtkTime = 0.5f;

    [field: SerializeField, BoxGroup("ShieldGang Values")]
    public float p_BreakWaitTime = 1f;

    [field: SerializeField, BoxGroup("ShieldGang Values"), PropertySpace(SpaceBefore = 0, SpaceAfter = 20)]
    public float p_AtkHoldTime = 0.5f;


    public WallSensor p_WallSensor;
    public RuntimeAnimatorController p_ShieldAnimator;
    public RuntimeAnimatorController p_NudeAnimator;
    public Transform p_HotBoxesTransform;
    public Enemy_HotBox p_HeadHotBox;
    public Enemy_HotBox p_BodyHotBox;
    public float p_AtkAniLerpPoint = 0.4f;
    

    // Member Variables
    public WeaponMgr m_WeaponMgr { get; private set; }

    private IDLE_ShieldGang m_IDLE;
    private FOLLOW_ShieldGang m_FOLLOW;
    private ATTACK_ShieldGang m_ATTACK;
    private DEAD_ShieldGang m_DEAD;
    private CHANGE_ShieldGang m_CHANGE;
    private ROTATION_ShieldGang m_ROTATION;
    private STUN_ShieldGang m_STUN;
    private HIT_ShieldGang m_HIT;
    
    public HitSFXMaker m_HitSFXMaker { get; private set; }
    public CoroutineHandler m_CoroutineHandler { get; private set; }
    public Shield m_Shield { get; private set; }
    public bool m_IsShieldBroken { get; private set; } = false;
    

    private bool m_SafeSFMLock = false;
    public readonly int AtkSpeed = Animator.StringToHash("AtkSpeed");


    // Constructor
    private void Awake()
    {
        InitHuman();
        InitEnemy();

        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_Animator.runtimeAnimatorController = p_ShieldAnimator;
        m_Shield = GetComponentInChildren<Shield>();
        
        m_WeaponMgr = GetComponentInChildren<WeaponMgr>();

        m_EnemyRigid = GetComponent<Rigidbody2D>();
        m_Foot = GetComponentInChildren<Enemy_FootMgr>();
        
        m_Animator.SetFloat(AtkSpeed, p_AtkAniSpeedMulti);
        
        InitISpriteMatChange();
    }

    private void Start()
    {
        m_OriginPos = transform.position;
        m_PlayerTransform = GameMgr.GetInstance().p_PlayerMgr.GetPlayer().transform;
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        m_SoundPlayer = GameMgr.GetInstance().p_SoundPlayer;
        m_HitSFXMaker = InstanceMgr.GetInstance().GetComponentInChildren<HitSFXMaker>();
        
        m_IDLE = new IDLE_ShieldGang(this);
        m_FOLLOW = new FOLLOW_ShieldGang(this);
        m_ATTACK = new ATTACK_ShieldGang(this);
        m_CHANGE = new CHANGE_ShieldGang(this);
        m_ROTATION = new ROTATION_ShieldGang(this);
        m_DEAD = new DEAD_ShieldGang(this);
        m_STUN = new STUN_ShieldGang(this);
        m_HIT = new HIT_ShieldGang(this);

        m_CurEnemyFSM = m_IDLE;
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM.StartState();

        // Weapon Value Initiate
        m_WeaponMgr.m_CurWeapon.p_BulletDamage = p_AttackDamage;
    }

    private void OnEnable()
    {
        m_Animator.SetFloat(AtkSpeed, p_AtkAniSpeedMulti);
    }


    // Updates
    private void Update()
    {
        if (m_SafeSFMLock)
            return;
        
        m_CurEnemyFSM.UpdateState();
    }


    // Functions
    public override void RaycastVisionCheck()
    {
        Vector2 position = transform.position;
        position.y -= 1f;
        
        if (m_IsRightHeaded)
        {
            m_VisionHit = Physics2D.Raycast(position, Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, Vector2.right * p_VisionDistance, Color.red);
        }
        else
        {
            m_VisionHit = Physics2D.Raycast( position, -Vector2.right, p_VisionDistance, LayerMask.GetMask("Player"));
            Debug.DrawRay(position, -Vector2.right * p_VisionDistance, Color.red);
        }
    }
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;

        p_Hp -= _damage;

        if (p_Hp <= 0)
        {
            // 불릿타임으로 막타 맞아서 사망 시
            if (_weaponType == WeaponType.BULLET_TIME)
            {
                m_DeadReasonForMat = 1;
                p_OriginalMat = p_DisappearMat;
                
                ChangeMat(SpriteMatType.DISAPPEAR);
            }
            
            ChangeEnemyFSM(EnemyStateName.DEAD);
        }
    }
    
    public int UpdateShieldDmg(int _dmg)
    {
        p_Shield_Hp -= _dmg * p_Shield_Dmg_Multi;

        if (p_Shield_Hp > 0)
        {
            // Shield Hit
            m_SoundPlayer.PlayEnemySound(3, 3, GetBodyCenterPos());
            
            if (m_CurEnemyStateName is EnemyStateName.IDLE or EnemyStateName.FOLLOW)
                ChangeEnemyFSM(EnemyStateName.HIT);

            return 1;
        }
        else
        {
            // Shield Break
            if (!m_IsShieldBroken)
            {
                m_SoundPlayer.PlayEnemySound(3, 4, GetBodyCenterPos());
                ChangeEnemyFSM(EnemyStateName.HIT);
            }
            
            return 0;
        }
    }
    
    public override void SetEnemyValues(EnemyMgr _mgr)
    {
        if (p_OverrideEnemyMgr)
            return;

        m_Shield = GetComponentInChildren<Shield>();
        
        p_Hp = _mgr.S_HP;
        p_Shield_Hp = _mgr.S_ShieldHp;
        p_AttackDamage = _mgr.S_MeleeDamage;
        p_MoveSpeed = _mgr.S_Speed;
        p_BackMoveSpeedMulti = _mgr.S_BackSpeedMulti;
        p_BrokenSpeedMulti = _mgr.S_BrokenSpeedMulti;
        p_VisionDistance = _mgr.S_VisionDistance;
        p_AttackDistance = _mgr.S_AttackDistance;
        p_GapDistance = _mgr.S_GapDistance;
        p_AtkAniSpeedMulti = _mgr.S_AtkAniSpeedMulti;
        p_PointAtkTime = _mgr.S_PointAtkTime;
        p_AtkHoldTime = _mgr.S_AtkHoldTime;
        p_Shield_Dmg_Multi = _mgr.S_ShieldDmgMulti;
        p_HeadHotBox.p_DamageMulti = _mgr.S_HeadDmgMulti;
        p_BodyHotBox.p_DamageMulti = _mgr.S_BodyDmgMulti;
        
        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(p_HeadHotBox);
            EditorUtility.SetDirty(p_BodyHotBox);
            EditorUtility.SetDirty(m_Shield);
        #endif
    }
    
    public void BreakShield()
    {
        m_IsShieldBroken = true;
    }
    
    public override void SetRigidByDirection(bool _isRight, float _addSpeed = 1f)
    {
        if (_isRight)
        {
            m_EnemyRigid.velocity = -StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed * _addSpeed);
        }
        else
        {
            m_EnemyRigid.velocity = StaticMethods.getLPerpVec(m_Foot.m_FootNormal).normalized * (p_MoveSpeed * _addSpeed);
        }
    }
    
    public override void ChangeEnemyFSM(EnemyStateName _name)
    {
        m_SafeSFMLock = true;
        
        Debug.Log(gameObject.name + "의 상태 전이" + _name);
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

            case EnemyStateName.ATTACK:
                m_CurEnemyFSM = m_ATTACK;
                break;
            
            case EnemyStateName.ROTATION:
                m_CurEnemyFSM = m_ROTATION;
                break;
            
            case EnemyStateName.CHANGE:
                m_CurEnemyFSM = m_CHANGE;
                break;

            case EnemyStateName.DEAD:
                m_CurEnemyFSM = m_DEAD;
                break;
            
            case EnemyStateName.HIT:
                m_CurEnemyFSM = m_HIT;
                break;

            default:
                Debug.Log("ERR : 존재하지 않는 상태 전이 요청");
                break;
        }

        m_CurEnemyFSM.StartState();
        m_SafeSFMLock = false;
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
    
    // For MatChanger
    public bool m_IgnoreMatChanger { get; set; }
    public SpriteType m_SpriteType { get; set; }
    public SpriteMatType m_CurSpriteMatType { get; set; }
    public Material p_OriginalMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_BnWMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_RedHoloMat { get; set; }

    [field: SerializeField, BoxGroup("ISpriteMatChange")]
    public Material p_DisappearMat { get; set; }

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
                m_CurSpriteMatType = SpriteMatType.ORIGIN;
                m_Renderer.material = p_OriginalMat;
                break;

            case SpriteMatType.BnW:
                m_CurSpriteMatType = SpriteMatType.BnW;
                m_Renderer.material = p_BnWMat;
                break;

            case SpriteMatType.REDHOLO:
                m_CurSpriteMatType = SpriteMatType.REDHOLO;
                m_MatTimeCoroutine = StartCoroutine(MatTimeInput());
                m_Renderer.material = p_RedHoloMat;
                break;

            case SpriteMatType.DISAPPEAR:
                m_CurSpriteMatType = SpriteMatType.DISAPPEAR;
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
