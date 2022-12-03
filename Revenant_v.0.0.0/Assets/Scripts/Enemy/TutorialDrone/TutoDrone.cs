using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


public class TutoDrone : BasicEnemy
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
}