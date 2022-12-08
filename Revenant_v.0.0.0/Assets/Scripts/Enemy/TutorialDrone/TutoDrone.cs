using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class TutoDrone : BasicEnemy, ISpriteMatChange
{
    [field: SerializeField, BoxGroup("TutoDrone Values")] public Transform[] p_PatrolPoses;

    [field: SerializeField, BoxGroup("TutoDrone Values")] public Enemy_HotBox p_HeadBox;
    [field: SerializeField, BoxGroup("TutoDrone Values")] public Enemy_HotBox p_BodyBox;

    [field: SerializeField, BoxGroup("TutoDrone Values")] public Animator p_Animator;

    // Member Variables
    private IDLE_TutoDrone m_IDLE;
    private PATROL_TutoDrone m_PATROL;
    private DEAD_TutoDrone m_DEAD;
    public CoroutineHandler m_CoroutineHandler { get; private set; } = null;

    // Constructors
    private void Awake()
    {
        m_Renderer = GetComponentInChildren<SpriteRenderer>();
        
        if (p_PatrolPoses.Length < 2)
        {
            Debug.Log("ERR : TutoDrone에 p_PatrolPoses 2개 미만");
        }
        
        m_Animator = p_Animator;
        m_EnemyRigid = GetComponent<Rigidbody2D>();
        
        m_IDLE = new IDLE_TutoDrone(this);
        m_PATROL = new PATROL_TutoDrone(this);
        m_DEAD = new DEAD_TutoDrone(this);
        
        m_CurEnemyStateName = EnemyStateName.IDLE;
        m_CurEnemyFSM = m_IDLE;
    }

    private void Start()
    {
        m_CoroutineHandler = GameMgr.GetInstance().p_CoroutineHandler;
        
        m_CurEnemyFSM.StartState();
    }

    private void FixedUpdate()
    {
        m_CurEnemyFSM.UpdateState();
    }
    
    
    // Functions
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
            
            case EnemyStateName.PATROL:
                m_CurEnemyFSM = m_PATROL;
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
    
    public override void AttackedByWeapon(HitBoxPoint _point, int _damage, int _stunValue, WeaponType _weaponType)
    {
        if (m_CurEnemyStateName == EnemyStateName.DEAD)
            return;
        
        p_Hp -= _damage;
        
        if (p_Hp <= 0)
        {
            ChangeEnemyFSM(EnemyStateName.DEAD);
        }
    }
    
    public override void SetHotBoxesActive(bool _isOn)
    {
        p_HeadBox.gameObject.SetActive(_isOn);
        p_BodyBox.gameObject.SetActive(_isOn);
    }
    
    
    
    // For MatChanger
    public bool m_IgnoreMatChanger { get; set; } = false;
    public SpriteType m_SpriteType { get; set; } = SpriteType.ENEMY;
    public SpriteMatType m_CurSpriteMatType { get; set; } = SpriteMatType.ORIGIN;
    [field : SerializeField] public Material p_OriginalMat { get; set; }
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